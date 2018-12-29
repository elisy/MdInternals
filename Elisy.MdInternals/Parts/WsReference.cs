using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("d26096fb-7a5d-4df9-af63-47d04771fa9b")]
    public class WsReference : MetadataPart
    {
        public WsReference()
            : base()
        {
        }


        public WsReference(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
