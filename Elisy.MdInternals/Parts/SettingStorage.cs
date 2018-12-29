using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("46b4cd97-fd13-4eaa-aba2-3bddd7699218")]
    public class SettingStorage : MetadataPart
    {
        public SettingStorage()
            : base()
        {
        }


        public SettingStorage(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
