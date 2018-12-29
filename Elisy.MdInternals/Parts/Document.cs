using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("061d872a-5787-460e-95ac-ed74ea3a3e84")]
    public class Document : MetadataPart
    {
        //internal override int[] _generalPropertiesLocation
        //{
        //    get { return new int[] { 1, 9, 1 }; }
        //}

        public Document()
            : base()
        {
        }



        public Document(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
