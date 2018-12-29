using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;

using Elisy.MdInternals.Serialization;

namespace Elisy.MdInternals.Cil
{
    public class Constant
    {
        public string Value { get; private set; }
        public Constant(string type, string value)
        {
            if (type == "S")
                Value = value.Replace(Environment.NewLine, Environment.NewLine + "|");
            else if (type == "N")
                Value = value;
            else if (type == "D")
                Value = "Дата('" + value + "')";
            else
                throw new NotImplementedException();
        }
    }
}
