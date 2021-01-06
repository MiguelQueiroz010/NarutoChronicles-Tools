using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace NUC_Raw_Tools.ArquivoRAW
{
    public class Bin
    {
        public static byte[] ReadBlock(byte[] s,uint offset, uint size)
        {
            byte[] bytes = null;
            var memory = new MemoryStream(s);
            var reader = new BinaryReader(memory);
            reader.BaseStream.Position = offset;
            bytes = reader.ReadBytes((int)size);
            reader.Close();
            memory.Close();
            return bytes;
        }

    }
}
