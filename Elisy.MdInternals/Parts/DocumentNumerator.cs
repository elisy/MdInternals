using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("36a8e346-9aaa-4af9-bdbd-83be3c177977")]
    public class DocumentNumerator : MetadataPart
    {
        public DocumentNumerator()
            : base()
        {
        }


        public DocumentNumerator(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
