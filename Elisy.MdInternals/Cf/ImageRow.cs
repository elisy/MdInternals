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
using System.IO.Compression;

namespace Elisy.MdInternals.Cf
{
    public class ImageRow
    {
		internal UInt32 HeaderAddress;
        internal UInt32 BodyAddress;

        public UInt32 Attributes { get; set; }

        public DateTime Creation { get; set; }
        public DateTime Modified { get; set; }

        public string FileName;

        private byte[] _binaryData;
        public byte[] BinaryData
        {
            get { return _binaryData; }
            set
            {
                _binaryData = value;
                _bodyType = ImageRowTypes.Unresolved;
                _body = null;
            }
        }

        private ImageRowTypes _bodyType = ImageRowTypes.Unresolved;
        public ImageRowTypes BodyType 
        {
            get
            {
                if (_bodyType == ImageRowTypes.Unresolved)
                    OpenBody();

                return _bodyType;
            }
        }

        private object _body;
        public object Body
        {
            get
            {
                if (_body == null)
                    OpenBody();

                return _body;
            }
        }

        internal void OpenBody()
        {
            if ((BinaryData[0] == 0xEF) && (BinaryData[1] == 0xBB) && (BinaryData[2] == 0xBF))
            {
                //UTF-8 signature
                _bodyType = ImageRowTypes.Utf8MarkedString;
                //Decode and avoid the first char 0xFEFF
                UTF8Encoding encoding = new UTF8Encoding(true);
                _body = encoding.GetString(BinaryData).Substring(1);
            }
            else
            {
                byte[] deflatedBody = new byte[0];
                try
                {
                    deflatedBody = Deflate(BinaryData);
                }
                catch (Exception)
                {
                    //BodyType is Unresolved. For example for Params table and FileName = "users.usr"
                    //_bodyType = ImageRowTypes.Unknown;
                    _body = BinaryData;
                    return;
                }

                if ((deflatedBody[0] == 0xFF) && (deflatedBody[1] == 0xFF) && (deflatedBody[2] == 0xFF) && (deflatedBody[3] == 0x7F))
                {
                    _bodyType = ImageRowTypes.CompressedImage;
                    MemoryStream stream = new MemoryStream(deflatedBody);
                    _body = ImageReader.ReadImageFrom(stream);
                }
                else if ((deflatedBody[0] == 0xEF) && (deflatedBody[1] == 0xBB) && (deflatedBody[2] == 0xBF))
                {
                    _bodyType = ImageRowTypes.CompressedUtf8MarkedString;
                    UTF8Encoding encoding = new UTF8Encoding(true);
                    //Decode and avoid the first char 0xFEFF
                    _body = encoding.GetString(deflatedBody).Substring(1);
                }
                else if (
                    (deflatedBody[0] == 0x4D) && 
                    (deflatedBody[1] == 0x4F) && 
                    (deflatedBody[2] == 0x58) &&
                    (deflatedBody[3] == 0x43) &&
                    (deflatedBody[4] == 0x45) && 
                    (deflatedBody[5] == 0x4C) && 
                    (deflatedBody[6] == 0x00))
                {
                    _bodyType = ImageRowTypes.CompressedMoxcel;
                    _body = deflatedBody;
                }
                else
                {
                    _bodyType = ImageRowTypes.Unknown;
                    _body = deflatedBody;
                }
            }
        }

        internal static byte[] Deflate(byte[] data)
        {
            MemoryStream resultStream = new MemoryStream();

            MemoryStream ms = new MemoryStream(data);
            DeflateStream stream = new DeflateStream(ms, CompressionMode.Decompress);

            int offset = 0;
            byte[] tempBuffer = new byte[100];
            while (true)
            {
                int bytesRead = stream.Read(tempBuffer, 0, 100);
                if (bytesRead == 0)
                    break;
                resultStream.Write(tempBuffer, 0, bytesRead);
                offset += bytesRead;
            }

            stream.Close();

            byte[] resultData = new byte[resultStream.Length];
            resultStream.Position = 0;
            resultStream.Read(resultData, 0, (int)resultStream.Length);

            return resultData;
        }


    }
}
