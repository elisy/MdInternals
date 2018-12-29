using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("0c89c792-16c3-11d5-b96b-0050bae0a95d")]
    public class CommonTemplate : MetadataPart
    {
        public CommonTemplate()
            : base()
        {
        }


        public CommonTemplate(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
