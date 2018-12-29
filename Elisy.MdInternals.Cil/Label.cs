using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elisy.MdInternals.Cil
{
    public class Label
    {
        public string Name { get; private set; }
        public int Data1 { get; private set; }
        public Label(string name, int data1)
        {
            Name = name;
            Data1 = data1;
        }
    }
}
