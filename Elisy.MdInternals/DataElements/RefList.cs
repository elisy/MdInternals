using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Markup;
using System.Collections.ObjectModel;
using System.Globalization;

using Elisy.MdInternals.Serialization;

namespace Elisy.MdInternals.DataElements
{
    public class RefList : ObservableCollection<MetadataPartRef>, IDataElement
    {
        public string TypeId { get; set; }
        public int Length { get; set; }

        public RefList()
            : base()
        {
        }

        public RefList(object value)
        {
            Collection list = value as Collection;

            TypeId = list[0].AsString();
            Length = list[1].AsInt32();
            for (int i = 0; i < Length; i++)
                this.Add(new MetadataPartRef(list[2+i].AsString()));
        }

        public object GetData()
        {
            Collection result = new Collection(
                TypeId,
                Length.ToString(CultureInfo.InvariantCulture)
            );
            foreach (var item in this)
                result.Add(item.GetData());
            return result;
        }

    }
}
