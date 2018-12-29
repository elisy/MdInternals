using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elisy.MdInternals.Cil
{
    [Flags]
    public enum VariableAttributes : short
    {
        Data1 = 1,
        Parameter = 4,
        ByValue = 8
    }
}
