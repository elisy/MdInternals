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

namespace Elisy.MdInternals.Cf
{
    public sealed class Image
    {
        //Image header
        internal static UInt32 Signature = 0x7fffffff;
        internal static UInt32 PageSize = 0x2000;
        //internal UInt32 Length;
        internal static UInt32 Data1 = 0x00;

        public ImageRow[] Rows = new ImageRow[0];
    }
}
