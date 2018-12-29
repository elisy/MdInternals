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
    public class ExternalReport : MetadataPart
    {
        public Guid Guid_1_0 { get; set; }

        public Collection List_3_1_1 { get; set; }
        public Collection List_3_1_3 { get; set; }
        public Collection List_3_1_4 { get; set; }
        public Collection Forms { get; set; }
        public Collection List_3_1_6 { get; set; }


        public ExternalReport()
            : base()
        {
        }

        public ExternalReport(ImageRow row)
            : base(row)
        {
        }
    }
}
