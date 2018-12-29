using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("8657032e-7740-4e1d-a3ba-5dd6e8afb78f")]
    public class WebService : MetadataPart
    {
        public WebService()
            : base()
        {
        }


        public WebService(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
