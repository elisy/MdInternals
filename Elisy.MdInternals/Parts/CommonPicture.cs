using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("7dcd43d9-aca5-4926-b549-1842e6a4e8cf")]
    public class CommonPicture : MetadataPart
    {
        public CommonPicture()
            : base()
        {
        }


        public CommonPicture(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
