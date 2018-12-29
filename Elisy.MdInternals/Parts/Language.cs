using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("9cd510ce-abfc-11d4-9434-004095e12fc7")]
    public class Language : MetadataPart
    {
        public Language()
            : base()
        {
        }


        public Language(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
