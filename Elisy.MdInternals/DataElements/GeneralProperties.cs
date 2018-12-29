using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;

using Elisy.MdInternals.Serialization;

namespace Elisy.MdInternals.DataElements
{
    [Obsolete]
    public class GeneralProperties : DataElement
    {
        public int Data0 { get; private set; }
        public int Data1 { get; private set; }
        public int Data2 { get; private set; }
        public string FileName {get; set;}
        public string Name { get; set; }
        public LocalStringType Synonym { get; set; }
        public string Comment { get; set; }

        public GeneralProperties()
            : base()
        {
        }

        public GeneralProperties(object value)
            : base(value)
        {
            Collection list = value as Collection;
            Data0 = list.GetElement(0).AsInt32();
            Data1 = list.GetElement(1, 0).AsInt32();
            Data2 = list.GetElement(1, 1).AsInt32();
            FileName = list.GetElement(1, 2).AsString();
            Name = list.GetElement(2).AsString();
            Synonym = new LocalStringType(list.GetElement(3) as Collection);
            Comment = list.GetElement(4).AsString();
        }

        public override object GetData()
        {
            return new Collection(
                Data0.ToString(CultureInfo.InvariantCulture),
                new Collection(Data1.ToString(CultureInfo.InvariantCulture), Data2.ToString(CultureInfo.InvariantCulture), FileName),
                "\"" + Name + "\"",
                Synonym.GetData(),
                "\"" + Comment + "\""
                );
        }

    }
}
