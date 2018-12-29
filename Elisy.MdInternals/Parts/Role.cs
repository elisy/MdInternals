using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("09736b02-9cac-4e3f-b4f7-d3e9576ab948")]
    public class Role : MetadataPart
    {
        public Role()
            : base()
        {
        }


        public Role(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
