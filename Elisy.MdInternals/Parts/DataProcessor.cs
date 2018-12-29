using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;
using Elisy.MdInternals.DataElements;

namespace Elisy.MdInternals
{
    [Guid("bf845118-327b-4682-b5c6-285d2a0eb296")]
    public class DataProcessor : MetadataPart
    {
        public Guid Guid_1_1 { get; set; }
        public Guid Guid_1_2 { get; set; }
        public Guid Guid_1_4 { get; set; }

        public int Int_1_5 { get; set; }
        public int Int_1_6 { get; set; }

        public Guid Guid_1_7 { get; set; }
        public Guid Guid_1_8 { get; set; }
        public Guid Guid_1_9 { get; set; }

        public Collection List_1_10 { get; set; }
        public Collection List_1_11 { get; set; }

        public Collection List_3 { get; set; }
        public RefList Templates { get; set; } //Templates
        public Collection List_5 { get; set; }
        public RefList Forms { get; set; } //Forms
        public Collection List_7 { get; set; }

        public DataProcessor()
            : base()
        {
        }


        public DataProcessor(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
