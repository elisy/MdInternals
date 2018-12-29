using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("2deed9b8-0056-4ffe-a473-c20a6c32a0bc")]
    public class AccountingRegister : MetadataPart
    {
        public AccountingRegister()
            : base()
        {
        }

        public AccountingRegister(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
