using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("1c57eabe-7349-44b3-b1de-ebfeab67b47d")]
    public class CommandGroup : MetadataPart
    {
        public CommandGroup()
            : base()
        {
        }


        public CommandGroup(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
