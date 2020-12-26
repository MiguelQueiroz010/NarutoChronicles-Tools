using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace NUC_Raw_Tools.Arquivo
{
    public class Bin
    {
        public static byte[] ReadBlock(Stream s,uint offset, uint size)
        {
            byte[] bytes = null;
            var reader = new BinaryReader(s);
            reader.BaseStream.Position = offset;
            bytes = reader.ReadBytes((int)size);
            return bytes;
        }

    }
}
