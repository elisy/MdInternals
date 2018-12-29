using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;

using Elisy.MdInternals.Cf;
using Elisy.MdInternals.Serialization;
using Elisy.MdInternals.DataElements;

namespace Elisy.MdInternals
{
    public class RootPointer : MetadataPart
    {
        public Guid MetadataPackageFileName { get; set; }
        public string Data0 { get; set; }

        public RootPointer()
            : base()
        {
        }


        public RootPointer(ImageRow row) : base (row)
        {
            if (row.FileName != "root")
                throw new ArgumentException("Image row is not root");
        }

        protected override void OnSetModelProperty(Collection model, string name, ref object value, int[] indexes)
        {
            base.OnSetModelProperty(model, name, ref value, indexes);

            //Remove quotes
            if (name == "Data0")
                value = value.AsString();
        }
    }
}
