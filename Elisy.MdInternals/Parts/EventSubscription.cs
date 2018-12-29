using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("4e828da6-0f44-4b5b-b1c0-a2b3cfe7bdcc")]
    public class EventSubscription : MetadataPart
    {
        public EventSubscription()
            : base()
        {
        }


        public EventSubscription(ImageRow imageRow)
            : base(imageRow)
        {
        }
    }
}
