using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("cf4abea6-37b2-11d4-940f-008048da11f9")]
    public class Catalog : MetadataPart
    {
        public Catalog()
            : base()
        {
        }


        public Catalog(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
