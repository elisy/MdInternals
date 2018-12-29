using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("3e7bfcc0-067d-11d6-a3c7-0050bae0a776")]
    public class Filter : MetadataPart
    {
        public Filter()
            : base()
        {
        }


        public Filter(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
