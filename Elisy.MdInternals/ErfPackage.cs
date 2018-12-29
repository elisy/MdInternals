using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;
using Elisy.MdInternals.Serialization;
using Elisy.MdInternals.DataElements;

namespace Elisy.MdInternals
{
    public class ErfPackage : MetadataPackage
    {
        public ErfPackage()
            : base()
        {
        }

        public ErfPackage(Image image)
            : base(image)
        {
        }
    }
}
