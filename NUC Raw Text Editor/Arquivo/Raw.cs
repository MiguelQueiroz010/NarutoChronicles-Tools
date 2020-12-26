using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace NUC_Raw_Tools.Arquivo
{
    public class RawFile : Raw
    {
        public byte[] Data;
        public uint Index;
        public uint Position;
        public uint Size;
        public bool isText = false;
        public RawFile(byte[] file, uint i)
        {
            var memory = new MemoryStream(file);
            Index = ReadUInt32(memory, i);
            Size = ReadUInt32(memory, i + 8);
            Position = ReadUInt32(memory, i + 4);
            //+4 bytes nulos
            Data = Bin.ReadBlock(memory, Position, Size);
            memory = new MemoryStream(Data);
            if (ReadUInt16(memory, (uint)(Data.Length - 2)).ToString("X2") == "0080")
                isText = true;
        }
    }
    public class RawFolder : Raw
    {
        public byte[] Data;
        public uint Index;
        public uint Position;
        public uint Size;
        public bool hasSubFolder = false;
        public List<RawFolder> SubFolders;
        public RawFolder(byte[] root, uint i)
        {
                var memory = new MemoryStream(root);
                Index = ReadUInt32(memory, i);
                Size = ReadUInt32(memory, i + 4);
                Position = ReadUInt32(memory, i + 8);
                //+4 bytes nulos
                Data = Bin.ReadBlock(memory, Position, Size);
                memory = new MemoryStream(Data);
                if(ReadUInt32(memory, 0)==0)
                {
                    hasSubFolder = true;
                }
                if (hasSubFolder)
                {
                   SubFolders = new List<RawFolder>();
                   var mem = new MemoryStream(Data);
                   uint endPointers = ReadUInt32(mem, 8);
                   for (uint x = 0; i < endPointers; x += 16)
                   {
                       SubFolders.Add(new RawFolder(Data, x));
                   }
                }
        }

    }
    public class Raw
    {
        public List<RawFolder> Root;
        bool isFolder = true;
        public void RawR(byte[] root)
        {
            var mem = new MemoryStream(root);
            if (ReadUInt32(mem, 0)!=0)
            {
                isFolder = false;

            }
            if (isFolder)
            {
                Root = new List<RawFolder>();
                var memory = new MemoryStream(root);
                uint endPointers = ReadUInt32(memory, 8);
                for (uint i = 0; i < endPointers; i += 16)
                {
                    Root.Add(new RawFolder(root, i));
                }
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
        public static UInt16 ReadUInt16(Stream s, uint offset)
        {
            //byte[] buff = new byte[4];
            var rr = new BinaryReader(s);
            rr.BaseStream.Position = offset;
            return rr.ReadUInt16();
            //return BitConverter.ToUInt32(buff, 0);
        }

        public static void WriteUInt32(Stream s, uint u)
        {
            s.Write(BitConverter.GetBytes(u), 0, 4);
        }
    }
}
