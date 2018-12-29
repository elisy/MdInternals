using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("37f2fa9a-b276-11d4-9435-004095e12fc7")]
    public class Subsystem : MetadataPart
    {
        public Subsystem()
            : base()
        {
        }


        public Subsystem(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
