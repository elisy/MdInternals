using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.IO.Compression;

using Elisy.MdInternals.Cf;
using Elisy.MdInternals.Serialization;
using Elisy.MdInternals.DataElements;

namespace Elisy.MdInternals
{
    public class CfuPackage : MetadataPackage
    {
        public CfuPackage()
            : base()
        {
        }

        public CfuPackage(Image image)
            : base(image)
        {
        }

        public override void Open(Stream stream)
        {
            //base.Open(stream);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length);
            byte[] dataDeflated = ImageRow.Deflate(data);
            MemoryStream memoryStream = new MemoryStream(dataDeflated);

            //Image = ImageReader.ReadImageFrom(memoryStream);
            //ReadMetadataPartsFromImage();
            //AddUnknownObjects();
            base.Open(memoryStream);
        }

        public override void Save(Stream stream)
        {
            Stream writerStream = new MemoryStream();
            base.Save(writerStream);

            writerStream.Position = 0;
            DeflateStream deflatedStream = new DeflateStream(stream, CompressionMode.Compress);
            deflatedStream.Write(((MemoryStream)writerStream).ToArray(), 0, (int)writerStream.Length);
            deflatedStream.Close();
        }

        protected override void ReadMetadataPartsFromImage()
        {
            base.ReadMetadataPartsFromImage();

            var requestUpdateInfo = from row in Image.Rows.AsParallel()
                                     where row.FileName == "UpdateInfo.inf"
                                    select new UpdateInfo(row);
            var updateInfo = requestUpdateInfo.FirstOrDefault();

            lock (MetadataObjects)
            {
                MetadataObjects.Add(updateInfo);
            }

        }
    }
}
