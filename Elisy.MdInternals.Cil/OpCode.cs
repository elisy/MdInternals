using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elisy.MdInternals.Cil
{
    public class OpCode
    {
        public CmdCode Code { get; set; }
        public int Op1 { get; set; }
        public double Index { get; set; }
        public OpCode(CmdCode code, int op1)
        {
            Code = code;
            Op1 = op1;
        }

        public object Reserved { get; set; }
    }
}
