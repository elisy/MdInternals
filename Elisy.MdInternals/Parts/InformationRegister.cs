using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("13134201-f60b-11d5-a3c7-0050bae0a776")]
    public class InformationRegister : MetadataPart
    {
        public InformationRegister()
            : base()
        {
        }


        public InformationRegister(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
