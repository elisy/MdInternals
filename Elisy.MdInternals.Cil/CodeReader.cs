using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;

using Elisy.MdInternals.Serialization;
using Elisy.MdInternals.DataElements;

namespace Elisy.MdInternals.Cil
{
    public class CodeReader
    {
        public Variable[] Variables { get; private set; }
        public OpCode[] OpCodes { get; private set; }
        public Constant[] Constants { get; private set; }
        public Method[] Methods { get; private set; }

        public bool DebugMode { get; private set; }

        public Collection Content { get; private set; }
        public CodeReader(string dataFlow, bool debubMode)
        {
            DebugMode = debubMode;
            DataSerializer reader = new DataSerializer();
            Content = reader.Deserialize(dataFlow);
        }

        public CodeReader(string dataFlow) : this(dataFlow, false)
        {
        }


        public string GetSourceCode()
        {
            var sourceCode = new SourceCode();

            var requestSections = from section in Content.AsParallel()
                                  where section is Collection
                                  select section as Collection;
            requestSections.ForAll(i => ResolveSection(i));
            FillAndResolveMethods();

            WriteSourceCode(sourceCode);

            return sourceCode.ToString();
        }

        private void WriteSourceCode(SourceCode sourceCode)
        {
            if (Methods == null)
                return;

            var requestMethods = from method in Methods
                                 where (method.Attributes & MethodAttributes.ExternalMethod) == 0
                                 orderby method.OpCodeFrom
                                 select method;
            var definedMethods = requestMethods.ToArray();
            foreach (var method in definedMethods)
            {
                sourceCode.GoToLine(method.OpCodes[0].Op1 - 1);
                WriteMethodHeader(sourceCode, method);
                sourceCode.Indent();

                Stack<string> stack = new Stack<string>();
                foreach (var opCode in method.OpCodes)
                {
                    if (DebugMode)
                    {
                        sourceCode.AppendLine(String.Format("//{0}: {1}, {2} {3}", opCode.Index, opCode.Code, opCode.Op1, opCode.Reserved == null ? "" : opCode.Reserved) + "   //stack: " + String.Join(", ", stack.ToArray()));
                    }

                    ProcessCodeLine(sourceCode, method, opCode, stack);
                    //_sourceCode.AppendLine(String.Format("{0}: {1}, {2}", opCode.Index, opCode.Code, opCode.Op1));
                }

                if (DebugMode)
                {
                    if (stack.Count != 0)
                        sourceCode.AppendLine("//error stack is not empty: " + String.Join(", ", stack.ToArray()));
                }

                sourceCode.Unindent();
                sourceCode.GoToLine(sourceCode.CurrentLineNunber + 1);
                WriteMethodFooter(sourceCode, method);
            }
        }

        private void ProcessCodeLine(SourceCode sourceCode, Method method, OpCode opCode, Stack<string> stack)
        {
            try
            {
                Label currentLabel = method.Labels.Where(m => m.Data1 == opCode.Index).FirstOrDefault();
                if (currentLabel != null)
                {
                    sourceCode.Unindent();
                    sourceCode.AppendLine(currentLabel.Name + ": ");
                }

                if (opCode.Code == CmdCode.LineNo)
                {
                    //sourceCode.AppendLine(String.Format("{0}: {1}, {2} {3}", opCode.Index, opCode.Code, opCode.Op1, opCode.Reserved == null ? "" : opCode.Reserved));
                    sourceCode.GoToLine(opCode.Op1);
                }
                else if (opCode.Code == CmdCode._IfSimple)
                {
                    sourceCode.AppendLine(String.Format("Если НЕ {0} тогда", stack.Pop()));
                    sourceCode.Indent();
                }
                else if (opCode.Code == CmdCode._IfSimpleEndIf)
                {
                    sourceCode.Unindent();
                    sourceCode.AppendLine("КонецЕсли;");
                }
                else if (opCode.Code == CmdCode._ContinueLabel)
                    ;
                else if (opCode.Code == CmdCode._Continue)
                {
                    sourceCode.AppendLine("Продолжить;");
                }
                else if (opCode.Code == CmdCode._BreakLabel)
                    ;
                else if (opCode.Code == CmdCode._Break)
                {
                    sourceCode.AppendLine("Прервать;");
                }
                else if (opCode.Code == CmdCode._While)
                {
                    sourceCode.AppendLine(String.Format("Пока {0} цикл", stack.Pop()));
                    sourceCode.Indent();
                }
                else if (opCode.Code == CmdCode._WhileEndDo)
                {
                    sourceCode.Unindent();
                    sourceCode.AppendLine("КонецЦикла;");
                }
                else if (opCode.Code == CmdCode._If)
                {
                    sourceCode.AppendLine(String.Format("Если {0} тогда", stack.Pop()));
                    sourceCode.Indent();
                }
                else if (opCode.Code == CmdCode._ElsIf)
                {
                    sourceCode.Unindent();
                    sourceCode.AppendLine(String.Format("ИначеЕсли {0} тогда", stack.Pop()));
                    sourceCode.Indent();
                }
                else if (opCode.Code == CmdCode._Else)
                {
                    sourceCode.Unindent();
                    sourceCode.AppendLine("Иначе");
                    sourceCode.Indent();
                }
                else if (opCode.Code == CmdCode._EndIf)
                {
                    sourceCode.Unindent();
                    sourceCode.AppendLine("КонецЕсли;");
                }
                else if (opCode.Code == CmdCode._For)
                {
                    sourceCode.AppendLine(String.Format("Для {2} = {1} по {0} Цикл", stack.Pop(), stack.Pop(), stack.Pop()));
                    sourceCode.Indent();
                }
                else if (opCode.Code == CmdCode._ForEndDo)
                {
                    sourceCode.Unindent();
                    sourceCode.AppendLine("КонецЦикла;");
                }
                else if (opCode.Code == CmdCode._ForEach)
                {
                    sourceCode.AppendLine(String.Format("Для каждого {0} из {1} Цикл", stack.Pop(), stack.Pop()));
                    sourceCode.Indent();
                }
                else if (opCode.Code == CmdCode._ForEachIn)
                {
                    //stack.Push(String.Format(" из {0} Цикл", stack.Pop()));
                }
                else if (opCode.Code == CmdCode._ForEachEndDo)
                {
                    sourceCode.Unindent();
                    sourceCode.AppendLine("КонецЦикла;");
                }
                else if (opCode.Code == CmdCode._Catch)
                {
                    sourceCode.Unindent();
                    sourceCode.AppendLine("Исключение");
                    sourceCode.Indent();
                }
                else if (opCode.Code == CmdCode._ReturnProc)
                    sourceCode.AppendLine("Возврат;");
                else if (opCode.Code == CmdCode._ReturnFunc)
                    sourceCode.AppendLine(string.Format("Возврат {0};", stack.Pop()));
                else if (opCode.Code == CmdCode._InlineIfThen)
                    stack.Push(String.Format("?({0}, {1})", stack.Pop(), opCode.Reserved));
                else if (opCode.Code == CmdCode._NOPInternal)
                    ;
                else if (opCode.Code == CmdCode.PushStatic)
                    stack.Push(Variables[opCode.Op1].Name);
                else if (opCode.Code == CmdCode.PushLocal)
                    stack.Push(method.Variables[opCode.Op1].Name);
                else if (opCode.Code == CmdCode.PushConst)
                    stack.Push(Constants[opCode.Op1].Value);
                else if (opCode.Code == CmdCode.PushReturn)
                    ;
                else if (opCode.Code == CmdCode.PushFalse)
                    stack.Push("Ложь");
                else if (opCode.Code == CmdCode.PushTrue)
                    stack.Push("Истина");
                else if (opCode.Code == CmdCode.PushUndefined)
                    stack.Push("Неопределено");
                else if (opCode.Code == CmdCode.PushNull)
                    stack.Push("NULL");
                else if (opCode.Code == CmdCode.PushEmpty)
                    stack.Push("");
                else if (opCode.Code == CmdCode.GetObjectProperty)
                    stack.Push(stack.Pop() + "." + Constants[opCode.Op1].Value.AsString());
                else if (opCode.Code == CmdCode.GetIndexed)
                    stack.Push(string.Format("{1}[{0}]", stack.Pop(), stack.Pop().AsString()));

                else if (opCode.Code == CmdCode.Assign)
                    sourceCode.AppendLine(string.Format("{1} = {0};", stack.Pop(), stack.Pop()));

                else if (opCode.Code == CmdCode.SetParamsCount)
                    stack.Push(opCode.Op1.ToString(CultureInfo.InvariantCulture));

                //else if (opCode.Code == CmdCode.AssignRetVal)
                // See _Return code
                //    _sourceCode.Append(string.Format("Возврат {0};", stack.Pop()));
                else if (opCode.Code == CmdCode.Call)
                {
                    var nextPushRetVal = method.GetOpCodeByIndex(opCode.Index + 1);
                    //if ((Methods[opCode.Op1].Attributes & MethodAttributes.Function) != 0)
                    if (nextPushRetVal.Code == CmdCode.PushReturn)
                        stack.Push(Methods[opCode.Op1].Name + "(" + GetParameters(stack, Int32.Parse(stack.Pop(), CultureInfo.InvariantCulture)) + ")");
                    else
                        //Call function as procedure
                        sourceCode.AppendLine(Methods[opCode.Op1].Name + "(" + GetParameters(stack, Int32.Parse(stack.Pop(), CultureInfo.InvariantCulture)) + ");");
                }
                else if (opCode.Code == CmdCode.CallObjectProcedure)
                    sourceCode.AppendLine(string.Format("{1}.{2}({0});", GetParameters(stack, Int32.Parse(stack.Pop(), CultureInfo.InvariantCulture)), stack.Pop(), Constants[opCode.Op1].Value.AsString()));
                else if (opCode.Code == CmdCode.CallObjectFunction)
                    stack.Push(string.Format("{1}.{2}({0})", GetParameters(stack, Int32.Parse(stack.Pop(), CultureInfo.InvariantCulture)), stack.Pop(), Constants[opCode.Op1].Value.AsString()));

                else if (opCode.Code == CmdCode.Ret)
                {
                    //Can be the last 2 codes Ret, Ret
                    //if (opCode.Index != method.OpCodes[method.OpCodes.Length - 1].Index)
                    //    throw new NotImplementedException();
                }

                else if (opCode.Code == CmdCode.Neg)
                    stack.Push(string.Format("-{0}", stack.Pop()));
                else if (opCode.Code == CmdCode.Add)
                    stack.Push(string.Format("{1} + {0}", stack.Pop(), stack.Pop()));
                else if (opCode.Code == CmdCode.Sub)
                    stack.Push(string.Format("{1} - {0}", stack.Pop(), stack.Pop()));

                else if (opCode.Code == CmdCode.Mul)
                    stack.Push(string.Format("{1} * {0}", stack.Pop(), stack.Pop()));
                else if (opCode.Code == CmdCode.Div)
                    stack.Push(string.Format("{1} / {0}", stack.Pop(), stack.Pop()));
                else if (opCode.Code == CmdCode.Mod)
                    stack.Push(string.Format("{1} % {0}", stack.Pop(), stack.Pop()));

                else if (opCode.Code == CmdCode.Not)
                    stack.Push(string.Format("НЕ {0}", stack.Pop()));
                else if (opCode.Code == CmdCode.AndJmp)
                {
                    if (method.GetOpCodeByIndex(opCode.Index - 1).Code == CmdCode.Boolean)
                        stack.Push(string.Format("{1}{0} И ", stack.Pop(), stack.Pop()));
                    else
                        stack.Push(string.Format("({0} И ", stack.Pop()));
                }
                else if (opCode.Code == CmdCode.OrJmp)
                {
                    if (method.GetOpCodeByIndex(opCode.Index - 1).Code == CmdCode.Boolean)
                        stack.Push(string.Format("{1}{0} ИЛИ ", stack.Pop(), stack.Pop()));
                    else
                        stack.Push(string.Format("({0} ИЛИ ", stack.Pop()));
                }

                else if (opCode.Code == CmdCode.EQ)
                    stack.Push(string.Format("{1} = {0}", stack.Pop(), stack.Pop()));
                else if (opCode.Code == CmdCode.NE)
                    stack.Push(string.Format("{1} <> {0}", stack.Pop(), stack.Pop()));
                else if (opCode.Code == CmdCode.GT)
                    stack.Push(string.Format("{1} > {0}", stack.Pop(), stack.Pop()));
                else if (opCode.Code == CmdCode.LT)
                    stack.Push(string.Format("{1} < {0}", stack.Pop(), stack.Pop()));
                else if (opCode.Code == CmdCode.GE)
                    stack.Push(string.Format("{1} >= {0}", stack.Pop(), stack.Pop()));
                else if (opCode.Code == CmdCode.LE)
                    stack.Push(string.Format("{1} <= {0}", stack.Pop(), stack.Pop()));

                else if (opCode.Code == CmdCode.JmpLbl)
                    sourceCode.AppendLine(string.Format("Перейти {0};{1}", method.Labels[opCode.Op1].Name, DebugMode ? " //" + method.Labels[opCode.Op1].Data1 : ""));
                    
                else if (opCode.Code == CmdCode.PushTry)
                {
                    sourceCode.AppendLine("Попытка");
                    sourceCode.Indent();
                }
                else if (opCode.Code == CmdCode.EndTry)
                {
                    sourceCode.Unindent();
                    sourceCode.AppendLine("КонецПопытки;");
                }
                else if (opCode.Code == CmdCode.Raise)
                {
                    if (opCode.Op1 != 0)
                        sourceCode.AppendLine(string.Format("ВызватьИсключение({0});", stack.Pop()));
                    else
                        sourceCode.AppendLine("ВызватьИсключение;");
                }

                else if (opCode.Code == CmdCode.PopFor)
                {
                    var opCodeJmpLbl = method.FindNextOpCode(opCode.Index, new CmdCode[] { CmdCode.JmpLbl }, new CmdCode[] { CmdCode.PopFor });
                    if (opCodeJmpLbl == null)
                        throw new NotImplementedException();
                }

                else if (opCode.Code == CmdCode.New)
                    stack.Push("Новый " + Constants[opCode.Op1].Value.AsString() + "(" + GetParameters(stack, Int32.Parse(stack.Pop(), CultureInfo.InvariantCulture)) + ")");

                else if (opCode.Code == CmdCode.Execute)
                    sourceCode.AppendLine(string.Format("Выполнить({0});", stack.Pop()));
                else if (opCode.Code == CmdCode.StrLen)
                    stack.Push(string.Format("СтрДлина({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.TrimL)
                    stack.Push(string.Format("СокрЛ({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.TrimR)
                    stack.Push(string.Format("СокрП({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.TrimAll)
                    stack.Push(string.Format("СокрЛП({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.Left)
                    stack.Push(string.Format("Лев({1}, {0})", stack.Pop(), stack.Pop()));
                else if (opCode.Code == CmdCode.Right)
                    stack.Push(string.Format("Прав({1}, {0})", stack.Pop(), stack.Pop()));
                else if (opCode.Code == CmdCode.Mid)
                    stack.Push(string.Format("Сред({2}, {1}, {0})", stack.Pop(), stack.Pop(), stack.Pop()));
                else if (opCode.Code == CmdCode.Find)
                    stack.Push(string.Format("Найти({1}, {0})", stack.Pop(), stack.Pop()));
                else if (opCode.Code == CmdCode.Upper)
                    stack.Push(string.Format("ВРег({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.Lower)
                    stack.Push(string.Format("НРег({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.Char)
                    stack.Push(string.Format("Символ({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.CharCode)
                    stack.Push(string.Format("КодСимвола({1}, {0})", stack.Pop(), stack.Pop()));
                else if (opCode.Code == CmdCode.IsBlankStr)
                    stack.Push(string.Format("ПустаяСтрока({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.Int)
                    stack.Push(string.Format("Цел({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.RoundDefault)
                    stack.Push(string.Format("Окр({1}, {0})", stack.Pop(), stack.Pop()));
                else if (opCode.Code == CmdCode.Round)
                    stack.Push(string.Format("Окр({2}, {1}, {0})", stack.Pop(), stack.Pop(), stack.Pop()));

                else if (opCode.Code == CmdCode.Boolean)
                {
                    var request = from orand in method.OpCodes
                                  where orand.Code == CmdCode.OrJmp || orand.Code == CmdCode.AndJmp
                                  let b = method.GetOpCodeByIndex(orand.Op1 - 1)
                                  where b.Index == opCode.Index
                                  select orand;

                    if (request.Count() == 0)
                        stack.Push(string.Format("Булево({0})", stack.Pop()));
                    else
                    {
                        if (method.GetOpCodeByIndex(opCode.Index + 1).Code == CmdCode.OrJmp || method.GetOpCodeByIndex(opCode.Index + 1).Code == CmdCode.AndJmp)
                            ;
                        else
                            stack.Push(string.Format("{1}{0})", stack.Pop(), stack.Pop()));
                    }
                }
                else if (opCode.Code == CmdCode.Numeric)
                    stack.Push(string.Format("Число({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.String)
                    stack.Push(string.Format("Строка({0})", stack.Pop()));

                else if (opCode.Code == CmdCode.Date)
                    stack.Push(string.Format("Дата({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.Date3)
                    stack.Push(string.Format("Дата({2}, {1}, {0})", stack.Pop(), stack.Pop(), stack.Pop()));
                else if (opCode.Code == CmdCode.Date3)
                    stack.Push(string.Format("Дата({5}, {4}, {3}, {2}, {1}, {0})", stack.Pop(), stack.Pop(), stack.Pop(), stack.Pop(), stack.Pop(), stack.Pop()));
                else if (opCode.Code == CmdCode.AddMonth)
                    stack.Push(string.Format("ДобавитьМесяц({1}, {0})", stack.Pop(), stack.Pop()));
                else if (opCode.Code == CmdCode.BegOfMonth)
                    stack.Push(string.Format("НачалоМесяца({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.EndOfMonth)
                    stack.Push(string.Format("КонецМесяца({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.BegOfQuart)
                    stack.Push(string.Format("НачалоКвартала({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.EndOfQuart)
                    stack.Push(string.Format("КонецКвартала({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.BegOfYear)
                    stack.Push(string.Format("НачалоГода({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.EndOfYear)
                    stack.Push(string.Format("КонецГода({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.Year)
                    stack.Push(string.Format("Год({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.Month)
                    stack.Push(string.Format("Месяц({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.Day)
                    stack.Push(string.Format("День({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.Hour)
                    stack.Push(string.Format("Час({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.Minute)
                    stack.Push(string.Format("Минута({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.Second)
                    stack.Push(string.Format("Секунда({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.DayOfYear)
                    stack.Push(string.Format("ДеньГода({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.WeekOfYear)
                    stack.Push(string.Format("НеделяГода({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.WeekDay)
                    stack.Push(string.Format("ДеньНедели({0})", stack.Pop()));

                else if (opCode.Code == CmdCode.BegOfDay)
                    stack.Push(string.Format("НачалоДня({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.EndOfDay)
                    stack.Push(string.Format("КонецДня({0})", stack.Pop()));

                else if (opCode.Code == CmdCode.CurDate)
                    stack.Push("ТекущаяДата()");

                else if (opCode.Code == CmdCode.StrReplace)
                    stack.Push(string.Format("СтрЗаменить({2}, {1}, {0})", stack.Pop(), stack.Pop(), stack.Pop()));
                else if (opCode.Code == CmdCode.LinesCount)
                    stack.Push(string.Format("СтрЧислоСтрок({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.LineGet)
                    stack.Push(string.Format("СтрПолучитьСтроку({1}, {0})", stack.Pop(), stack.Pop()));

                else if (opCode.Code == CmdCode.Min)
                    stack.Push("Мин(" + GetParameters(stack, Int32.Parse(stack.Pop(), CultureInfo.InvariantCulture)) + ")");
                else if (opCode.Code == CmdCode.Max)
                    stack.Push("Макс(" + GetParameters(stack, Int32.Parse(stack.Pop(), CultureInfo.InvariantCulture)) + ")");
                else if (opCode.Code == CmdCode.StrCountOccur)
                    stack.Push(string.Format("СтрЧислоВхождений({1}, {0})", stack.Pop(), stack.Pop()));
                else if (opCode.Code == CmdCode.ErrorDescr)
                    stack.Push("ОписаниеОшибки()");
                else if (opCode.Code == CmdCode.TypeOf)
                    stack.Push(string.Format("ТипЗнч({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.Type)
                    stack.Push(string.Format("Тип({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.Eval)
                    stack.Push(string.Format("Вычислить({0})", stack.Pop()));
                else if (opCode.Code == CmdCode.Format)
                    stack.Push(string.Format("Формат({1}, {0})", stack.Pop(), stack.Pop()));
                else if (opCode.Code == CmdCode.NewObject)
                    stack.Push(string.Format("Новый {1}({0})", stack.Pop(), stack.Pop().AsString()));

                else if (opCode.Code == CmdCode.Pow)
                    stack.Push(string.Format("Pow({1}, {0})", stack.Pop(), stack.Pop()));
                else if (opCode.Code == CmdCode.Sin)
                    stack.Push(string.Format("Sin({0})", stack.Pop()));

                else if (opCode.Code == CmdCode.ErrorInfo)
                    stack.Push("ИнформацияОбОшибке()");
                else
                {
                    if (!sourceCode.IsNewLine)
                        sourceCode.AppendLine();
                    sourceCode.Append(String.Format("//unknown {0}: {1}, {2} {3}", opCode.Index, opCode.Code, opCode.Op1, opCode.Reserved == null ? "" : opCode.Reserved));
                    sourceCode.AppendLine("      //stack: " + String.Join(", ", stack.ToArray()));
                }


                if (currentLabel != null)
                    sourceCode.Indent();
            }
            catch (Exception ex)
            {
                sourceCode.AppendLine(String.Format("//error {0}: {1}, {2} {3}", opCode.Index, opCode.Code, opCode.Op1, opCode.Reserved == null ? "" : opCode.Reserved));
                sourceCode.AppendLine("" + ex.ToString());
                sourceCode.AppendLine("//stack: " + String.Join(", ", stack.ToArray()));
            }
        }

        private string GetParameters(Stack<string> stack, int parametersCount)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < parametersCount; i++)
                result.Add(stack.Pop());
            return String.Join(", ", result);

        }

        private string GetDebugString(Method method, List<OpCode> lineCodes, Stack<string> stack)
        {
            var request = from opCode in lineCodes
                          select String.Format("{0}:{1}:{2}", opCode.Code, opCode.Op1, opCode.Index);
            return String.Join(", ", request.ToArray()) + " stack: " + String.Join(", ", stack.ToArray());
        }

        private void WriteMethodHeader(SourceCode sourceCode, Method method)
        {
            string[] parameters = new string[0];
            if (method.Variables != null)
            {
                var requestParameters = from variable in method.Variables
                                        where (variable.Attributes & VariableAttributes.Parameter) != 0
                                        select String.Format("{0}{1}", (variable.Attributes & VariableAttributes.ByValue)!=0?"Знач ":"", variable.Name);
                parameters = requestParameters.ToArray();
                if (method.DefaultParameters != null)
                {
                    for (int i = 0; i < method.DefaultParameters.Count; i++)
                    {
                        string defaultValue = method.DefaultParameters[i];
                        if (String.IsNullOrEmpty(defaultValue))
                            continue;
                        parameters[i] = parameters[i] + " = " + defaultValue;
                    }
                }
            }

            string parametersString = String.Join(", ", parameters);

            string header = String.Format("{0} {1}({2}) {3}", 
                (method.Attributes & MethodAttributes.Function) != 0 ? "Функция" : "Процедура", 
                method.Name,
                parametersString, 
                (method.Attributes & MethodAttributes.Export) != 0 ? "Экспорт" : "");
            //_sourceCode.AppendLine(header);
            //_currentLineNunber++;
            sourceCode.AppendLine(header);
        }

        private void WriteMethodFooter(SourceCode sourceCode, Method method)
        {
            string header = String.Format("{0} //{1}()", (method.Attributes & MethodAttributes.Function) != 0 ? "КонецФункции" : "КонецПроцедуры", method.Name);
            sourceCode.AppendLine(header);
            //sourceCode.CurrentLineNunber++;
        }

        private void ResolveSection(Collection section)
        {
            string sectionName = section[0].AsString();
            int length = section[1].AsInt32();

            if (sectionName == "Var")
            {
                var request = from var in section.Skip(2)
                              let list = var as Collection
                              select new Variable(list[0].AsString(), (VariableAttributes)list[1].AsInt32(), list[2].AsInt32());
                Variables = request.ToArray();
            }
            else if (sectionName == "Cmd")
            {
                var request = from opCode in section.Skip(3)
                              let list = opCode as Collection
                              select new OpCode((CmdCode)Convert.ToInt32(list[0]), Convert.ToInt32(list[1]));
                OpCodes = request.ToArray();
                for (int i = 0; i < OpCodes.Length; i++)
                    OpCodes[i].Index = i;
            }
            else if (sectionName == "Const")
            {
                var request = from constant in section.Skip(2)
                              let list = constant as Collection
                              select new Constant(list[0].AsString(), list[1].ToString());
                Constants = request.ToArray();
            }
            else if (sectionName == "Proc")
            {
                var request = from procedure in section.Skip(2)
                              let list = procedure as Collection
                              select new Method(list);
                Methods = request.ToArray();
            }
            else
                return;
        }

        private void FillAndResolveMethods()
        {
            if (Methods == null)
                return;

            var requestMethods = from method in Methods
                                 where (method.Attributes & MethodAttributes.ExternalMethod) == 0
                                 orderby method.OpCodeFrom
                                 select method;
            var definedMethods = requestMethods.ToArray();

            for (int i = 0; i < definedMethods.Length; i++)
            {
                int opCodesFrom = definedMethods[i].OpCodeFrom;
                definedMethods[i].OpCodeTo = 0;
                if (i < definedMethods.Length - 1)
                    definedMethods[i].OpCodeTo = definedMethods[i + 1].OpCodeFrom - 1;
                else
                    definedMethods[i].OpCodeTo = OpCodes.Length - 1;

                definedMethods[i].OpCodes = OpCodes.Skip(opCodesFrom).Take(definedMethods[i].OpCodeTo - opCodesFrom + 1).ToArray();
                //Enable obfuscation resolving
                definedMethods[i].ModuleOpCodes = OpCodes;

                ResolveInlineIf(definedMethods[i]);
                ResolveReturnFunc(definedMethods[i]);
                ResolveReturnProc(definedMethods[i]);
                ResolveCatch(definedMethods[i]);

                ResolveForEach(definedMethods[i]);
                ResolveFor(definedMethods[i]);
                ResolveWhileDo(definedMethods[i]);
                ResolveBreakContinue(definedMethods[i]);

                ResolveIfThen(definedMethods[i]);
                ResolveIfNot(definedMethods[i]);
                ResolveJmpInIfThen(definedMethods[i]);
            }
        }

        private void ResolveInlineIf(Method method)
        {
            //Enable inline ifthenelse nesting using orderby desc
            var opCodesRequest = from opCodeIf in method.OpCodes
                                 where (opCodeIf.Code == CmdCode.JZ) && (method.GetOpCodeByIndex(opCodeIf.Op1).Code != CmdCode.LineNo)
                                 let opCodeElse = method.GetOpCodeByIndex(opCodeIf.Op1 - 1)
                                 where opCodeElse.Code == CmdCode.Jmp
                                 where (opCodeIf.Op1 < opCodeElse.Op1) && (opCodeIf.Index < opCodeElse.Index)
                                 orderby opCodeIf.Index descending
                                 select new { OpCodeIf = opCodeIf, OpCodeElse = opCodeElse };
            var opCodes = opCodesRequest.ToArray();

            foreach (var clause in opCodes)
            {
                try
                {
                    var opCodeIf = clause.OpCodeIf;
                    var opCodeElse = clause.OpCodeElse;

                    Stack<string> stackIf = new Stack<string>();
                    SourceCode sourceCode = new SourceCode();
                    for (int i = (int)(opCodeIf.Index + 1); i < opCodeElse.Index; i++)
                    {
                        var currentOpCode = method.GetOpCodeByIndex(i);
                        ProcessCodeLine(sourceCode, method, currentOpCode, stackIf);
                    }

                    Stack<string> stackElse = new Stack<string>();
                    for (int i = (int)(opCodeElse.Index + 1); i < opCodeElse.Op1; i++)
                    {
                        var currentOpCode = method.GetOpCodeByIndex(i);
                        ProcessCodeLine(sourceCode, method, currentOpCode, stackElse);
                    }

                    if (stackIf.Count == 0 || stackElse.Count == 0)
                        continue;

                    opCodeIf.Code = CmdCode._InlineIfThen;
                    opCodeIf.Op1 = 0;
                    opCodeIf.Reserved = string.Format("{0}, {1}", stackIf.Pop(), stackElse.Pop());

                    var from = (int)(opCodeIf.Index + 1);
                    var to = opCodeElse.Op1;
                    for (int i = from; i < to; i++)
                    {
                        var code = method.GetOpCodeByIndex(i);
                        code.Code = CmdCode._NOPInternal;
                        code.Op1 = 0;
                        code.Reserved = "Cleared by ResolveInlineIf";
                    }
                }
                catch(Exception)
                {
                }
            }


        }

        private void ResolveReturnFunc(Method method)
        {
            var opCodesRequest = from opCodeRet in method.OpCodes
                                 where opCodeRet.Code == CmdCode.AssignReturnValue
                                 let opCodePopTry = method.GetOpCodeByIndex(opCodeRet.Index + 1)
                                 let opCodeJmp = method.GetOpCodeByIndex(opCodeRet.Index + 2)
                                 where (opCodePopTry != null) && (opCodeJmp != null)
                                 where (opCodePopTry.Code == CmdCode.PopTry) && (opCodeJmp.Code == CmdCode.Jmp)
                                 select new { O1 = opCodeRet, O2 = opCodePopTry, O3 = opCodeJmp };
            var opCodes = opCodesRequest.ToArray();

            foreach (var opCode in opCodes)
            {
                opCode.O1.Code = CmdCode._ReturnFunc;
                opCode.O2.Code = CmdCode._NOPInternal;
                opCode.O2.Reserved = "Cleared by _Return";
                opCode.O3.Code = CmdCode._NOPInternal;
                opCode.O3.Reserved = "Cleared by _Return";
            }
        }

        private void ResolveReturnProc(Method method)
        {
            var opCodesRequest = from opCodePopTry in method.OpCodes
                                 where opCodePopTry.Code == CmdCode.PopTry
                                 let opCodeJmp = method.GetOpCodeByIndex(opCodePopTry.Index + 1)
                                 where (opCodeJmp != null) && (opCodeJmp.Code == CmdCode.Jmp)
                                 let opCodeRet = method.GetOpCodeByIndex(opCodeJmp.Op1 + 1)
                                 where (opCodeRet != null) && (opCodeRet.Code == CmdCode.Ret)
                                 select new { O1 = opCodePopTry, O2 = opCodeJmp };
            var opCodes = opCodesRequest.ToArray();

            foreach (var opCode in opCodes)
            {
                opCode.O1.Code = CmdCode._ReturnProc;
                opCode.O2.Code = CmdCode._NOPInternal;
                opCode.O2.Reserved = "Cleared by _ReturnProc";
            }
        }

        private void ResolveCatch(Method method)
        {
            var opCodesRequest = from opCodePopTry in method.OpCodes
                                 where opCodePopTry.Code == CmdCode.PopTry
                                 let opCodeJmp = method.GetOpCodeByIndex(opCodePopTry.Index + 1)
                                 where (opCodeJmp != null) && (opCodeJmp.Code == CmdCode.Jmp)
                                 select new { O1 = opCodePopTry, O2 = opCodeJmp };
            var opCodes = opCodesRequest.ToArray();

            foreach (var opCode in opCodes)
            {
                opCode.O1.Code = CmdCode._Catch;
                opCode.O2.Code = CmdCode._NOPInternal;
                opCode.O2.Reserved = "Cleared by _Catch";
            }
        }

        private void ResolveForEach(Method method)
        {
            //Enable nesting using orderby desc
            var opCodesRequest = from opCodeSelEnum in method.OpCodes
                                 where opCodeSelEnum.Code == CmdCode.SelEnum
                                 let opCodeAssign = method.GetOpCodeByIndex(opCodeSelEnum.Index + 1)
                                 where (opCodeAssign != null) && (opCodeAssign.Code == CmdCode.Assign)
                                 orderby opCodeSelEnum.Index descending
                                 select new { O1 = opCodeSelEnum, O2 = opCodeAssign };
            var opCodes = opCodesRequest.ToArray();

            foreach (var opCode in opCodes)
            {
                var selEnum = opCode.O1;

                //Find PushFor, PushForVar before
                var pushForVar = selEnum;
                var pushFor = selEnum;
                do
                {
                    pushForVar = method.GetOpCodeByIndex(pushForVar.Index - 1);
                    pushFor = method.GetOpCodeByIndex(pushForVar.Index - 1);
                }
                while (!(pushForVar == null || (pushForVar.Code == CmdCode.PushForVar && pushFor.Code == CmdCode.PushFor)));

                //Find Assign after
                var assign = opCode.O2;
                do
                {
                    assign = method.GetOpCodeByIndex(assign.Index + 1);
                }
                while (!(assign == null || assign.Code == CmdCode.Assign));

                //Find EnumNext, JZ after
                var enumNext = assign;
                var jz = assign;
                do
                {
                    enumNext = method.GetOpCodeByIndex(enumNext.Index + 1);
                    jz = method.GetOpCodeByIndex(enumNext.Index + 1);
                }
                while (!(enumNext == null || (enumNext.Code == CmdCode.NextEnum && jz.Code == CmdCode.JZ)));

                var assign1 = jz;
                do
                {
                    assign1 = method.GetOpCodeByIndex(assign1.Index + 1);
                }
                while (!(assign1 == null || assign1.Code == CmdCode.Assign));
                var assign2 = assign1;
                do
                {
                    assign2 = method.GetOpCodeByIndex(assign2.Index + 1);
                }
                while (!(assign2 == null || assign2.Code == CmdCode.Assign));

                var popFor = method.GetOpCodeByIndex(jz.Op1);
                var jmp = method.GetOpCodeByIndex(jz.Op1 - 1);

                var assign3 = popFor;
                do
                {
                    assign3 = method.GetOpCodeByIndex(assign3.Index + 1);
                }
                while (!(assign3 == null || assign3.Code == CmdCode.Assign));
                var assign4 = assign3;
                do
                {
                    assign4 = method.GetOpCodeByIndex(assign4.Index + 1);
                }
                while (!(assign4 == null || assign4.Code == CmdCode.Assign));

                if (new OpCode[] { pushForVar, pushFor, assign, enumNext, jz, popFor, jmp }.Contains(null))
                    continue;

                pushFor.Code = CmdCode._NOPInternal;
                pushFor.Reserved = "Cleared by ForEach";
                pushForVar.Code = CmdCode._NOPInternal;
                pushForVar.Reserved = "Cleared by ForEach";

                selEnum.Code = CmdCode._ForEachIn;
                int from = (int)selEnum.Index + 1;
                int to = (int)assign.Index;
                for (int i = from; i <= to; i++)
                {
                    var currentCode = method.GetOpCodeByIndex(i);
                    currentCode.Code = CmdCode._NOPInternal;
                    currentCode.Reserved = "Cleared by ForEach";
                }

                //Change variable name from "0Колонка" to "Колонка"
                var pushToChange = method.GetOpCodeByIndex(enumNext.Index - 1);
                pushToChange.Op1 = method.GetOpCodeByIndex(jz.Index + 1).Op1;
                pushToChange.Reserved = "Changed variable by ForEach";

                enumNext.Code = CmdCode._ForEach;
                jz.Code = CmdCode._NOPInternal;
                jz.Reserved = "Cleared by ForEach";

                //Clear 2 assigns after
                //Колонка = 0Колонка;
                //0Колонка = Неопределено;
                from = (int)jz.Index + 1;
                to = (int)assign2.Index;
                for (int i = from; i <= to; i++)
                {
                    var currentCode = method.GetOpCodeByIndex(i);
                    currentCode.Code = CmdCode._NOPInternal;
                    currentCode.Reserved = "Cleared by ForEach";
                }

                jmp.Code = CmdCode._ForEachEndDo;
                //popFor.Code = CmdCode._NOPInternal;
                popFor.Code = CmdCode._BreakLabel;
                popFor.Reserved = "Cleared by ForEach";

                //Clear 2 assigns after
                //0ЗначениеСтроки = Неопределено;
                //0ЗначениеСтроки = Неопределено;
                from = (int)popFor.Index + 1;
                to = (int)assign4.Index;
                for (int i = from; i <= to; i++)
                {
                    var currentCode = method.GetOpCodeByIndex(i);
                    currentCode.Code = CmdCode._NOPInternal;
                    currentCode.Reserved = "Cleared by ForEach";
                }

            }
        }


        private void ResolveFor(Method method)
        {
            //Enable nesting using orderby desc
            var opCodesRequest = from opCodeIncr in method.OpCodes
                                 where opCodeIncr.Code == CmdCode.Inc
                                 let opCodeJmp = method.GetOpCodeByIndex(opCodeIncr.Index + 1)
                                 where (opCodeJmp != null) && (opCodeJmp.Code == CmdCode.Jmp)
                                 let opCodePopFor = method.GetOpCodeByIndex(opCodeIncr.Index + 2)
                                 where (opCodePopFor != null) && (opCodePopFor.Code == CmdCode.PopFor)
                                 orderby opCodeJmp.Op1 descending
                                 select new { O1 = opCodeIncr, O2 = opCodeJmp, O3 = opCodePopFor };
            var opCodes = opCodesRequest.ToArray();

            foreach (var opCode in opCodes)
            {
                var assign2 = method.GetOpCodeByIndex(opCode.O2.Op1 - 1);
                if (assign2.Code != CmdCode.Assign)
                    throw new NotImplementedException();

                var pushForVar = assign2;
                do
                {
                    pushForVar = method.GetOpCodeByIndex(pushForVar.Index - 1);
                }
                while (!(pushForVar == null || pushForVar.Code == CmdCode.PushForVar));

                var assign1 = pushForVar;
                do
                {
                    assign1 = method.GetOpCodeByIndex(assign1.Index - 1);
                }
                while (!(assign1 == null || assign1.Code == CmdCode.Assign));

                var jz = method.GetOpCodeByIndex(opCode.O2.Op1);
                do
                {
                    jz = method.GetOpCodeByIndex(jz.Index + 1);
                }
                while (!(jz == null || jz.Code == CmdCode.JZ));

                if (new OpCode[] { assign1, pushForVar, assign2, jz }.Contains(null))
                    continue;

                int from = (int)assign1.Index;
                int to = (int)pushForVar.Index;
                for (int i = from; i <= to; i++)
                {
                    var currentCode = method.GetOpCodeByIndex(i);
                    currentCode.Code = CmdCode._NOPInternal;
                    currentCode.Reserved = "Cleared by For";
                }

                assign2.Code = CmdCode._For;
                from = (int)assign2.Index + 1;
                to = (int)jz.Index;
                for (int i = from; i <= to; i++)
                {
                    var currentCode = method.GetOpCodeByIndex(i);
                    currentCode.Code = CmdCode._NOPInternal;
                    currentCode.Reserved = "Cleared by For";
                }

                opCode.O1.Code = CmdCode._ForEndDo;

                opCode.O2.Code = CmdCode._NOPInternal;
                opCode.O2.Reserved = "Cleared by For";

                opCode.O3.Code = CmdCode._BreakLabel;
                opCode.O3.Reserved = "Cleared by For";

                var pushLocalBeforIncr = method.GetOpCodeByIndex(opCode.O1.Index - 1);
                pushLocalBeforIncr.Code = CmdCode._ContinueLabel;
                pushLocalBeforIncr.Reserved = "Cleared by For";
            }
        }

        private void ResolveWhileDo(Method method)
        {
            var opCodesRequest = from opCodeJz in method.OpCodes
                                 where opCodeJz.Code == CmdCode.JZ
                                 let opCodeJmp = method.GetOpCodeByIndex(opCodeJz.Op1 - 1)
                                 where (opCodeJmp != null) && (opCodeJmp.Code == CmdCode.Jmp) && (opCodeJmp.Op1 < opCodeJz.Index)
                                 select new { O1 = opCodeJz, O2 = opCodeJmp };
            var opCodes = opCodesRequest.ToArray();

            foreach (var opCode in opCodes)
            {
                opCode.O1.Code = CmdCode._While;
                opCode.O2.Code = CmdCode._WhileEndDo;
            }
        }

        private void ResolveIfThen(Method method)
        {
            var opCodesRequest = from opCodeJz in method.OpCodes
                                 where opCodeJz.Code == CmdCode.JZ
                                 let opCodeJmp = method.GetOpCodeByIndex(opCodeJz.Op1 - 1)
                                 where (opCodeJmp != null) && (opCodeJmp.Code == CmdCode.Jmp)
                                 select new { O1 = opCodeJz, O2 = opCodeJmp };
            var opCodes = opCodesRequest.ToArray();

            foreach (var opCode in opCodes)
            {
                var sameCluaseRequest = from op in opCodes
                                        where op.O2.Op1 == opCode.O2.Op1
                                        orderby op.O1.Index
                                        select op.O1;
                var sameClause = sameCluaseRequest.ToArray();

                if (opCode.O1.Index == sameClause.First().Index)
                    opCode.O1.Code = CmdCode._If;
                else
                {
                    opCode.O1.Code = CmdCode._ElsIf;
                    opCode.O1.Reserved = sameClause.First().Index;
                }

                if (opCode.O1.Index == sameClause.Last().Index)
                {
                    if (opCode.O2.Op1 == opCode.O2.Index + 1)
                    {
                        //If-EndIf clause
                        opCode.O2.Code = CmdCode._EndIf;
                        opCode.O2.Reserved = sameClause.First().Index;
                    }
                    else
                    {
                        var endIf = method.GetOpCodeByIndex(opCode.O2.Op1);
                        if (endIf.Code != CmdCode.LineNo)
                            throw new NotImplementedException();
                        endIf.Code = CmdCode._EndIf;
                        endIf.Reserved = sameClause.First().Index;

                        opCode.O2.Code = CmdCode._Else;
                        opCode.O2.Reserved = sameClause.First().Index;
                    }
                }
            }
        }

        private void ResolveBreakContinue(Method method)
        {
            var break1 = from opCodeJmp in method.OpCodes
                                 where opCodeJmp.Code == CmdCode.Jmp
                                 let opBreakLabel = method.GetOpCodeByIndex(opCodeJmp.Op1)
                                 where (opBreakLabel != null) && (opBreakLabel.Code == CmdCode._BreakLabel)
                                 select opCodeJmp;
            var break2 = from opCodeJmp in method.OpCodes
                         where opCodeJmp.Code == CmdCode.Jmp
                         let opBreakLabel = method.GetOpCodeByIndex(opCodeJmp.Op1 - 1)
                         where (opBreakLabel != null) && (opBreakLabel.Code == CmdCode._WhileEndDo)
                         select opCodeJmp;

            var breakCodes = break1.Union(break2).ToArray();

            foreach (var opCode in breakCodes)
            {
                opCode.Code = CmdCode._Break;
            }

            var continue1 = from opCodeJmp in method.OpCodes
                               where opCodeJmp.Code == CmdCode.Jmp
                               let opBreakLabel = method.GetOpCodeByIndex(opCodeJmp.Op1)
                               where (opBreakLabel != null) && (opBreakLabel.Code == CmdCode._ForEachEndDo)
                               select opCodeJmp;
            var continue2 = from opCodeJmp in method.OpCodes
                            where opCodeJmp.Code == CmdCode.Jmp
                            let opBreakLabel = method.GetOpCodeByIndex(opCodeJmp.Op1)
                            where (opBreakLabel != null) && (opBreakLabel.Code == CmdCode._ContinueLabel)
                            select opCodeJmp;
            var continueCodes = continue1.Union(continue2).ToArray();
            foreach (var opCode in continueCodes)
            {
                opCode.Code = CmdCode._Continue;
            }

            //Continue before While loops
            var possibleWhileContinues = method.OpCodes.Where(m => m.Code == CmdCode.Jmp && m.Op1 < m.Index).ToArray();
            foreach (var opCode in possibleWhileContinues)
            {
                var whileCode = method.GetOpCodeByIndex(opCode.Op1);
                Stack<string> stack = new Stack<string>();
                SourceCode sourceCode = new SourceCode();
                do
                {
                    whileCode = method.GetOpCodeByIndex(whileCode.Index + 1);
                    ProcessCodeLine(sourceCode, method, whileCode, stack);
                    if (stack.Count == 0)
                        break;
                }
                while (whileCode != null);

                if (whileCode.Code == CmdCode._While)
                    opCode.Code = CmdCode._Continue;
            }
        }

        private void ResolveJmpInIfThen(Method method)
        {
            var opCodesIfEndIf = from opCodeEndIf in method.OpCodes
                                 where opCodeEndIf.Code == CmdCode._EndIf
                                 let opCodeIf = method.GetOpCodeByIndex((double)opCodeEndIf.Reserved)
                                 where opCodeIf != null && opCodeIf.Code == CmdCode._If
                                 select new { O1 = opCodeIf, O2 = opCodeEndIf };

            var opCodes = opCodesIfEndIf.ToArray();

            foreach (var opCode in opCodes)
            {
                for (int i = (int)opCode.O1.Index + 1; i < opCode.O2.Index; i++)
                {
                    var jmp = method.GetOpCodeByIndex(i);
                    if (jmp == null || jmp.Code != CmdCode.Jmp)
                        continue;
                    if (jmp.Op1 - 1 == opCode.O2.Index //if else
                        || jmp.Op1 == opCode.O2.Index) //if elsif
                    {
                        jmp.Code = CmdCode._NOPInternal;
                        jmp.Reserved = "Cleared by ResolveJmpInIfThen";
                    }
                }
            }
        }

        private void ResolveIfNot(Method method)
        {
            var opCodesJZ = from opCodeJZ in method.OpCodes
                             where opCodeJZ.Code == CmdCode.JZ
                             let opCodeLine = method.GetOpCodeByIndex((double)opCodeJZ.Op1)
                             where opCodeLine != null && opCodeLine.Code == CmdCode.LineNo
                             select new { O1 = opCodeJZ, O2 = opCodeLine };

            var opCodes = opCodesJZ.ToArray();

            foreach (var opCode in opCodes)
            {
                opCode.O1.Code = CmdCode._IfSimple;
                opCode.O2.Code = CmdCode._IfSimpleEndIf;
                opCode.O2.Reserved = opCode.O1.Index;
            }
        }

    }
}
