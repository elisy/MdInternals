using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;

using Elisy.MdInternals.Cf;
using Elisy.MdInternals.Serialization;
using Elisy.MdInternals.DataElements;

namespace Elisy.MdInternals
{
    public class Configuration : MetadataPart
    {
        public Guid Guid_1_0 { get; set; }

        public int Int_3_1_1_3 { get; set; }

        //public Collection Localizable_3_1_1_4 { get; set; }
        public LocalStringType BriefInformation { get; set; }
        public LocalStringType DetailedInformation { get; set; }
        public LocalStringType Copyright { get; set; }
        public LocalStringType VendorInformationAddress { get; set; }
        public LocalStringType ConfigurationInformationAddress { get; set; }

        public Guid Guid_3_1_1_9 { get; set; }
        public Guid Guid_3_1_1_10 { get; set; }
        public Guid Guid_3_1_1_11 { get; set; }
        public Guid Guid_3_1_1_12 { get; set; }

        public int Int_3_1_1_13 { get; set; }

        public string String_3_1_1_14 { get; set; }
        public string String_3_1_1_15 { get; set; }
        public string String_3_1_1_16 { get; set; }

        public Guid Guid_3_1_1_24 { get; set; }
        public int Int_3_1_1_26 { get; set; }
        public int Int_3_1_1_29 { get; set; }

        public Collection Roles { get; set; }
        public Collection CommonTemplates { get; set; }
        public Collection CommonModules { get; set; }
        public Collection ScheduledJobs { get; set; }
        public Collection SharedAttributes { get; set; }
        public Collection SessionParameters { get; set; }
        public Collection FunctionalOptionsParameters { get; set; }
        public Collection Subsystems { get; set; }
        public Collection Interfaces { get; set; }
        public Collection Styles { get; set; }
        public Collection Filters { get; set; }
        public Collection SettingStorages { get; set; }
        public Collection EventSubscriptions { get; set; }
        public Collection StyleElements { get; set; }
        public Collection CommonPictures { get; set; }
        public Collection ExchangePlans { get; set; }
        public Collection WebServices { get; set; }
        public Collection Languages { get; set; }
        public Collection FunctionalOptions { get; set; }
        public Collection XdtoPackages { get; set; }
        public Collection WsReferences { get; set; }
            
        public Guid Guid_4_1_1_1_0_2 { get; set; }
        public Guid Guid_4_1_1_1_1 { get; set; }

        public Collection Constants { get; set; }
        public Collection Documents { get; set; }
        public Collection CommonForms { get; set; }
        public Collection InformationRegisters { get; set; }
        public Collection CommandGroups { get; set; }
        public Collection CommonCommands { get; set; }
        public Collection DocumentNumerators { get; set; }
        public Collection DocumentJournals { get; set; }
        public Collection Reports { get; set; }
        public Collection ChartsOfCharacteristicTypes { get; set; }
        public Collection AccumulationRegisters { get; set; }
        public Collection Sequences { get; set; }
        public Collection DataProcessors { get; set; }
        public Collection Catalogs { get; set; }
        public Collection Enums { get; set; }

        public Guid Guid_5_1_1_1_2 { get; set; }

        public Collection ChartsOfAccounts { get; set; }
        public Collection AccountingRegisters { get; set; }

        public Guid Guid_6_1_1_0_2 { get; set; }
        public Guid Guid_7_1_1_1_2 { get; set; }

        public Collection List_7_1_3 { get; set; }
        public Collection List_7_1_4 { get; set; }

        public Guid Guid_8_1_1_0_2 { get; set; }

        public Configuration()
            : base()
        {
        }

        public Configuration(ImageRow row)
            : base(row)
        {
        }
    }
}
