using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("15794563-ccec-41f6-a83c-ec5f7b9a5bc1")]
    public class SharedAttribute : MetadataPart
    {
        //internal override int[] _generalPropertiesLocation
        //{
        //    get { return new int[] {1, 1, 1, 1}; }
        //}

        public SharedAttribute()
            : base()
        {
        }


        public SharedAttribute(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
