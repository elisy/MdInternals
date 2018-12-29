//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation version 3 of the License.
//
//This  program  is  distributed  in  the hope that it will be useful,
//but  WITHOUT  ANY  WARRANTY;  without  even  the implied warranty of
//MERCHANTABILITY  or  FITNESS  FOR  A  PARTICULAR  PURPOSE.  See  the
//GNU General Public License for more details.
//
//Visit http://www.1csoftware.com/dotnet/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Globalization;

namespace Elisy.MdInternals.Cf
{
    public sealed class ImageReader : BinaryReader
    {
        readonly Image Image;

        public ImageReader(Stream stream)
            : base(stream)
        {
            Image = new Image();
        }

        private void ReadImage()
        {
            if (BaseStream.Length < 48)
                throw new BadImageFormatException("Length is less than 48");

            //Image.Signature = ReadUInt32();
            var imageSignature = ReadUInt32();

            //Error reading erf converted by 1C from 8.1
            //if (imageSignature != Image.Signature)
            //    throw new BadImageFormatException("Invalid Image Signature");

            //Image.PageSize = ReadUInt32();
            var imagePageSize = ReadUInt32();
            //if (imagePageSize != Image.PageSize)
            //    throw new BadImageFormatException("Invalid Image PageSize");

            //Image.Length = ReadUInt32();
            var imageLength = ReadUInt32();
            
            //Image.Data1 = ReadUInt32();
            var imageData1 = ReadUInt32();
            if (imageData1 != Image.Data1)
                throw new BadImageFormatException("Invalid Image Data1");


            Image.Rows = ReadRows();
            //Error extracting internal images like modules
            //if (imageLength != Image.Rows.Length)
            //    throw new InvalidOperationException("Incorrect Length");
        }


        private ImageRow[] ReadRows()
        {
            List<ImageRow> result = new List<ImageRow>();

            long currentPosition = BaseStream.Position;

            ImagePageHeader[] pageHeaders = ReadPageHeaders();
            byte[] imgageRows = GetDataFromPageHeaders(pageHeaders);
            using (MemoryStream ms = new MemoryStream(imgageRows))
            {
                int mpCount = (int)(ms.Length / 12);
                for (int i = 0; i < mpCount; i++)
                {
                    ImageRow mp = ReadImageRow(ms);
                    BaseStream.Position = mp.HeaderAddress;
                    ReadImageRowHeader(mp);

                    BaseStream.Position = mp.BodyAddress;
                    mp.BinaryData = GetDataFromPageHeaders(ReadPageHeaders());
                     result.Add(mp);
                }
            }

            BaseStream.Position = currentPosition;

            return result.ToArray();
        }


        private void ReadImageRowHeader(ImageRow ir)
        {
            var ph = ReadPageHeader();

            //ir.Data1 = ReadUInt16();
            //ir.Data2 = ReadUInt16();
            //ir.Data3 = ReadUInt16();
            //ir.Data4 = ReadUInt16();
            //ir.Data5 = ReadUInt16();
            //ir.Data6 = ReadUInt16();
            //ir.Data7 = ReadUInt16();
            //ir.Data8 = ReadUInt16();
            ir.Creation = new DateTime(ReadInt64() * 1000);
            ir.Modified = new DateTime(ReadInt64() * 1000);

            ir.Attributes = ReadUInt32();

            byte[] idBytes = ReadBytes((int)(ph.FullSize - 20 - 4));

            Encoding u16LE = Encoding.Unicode;
            ir.FileName = u16LE.GetString(idBytes);
        }

        private byte[] GetDataFromPageHeaders(ImagePageHeader[] pageHeaders)
        {
            long currentPosition = BaseStream.Position;

            byte[] result;

            //The first PageHeader contains full size. The others' size is 0.
            UInt32 fullDataSize = pageHeaders[0].FullSize;

            //Full Data Size can be 0
            //if (fullDataSize == 0)
            //    throw new BadImageFormatException("PageHeader is not the first in group");

            UInt32 dataRead = 0;
            using (MemoryStream ms = new MemoryStream((int)pageHeaders[0].FullSize))
            {
                foreach (ImagePageHeader ph in pageHeaders)
                {
                    UInt32 dataSize1 = fullDataSize - dataRead;
                    UInt32 dataSize = Math.Min(ph.PageSize, dataSize1);
                    BaseStream.Position = ph.Position + 31;
                    ms.Write(ReadBytes((int)dataSize), 0, (int)dataSize);
                    dataRead += dataSize;
                }

                if (fullDataSize != dataRead)
                    throw new BadImageFormatException("Not all data has been read from PageHeader");

                ms.Seek(0, SeekOrigin.Begin);
                result = new byte[ms.Length];
                ms.Read(result, 0, (int)ms.Length);
            }

            BaseStream.Position = currentPosition;

            return result;
        }


        private ImagePageHeader[] ReadPageHeaders()
        {
            List<ImagePageHeader> result = new List<ImagePageHeader>();
            ImagePageHeader ph;
            do
            {
                ph = ReadPageHeader();
                result.Add(ph);
                if (ph.NextPageAddress != 0x7fffffff)
                    BaseStream.Position = ph.NextPageAddress;
            }
            while (ph.NextPageAddress != 0x7fffffff);

            return result.ToArray();
        }


        private ImagePageHeader ReadPageHeader()
        {
            ImagePageHeader ph = new ImagePageHeader();

            ph.Position = BaseStream.Position;

            ph.Data1 = ReadUInt16();

            char[] ch = ReadChars(27);
            string str = new String(ch);
            ph.FullSize = UInt32.Parse(str.Substring(0, 8), NumberStyles.HexNumber);
            ph.PageSize = UInt32.Parse(str.Substring(9, 8), NumberStyles.HexNumber);
            ph.NextPageAddress = UInt32.Parse(str.Substring(18, 8), NumberStyles.HexNumber);

            ph.Data2 = ReadUInt16();

            return ph;
        }



        public static Image ReadImageFromConfig(string connectionString)
        {
            MSSQL1C81DataContext context = new MSSQL1C81DataContext(connectionString);
            var imageRequest = from row in context.Configs
                               select new ImageRow() { Attributes = (uint)row.Attributes, Creation = row.Creation, Modified = row.Modified, FileName = row.FileName, BinaryData = row.BinaryData.ToArray() };
            return new Image() { Rows = imageRequest.ToArray() };
        }

        public static Image ReadImageFromConfigSave(string connectionString)
        {
            MSSQL1C81DataContext context = new MSSQL1C81DataContext(connectionString);
            var imageRequest = from row in context.ConfigSaves
                               select new ImageRow() { Attributes = (uint)row.Attributes, Creation = row.Creation, Modified = row.Modified, FileName = row.FileName, BinaryData = row.BinaryData.ToArray() };
            return new Image() { Rows = imageRequest.ToArray() };
        }

        public static Image ReadImageFromParams(string connectionString)
        {
            MSSQL1C81DataContext context = new MSSQL1C81DataContext(connectionString);
            var imageRequest = from row in context.Params
                               select new ImageRow() { Attributes = (uint)row.Attributes, Creation = row.Creation, Modified = row.Modified, FileName = row.FileName, BinaryData = row.BinaryData.ToArray() };
            return new Image() { Rows = imageRequest.ToArray() };
        }


        public static Image ReadImageFrom(Stream stream)
        {
            try
            {
                var reader = new ImageReader(stream);
                reader.ReadImage();
                return reader.Image;
            }
            catch (EndOfStreamException e)
            {
                throw new BadImageFormatException(GetFullyQualifiedName(stream), e);
            }
        }

        private static string GetFullyQualifiedName(Stream stream)
        {
            var file_stream = stream as FileStream;
            if (file_stream == null)
                return string.Empty;

            return Path.GetFullPath(file_stream.Name);
        }


        private static ImageRow ReadImageRow(Stream stream)
        {
            ImageRow result = new ImageRow();
            BinaryReader reader = new BinaryReader(stream);
            result.HeaderAddress = reader.ReadUInt32();
            result.BodyAddress = reader.ReadUInt32();

            //0x7fffffff
            UInt32 signature = reader.ReadUInt32();
            if (signature != 0x7fffffff)
                throw new BadImageFormatException("ImageRow signature is not 0x7fffffff");

            return result;
        }

    }
}
