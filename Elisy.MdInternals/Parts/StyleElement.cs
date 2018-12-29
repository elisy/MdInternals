using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("58848766-36ea-4076-8800-e91eb49590d7")]
    public class StyleElement : MetadataPart
    {
        public StyleElement()
            : base()
        {
        }


        public StyleElement(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
