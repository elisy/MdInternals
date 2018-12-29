using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;
using Elisy.MdInternals.Serialization;
using Elisy.MdInternals.DataElements;

namespace Elisy.MdInternals
{
    public class CfPackage : MetadataPackage
    {

        private RootPointer _rootPointer;
        private Version _version;
        private Versions _versions;

        public CfPackage()
            : base()
        {
        }

        public CfPackage(Image image)
            : base(image)
        {
        }

        protected override void ReadMetadataPartsFromImage()
        {
            base.ReadMetadataPartsFromImage();

            var requestRootPointer = from row in Image.Rows.AsParallel()
                                     where row.FileName == "root"
                                     select new RootPointer(row);
            _rootPointer = requestRootPointer.FirstOrDefault();
            if (_rootPointer == null)
                throw new InvalidOperationException("Unsupported file structure. May be 8.0 format. RootPointer == null");

            lock (MetadataObjects)
            {
                MetadataObjects.Add(_rootPointer);
            }

            var requestRoot = from row in Image.Rows.AsParallel()
                              where row.FileName.Contains(_rootPointer.MetadataPackageFileName.ToString())
                              select row;
            PackagePart = new Configuration(requestRoot.FirstOrDefault());
            lock (MetadataObjects)
            {
                MetadataObjects.Add(PackagePart);
            }

            var requestVersion = from row in Image.Rows.AsParallel()
                                 where row.FileName == "version"
                                 select row;
            _version = new Version(requestVersion.FirstOrDefault());
            lock (MetadataObjects)
            {
                MetadataObjects.Add(_version);
            }

            var requestVersions = from row in Image.Rows.AsParallel()
                                  where row.FileName == "versions"
                                  select row;
            _versions = new Versions(requestVersions.FirstOrDefault());
            lock (MetadataObjects)
            {
                MetadataObjects.Add(_versions);
            }

            ReadMetadataObjects();
        }

        private void ReadMetadataObjects()
        {
            Configuration configurationPart = (Configuration)PackagePart;

            Dictionary<Type, Collection> parts = new Dictionary<Type, Collection>();

            //Common
            parts.Add(typeof(Role), configurationPart.Roles);
            parts.Add(typeof(CommonTemplate), configurationPart.CommonTemplates);
            parts.Add(typeof(CommonModule), configurationPart.CommonModules);
            parts.Add(typeof(ScheduledJob), configurationPart.ScheduledJobs);
            parts.Add(typeof(SharedAttribute), configurationPart.SharedAttributes);
            parts.Add(typeof(SessionParameter), configurationPart.SessionParameters);
            parts.Add(typeof(FunctionalOptionsParameter), configurationPart.FunctionalOptionsParameters);
            parts.Add(typeof(Subsystem), configurationPart.Subsystems);
            parts.Add(typeof(Interface), configurationPart.Interfaces);
            parts.Add(typeof(Style), configurationPart.Styles);
            parts.Add(typeof(Filter), configurationPart.Filters);
            parts.Add(typeof(SettingStorage), configurationPart.SettingStorages);
            parts.Add(typeof(EventSubscription), configurationPart.EventSubscriptions);
            parts.Add(typeof(StyleElement), configurationPart.StyleElements);
            parts.Add(typeof(CommonPicture), configurationPart.CommonPictures);
            parts.Add(typeof(ExchangePlan), configurationPart.ExchangePlans);
            parts.Add(typeof(WebService), configurationPart.WebServices);
            parts.Add(typeof(Language), configurationPart.Languages);
            parts.Add(typeof(FunctionalOption), configurationPart.FunctionalOptions);
            parts.Add(typeof(XdtoPackage), configurationPart.XdtoPackages);
            parts.Add(typeof(WsReference), configurationPart.WsReferences);
            //End Common

            parts.Add(typeof(Constant), configurationPart.Constants);
            parts.Add(typeof(Document), configurationPart.Documents);
            parts.Add(typeof(CommonForm), configurationPart.CommonForms);
            parts.Add(typeof(InformationRegister), configurationPart.InformationRegisters);
            parts.Add(typeof(CommandGroup), configurationPart.CommandGroups);
            parts.Add(typeof(CommonCommand), configurationPart.CommonCommands);
            parts.Add(typeof(DocumentNumerator), configurationPart.DocumentNumerators);
            parts.Add(typeof(DocumentJournal), configurationPart.DocumentJournals);
            parts.Add(typeof(Report), configurationPart.Reports);
            parts.Add(typeof(ChartOfCharacteristicTypes), configurationPart.ChartsOfCharacteristicTypes);
            parts.Add(typeof(AccumulationRegister), configurationPart.AccumulationRegisters);
            parts.Add(typeof(Sequence), configurationPart.Sequences);
            parts.Add(typeof(DataProcessor), configurationPart.DataProcessors);
            parts.Add(typeof(Catalog), configurationPart.Catalogs);
            parts.Add(typeof(Enum), configurationPart.Enums);

            parts.Add(typeof(ChartOfAccounts), configurationPart.ChartsOfAccounts);
            parts.Add(typeof(AccountingRegister), configurationPart.AccountingRegisters);



            parts.AsParallel().ForAll(i => ReadMetadataObjects(i.Key, i.Value));

            //Add unknown objects
            AddUnknownObjects();
        }

        private void ReadMetadataObjects(Type typePart, Collection list)
        {
            Collection packagePartModel = PackagePart.GetModel();

            var sectionGuid = typePart.GetCustomAttributes(false).OfType<GuidAttribute>().FirstOrDefault();
            if (sectionGuid != null)
            {
                if (sectionGuid.Value != list[0].ToString())
                    throw new InvalidOperationException("Section Guid is invalid");
            }

            var guids = list.Skip(2);

            var request = from row in Image.Rows.AsParallel()
                          where guids.Contains(row.FileName)
                          select Activator.CreateInstance(typePart, new object[] { row }) as ImageRowElement;

            var array = request.ToArray();
            array.AsParallel().ForAll(m => m.Package = this);

            lock (MetadataObjects)
            {
                MetadataObjects.AddRange(array);
            }

        }

    }
}
