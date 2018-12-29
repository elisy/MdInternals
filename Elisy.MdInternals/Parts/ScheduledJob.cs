using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("11bdaf85-d5ad-4d91-bb24-aa0eee139052")]
    public class ScheduledJob : MetadataPart
    {
        public ScheduledJob()
            : base()
        {
        }


        public ScheduledJob(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
