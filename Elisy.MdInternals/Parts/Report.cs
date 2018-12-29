using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;
using Elisy.MdInternals.DataElements;

namespace Elisy.MdInternals
{
    [Guid("631b75a0-29e2-11d6-a3c7-0050bae0a776")]
    public class Report : MetadataPart
    {

        public Guid Guid_1_1 { get; set; }
        public Guid Guid_1_2 { get; set; }
        public Guid Guid_1_4 { get; set; }
        public Guid Guid_1_5 { get; set; }
        public int Int_1_7 { get; set; }
        public Guid Guid_1_8 { get; set; }
        public int Int_1_11 { get; set; }
        public Guid Guid_1_12 { get; set; }
        public Guid Guid_1_13 { get; set; }

        public Collection List_1_15 { get; set; }
        public Collection List_1_16 { get; set; }

        public Collection List_3 { get; set; }
        public Collection List_4 { get; set; }
        public Collection List_5 { get; set; }
        //public Collection List_6 { get; set; }
        public Collection List_7 { get; set; }


        public Report()
            : base()
        {
        }


        public Report(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
