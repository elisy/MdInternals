using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("cc9df798-7c94-4616-97d2-7aa0b7bc515e")]
    public class XdtoPackage : MetadataPart
    {
        public XdtoPackage()
            : base()
        {
        }


        public XdtoPackage(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
