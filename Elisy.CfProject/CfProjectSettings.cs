using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.ObjectModel;

namespace Elisy.MdInternals.CfProject
{
    public class CfProjectSettings
    {
        public ProjectType Type {get; set;}
        public ObservableCollection<FileInfo> Files { get; set; }
        public string Version { get; set; }
        public string MdInternalsVersion { get; set; }
        public string PackageType { get; set; }

        public CfProjectSettings()
        {
            Files = new ObservableCollection<FileInfo>();
        }
    }
}
