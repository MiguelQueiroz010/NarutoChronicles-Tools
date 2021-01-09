using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.IO;
#region Usando RainbowImgLib https://github.com/marco-calautti/Rainbow/tree/master/Rainbow.ImgLib
using Rainbow.ImgLib;
using Microsoft.Win32;
#endregion
namespace NUC_Raw_Tools.ArquivoRAW
{
    public class RAW
    {
        public byte[] Data;
        public int folderCount;
        public List<Folder> Pastas;
        public RAW(byte[] opened, byte alfa)
        {
            #region Abertura e verificar se é pasta
            Data = opened;
            folderCount = (int)ReadUInt(Data, 8, Int.UInt32)/0xF;
            if(folderCount==0)
            {
                MessageBox.Show("Erro: Arquivo inválido(0 Pastas lidas)!!");
                return;
            }
            Pastas = new List<Folder>();
            for (int i = 0; i < folderCount; i++)
            {
                Pastas.Add(new Folder(Data, i,alfa));
            }
            #endregion
            
        }
        public void ListInsert(TreeView tree, RAW raw, string filename) 
        {
            if (tree.Nodes.Count != 0)
                tree.Nodes.Clear();
            TreeNode Raiz = new TreeNode(Path.GetFileName(filename));
            int i = 0;
            foreach (var folder in raw.Pastas)
            {
                TreeNode Raiz2 = null;
                if (folder.type==Types.Texto)
                    Raiz2 = new TreeNode(Path.GetFileName(filename) + "_" + i + " -Texto" + "-" + folder.texto.SeqCount);
                else if (folder.type == Types.Textura)
                    Raiz2 = new TreeNode(Path.GetFileName(filename) + "_" + i + " -Textura");
                else if (folder.type==Types.Unknow)
                    Raiz2 = new TreeNode(Path.GetFileName(filename) + "_" + i);
                try
                {
                    if (folder.filescount != 0)
                    {
                        int j = 0;
                        foreach (var file in folder.Arquivos)
                        {
                            if (file.type == Types.Texto)
                                Raiz2.Nodes.Add(Path.GetFileName(filename) + "_" + i + "_" + j + " -Texto" + "-" + file.texto.SeqCount);
                            else if (file.type == Types.Textura)
                            {
                                TreeNode Raiz3 = new TreeNode(Path.GetFileName(filename) + "_" + i + "_" + j + " -Textura");
                                int y = 0;
                                foreach(var image in file.textura.images)
                                {
                                    Raiz3.Nodes.Add(Path.GetFileName(filename) + "_" + i + "_" + j + "_" + y + " -Imagem");
                                    y++;
                                }
                                Raiz2.Nodes.Add(Raiz3);

                            }
                            else if (file.type == Types.Unknow)
                                Raiz2.Nodes.Add(Path.GetFileName(filename) + "_" + i + "_" + j);
                            

                            j++;
                        }
                    }
                }
                catch (NullReferenceException) { }
                Raiz.Nodes.Add(Raiz2);
                i++;
              
            }
            tree.Nodes.Add(Raiz);
        }
        #region Estruturas
        /*
       [Pastas] A estrutura da pastas é:

            Offset - Tipo - Valor
            ----------------------
            0 - Uint32 - Index
            4 - Uint32 - Tamanho
            8 - Uint32 - Posição
            C - Uint32 - Null

       [Arquivos] A estrutura dos arquivos é:

            Offset - Tipo - Valor
            ----------------------
            0 - Uint32 - Contagem
            ---//
            4 - Uint32 - Posição
            8 - Uint32 - Tamanho
            C - Uint32 - Posição
            ...
    */
        #endregion
        byte[] pastas;
        public void Rebuild(string location, string filename)
        {
            #region Salvar
            #region Pastas
            var files = new List<byte[]>();
            var pastabin = new List<byte>();
            int totalsizefold = 0;
            foreach (var pasta in Pastas)
            {
                if (pasta.type == Types.Texto)
                {
                    pasta.Size = pasta.texto.Size;
                    byte[] xfiles = new byte[pasta.Size];
                    Array.Copy(pasta.texto.Data, 0, xfiles, 0, (int)pasta.Size);
                    files.Add(xfiles);
                    pasta.Size = (uint)xfiles.Length;
                    totalsizefold += xfiles.Length;
                    byte[] index = BitConverter.GetBytes(Convert.ToUInt32(pasta.Index));
                    byte[] position2 = BitConverter.GetBytes(Convert.ToUInt32(pasta.Position));
                    byte[] size2 = BitConverter.GetBytes(Convert.ToUInt32(pasta.Size));
                    pastabin.AddRange(index);
                    pastabin.AddRange(size2);
                    pastabin.AddRange(position2);
                    pastabin.AddRange(new byte[4]); 
                }
                else
                {
                    if (pasta.filescount != 0) //se a contagem de pastas é diferente de 0
                    {
                        #region Arquivos da Pasta
                        #region Ponteiros do Arquivo
                        var filebin = new List<byte>();
                        byte[] count = BitConverter.GetBytes(Convert.ToUInt32(pasta.Arquivos.Count));
                        filebin.AddRange(count);
                        int totalsize = 0;
                        byte[] size;
                        foreach (var file in pasta.Arquivos)
                        {
                            if (file.type == Types.Texto)
                            {
                                file.Size = file.texto.Size;
                            }
                            byte[] position = BitConverter.GetBytes(Convert.ToUInt32(file.Position));
                            size = BitConverter.GetBytes(Convert.ToUInt32(file.Size));
                            totalsize += (int)file.Size;
                            filebin.AddRange(position);
                            filebin.AddRange(size);

                        }
                        while (filebin.Count != pasta.Arquivos[0].Position)
                        {
                            filebin.Add(0);
                        }
                        #endregion
                        byte[] xfiles = new byte[totalsize + filebin.ToArray().Length + 0x20]; //esse ponto aqui MIGUEL!!
                        Array.Copy(filebin.ToArray(), xfiles, filebin.Count);
                        foreach (var file in pasta.Arquivos)
                        {
                            if (file.type == Types.Texto)
                            {
                                Array.Copy(file.texto.Data, 0, xfiles, (int)file.Position, (int)file.Size);

                            }
                            else
                                Array.Copy(file.FileData, 0, xfiles, (int)file.Position, (int)file.Size);
                        }
                        files.Add(xfiles);
                        pasta.Size = (uint)xfiles.Length;
                        totalsizefold += xfiles.Length;
                        #endregion
                    }
                    #region Ponteiros da Pasta
                    byte[] index = BitConverter.GetBytes(Convert.ToUInt32(pasta.Index));
                    byte[] position1 = BitConverter.GetBytes(Convert.ToUInt32(pasta.Position));
                    byte[] size1 = BitConverter.GetBytes(Convert.ToUInt32(pasta.Size));
                    pastabin.AddRange(index);
                    pastabin.AddRange(size1);
                    pastabin.AddRange(position1);
                    pastabin.AddRange(new byte[4]);
                }
                #endregion
            }
            #endregion
            #region Salvar pastas
            pastas = new byte[totalsizefold + pastabin.Count];
            Array.Copy(pastabin.ToArray(), pastas, pastabin.Count);
            int i = 0;
            foreach (var past in files)
            {
                Array.Copy(past, 0, pastas, (int)Pastas[i].Position, (int)Pastas[i].Size);
                i++;
            }
            #endregion
            System.IO.File.WriteAllBytes(location+"/"+filename, pastas);
            #endregion
        }
        public void Import()
        {

        }
        public class Folder
        {
            public uint Index;
            public uint Position;
            public uint Size;
            public byte[] FileData;
            public Text texto;
            public Types type = Types.Unknow;
            public List<File> Arquivos;
            public Texture textura;
            public int filescount;
            public Folder(byte[] Data, int index, byte alfa)
            {
                #region Pasta de arquivos
                Index = (uint)index;
                Size = (uint)ReadUInt(Data, 4 + (16 * (index)), Int.UInt32);
                Position = (uint)ReadUInt(Data, 8 + (16 * (index)), Int.UInt32);
                FileData = Bin.ReadBlock(Data, Position, Size);
                //MessageBox.Show("Índice: " + Index.ToString() + "\n" +
                //    "Posição: " + Position.ToString("X2") + "\n" +
                //    "Tamanho: " + Size.ToString("X2"));
                #endregion
                #region Arquivos dentro da Pasta
                filescount = (int)ReadUInt(FileData, 0, Int.UInt32);
                if(filescount==0|filescount>0x9f|ReadUInt(FileData, FileData.Length-2,Int.UInt16).ToString("X2")=="8000")
                {
                    if (ReadUInt(FileData, FileData.Length - 2, Int.UInt16).ToString("X2") == "8000")
                    {
                        type = Types.Texto;
                        texto = new Text(FileData, index);
                    }
                    if (ReadUInt(FileData, 16, Int.UInt32).ToString("X2") == "8004")
                    {
                        type = Types.Textura;
                        textura = new Texture(FileData, index, alfa);
                    }
                    return;
                }
                Arquivos = new List<File>();
                for (int i = 0; i < filescount; i++)
                {
                    Arquivos.Add(new File(this, i, alfa));
                }
                #endregion

            }

        }
        public class File
        {
            public int Index;
            public uint Position;
            public uint Size;
            public Types type = Types.Unknow;
            public byte[] FileData;
            public Text texto;
            public Texture textura;
            public File(Folder folder, int index,byte alfa)
            {
                Index = index;
                Position = (uint)ReadUInt(folder.FileData, 4 + (8 * (index)) , Int.UInt32);
                Size = (uint)ReadUInt(folder.FileData, 8 + (8 * (index)), Int.UInt32);
                FileData = Bin.ReadBlock(folder.FileData, Position, Size);
                if (FileData.Length > 20)
                {
                    if (ReadUInt(FileData, FileData.Length - 2, Int.UInt16).ToString("X2") == "8000")
                    {
                        type = Types.Texto;
                        texto = new Text(FileData, index);
                    }
                    if(ReadUInt(FileData, 16, Int.UInt32).ToString("X2")=="8004")
                    {
                        type = Types.Textura;
                        textura = new Texture(FileData, index, alfa);
                    }
                }
                //MessageBox.Show("Índice: " + Index.ToString() + "\n" +
                //    "Posição: " + Position.ToString("X2") + "\n" +
                //    "Tamanho: " + Size.ToString("X2"));
            }
        }
        public class Text
        {
            public byte[] Data;
            public int Index;
            public uint Position;
            public uint Size;
            public uint SeqCount;
            public List<byte[]> sequences;
            public List<int> seqpointers;

            public Text(byte[] file, int index)
            {
                sequences = new List<byte[]>();
                Index = index;
                Position = 0;
                Size = (uint)file.Length;
                Data = Bin.ReadBlock(file, 0, Size);
                SeqCount = (uint)ReadUInt(Data, (int)Position, Int.UInt32);
                int pos = 4;
                for (int i = 0; i < SeqCount; i++)
                {
                    sequences.Add(ReadSequence(Data, pos + (i * 4), "8001"));
                }
            }
            public void Save()
            {
                //tamanho do bloco de pointers: Contagem *4 +4(contagem uint32)
                #region Iniciar Array de Ponteiro/Texto
                int tablesize = ((int)SeqCount * 4) + 4;
                var table = new List<byte>();
                table.AddRange(BitConverter.GetBytes(Convert.ToInt32(SeqCount)));
                #endregion
                int offset = tablesize;
                for(int ia=0;ia<SeqCount;ia++)
                {
                    byte[] off = BitConverter.GetBytes(Convert.ToInt32(offset));
                    table.AddRange(off);
                    offset += sequences[ia].Length + 2;
                }
                foreach(var s in sequences)
                {
                    table.AddRange(s);
                    table.Add(0);
                    table.Add(0x80);
                }
                
                Size = (uint)table.Count;
                Data = new byte[Size];
                Data = table.ToArray();
            }
            public int AllLength(List<byte[]> data)
            {
                int length = 0;
                foreach(var d in data)
                {
                    length += d.Length+2;
                }
                return length;
            }
        }
        public class Texture
        {
            public byte[] Data;
            public int Index;
            public uint Position;
            public uint Size;
            public uint Count;
            public List<Image> images;
            public List<TextureDATA.TEXs> TEXs;
            public List<TextureDATA.CLUTs> CLUTs;

            public Texture(byte[] file, int index, byte alfa)
            {
                #region Variáveis
                images = new List<Image>();
                Index = index;
                Position = 0;
                int bpp=8;
                TEXs = new List<TextureDATA.TEXs>();
                CLUTs = new List<TextureDATA.CLUTs>();
                byte[] TEX = null, CLT = null;
                Types types;
                #endregion
                #region Tamanho, Array e Contagem de texturas
                Size = (uint)file.Length;
                Data = Bin.ReadBlock(file, 0, Size);
                Count = (uint)ReadUInt(Data, 0, Int.UInt32);
                #endregion
                #region Offset de tabela de TEX e CLT
                int tableoffset = 16 + (80 * (int)Count + 20 *(int)Count);
                #endregion
                #region Verificar e Adicionar TEX/CLT
                for (int t = 1;t<=Count*2;t++)
                {
                    uint t1 = (uint)ReadUInt(Data, (tableoffset + 8), Int.UInt16);
                    uint t2 = (uint)ReadUInt(Data, (tableoffset + 10), Int.UInt16);
                    uint t3 = (uint)ReadUInt(Data, (tableoffset + 12), Int.UInt16);
                    uint type2 = t1 * t2 * t3; //a multiplicação resulta num id para a paleta(
                    //se for 16 >> 4 bpp (16)
                    //se for 1024 >> 8bpp(256)
                    //se for qualquer outra coisa >> TEX(Data de Textura)
                    #region Verificar Tipo
                    switch (type2)
                    {
                        case 1024:
                            types = Types.CLT;
                            break;
                        case 16:
                            types = Types.CLT;
                            break;
                        case 0:
                            types = Types.Unknow;
                            break;
                        default:
                            types = Types.TEX;
                            break;
                    }
                    #endregion
                    #region Adicionar tipos separadamente(TextureDATA)
                    switch (types)
                    {
                        case Types.TEX:
                        uint texoffset = (uint)ReadUInt(Data, (tableoffset + 4), Int.UInt32);
                        uint width = (uint)ReadUInt(Data, (tableoffset + 8), Int.UInt16);
                        uint height = (uint)ReadUInt(Data, (tableoffset + 10), Int.UInt16);
                        uint height2 = (uint)ReadUInt(Data, (tableoffset + 12), Int.UInt32);
                        uint size = width * height * 4;
                        width *= 2;
                        height *= 2;
                        texoffset += (uint)tableoffset;
                        TEX = Bin.ReadBlock(Data, texoffset, size);
                        tableoffset += 16;
                            TEXs.Add(new TextureDATA.TEXs(TEX, width, height));
                            break;
                        case Types.CLT:
                            uint cltoffset = (uint)ReadUInt(Data, (tableoffset + 4), Int.UInt32);
                            uint f1 = (uint)ReadUInt(Data, (tableoffset + 8), Int.UInt16);
                            uint f2 = (uint)ReadUInt(Data, (tableoffset + 10), Int.UInt16);
                            uint f3 = (uint)ReadUInt(Data, (tableoffset + 12), Int.UInt16);
                            cltoffset += (uint)tableoffset;
                            uint type = f1 * f2 * f3;
                            CLT = Bin.ReadBlock(Data, cltoffset, 1024);
                            tableoffset += 16;
                            if (type == 16)
                                bpp = 4;
                            else if (type == 1024)
                                bpp = 8;
                            CLUTs.Add(new TextureDATA.CLUTs(CLT, bpp));
                            break;
                        case Types.Unknow:
                            return;
                            break;
                    }
                    #endregion
                }
                #endregion
                #region Converter TEX e CLUT para Imagens
                int i = 0;
                foreach(TextureDATA.CLUTs clut in CLUTs) {
                    if (clut.bpp == 8)
                    {
                        byte[] unswizzled = null;
                        clut.ChangeAlfa(alfa);
                        Color[] colors = Rainbow.ImgLib.Encoding.ColorCodec.CODEC_32BIT_RGBA.DecodeColors(clut.CLUT);
                        colors = unswizzlePalette(colors);
                        unswizzled = UnSwizzle8(TEXs[i].width, TEXs[i].height, TEXs[i].TEX);
                        Rainbow.ImgLib.Encoding.ImageDecoderIndexed imageDecoder = new Rainbow.ImgLib.Encoding.ImageDecoderIndexed(unswizzled, TEXs[i].width, TEXs[i].height, Rainbow.ImgLib.Encoding.IndexCodec.FromNumberOfColors(256, Rainbow.ImgLib.Common.ByteOrder.BigEndian), colors);
                        images.Add(imageDecoder.DecodeImage());
                    }
                    i++;
                }
                #endregion
            }

        }
        public class TextureDATA
        {
            public class TEXs
            {
                public byte[] TEX;
                public int width = 0;
                public int height = 0;
                public TEXs(byte[] TEXs, uint Width, uint Height)
                {
                    TEX = TEXs;
                    width = (int)Width;
                    height = (int)Height;
                }
            }
            public class CLUTs
            {
                public byte[] CLUT,CL;
                public int bpp;
                public CLUTs(byte[] CLT,int Bpp)
                {
                    CL = CLT;
                    CLUT = CLT;
                    bpp = Bpp;
                }
                public void ChangeAlfa(byte alfa)
                {
                    CLUT = new byte[CL.Length];
                    for (int i = 0; i < CL.Length; i += 4)
                    {
                        CLUT[i] = CL[i];
                        CLUT[i + 1] = CL[i + 1];
                        CLUT[i + 2] = CL[i + 2];
                        if (CL[i + 3] == 0x80)
                            CLUT[i + 3] = alfa;
                        else
                            CLUT[i + 3] = CL[i + 3];
                    }
                }
            }
        }
        #region Swizzlers/Unswizzlers
        public static byte[] UnSwizzle4(byte[] buffer, int width, int height, int where)
        {
            //Fireboyd78
            // HUGE THANKS TO:
            // L33TMasterJacob for finding the information on unswizzling 4-bit textures
            // Dageron for his 4-bit unswizzling code; he's truly a genius!
            //
            // Source: https://gta.nick7.com/ps2/swizzling/unswizzle_delphi.txt

            byte[] InterlaceMatrix = {
        0x00, 0x10, 0x02, 0x12,
        0x11, 0x01, 0x13, 0x03,
    };

            int[] Matrix = { 0, 1, -1, 0 };
            int[] TileMatrix = { 4, -4 };

            var pixels = new byte[width * height];
            var newPixels = new byte[width * height];

            var d = 0;
            var s = where;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < (width >> 1); x++)
                {
                    var p = buffer[s++];

                    pixels[d++] = (byte)(p & 0xF);
                    pixels[d++] = (byte)(p >> 4);
                }
            }

            // not sure what this was for, but it actually causes issues
            // we can just use width directly without issues!
            //var mw = width;

            //if ((mw % 32) > 0)
            //    mw = ((mw / 32) * 32) + 32;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var oddRow = ((y & 1) != 0);

                    var num1 = (byte)((y / 4) & 1);
                    var num2 = (byte)((x / 4) & 1);
                    var num3 = (y % 4);

                    var num4 = ((x / 4) % 4);

                    if (oddRow)
                        num4 += 4;

                    var num5 = ((x * 4) % 16);
                    var num6 = ((x / 16) * 32);

                    var num7 = (oddRow) ? ((y - 1) * width) : (y * width);

                    var xx = x + num1 * TileMatrix[num2];
                    var yy = y + Matrix[num3];

                    var i = InterlaceMatrix[num4] + num5 + num6 + num7;
                    var j = yy * width + xx;

                    newPixels[j] = pixels[i];
                }
            }

//if UNSWIZZLE_TO_4BIT
    var result = new byte[width * height];

    s = 0;
    d = 0;

    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < (width >> 1); x++)
            result[d++] = (byte)((newPixels[s++] & 0xF) | (newPixels[s++] << 4));
    }
    return result;
//#else
            // return an 8-bit texture
            return newPixels;
//#endif
        }
        #region Code from: https://github.com/TGEnigma/DDS3-Model-Studio/blob/d96e6b3e7821f0d527105641be6e300521291e6f/Source/DDS3ModelLibrary/PS2/GS/GSPixelFormatHelper.cs
        public static byte[] UnSwizzle8(int width, int height, byte[] paletteIndices)
        {
            var newPaletteIndices = new byte[paletteIndices.Length];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int blockLocation = (y & (~0xF)) * width + (x & (~0xF)) * 2;
                    int swapSelector = (((y + 2) >> 2) & 0x1) * 4;
                    int positionY = (((y & (~3)) >> 1) + (y & 1)) & 0x7;
                    int columnLocation = positionY * width * 2 + ((x + swapSelector) & 0x7) * 4;
                    int byteNumber = ((y >> 1) & 1) + ((x >> 2) & 2); // 0,1,2,3
                    newPaletteIndices[y * width + x] = paletteIndices[blockLocation + columnLocation + byteNumber];
                }
            }

            return newPaletteIndices;
        }
        public static byte[] Swizzle8(int width, int height, byte[] paletteIndices)
        {
            var newPaletteIndices = new byte[paletteIndices.Length];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte uPen = paletteIndices[(y * width + x)];

                    int blockLocation = (y & (~0xF)) * width + (x & (~0xF)) * 2;
                    int swapSelector = (((y + 2) >> 2) & 0x1) * 4;
                    int positionY = (((y & (~3)) >> 1) + (y & 1)) & 0x7;
                    int columnLocation = positionY * width * 2 + ((x + swapSelector) & 0x7) * 4;
                    int byteNumber = ((y >> 1) & 1) + ((x >> 2) & 2); // 0,1,2,3

                    newPaletteIndices[blockLocation + columnLocation + byteNumber] = uPen;
                }
            }

            return newPaletteIndices;
        }
        #endregion
        public static Color[] unswizzlePalette(Color[] palette)
        {
            if (palette.Length == 256)
            {
                Color[] unswizzled = new Color[palette.Length];

                int j = 0;
                for (int i = 0; i < 256; i += 32, j += 32)
                {
                    copy(unswizzled, i, palette, j, 8);
                    copy(unswizzled, i + 16, palette, j + 8, 8);
                    copy(unswizzled, i + 8, palette, j + 16, 8);
                    copy(unswizzled, i + 24, palette, j + 24, 8);
                }
                return unswizzled;
            }
            else
            {
                return palette;
            }
        }

        private static void copy(Color[] unswizzled, int i, Color[] swizzled, int j, int num)
        {
            for (int x = 0; x < num; ++x)
            {
                unswizzled[i + x] = swizzled[j + x];
            }
        }
#endregion
        public enum Int
        {
            Int16,
            Int32,
            Int64,
            UInt16,
            UInt32,
            UInt64
        };
        public enum Types
        {
            Texto,
            Textura,
            Unknow,
            CLT,
            TEX
        };
        public static byte[] ReadSequence(byte[] file, int offset, string breaker)
        {
            var sequence = new List<byte>();
            var memory = new MemoryStream(file);
            var reader = new BinaryReader(memory);
            reader.BaseStream.Position = offset;
            uint pointer = reader.ReadUInt32();
            reader.Close();
            memory.Close();
            for (uint i = pointer; file[i].ToString("X2") + file[i + 1].ToString("X2") != "0080"; i += 2)
            {
                //MessageBox.Show(file[i].ToString("X2") + file[i + 1].ToString("X2"));
                sequence.Add(file[i]);
                sequence.Add(file[i + 1]);
            }
            return sequence.ToArray();
        }
        public static ulong ReadUInt(byte[] s, int offset, Int type)
        {
            ulong retur = 0;
            var memory = new MemoryStream(s);
            var reader = new BinaryReader(memory);
            reader.BaseStream.Position = offset;
            switch(type)
            {
                case Int.UInt16:
                    retur = reader.ReadUInt16();
                    break;
                case Int.UInt32:
                    retur = reader.ReadUInt32();
                    break;
                case Int.UInt64:
                    retur = reader.ReadUInt64();
                    break;
            }
            reader.Close();
            memory.Close();
            return retur;
        }

    }
}
