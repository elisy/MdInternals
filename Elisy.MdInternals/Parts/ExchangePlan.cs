using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("857c4a91-e5f4-4fac-86ec-787626f1c108")]
    public class ExchangePlan : MetadataPart
    {
        public ExchangePlan()
            : base()
        {
        }


        public ExchangePlan(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
