using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;

using Elisy.MdInternals.Serialization;
using Elisy.MdInternals.DataElements;

namespace Elisy.MdInternals.Cil
{
    public class Method
    {
        public string Name { get; private set; }
        public MethodAttributes Attributes { get; private set; }
        public int Parameters { get; private set; }

        public int OpCodeFrom { get; private set; }
        public int OpCodeTo { get; set; }

        public Variable[] Variables { get; private set; }
        public Label[] Labels { get; private set; }
        public List<string> DefaultParameters { get; private set; }

        internal OpCode[] OpCodes { get; set; }

        //22: Jmp, 14379 ... 14381: Jmp, 23
        internal OpCode[] ModuleOpCodes { get; set; } //Enable obfuscation

        public OpCode GetOpCodeByIndex(double index)
        {
            OpCode result = OpCodes.Where(i => i.Index == index).FirstOrDefault();
            if (result == null)
                result = ModuleOpCodes.Where(i => i.Index == index).FirstOrDefault();

            return result;
        }

        public OpCode GetNextOpCode(double currentIndex)
        {
            OpCode result = OpCodes.Where(i => i.Index > currentIndex).OrderBy(m => m.Index).FirstOrDefault();
            if (result == null)
                result = ModuleOpCodes.Where(i => i.Index > currentIndex).OrderBy(m => m.Index).FirstOrDefault();
            return result;
        }

        public OpCode FindNextOpCode(double currentIndex, CmdCode[] codesToFind, CmdCode[] skipCodes)
        {
            OpCode nextCode = GetNextOpCode(currentIndex);
            if (codesToFind != null || codesToFind.Length != 0)
            {
                while (skipCodes.Contains(nextCode.Code))
                {
                    nextCode = GetNextOpCode(currentIndex);
                }
            }

            if (codesToFind.Contains(nextCode.Code))
                return nextCode;
            else
                return null;
        }

        public Method(Collection list)
        {
            DefaultParameters = new List<string>();

            Variables = new Variable[0];
            Labels = new Label[0];

            Name = list[0].AsString();
            Attributes = (MethodAttributes)list[1].AsInt32();
            Parameters = list[2].AsInt32();
            OpCodeFrom = list[3].AsInt32();

            if (Attributes != MethodAttributes.ExternalMethod)
            {
                var requestSections = from section in list.Skip(3).AsParallel()
                                      where section is Collection
                                      select section as Collection;
                requestSections.ForAll(m => ResolveMethodSection(m));
            }
        }

        private void ResolveMethodSection(Collection section)
        {
            string sectionName = section[0].AsString();
            int length = section[1].AsInt32();

            if (sectionName == "Var")
            {
                //Vars section can be defined
                var requestVars = from var in section.Skip(2)
                                  let list1 = var as Collection
                                  select new Variable(list1[0].AsString(), (VariableAttributes)list1[1].AsInt32(), list1[2].AsInt32());
                Variables = requestVars.ToArray();
            }
            else if (sectionName == "DefPrm")
            {
                //Defs section can be defined
                var requestDefs = from def in section.Skip(2)
                                    let listDef = def as Collection
                                    select listDef;
                var defaultParametersAsLists = requestDefs.ToArray();
                for (int i = 0; i < defaultParametersAsLists.Length; i++)
                {
                    Collection value = (Collection)defaultParametersAsLists[i];
                    string value1 = value[0].AsString();
                    //{"DefPrm",10,
                    //{""},
                    //{""},
                    //{""},
                    //{""},
                    //{""},
                    //{""},
                    //{""},
                    //{"U"},
                    //{"N",0},
                    //{"N",0}
                    //}
                    if (String.IsNullOrEmpty(value1))
                    {
                        DefaultParameters.Add(null);
                        continue;
                    }
                    if (value1 == "U")
                    {
                        DefaultParameters.Add("Неопределено");
                        continue;
                    }

                    object value2 = value[1].AsString();
                    if (value1 == "S")
                        DefaultParameters.Add("\"" + value2.AsString() + "\"");
                    else if (value1 == "N")
                        DefaultParameters.Add(value2.AsDouble().ToString(CultureInfo.InvariantCulture));
                    else if (value1 == "B")
                        DefaultParameters.Add(value2.AsBoolean() ? "Истина" : "Ложь");
                    else
                        throw new NotImplementedException();
                }
            }
            else if (sectionName == "Lbl")
            {
                //Vars section can be defined
                var requestLabels = from label in section.Skip(2)
                                    let list1 = label as Collection
                                    select new Label(list1[0].AsString(), list1[1].AsInt32());
                Labels = requestLabels.ToArray();
            }
            else
                throw new NotImplementedException();
        }
    }
}



