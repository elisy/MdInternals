using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;
using System.Windows.Markup;

using Elisy.MdInternals.Cf;
using Elisy.MdInternals.DataElements;
using Elisy.MdInternals.Serialization;

using System.Reflection;
using System.ComponentModel;

namespace Elisy.MdInternals
{
    //[ContentProperty("UnresolvedDataElements")] 
    [Obsolete("Please use MetadataPart instead")]
    public class PartiallyResolvedPart : ImageRowElement
    {
        private Collection _unresolvedDataElements = new Collection();
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Collection UnresolvedDataElements 
        {
            get
            {
                return _unresolvedDataElements;
            }
            set
            {
                _unresolvedDataElements = value;
                CreateModelFromProperties();
            }
        }

        public void CreateModelFromProperties()
        {
            var result = _unresolvedDataElements.Clone() as Collection;
            Type type = this.GetType();
            foreach (KeyValuePair<string, int[]> kvp in _boundProperties)
                SetBoundProperty(result, kvp.Key, type.GetProperty(kvp.Key).GetValue(this, null));
            _model = result;
        }

        internal void UpdateUnresolvedDataElements()
        {
            _unresolvedDataElements = Model.Clone() as Collection;
            foreach (KeyValuePair<string, int[]> kvp in _boundProperties)
                SetBoundProperty(_unresolvedDataElements, kvp.Key, "#ElisyBound:" + kvp.Key);
        }


        private Collection _model;
        public virtual Collection Model
        {
            get
            {
                //if (_model == null && UnresolvedDataElements == null)
                //    throw new InvalidOperationException("Model is null and UnresolvedDataElements are null");

                //if (_model == null)
                //    UpdateModel();

                return _model;
            }
            private set
            {
                _model = value;
                UpdateUnresolvedDataElements();
            }
        }

        public PartiallyResolvedPart(ImageRow imageRow) : base(imageRow)
        {
            ResolveBoundProperties();

            DataSerializer ds = new DataSerializer();
            Model = ds.Deserialize(ImageRow.Body as string);

            //ResolveGeneralProperties(_generalPropertiesLocation);
        }

        public PartiallyResolvedPart()
            : base()
        {
            ResolveBoundProperties();
        }

        //private void ResolveGeneralProperties(int[] indexes)
        //{
        //    if (indexes.Length == 0)
        //    {
        //        GeneralProperties = null;
        //        return;
        //    }

        //    if ((ImageRow.BodyType == ImageRowTypes.Utf8MarkedString) || (ImageRow.BodyType == ImageRowTypes.CompressedUtf8MarkedString))
        //    {
        //        GeneralProperties = new GeneralProperties(Body.GetElement(indexes) as Collection);
        //        //Body.SetElement(indexes, GeneralProperties);
        //    }
        //}


        private GeneralProperties _generalProperties;
        public GeneralProperties GeneralProperties
        {
            get
            {
                if (Model == null)
                    return _generalProperties;
                else
                    return GetBoundProperty<GeneralProperties>("GeneralProperties");
            }
            set
            {
                _generalProperties = value;
                if (Model != null)
                    SetBoundProperty(Model, "GeneralProperties", value);
            }
        }

        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual string FileName
        {
            get
            {
                return ImageRow.FileName;
            }
            set
            {
                //Ignore to support XAML serialization
            }
        }

        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual string FullName
        {
            get
            {
                string id = FileName;
                if (GeneralProperties != null)
                    id = GeneralProperties.Name;
                return GetType().Name + "." + id;
            }
            set
            {
                //Ignore to support XAML serialization
            }
        }

        protected T GetBoundProperty<T>(string name)
        {
            if (_boundProperties.ContainsKey(name))
            {
                int[] indexes = _boundProperties[name];
                if (typeof(T) == typeof(String))
                    return (T)Model.GetElement(indexes);
                else if (typeof(T) == typeof(Boolean))
                    return (T)(Model.GetElement(indexes).AsBoolean() as Object);
                else
                    return (T)Activator.CreateInstance(typeof(T), Model.GetElement(indexes));
            }
            else
                return default(T);
        }

        //protected void SetBoundProperty(string name, object value)
        //{
        //    if (Model != null)
        //        SetBoundProperty(Model, name, value);
        //}

        protected void SetBoundProperty(Collection collection, string name, object value)
        {
            if (value is IDataElement)
                collection.SetElement(_boundProperties[name], ((IDataElement)value).GetData());
            else
                collection.SetElement(_boundProperties[name], value);
        }


        //internal void CreateInternal(MetadataPackage metadataPackage, MetadataPart parent, ImageRow imageRow)
        //{
        //    if ((metadataPackage == null) && (parent == null))
        //        throw new ArgumentNullException("metadataPackage or parent");

        //    if (parent != null)
        //        metadataPackage = parent.MetadataPackage;

        //    MetadataPackage = metadataPackage;
        //}


        private Dictionary<string, int[]> _boundProperties = null;
        private void ResolveBoundProperties()
        {
            if (_boundProperties == null)
            {
                _boundProperties = new Dictionary<string, int[]>();
                string template = Elisy.MdInternals.Resources.StringTemplates.ResourceManager.GetString(GetType().Name);
                if (!string.IsNullOrEmpty(template))
                {
                    DataSerializer ds = new DataSerializer();
                    Collection tree = ds.Deserialize(template);
                    ResolveBoundProperties(tree, new int[] { });
                }
            }
        }

        private void ResolveBoundProperties(Collection tree, int[] indexes)
        {
            for (int i = 0; i < tree.Count; i++)
            {
                if (tree[i] is Collection)
                {
                    int[] currentIndexes = indexes.Clone() as int[];
                    Array.Resize(ref currentIndexes, currentIndexes.Length + 1);
                    currentIndexes[currentIndexes.Length - 1] = i;

                    ResolveBoundProperties(tree[i] as Collection, currentIndexes);
                }
                else if (tree[i].AsString().StartsWith("#ElisyBound:"))
                {
                    int[] currentIndexes = indexes.Clone() as int[];
                    Array.Resize(ref currentIndexes, currentIndexes.Length + 1);
                    currentIndexes[currentIndexes.Length - 1] = i;

                    _boundProperties.Add(tree[i].AsString().Substring(12), currentIndexes);
                }
            }
        }
    }
}
