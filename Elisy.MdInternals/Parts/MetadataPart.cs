using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;
using System.Windows.Markup;
using System.Globalization;

using Elisy.MdInternals.Cf;
using Elisy.MdInternals.DataElements;
using Elisy.MdInternals.Serialization;

using System.Reflection;
using System.ComponentModel;

namespace Elisy.MdInternals
{
    //[ContentProperty("UnresolvedDataElements")] 
    public class MetadataPart : ImageRowElement
    {
        [DefaultValue(null)]
        public Collection Unresolved { get; set; }

        public virtual Collection GetModel()
        {
            if (Unresolved != null)
                return Unresolved.Clone() as Collection;

            DataSerializer ds = new DataSerializer();
            string modelTemplate = Elisy.MdInternals.Resources.StringTemplates.ResourceManager.GetString(GetType().Name);
            Collection result = ds.Deserialize(modelTemplate);

            Type type = this.GetType();
            foreach (KeyValuePair<string, int[]> kvp in _boundProperties)
                SetModelProperty(result, kvp.Key, type.GetProperty(kvp.Key).GetValue(this, null));

            return result;
        }


        public virtual void SetModel(Collection value)
        {
            Type type = this.GetType();
            foreach (KeyValuePair<string, int[]> kvp in _boundProperties)
            {
                PropertyInfo property = type.GetProperty(kvp.Key);
                if (property == null)
                    throw new InvalidOperationException("Unknown bound property: " + kvp.Key);
                object o = GetModelProperty(property.PropertyType, value, kvp.Key);
                property.SetValue(this, o, null);
            }
        }

        public MetadataPart(ImageRow imageRow) : base(imageRow)
        {
            BindModel();

            DataSerializer ds = new DataSerializer();
            SetModel(ds.Deserialize(ImageRow.Body as string));
        }

        public MetadataPart()
            : base()
        {
            BindModel();
        }

        private Dictionary<string, int[]> _boundProperties = null;
        private void BindModel()
        {
            if (_boundProperties == null)
            {
                _boundProperties = new Dictionary<string, int[]>();
                string template = Elisy.MdInternals.Resources.StringTemplates.ResourceManager.GetString(GetType().Name);
                if (string.IsNullOrEmpty(template))
                {
                    _boundProperties.Add("Unresolved", new int[] {});
                }
                else
                {
                    DataSerializer ds = new DataSerializer();
                    Collection tree = ds.Deserialize(template);
                    BindModel(tree, new int[] { });
                }
            }
        }

        private void BindModel(Collection tree, int[] indexes)
        {
            for (int i = 0; i < tree.Count; i++)
            {
                int[] currentIndexes = indexes.Clone() as int[];
                Array.Resize(ref currentIndexes, currentIndexes.Length + 1);
                currentIndexes[currentIndexes.Length - 1] = i;

                if (tree[i] is Collection)
                {
                    BindModel(tree[i] as Collection, currentIndexes);
                }
                else if (tree[i].AsString().StartsWith("#ElisyBound:"))
                {
                    _boundProperties.Add(tree[i].AsString().Substring(12), currentIndexes);
                }
            }
        }

        protected object GetModelProperty(Type type, Collection model, string name)
        {
            if (_boundProperties.ContainsKey(name))
            {
                object result = null;
                int[] indexes = _boundProperties[name];
                if (type == typeof(string))
                    result = model.GetElement(indexes).AsString();
                else if (type == typeof(bool))
                    result = model.GetElement(indexes).AsBoolean();
                else if (type == typeof(int))
                    result = model.GetElement(indexes).AsInt32();
                else if (type == typeof(double))
                    result = model.GetElement(indexes).AsDouble();
                else if (type == typeof(DateTime))
                    result = model.GetElement(indexes).AsDateTime();
                else if (type == typeof(Collection))
                    result = model.GetElement(indexes) as Collection;
                else
                    result = Activator.CreateInstance(type, model.GetElement(indexes));

                OnGetModelProperty(type, ref result, model, name, indexes);

                return result;
            }
            else
                throw new InvalidOperationException();
        }

        protected virtual void OnGetModelProperty(Type type, ref object value, Collection model, string name, int[] indexes)
        {
            //if (name == "Name" || name == "Comment")
            //    value = value.AsString();
        }

        protected void SetModelProperty(Collection model, string name, object value)
        {
            if (value is IDataElement)
                value = ((IDataElement)value).GetData();
            else if (value is Boolean)
                value = ((int)((bool)value ? 1 : 0)).ToString(CultureInfo.InvariantCulture);
            else if (value is int)
                value = ((int)value).ToString(CultureInfo.InvariantCulture);
            else if (value is string)
                value = "\"" + value + "\"";
            else if (value is Guid)
                value = value.ToString();

            OnSetModelProperty(model, name, ref value, _boundProperties[name]);

            model.SetElement(_boundProperties[name], value);
        }

        protected virtual void OnSetModelProperty(Collection model, string name, ref object value, int[] indexes)
        {
            //if (name == "Name" || name == "Comment")
            //    value = "\"" + value + "\"";
        }

        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public LocalStringType Synonym { get; set; }
        public string Comment { get; set; }

        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual string FullName
        {
            get
            {
                string id = ImageRow.FileName;
                if (Uuid != Guid.Empty)
                    id = Uuid.ToString();
                if (!String.IsNullOrEmpty(Name))
                    id = Name;
                return GetType().Name + "." + id;
            }
            set
            {
                //Ignore to support XAML serialization
            }
        }
    }
}
