using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals.CfProject
{
    public class FileInfo
    {
        public string Id {get; set;}
        public string Path {get; set;}
        public UInt32 Attributes { get; set; }
        public ImageRowTypes BodyType { get; set; }
        public string Class { get; set; }
    }
}
