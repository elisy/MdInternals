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

namespace Elisy.MdInternals.Cf
{
    internal class ImagePageHeader
    {
        public UInt16 Data1;
        public UInt32 FullSize;
        public UInt32 PageSize;
        public UInt32 NextPageAddress;
        public UInt16 Data2;

        public long Position;

        internal int Index;
        internal int DataSize;
    }
}
