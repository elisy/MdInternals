using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Elisy.MdInternals.DataElements
{
    public class DataElement : IDataElement
    {
        public DataElement()
        {
        }

        public DataElement(object value)
        {
        }

        public virtual object GetData()
        {
            throw new NotImplementedException();
        }

    }

}
