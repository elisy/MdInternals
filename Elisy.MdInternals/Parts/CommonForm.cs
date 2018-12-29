using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("07ee8426-87f1-11d5-b99c-0050bae0a95d")]
    public class CommonForm : MetadataPart
    {
        public CommonForm()
            : base()
        {
        }


        public CommonForm(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
