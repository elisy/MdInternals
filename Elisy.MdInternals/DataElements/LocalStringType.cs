using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Markup;
using System.Collections.ObjectModel;

using System.Globalization;

using Elisy.MdInternals.Serialization;

namespace Elisy.MdInternals.DataElements
{
    [ContentProperty("Items")]
    public class LocalStringType : DataElement
    {
        public ObservableCollection<LocalString> Items { get; set; }
        public LocalStringType(object value) : base(value)
        {
            var content = value as Collection;
            Items = new ObservableCollection<LocalString>();

            int count = content[0].AsInt32();
            for (int i = 0; i < count; i++)
                Items.Add(new LocalString(content[i * 2 + 1].AsString(), content[i * 2 + 2].AsString()));
        }

        public LocalStringType()
            : base()
        {
            Items = new ObservableCollection<LocalString>();
        }

        public override object GetData()
        {
            var result = new Collection(
                Items.Count.ToString(CultureInfo.InvariantCulture));
            foreach (LocalString item in Items)
            {
                result.Add("\"" + item.Lang + "\"");
                result.Add("\"" + item.Content + "\"");
            }
            return result;
        }

    }

    [ContentProperty("Value")]
    public class LocalString
    {
        public string Lang { get; set; }
        public string Content { get; set; }

        public LocalString()
            : base()
        {
        }

        public LocalString(string language, string value)
        {
            Lang = language;
            Content = value;
        }
    }
}
