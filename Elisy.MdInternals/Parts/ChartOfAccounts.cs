using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("238e7e88-3c5f-48b2-8a3b-81ebbecb20ed")]
    public class ChartOfAccounts : MetadataPart
    {
        public ChartOfAccounts()
            : base()
        {
        }


        public ChartOfAccounts(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
