using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Xml;
using System.IO.Compression;

using Elisy.MdInternals.Cf;
using Elisy.MdInternals.Serialization;
using Elisy.MdInternals.DataElements;

namespace Elisy.MdInternals
{
    public class MetadataPackage : IDisposable
    {
        protected Image Image { get; set; }
        public MetadataPart PackagePart { get; protected set; }
        public List<ImageRowElement> MetadataObjects { get; set; }

        public MetadataPackage()
        {
            Image = new Image();
            MetadataObjects = new List<ImageRowElement>();
        }

        public MetadataPackage(Image image)
        {
            Image = image;
            MetadataObjects = new List<ImageRowElement>();

            ReadMetadataPartsFromImage();
            AddUnknownObjects();
        }

        public virtual void Open(Stream stream)
        {
            MetadataObjects = new List<ImageRowElement>();

            Image = ImageReader.ReadImageFrom(stream);
            ReadMetadataPartsFromImage();
            AddUnknownObjects();

            //Image image;

            //if (type == typeof(CfuPackage))
            //{
            //    byte[] data = new byte[stream.Length];
            //    stream.Read(data, 0, (int)stream.Length);
            //    byte[] dataDeflated = ImageRow.Deflate(data);
            //    MemoryStream memoryStream = new MemoryStream(dataDeflated);
            //    image = ImageReader.ReadImageFrom(memoryStream);
            //}
            //else
            //    image = ImageReader.ReadImageFrom(stream);

            //return Activator.CreateInstance(type, new object[] { image }) as MetadataPackage;
        }

        public void Open(String path)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                //MetadataPackage result = Open(stream);
                //stream.Close();
                //return result;
                Open(stream);
            }
        }

        public virtual void Save(Stream stream)
        {
            Image image = new Image();
            image.Rows = MetadataObjects.AsParallel().Select(m => m.ImageRow).ToArray();

            //writer closes stream on disposing. Error in CfuPackage
            //using (ImageWriter writer = new ImageWriter(stream))
            //{
            //    writer.WriteImage(image);
            //}
            ImageWriter writer = new ImageWriter(stream);
            writer.WriteImage(image);
            writer.Flush();
        }


        public void Save(String path)
        {
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                Save(stream);
                stream.Close();
            }
        }


        protected virtual void ReadMetadataPartsFromImage()
        {
        }

        protected void AddUnknownObjects()
        {
            var unknownObjectsRequest = from row in Image.Rows.AsParallel()
                                        where !MetadataObjects.AsParallel().Select(m => m.ImageRow.FileName).Contains(row.FileName)
                                        select new UnresolvedPart(row);
            lock (MetadataObjects)
            {
                MetadataObjects.AddRange(unknownObjectsRequest.ToArray());
            }

            if (Image.Rows.Length != MetadataObjects.Count)
                throw new InvalidOperationException("_image.Rows.Length != MetadataObjects.Count");
        }


        public virtual void Dispose()
        {
            Image = null;
            MetadataObjects = null;
        }
    }
}
