using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("4612bd75-71b7-4a5c-8cc5-2b0b65f9fa0d")]
    public class DocumentJournal : MetadataPart
    {
        //internal override int[] _generalPropertiesLocation
        //{
        //    get { return new int[] { }; }
        //}

        public DocumentJournal()
            : base()
        {
        }



        public DocumentJournal(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
