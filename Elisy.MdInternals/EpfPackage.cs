using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elisy.MdInternals.Cf;
using Elisy.MdInternals.Serialization;
using Elisy.MdInternals.DataElements;

namespace Elisy.MdInternals
{
    public class EpfPackage : MetadataPackage
    {
        public EpfPackage()
            : base()
        {
        }

        public EpfPackage(Image image)
            : base(image)
        {
        }

        protected override void ReadMetadataPartsFromImage()
        {
            base.ReadMetadataPartsFromImage();

            var requestRootPointer = from row in Image.Rows.AsParallel()
                                     where row.FileName == "root"
                                     select new RootPointer(row);
            var rootPointer = requestRootPointer.FirstOrDefault();
            if (rootPointer == null)
                throw new InvalidOperationException("Unsupported file structure. May be 8.0 format. RootPointer == null");

            lock (MetadataObjects)
            {
                MetadataObjects.Add(rootPointer);
            }

            var requestRoot = from row in Image.Rows.AsParallel()
                              where row.FileName.Contains(rootPointer.MetadataPackageFileName.ToString())
                              select row;
            PackagePart = new ExternalDataProcessor(requestRoot.FirstOrDefault());
            lock (MetadataObjects)
            {
                MetadataObjects.Add(PackagePart);
            }

            var requestVersion = from row in Image.Rows.AsParallel()
                                 where row.FileName == "version"
                                 select row;
            var version = new Version(requestVersion.FirstOrDefault());
            lock (MetadataObjects)
            {
                MetadataObjects.Add(version);
            }

            var requestVersions = from row in Image.Rows.AsParallel()
                                  where row.FileName == "versions"
                                  select row;
            var versions = new Versions(requestVersions.FirstOrDefault());
            lock (MetadataObjects)
            {
                MetadataObjects.Add(versions);
            }

        }
    }
}
