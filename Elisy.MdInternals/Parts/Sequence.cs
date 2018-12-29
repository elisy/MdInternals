using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("bc587f20-35d9-11d6-a3c7-0050bae0a776")]
    public class Sequence : MetadataPart
    {
        public Sequence()
            : base()
        {
        }


        public Sequence(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
