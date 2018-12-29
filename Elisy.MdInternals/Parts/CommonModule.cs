using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;

namespace Elisy.MdInternals
{
    [Guid("0fe48980-252d-11d6-a3c7-0050bae0a776")]
    public class CommonModule : MetadataPart
    {
        public CommonModule()
            : base()
        {
        }

        public CommonModule(ImageRow imageRow)
            : base(imageRow)
        {
        }

        public bool Privileged {get; set;}
        public bool ServerCall {get; set;}
        public bool Server {get; set;}
        public bool Global {get; set;}
        public bool ClientOrdinaryApplication {get; set;}
        public bool ExternalConnection {get; set;}

        public bool ClientManagedApplication { get; set; } //ClientManagedApplication
        public int ReturnValuesReuse { get; set; } //ReturnValuesReuse

    }
}
