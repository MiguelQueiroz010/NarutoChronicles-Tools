using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace NUC_Raw_Tools.Arquivo
{
    public class RawF : Raw
    {
        public byte[] Data;
        public uint Index;
        public uint Position;
        public uint Size;
        public RawF(byte[] root, uint i)
        {
                var memory = new MemoryStream(root);
                Index = ReadUInt32(memory, i);
                Size = ReadUInt32(memory, i + 4);
                Position = ReadUInt32(memory, i + 8);
                //+4 bytes nulos
                Data = Bin.ReadBlock(memory, Position, Size);
                
        }

    }
    public class Raw
    {
        public List<RawF> Root;
        public void RawR(byte[] root)
        {
            Root = new List<RawF>();
            var memory = new MemoryStream(root);
            uint endPointers = ReadUInt32(memory, 8);
            for (uint i =0;i<endPointers;i+=16)
            {
                Root.Add(new RawF(root, i));
            }
            
        }
        public static UInt32 ReadUInt32(Stream s, uint offset)
        {
            //byte[] buff = new byte[4];
            var rr = new BinaryReader(s);
            rr.BaseStream.Position = offset;
            return rr.ReadUInt32();
            //return BitConverter.ToUInt32(buff, 0);
        }

        public static void WriteUInt32(Stream s, uint u)
        {
            s.Write(BitConverter.GetBytes(u), 0, 4);
        }
    }
}
