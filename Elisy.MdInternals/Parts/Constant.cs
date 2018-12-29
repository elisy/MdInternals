using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("0195e80c-b157-11d4-9435-004095e12fc7")]
    public class Constant : MetadataPart
    {
        //internal override int[] _generalPropertiesLocation
        //{
        //    get { return new int[] { 1, 1, 1, 1}; }
        //}

        public Constant()
            : base()
        {
        }


        public Constant(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
