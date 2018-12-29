using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("3e5404af-6ef8-4c73-ad11-91bd2dfac4c8")]
    public class Style : MetadataPart
    {
        public Style()
            : base()
        {
        }


        public Style(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
