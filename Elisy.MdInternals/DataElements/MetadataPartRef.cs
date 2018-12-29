using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Markup;

using Elisy.MdInternals.Serialization;
using Elisy.MdInternals;

namespace Elisy.MdInternals.DataElements
{
    [ContentProperty("FullName")]
    public class MetadataPartRef : DataElement
    {
        public string FileName {get; set;}
        public string FullName { get; set; }

        public MetadataPartRef()
            : base()
        {
        }

        public MetadataPartRef(object value)
            : base(value)
        {
            FileName = value.ToString();
            FullName = "";
        }

        public override object GetData()
        {
            return FileName;
        }

    }
}
