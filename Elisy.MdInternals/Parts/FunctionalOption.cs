using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;
using Elisy.MdInternals.DataElements;

namespace Elisy.MdInternals
{
    [Guid("af547940-3268-434f-a3e7-e47d6d2638c3")]
    public class FunctionalOption : MetadataPart
    {

        public Collection List_1 { get; set; }

        public FunctionalOption()
            : base()
        {
        }


        public FunctionalOption(ImageRow imageRow)
            : base(imageRow)
        {
        }

        //private MetadataPartRef _storage;
        //public MetadataPartRef Storage
        //{
        //    get
        //    {
        //        if (Model == null)
        //            return _storage;
        //        else
        //        {
        //            var result = GetBoundProperty<MetadataPartRef>("Storage");
        //            if (Package != null)
        //                result.FullName = Package.MetadataObjects.AsParallel().OfType<MetadataPart>().Where(m => m.ImageRow.FileName == result.FileName).Select(m => m.FullName).FirstOrDefault();
        //            return result;
        //        }
        //    }
        //    set
        //    {
        //        _storage = value;
        //        if (Model != null)
        //            SetBoundProperty(Model, "Storage", value);
        //    }
        //}

    }
}
