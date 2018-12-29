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
    public sealed class ImageWriter : BinaryWriter
    {
        public ImageWriter(Stream stream)
            : base(stream)
        {
        }

        public void WriteImage(Image image)
        {
            Write(Image.Signature); //Image.Signature is 0x7fffffff
            Write(Image.PageSize);
            Write(image.Rows.Length);
            Write(Image.Data1);

            UInt32 pageSize = 42 * 12;
            int rowsInPage = (int)(pageSize / 12);

            double doublePages = (double)image.Rows.Length / rowsInPage;
            int pages = (int)doublePages;
            if (doublePages != pages)
                pages++;

            ImagePageHeader[] pageHeaders = Enumerable.Range(0, pages).Select(
                m => new ImagePageHeader()
                {
                    Index = m,
                    PageSize = pageSize,
                    Data1 = 0x0A0D,
                    Data2 = 0x0A0D
                }
                    ).ToArray();

            for (int i = 0; i < pageHeaders.Length; i++)
            {
                var pageHeader = pageHeaders[i];

                if (i == 0)
                    pageHeader.FullSize = (uint)image.Rows.Length * 12;
                else
                    pageHeader.FullSize = 0;

                SavePage(image, pageHeader, rowsInPage, pageHeader.Index == pageHeaders.Length - 1);
            }
        }


        private void SavePage(Image image, ImagePageHeader pageHeader, int rowsInPage, bool lastPage)
        {
            var imageRows = image.Rows.Skip(pageHeader.Index * rowsInPage).Take(rowsInPage).ToArray();

            var pageHeaderPosition = BaseStream.Position;
            this.BaseStream.Position += 31;
            this.BaseStream.Position += pageHeader.PageSize;

            foreach (ImageRow imageRow in imageRows)
            {
                imageRow.HeaderAddress = (uint)BaseStream.Position;
                //Image Row Header
                WriteImageRowHeader(imageRow);

                imageRow.BodyAddress = (uint)BaseStream.Position;
                //Imabe body
                WriteImageBody(imageRow);
            }

            uint nextPagePosition = (uint)BaseStream.Position;
            BaseStream.Position = pageHeaderPosition;

            //Save page header
            Write(pageHeader.Data1);

            StringBuilder sb = new StringBuilder();
            sb.Append(pageHeader.FullSize.ToString("X").PadLeft(8, '0').ToLower() + " ");
            sb.Append(pageHeader.PageSize.ToString("X").PadLeft(8, '0').ToLower() + " ");
            if (lastPage)
                sb.Append(((uint)0x7fffffff).ToString("X").PadLeft(8, '0').ToLower() + " ");
            else
                sb.Append(nextPagePosition.ToString("X").PadLeft(8, '0').ToLower() + " ");
            Write(sb.ToString().ToCharArray());

            Write(pageHeader.Data2);

            foreach (ImageRow imageRow in imageRows)
            {
                Write(imageRow.HeaderAddress);
                Write(imageRow.BodyAddress);
                Write((uint)0x7fffffff);
            }

            BaseStream.Position = nextPagePosition;
        }

        private void WriteImageRowHeader(ImageRow imageRow)
        {
            Write((ushort)0x0A0D);

            byte[] encodedFileName = Encoding.Unicode.GetBytes(imageRow.FileName);
            uint headerLength = (uint)(encodedFileName.Length);
            headerLength += 24;

            StringBuilder sb1 = new StringBuilder();
            sb1.Append(headerLength.ToString("X").PadLeft(8, '0').ToLower() + " ");
            sb1.Append(headerLength.ToString("X").PadLeft(8, '0').ToLower() + " ");
            sb1.Append(((uint)0x7fffffff).ToString("X").PadLeft(8, '0').ToLower() + " ");
            Write(sb1.ToString().ToCharArray());

            Write((ushort)0x0A0D);

            Write((Int64)(imageRow.Creation.Ticks / 1000));
            Write((Int64)(imageRow.Modified.Ticks / 1000));
            //Write(imageRow.Attributes);
            Write((uint)0x00);

            Write(encodedFileName);
            Write((uint)0x0);

        }

        private void WriteImageBody(ImageRow imageRow)
        {
            Write((ushort)0x0A0D);

            StringBuilder sb2 = new StringBuilder();
            sb2.Append(imageRow.BinaryData.Length.ToString("X").PadLeft(8, '0').ToLower() + " ");
            sb2.Append(imageRow.BinaryData.Length.ToString("X").PadLeft(8, '0').ToLower() + " ");
            sb2.Append(((uint)0x7fffffff).ToString("X").PadLeft(8, '0').ToLower() + " ");
            Write(sb2.ToString().ToCharArray());

            Write((ushort)0x0A0D);

            Write(imageRow.BinaryData);
        }
    }
}
