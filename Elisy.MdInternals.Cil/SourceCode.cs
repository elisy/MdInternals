using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;

using Elisy.MdInternals.Serialization;

namespace Elisy.MdInternals.Cil
{
    public class SourceCode
    {
        internal int CurrentLineNunber {get; set;}
        private int _indentLevel = 0;
        internal bool IsNewLine { get; set; }

        private StringBuilder _sourceCode = new StringBuilder();

        public SourceCode()
        {
            CurrentLineNunber = 1;
            IsNewLine = true;
        }

        public override string ToString()
        {
            return _sourceCode.ToString();
        }

        public void AppendLine()
        {
            _sourceCode.AppendLine();
            IsNewLine = true;
            CurrentLineNunber++;
        }

        public void AppendLine(string str)
        {
            _sourceCode.AppendLine();
            _sourceCode.Append("".PadLeft(_indentLevel * 4, ' '));
            _sourceCode.Append(str);
            IsNewLine = false;
            CurrentLineNunber++;
        }

        public void Append(string str)
        {
            if (IsNewLine)
                _sourceCode.Append("".PadLeft(_indentLevel * 4, ' '));

            _sourceCode.Append(str);
            IsNewLine = false;
        }

        internal void GoToLine(int lineNumber)
        {
            int linesToAdd = 0;
            lineNumber = Math.Max(0, lineNumber);
            if (lineNumber <= CurrentLineNunber)
                linesToAdd = 1;
            else
                linesToAdd = lineNumber - CurrentLineNunber;
            for (int i = 0; i < linesToAdd; i++)
            {
                this.AppendLine();
                //CurrentLineNunber++;
            }
            //_sourceCode.Append("".PadLeft(_indentLevel * 4, ' '));
        }


        internal void Unindent()
        {
            _indentLevel = Math.Max(0, _indentLevel - 1);
        }


        internal void Indent()
        {
            _indentLevel++;
        }


    }
}
