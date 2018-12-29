using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;

using Elisy.MdInternals.Cf;
using Elisy.MdInternals.DataElements;
using Elisy.MdInternals.Serialization;

namespace Elisy.MdInternals
{
    public class ImageRowElement
    {
        public ImageRow ImageRow { get; private set; }
        internal MetadataPackage Package { get; set; }

        public ImageRowElement(ImageRow imageRow) : this()
        {
            ImageRow = imageRow;
        }

        public ImageRowElement()
        {
        }
    }
}
