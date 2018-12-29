using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elisy.MdInternals.DataElements
{
    //public class Collection : System.Collections.ObjectModel.ObservableCollection<object>, ICloneable
    public class Collection : List<object>, ICloneable
    {
        public Collection() : base()
        {
        }

        public Collection(params object[] collection)
            : base(collection)
        {
            
        }

        public object Clone()
        {
            Collection result = new Collection();
            Clone(this, result);
            return result;
        }

        private void Clone(Collection source, Collection destination)
        {
            foreach (object item in source)
            {
                if (item is Collection)
                {
                    Collection child = new Collection();
                    Clone(item as Collection, child);
                    destination.Add(child);
                }
                else
                    destination.Add(item);
            }
        }
    }

}
