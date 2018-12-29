using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("82a1b659-b220-4d94-a9bd-14d757b95a48")]
    public class ChartOfCharacteristicTypes : MetadataPart
    {
        public ChartOfCharacteristicTypes()
            : base()
        {
        }


        public ChartOfCharacteristicTypes(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
