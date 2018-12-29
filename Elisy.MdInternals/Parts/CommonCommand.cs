using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("2f1a5187-fb0e-4b05-9489-dc5dd6412348")]
    public class CommonCommand : MetadataPart
    {
        public CommonCommand()
            : base()
        {
        }


        public CommonCommand(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
