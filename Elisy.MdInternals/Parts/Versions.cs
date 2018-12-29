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
    public class Versions : MetadataPart
    {

        public Versions()
            : base()
        {
        }

        public Versions(ImageRow row)
            : base(row)
        {
        }
    }
}
