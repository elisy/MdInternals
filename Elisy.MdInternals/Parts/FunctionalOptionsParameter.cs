using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("30d554db-541e-4f62-8970-a1c6dcfeb2bc")]
    public class FunctionalOptionsParameter : MetadataPart
    {
        public FunctionalOptionsParameter()
            : base()
        {
        }


        public FunctionalOptionsParameter(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
