using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;
using Elisy.MdInternals.Serialization;
using Elisy.MdInternals.DataElements;


namespace Elisy.MdInternals
{
    [Guid("f6a80749-5ad7-400b-8519-39dc5dff2542")]
    public class Enum : MetadataPart
    {

        //public Collection List_1 { get; set; }

        public Guid Guid_1_1 { get; set; }
        public Guid Guid_1_2 { get; set; }
        public Guid Guid_1_3 { get; set; }
        public Guid Guid_1_4 { get; set; }

        public int Int_1_6 { get; set; }
        public Guid Guid_1_7 { get; set; }
        public Guid Guid_1_8 { get; set; }

        public int Int_1_12 { get; set; }

        public Collection List_1_15 { get; set; }
        public Collection List_1_16 { get; set; }
        public Collection List_1_17 { get; set; }

        public Collection List_1_18 { get; set; }

        public Collection List_3 { get; set; }
        public Collection List_6 { get; set; }

        public Enum()
            : base()
        {
        }

        public Enum(ImageRow row)
            : base(row)
        {
        }
    }
}
