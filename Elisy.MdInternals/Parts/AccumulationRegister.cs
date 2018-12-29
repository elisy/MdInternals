using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;
using Elisy.MdInternals.DataElements;

namespace Elisy.MdInternals
{
    [Guid("b64d9a40-1642-11d6-a3c7-0050bae0a776")]
    public class AccumulationRegister : MetadataPart
    {
        public AccumulationRegister()
            : base()
        {
        }


        public AccumulationRegister(ImageRow imageRow)
            : base(imageRow)
        {
        }

        //public string Guid_1_1 { get; set; }
        public Guid RecordTypeId { get; set; }
        //public string Guid_1_2 { get; set; }
        public Guid RecordValTypeId { get; set; }

        //public string Guid_1_3 { get; set; }
        public Guid ManagerTypeId { get; set; }
        //public string Guid_1_4 { get; set; }
        public Guid ManagerValTypeId { get; set; }

        //public string Guid_1_5 { get; set; }
        public Guid SelectionTypeId { get; set; }
        //public string Guid_1_6 { get; set; }
        public Guid SelectionValTypeId { get; set; }

        //public string Guid_1_7 { get; set; }
        public Guid ListTypeId { get; set; }
        //public string Guid_1_8 { get; set; }
        public Guid ListValTypeId { get; set; }

        //public string Guid_1_9 { get; set; }
        public Guid RecordSetTypeId { get; set; }
        //public string Guid_1_10 { get; set; }
        public Guid RecordSetValTypeId { get; set; }

        //public string Guid_1_11 { get; set; }
        public Guid RecordKeyTypeId { get; set; }
        //public string Guid_1_12 { get; set; }
        public Guid RecordKeyValTypeId { get; set; }

        public Guid Guid_1_14 { get; set; }

        public int Int_1_15 { get; set; }
        public int Int_1_16 { get; set; }
        public int Int_1_18 { get; set; }
        public int Int_1_20 { get; set; }

        public Collection List_1_21 { get; set; }

        public Collection List_1_25 { get; set; }

        public Collection List_5 { get; set; }
        public Collection List_6 { get; set; }
        public Collection List_7 { get; set; }
        public Collection List_8 { get; set; }
    }
}
