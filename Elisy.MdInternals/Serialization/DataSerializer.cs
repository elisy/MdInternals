using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;

using Elisy.MdInternals.DataElements;

namespace Elisy.MdInternals.Serialization
{
    /// <summary>
    /// System string presentation of value in infobase serializer
    /// </summary>
    public class DataSerializer
    {
        private string _content;
        private int _index;

        private char? GetNextNonEmptyChar()
        {
            while (this._content.Length > this._index)
            {
                char c = this._content[this._index++];
                if (!char.IsWhiteSpace(c))
                {
                    return new char?(c);
                }
            }
            return null;
        }

        private char? MoveNext()
        {
            if (this._content.Length > this._index)
            {
                return new char?(this._content[this._index++]);
            }
            return null;
        }

        private string MoveNext(int count)
        {
            if (this._content.Length >= (this._index + count))
            {
                string str = this._content.Substring(this._index, count);
                this._index += count;
                return str;
            }
            return null;
        }

        private void MovePrev()
        {
            if (this._index > 0)
            {
                this._index--;
            }
        }

        private void MovePrev(int count)
        {
            while ((this._index > 0) && (count > 0))
            {
                this._index--;
                count--;
            }
        }

        private static bool IsNextElementObject(char? c)
        {
            return (c == '{');
        }

        private static bool IsNextElementString(char? c)
        {
            return (c == '"');
        }

        public Collection Deserialize(string content)
        {
            _content = content;
            _index = 0;
            return Deserialize(0) as Collection;
        }

        private object Deserialize(int depth)
        {
            char? nextNonEmptyChar = GetNextNonEmptyChar();
            MovePrev();

            if (!nextNonEmptyChar.HasValue)
                return null;

            if (IsNextElementObject(nextNonEmptyChar))
            {
                Collection o = DeserializeList(depth);
                return o;
            }
            if (IsNextElementString(nextNonEmptyChar))
                return DeserializeString();

            return DeserializePrimitiveObject();
        }

        private string DeserializeString()
        {
            StringBuilder sb = new StringBuilder();
            char? c = MoveNext();
            if (c != '"')
                throw new InvalidOperationException(GetDebugString("String start char is not \""));
            c = MoveNext();

            while (true)
            {
                if (c == '"')
                {
                    if (this.MoveNext() == '"')
                        sb.Append('"');
                    else
                    {
                        this.MovePrev();
                        return "\"" + sb.ToString() + "\"";
                    }
                }
                else
                    sb.Append(c);
                c = MoveNext();
                if (!c.HasValue)
                    throw new InvalidOperationException(GetDebugString("Incorrect string format"));
            }
        }


        private Collection DeserializeList(int depth)
        {
            Collection list = new Collection();

            char? nextNonEmptyChar = this.GetNextNonEmptyChar();
            if (nextNonEmptyChar != '{')
                throw new InvalidOperationException(GetDebugString("The next symbol is not '{'"));

            if (!nextNonEmptyChar.HasValue)
                return list;

            while (!(nextNonEmptyChar == '}') && (nextNonEmptyChar.HasValue))
            {
                object o = Deserialize(depth);
                list.Add(o);

                nextNonEmptyChar = GetNextNonEmptyChar();
                if (nextNonEmptyChar == '}')
                    break;
                if (nextNonEmptyChar != ',')
                    throw new InvalidOperationException(GetDebugString("The next symbol is not ',' and '}'"));
            }
            return list;
        }


        private string GetDebugString(string message)
        {
            return string.Concat(new object[] { message, " (", _index, "): ", _content.Substring(_index) });
        }


        private object DeserializePrimitiveObject()
        {
            string s = this.DeserializePrimitiveToken();
            return s;
        }

        private string DeserializePrimitiveToken()
        {
            StringBuilder builder = new StringBuilder();
            char? nullable = this.MoveNext();
            while (nullable.HasValue)
            {
                //Error in deserializing base64
                //if ((char.IsLetterOrDigit(nullable.Value) || (nullable.Value == '.')) || (((nullable.Value == '-') || (nullable.Value == '_')) || (nullable.Value == '+')))
                if (char.IsLetterOrDigit(nullable.Value) || (nullable.Value == '.') || (nullable.Value == '-') || (nullable.Value == '_') || (nullable.Value == '+') || (nullable.Value == '=') || (nullable.Value == '\r') || (nullable.Value == '\n') || (nullable.Value == '/')
                    || (nullable.Value == '#') || (nullable.Value == ':')) //{#base64:77u   =}
                    builder.Append(nullable);
                else
                {
                    this.MovePrev();
                    break;
                }
                nullable = this.MoveNext();
            }
            return builder.ToString();
        }




        public string Serialize(Collection list)
        {
            StringBuilder sb = new StringBuilder();
            Serialize(sb, 0, list);
            return sb.ToString();
        }

        private void Serialize(StringBuilder sb, int depth, Collection list)
        {
            bool complexList = (list.Where(m => m is Collection).Count() != 0) || list.Count > 7;

            if (complexList)
            {
                //sb.AppendLine();
                //sb.Append("{".PadLeft(depth * 4 + 1));
                sb.Append("{");
                sb.AppendLine();
                sb.Append("".PadLeft(depth * 2 + 2));
            }
            else
                sb.Append("{".PadLeft(depth * 0 + 1));

            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (item is Collection)
                {
                    Serialize(sb, depth + 1, item as Collection);
                }
                else if (item is string)
                    sb.Append(NormalizeString(item as string));
                else
                    throw new InvalidOperationException(string.Format("Usupported type {0}", item.GetType().Name));

                if (i != list.Count - 1)
                {
                    sb.Append(",");
                    if (complexList)
                    {
                        sb.AppendLine();
                        sb.Append("".PadLeft(depth * 2 + 2));
                    }
                }
            }

            if (complexList)
            {
                sb.AppendLine();
                sb.Append("}".PadLeft(depth * 2 + 1));
            }
            else
                sb.Append("}".PadLeft(depth * 0 + 1));
        }


        private string NormalizeString(string value)
        {
            bool quotedString = value.StartsWith("\"") && value.EndsWith("\"");
            if (quotedString)
            {
                value = value.Substring(1, value.Length - 2).Replace("\"", "\"\"");
                return "\"" + value + "\"";
            }
            else
                return value.Replace("\"", "\"\"");
        }

    }
}
