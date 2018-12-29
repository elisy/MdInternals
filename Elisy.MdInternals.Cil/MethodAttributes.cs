using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elisy.MdInternals.Cil
{
    [Flags]
    public enum MethodAttributes : short
    {
        Procedure = 0,
        Function = 1,
        Export = 16,
        ExternalMethod = 32
    }
}
