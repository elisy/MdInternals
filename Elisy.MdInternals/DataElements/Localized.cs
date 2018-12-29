using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Markup;
using System.Collections.ObjectModel;

using Elisy.MdInternals.Serialization;

namespace Elisy.MdInternals.DataElements
{
    [ContentProperty("Values")]
    public class Localized : DataElement
    {
        public ObservableCollection<LocalizedString> Values { get; set; }
        public Localized(Collection content)
        {
            Values = new ObservableCollection<LocalizedString>();

            int count = content[0].AsInt32();
            for (int i = 0; i < count; i++)
                Values.Add(new LocalizedString(content[i * 2 + 1].AsString(), content[i * 2 + 2].AsString()));
        }
    }

    [ContentProperty("Value")]
    public class LocalizedString
    {
        public string Language { get; set; }
        public string Value { get; set; }
        public LocalizedString(string language, string value)
        {
            Language = language;
            Value = value;
        }
    }
}
