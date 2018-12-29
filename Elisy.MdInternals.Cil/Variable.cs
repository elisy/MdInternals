using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elisy.MdInternals.Cil
{
    public class Variable
    {
        public string Name { get; private set; }
        public VariableAttributes Attributes { get; private set; }
        public int Data2 { get; private set; }
        public Variable(string name, VariableAttributes attributes, int data2)
        {
            Name = name;
            Attributes = attributes;
            Data2 = data2;
        }
    }
}
