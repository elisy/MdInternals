using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("24c43748-c938-45d0-8d14-01424a72b11e")]
    public class SessionParameter : MetadataPart
    {
        public SessionParameter()
            : base()
        {
        }


        public SessionParameter(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
