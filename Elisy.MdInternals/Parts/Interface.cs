using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("39bddf6a-0c3c-452b-921c-d99cfa1c2f1b")]
    public class Interface : MetadataPart
    {
        public Interface()
            : base()
        {
        }


        public Interface(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
