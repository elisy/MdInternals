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
    public class Version : MetadataPart
    {
        public Version()
            : base()
        {
        }

        public Version(ImageRow row)
            : base(row)
        {
        }
    }
}
