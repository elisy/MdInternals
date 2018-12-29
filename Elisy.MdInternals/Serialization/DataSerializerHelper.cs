using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;

using Elisy.MdInternals.DataElements;

namespace Elisy.MdInternals.Serialization
{
    public static class DataSerializerHelper
    {

        public static string AsString(this object o)
        {
            string value = o as string;
            if (value.StartsWith("\"") && value.EndsWith("\""))
                return value.Substring(1, value.Length-2);
            else
                return value;
        }

        public static bool AsBoolean(this object o)
        {
            string value = o as string;
            if (value.Equals("true", StringComparison.CurrentCultureIgnoreCase) 
                || value.Equals("истина", StringComparison.CurrentCultureIgnoreCase)
                || value.Equals("1", StringComparison.CurrentCultureIgnoreCase))
                return true;
            else
                return false;
        }

        public static DateTime AsDateTime(this object o)
        {
            string value = o as string;
            if (value.Length == 14)
            {
                int year = Convert.ToInt32(value.Substring(0, 4));
                int month = Convert.ToInt32(value.Substring(4, 2));
                int day = Convert.ToInt32(value.Substring(6, 2));
                int hour = Convert.ToInt32(value.Substring(8, 2));
                int minute = Convert.ToInt32(value.Substring(10, 2));
                int second = Convert.ToInt32(value.Substring(12, 2));
                return new DateTime(year, month, day, hour, minute, second);
            }
            else
                throw new ArgumentException("Invalid object");
        }

        public static double AsDouble(this object o)
        {
            string value = o as string;
            double result;
            if (double.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out result))
                return result;
            else
                throw new ArgumentException("Invalid object");
        }


        public static Int32 AsInt32(this object o)
        {
            string value = o as string;
            Int32 result;
            if (Int32.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
                return result;
            else
                throw new ArgumentException("Invalid object");
        }


        public static object GetElement(this Collection list, IEnumerable<int> indexes)
        {
            object result = list;
            
            foreach (int index in indexes)
            {
                if (result is Collection)
                {
                    if (index > (result as Collection).Count - 1)
                        throw new ArgumentException(string.Format("Invalid index {0}", index), "indexes");
                    result = (result as Collection)[index];
                }
                else
                    throw new InvalidOperationException("The current element is not list");
            }

            return result;
        }

        public static void SetElement(this Collection list, int[] indexes, object value)
        {
            object result = list;

            for (int i = 0; i < indexes.Length; i++)
            {
                var index = indexes[i];

                if (result is Collection)
                {
                    if (index > (result as Collection).Count - 1)
                        throw new ArgumentException(string.Format("Invalid index {0}", index), "indexes");
                    if (i == indexes.Length - 1)
                    {
                        (result as Collection)[index] = value;
                        return;
                    }

                    result = (result as Collection)[index];
                }
                else
                    throw new InvalidOperationException("The current element is not list");
            }
        }

        public static object GetElement(this Collection list, params int[] indexes)
        {
            return GetElement(list, indexes as IEnumerable<int>);
        }

    }
}
