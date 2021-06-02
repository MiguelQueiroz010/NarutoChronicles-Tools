﻿#define UNSWIZZLE_TO_4BIT
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using Rainbow.ImgLib.Encoding;
using System.Text;
using System.IO;
#region Usando RainbowImgLib https://github.com/marco-calautti/Rainbow/tree/master/Rainbow.ImgLib
using Rainbow.ImgLib;
using Microsoft.Win32;
using System.Reflection;
using System.Xml;
#endregion
namespace NUC_Raw_Tools.ArquivoRAW
{
    public class RAW
    {
        public byte[] Data;
        public int folderCount;
        public List<Folder> Pastas;
        public RAW(byte[] opened)
        {
            #region Abertura e verificar se é pasta
            Data = opened;
            folderCount = (int)ReadUInt(Data, 8, Int.UInt32) / 0x10;
            if (folderCount == 0)
            {
                MessageBox.Show("Erro: Arquivo inválido!!");
                return;
            }
            Pastas = new List<Folder>();
            for (int i = 0; i < folderCount; i++)
            {
                //if (ReadUInt(Data, 4, Int.UInt32) > 0xf)
                Pastas.Add(new Folder(Data, i));
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
                TreeNode Raiz4 = null;
                if (folder.type == Types.Texto)
                    Raiz2 = new TreeNode(Path.GetFileName(filename) + "_" + i);
                else if (folder.type == Types.Textura)
                    Raiz2 = new TreeNode(Path.GetFileName(filename) + "_" + i);
                else if (folder.type == Types.Unknow)
                    Raiz2 = new TreeNode(Path.GetFileName(filename) + "_" + i);
                else if (folder.type == Types.Link)
                    Raiz2 = new TreeNode(Path.GetFileName(filename) + "_" + i);
                else
                    Raiz2 = new TreeNode(Path.GetFileName(filename) + "_" + i);

                if (folder.filescount != 0)
                {

                    if (folder.Arquivos != null)
                    {
                        int j = 0;

                        foreach (var file in folder.Arquivos)
                        {
                            if (file.type == Types.Texto)
                                Raiz2.Nodes.Add(Path.GetFileName(filename) + "_" + i + "_" + j);
                            else if (file.type == Types.Textura)
                            {
                                TreeNode Raiz3 = new TreeNode(Path.GetFileName(filename) + "_" + i + "_" + j);
                                int y = 0;
                                for (int c = 0; c < file.textura.TextureCount; c++)
                                {
                                    Raiz3.Nodes.Add(Path.GetFileName(filename) + "_" + i + "_" + j + "_" + y + " -Imagem");
                                    y++;
                                }
                                Raiz2.Nodes.Add(Raiz3);

                            }
                            else if (file.type == Types.Unknow)
                                Raiz2.Nodes.Add(Path.GetFileName(filename) + "_" + i + "_" + j);
                            else if (file.type == Types.Link)
                                Raiz2.Nodes.Add(Path.GetFileName(filename) + "_" + i + "_" + j);
                            else if (file.type == Types.Null)
                                Raiz2.Nodes.Add("Espaço reservado - Vazio");
#if Pasta_inside_file
                            else if (file.type == Types.Pasta)
                            {
                                int xid = 0;
                                try { Raiz4 = new TreeNode(Path.GetFileName(filename) + "_" + i + "_" + j + "-Pasta");
                                //xid = 0;
                                Folder fd = file.pasta;
                                foreach(var fileINSIDEfile in file.pasta.Arquivos)
                                {
                                    if (fileINSIDEfile.type == Types.Texto)
                                        Raiz4.Nodes.Add(Path.GetFileName(filename) + "_" + i + "_" + j+"_"+ xid);
                                    else if (fileINSIDEfile.type == Types.Textura)
                                    {
                                        TreeNode Raiz5 = new TreeNode(Path.GetFileName(filename) + "_" + i + "_" + j + "_" + xid);
                                        int y = 0;
                                        for (int c = 0; c < fileINSIDEfile.textura.TextureCount; c++)
                                        {
                                            Raiz5.Nodes.Add(Path.GetFileName(filename) + "_" + i + "_" + j + "_" + y + "_" + xid + " -Imagem");
                                            y++;
                                        }
                                        Raiz4.Nodes.Add(Raiz5);

                                    }
                                    else if (fileINSIDEfile.type == Types.Unknow)
                                        Raiz4.Nodes.Add(Path.GetFileName(filename) + "_" + i + "_" + j + "_" + xid);
                                    else if (fileINSIDEfile.type == Types.Link)
                                        Raiz4.Nodes.Add(Path.GetFileName(filename) + "_" + i + "_" + j + "_" + xid);
                                    else if (fileINSIDEfile.type == Types.Null)
                                        Raiz4.Nodes.Add("Espaço reservado - Vazio");
                                    else
                                        Raiz4.Nodes.Add(Path.GetFileName(filename) + "_" + i + "_" + j + "_" + xid);

                                    xid++;
                                }
                                }
                                catch (Exception) { }
                                Raiz2.Nodes.Add(Raiz4);
                            }
#endif
                            else
                                Raiz2.Nodes.Add(Path.GetFileName(filename) + "_" + i + "_" + j);
                            j++;
                        }
                    }
                }
                Raiz.Nodes.Add(Raiz2);
                i++;

            }
            tree.Nodes.Add(Raiz);
            tree.ExpandAll();
            #region Colorir Tipos
            int f = 0;
            foreach (TreeNode node in tree.Nodes)
            {
                int k = 0;
                foreach (TreeNode treeNode in node.Nodes)
                {
                    int c = 0;
                    foreach (TreeNode node1 in treeNode.Nodes)
                    {
                        switch (raw.Pastas[k].Arquivos[c].type)
                        {
                            case Types.Link:
                                node1.ForeColor = Color.DarkGreen;
                                break;
                            case Types.Null:
                                node1.ForeColor = Color.Red;
                                break;
                            case Types.Texto:
                                node1.ForeColor = Color.Blue;
                                break;
                            case Types.Unknow:
                                node1.ForeColor = Color.DarkGray;
                                break;
                            case Types.Pasta:
                                node1.ForeColor = Color.DarkOrange;
                                break;
                            case Types.Textura:
                                node1.ForeColor = Color.Chocolate;
                                break;
                        }

                        c++;
                    }
                    k++;
                }
                f++;
            }

            #endregion
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
        public void ColorChild(TreeNode nodes, int indx, Color color)
        {
            foreach (TreeNode node_tmp in nodes.Nodes[indx].Nodes)
            {
                node_tmp.ForeColor = color;
                foreach (TreeNode node_tmp2 in node_tmp.Nodes)
                {
                    node_tmp2.ForeColor = color;
                    foreach (TreeNode node_tmp3 in node_tmp2.Nodes)
                    {
                        node_tmp3.ForeColor = color;
                        foreach (TreeNode node_tmp4 in node_tmp3.Nodes)
                        {
                            node_tmp4.ForeColor = color;
                            foreach (TreeNode node_tmp5 in node_tmp4.Nodes)
                            {
                                node_tmp5.ForeColor = color;
                            }
                        }
                    }
                }
            }
        }
        public void Rebuild(RAW raw, string path, string name)
        {
            #region Ponteiros e Pastas no ROOT
            var root = new List<byte>();
            int offset = 0x10 * raw.folderCount;
            foreach (Folder folder in raw.Pastas)
            {
                int padd = 0;
                if (folder.Arquivos != null)
                {
                    folder.SaveChanges();
                }
                root.AddRange(BitConverter.GetBytes((UInt32)folder.Index));
                root.AddRange(BitConverter.GetBytes((UInt32)folder.FileData.Length));
                root.AddRange(BitConverter.GetBytes((UInt32)offset));
                root.AddRange(BitConverter.GetBytes((UInt32)0));
                offset += folder.FileData.Length;
                while (offset % 0x10 != 0)
                {
                    offset++;
                    padd++;
                }
                folder.Size += (uint)padd;
                Array.Resize(ref folder.FileData, (int)folder.Size);
            }
            foreach (Folder fold in raw.Pastas)
                root.AddRange(fold.FileData);
            #endregion
            Data = root.ToArray();
            //#region Salvar o arquivo
            //System.IO.File.WriteAllBytes(path + @"/" + name, root.ToArray());
            //#endregion
        }
        public class Folder
        {
            public uint Index;
            uint Position;
            public uint Size;
            public int filescount;
            public byte[] FileData;
            public Text texto;
            public List<File> Arquivos;
            public TextureArray textura;
            public Types type = Types.Unknow;
            public Folder(byte[] Data, int index, bool isinside = false)
            {
                #region Pasta de arquivos
                if (isinside == false)
                {
                    Index = (uint)index;
                    Size = (uint)ReadUInt(Data, 4 + (16 * (index)), Int.UInt32);
                    Position = (uint)ReadUInt(Data, 8 + (16 * (index)), Int.UInt32);
                    FileData = Bin.ReadBlock(Data, Position, Size);
                }
                else
                    FileData = Data;
                //MessageBox.Show("Índice: " + Index.ToString() + "\n" +
                //    "Posição: " + Position.ToString("X2") + "\n" +
                //    "Tamanho: " + Size.ToString("X2"));
                #endregion
                #region Arquivos dentro da Pasta
                filescount = (int)ReadUInt(FileData, 0, Int.UInt32);
                if (filescount == 0 | filescount > 0x9f | ReadUInt(FileData, FileData.Length - 2, Int.UInt16).ToString("X2") == "8000")
                {
                    if (ReadUInt(FileData, FileData.Length - 2, Int.UInt16).ToString("X2") == "8000")
                    {
                        type = Types.Texto;
                        texto = new Text(null, this, index);
                    }
                    if (ReadUInt(FileData, 16, Int.UInt32).ToString("X2") == "8004")
                    {
                        type = Types.Textura;
                        textura = new TextureArray(null, this, FileData, index);
                    }
                    return;
                }
                int test = (int)ReadUInt(FileData, 4, Int.UInt32) - ((int)ReadUInt(FileData, 0, Int.UInt32) * 8);
                int soma = 0;
                if (test < (int)ReadUInt(FileData, 4, Int.UInt32) && test > 0)
                    soma = test + ((int)ReadUInt(FileData, 0, Int.UInt32) * 8);
                int ste = filescount * 8 + 4;
                while (ste % 0x10 != 0)
                    ste++;
                if ((int)ReadUInt(FileData, 4, Int.UInt32) != 0 && test < (int)ReadUInt(FileData, 4, Int.UInt32) && soma == (int)ReadUInt(FileData, 4, Int.UInt32) && (int)ReadUInt(FileData, 4, Int.UInt32) == ste)
                {
                    Arquivos = new List<File>();
                    for (int i = 0; i < filescount; i++)
                    {
                        Arquivos.Add(new File(this, i));
                    }
                    #region Separar LINKS nos arquivos
                    foreach (var file in Arquivos)
                    {
                        if (file.type == Types.Link)
                        {
                            foreach (var searchfile in Arquivos)
                                if (file.Position == searchfile.Position && searchfile.type != Types.Link)
                                    file.LinkerIndex = searchfile.Index;
                        }
                    }
                    #endregion
                    #region Separar Padding nos arquivos
                    var verifier = new List<File>();
                    foreach (var fi in Arquivos)
                    {
                        if (fi.Position != 0 && fi.type != Types.Null && fi.type != Types.Link && fi.type != Types.Empty)
                        {
                            verifier.Add(fi);
                        }
                    }
                    int padd = 0;
                    for (int l = 0; l < verifier.Count; l++)
                    {

                        if (l == verifier.Count - 1)
                        {
                            padd = FileData.Length - (int)verifier[l].Position;
                        }
                        else if (l != verifier.Count)
                        {
                            padd = (int)(verifier[l + 1].Position - verifier[l].Position);
                        }
                        verifier[l].Padding = padd - (int)verifier[l].Size;
                    }
                    for (int k = 0; k < Arquivos.Count; k++)
                    {
                        foreach (var f in verifier)
                        {
                            if (f.Index == Arquivos[k].Index)
                                Arquivos[k] = f;
                        }
                    }
                    //Teste de padding:
                    //foreach(var p in Arquivos)
                    //{
                    //    if(p.type!=Types.Null&&p.type!=Types.Link&&p.type!=Types.Empty)
                    //      MessageBox.Show("Padding: " + p.Padding.ToString("X2"));
                    //}
                    #endregion
                }
                #endregion

                //Teste dos Links
                //foreach (var arq in Arquivos)
                //    if (arq.type == Types.Link)
                //    {
                //        MessageBox.Show("Posição: " + arq.Position.ToString("X2")+"\n"+
                //            "Size: " + Arquivos[arq.LinkerIndex].Size.ToString("X2")+"\n"+
                //            "Tipo do link: "+ Arquivos[arq.LinkerIndex].type.ToString()+"\n"+
                //            "Atalho para: "+arq.LinkerIndex.ToString());
                //    }
            }

            public void SaveChanges()
            {
                int foffset = (int)ReadUInt(FileData, 4, Int.UInt32);
                #region Salvar tipos
                //foreach(var fi in Arquivos)
                //{
                //    if (fi.type != Types.Null && fi.type != Types.Link && fi.type != Types.Empty)
                //    {
                //        switch (fi.type)
                //        {
                //            case Types.Texto:
                //                fi.Size = fi.texto.Size;
                //                fi.FileData = new byte[fi.texto.Size];
                //                fi.FileData = fi.texto.Data;
                //                break;
                //        }
                //    }
                //}
                #endregion//NULL
                #region Calcular novas Posições
                int offset = foffset;
                foreach (var file in Arquivos)
                {
                    if (file.type != Types.Null && file.type != Types.Link && file.type != Types.Empty)
                    {
                        int padd = 0;
                        file.Position = (uint)offset;
                        offset += (int)file.Size;
                        while (offset % 0x10 != 0)
                        {
                            offset++;
                            padd++;
                        }
                        Array.Resize(ref file.FileData, ((int)file.Size) + padd);
                    }
                }
                foreach (var f in Arquivos)
                {
                    if (f.type == Types.Link)
                    {
                        f.Position = Arquivos[f.LinkerIndex].Position;
                    }
                }//Linkar os LINKS
                #endregion
                #region Tabela de Ponteiros
                var ptable = new List<byte>();
                //Adicionar a contagem de arquivos
                ptable.AddRange(BitConverter.GetBytes((UInt32)filescount));
                #region Para cada arquivo
                for (int i = 0; i < filescount; i++)
                {
                    ptable.AddRange(BitConverter.GetBytes((UInt32)Arquivos[i].Position));
                    ptable.AddRange(BitConverter.GetBytes((UInt32)Arquivos[i].Size));
                }
                #endregion
                while (ptable.Count != foffset)
                {
                    ptable.Add(0);
                }//Enquanto a quantia diferir o total, adicionar padding no fim da tabela
                #endregion
                #region Adicionar arquivos
                foreach (var fsav in Arquivos)
                {
                    if (fsav.type != Types.Null && fsav.type != Types.Link && fsav.type != Types.Empty)
                    {
                        ptable.AddRange(fsav.FileData);
                    }
                }
                FileData = new byte[ptable.Count];
                FileData = ptable.ToArray();
                Size = (uint)FileData.Length;
                #endregion
            }
        }
        public class File
        {
            public int Index, Padding, LinkerIndex;
            public uint Position;
            public uint Size;
            public Types type = Types.Unknow;
            public byte[] FileData;
            public Text texto;
            public TextureArray textura;
            public Folder pasta;
            public File(Folder folder, int index)
            {
                #region Arquivo
                Index = index;
                Position = (uint)ReadUInt(folder.FileData, 4 + (8 * (index)), Int.UInt32);
                Size = (uint)ReadUInt(folder.FileData, 8 + (8 * (index)), Int.UInt32);
                FileData = Bin.ReadBlock(folder.FileData, Position, Size);
                //MessageBox.Show("Índice: " + Index.ToString() + "\n" +
                //    "Posição: " + Position.ToString("X2") + "\n" +
                //    "Tamanho: " + Size.ToString("X2") + "\n" +
                //    "Padding: " + Padding.ToString() + "\n" +
                //    "Tipo: " + type.ToString());
                #endregion
                #region Separar os Tipos
                if (FileData.Length == 0 && Position != 0)
                {
                    type = Types.Link;
                }
                else if (FileData.Length > 0)
                {
                    type = Types.Unknow;
                    if (ReadUInt(FileData, FileData.Length - 2, Int.UInt16).ToString("X2") == "8000")
                    {
                        type = Types.Texto;
                        texto = new Text(this, null, index);
                    }
                    if (ReadUInt(FileData, 0x10, Int.UInt32).ToString("X2") == "8004")
                    {
                        type = Types.Textura;
                        textura = new TextureArray(this, null, FileData, index);

                        //catch (Exception) { type = Types.Unknow; }
                    }
                    try
                    {
                        if (ReadUInt(FileData, 0x04, Int.UInt32) > (ReadUInt(FileData, 0, Int.UInt32) * 8))
                        {
                            type = Types.Pasta;
                            pasta = new Folder(FileData, index, true);
                        }
                    }
                    catch (Exception) { type = Types.Null; }
                }
                else
                {
                    type = Types.Null;
                }
                #endregion
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
            private File intfile;
            private Folder intfold;

            public Text(File fil, Folder fold, int index)
            {
                if (fil != null)
                {
                    Data = fil.FileData;
                    intfile = fil;
                }
                if (fold != null)
                {
                    Data = fold.FileData;
                    intfold = fold;
                }
                sequences = new List<byte[]>();
                Index = index;
                Position = 0;
                Size = (uint)Data.Length;
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
                for (int ia = 0; ia < SeqCount; ia++)
                {
                    byte[] off = BitConverter.GetBytes(Convert.ToInt32(offset));
                    table.AddRange(off);
                    offset += sequences[ia].Length + 2;
                }
                foreach (var s in sequences)
                {
                    table.AddRange(s);
                    table.Add(0);
                    table.Add(0x80);
                }

                Size = (uint)table.Count;
                Data = new byte[Size];
                Data = table.ToArray();
                if (intfile != null)
                {
                    intfile.Size = Size;
                    intfile.FileData = new byte[Size];
                    intfile.FileData = Data;
                }
                if (intfold != null)
                {
                    intfold.Size = Size;
                    intfold.FileData = new byte[Size];
                    intfold.FileData = Data;
                }

            }
            public int AllLength(List<byte[]> data)
            {
                int length = 0;
                foreach (var d in data)
                {
                    length += d.Length + 2;
                }
                return length;
            }
        }
        public class TextureArray
        {
            public byte[] Data;
            public int Index;
            public uint Position;
            public uint Size;
            public int TextureCount, PointersCount;

            public List<byte[]> DadosTipo1, DadosTipo2;
            public List<PointerEntry> ponteiros;
            public List<TextureEntries> texentries;
            public List<byte[]> Entries;

            private Folder folder;
            private File filex;

            public TextureArray(File fil, Folder fold, byte[] file, int index)
            {
                #region Variáveis
                Index = index;
                Position = 0;
                #endregion
                #region Tamanho, Array e Contagem de texturas
                Size = (uint)file.Length;
                if (fil != null)
                {
                    Data = fil.FileData;
                    filex = fil;
                }
                if (fold != null)
                {
                    Data = fold.FileData;
                    folder = fold;
                }
                #endregion
                #region Header
                TextureCount = (int)ReadUInt(Data, 0, Int.UInt32);
                PointersCount = (int)ReadUInt(Data, 4, Int.UInt32);
                //Offset 0 - Texture Count
                //Offset 4 - Pointers(Pixel and Color Data) Count
                //Offset 0x10 - Start of first data type(probaly the position in screen data or else)
                #endregion
                int pos = 0x10; //setar em 0 ou na posição de leitura!!

                #region Primeiros Dados (0x0480)
                DadosTipo1 = new List<byte[]>();
                for (int t = 0; t < TextureCount; t++)
                {
                    DadosTipo1.Add(Bin.ReadBlock(Data, (uint)pos, 0x50));
                    pos += 0x50; //de 0x50 em 0x50 bytes
                }
                #endregion

                #region Segundos Dados (0x0000FF)
                DadosTipo2 = new List<byte[]>();
                for (int t = 0; t < TextureCount; t++)
                {
                    DadosTipo2.Add(Bin.ReadBlock(Data, (uint)pos, 0x14));
                    pos += 0x14; //de 0x14 em 0x14 bytes
                }
                #endregion
                byte[] pointeranddata = Bin.ReadBlock(Data, (uint)pos, (uint)(Data.Length - pos));
                #region PointerRows
                ponteiros = new List<PointerEntry>();
                for (int tc = 0; tc < PointersCount; tc++)
                {
                    PointerEntry entry = new PointerEntry();
                    int offset = (int)Bin.ReadUInt(Data, pos + 4, Bin.Int.UInt32);
                    offset += 0x10 * tc;

                    int Width = (int)Bin.ReadUInt(Data, pos + 8, Bin.Int.UInt16);
                    int Height = (int)Bin.ReadUInt(Data, pos + 10, Bin.Int.UInt16);
                    int Height1 = (int)Bin.ReadUInt(Data, pos + 12, Bin.Int.UInt16);
                    entry.offset = offset;

                    entry.Width = Width;
                    entry.Height = Height;
                    entry.Height1 = Height1;

                    ponteiros.Add(entry);
                    pos += 0x10;

                }
                #region PointerRowsSizes
                for (int tc = 0; tc < PointersCount; tc++)
                {
                    int size = 0;
                    if (tc == PointersCount - 1) //final(usar length total)
                    {
                        size = (Data.Length - (0x10 + (0x50 * TextureCount) + (0x14 * TextureCount))) - ponteiros[tc].offset;
                    }
                    else //início pra frente
                    {
                        size = ponteiros[tc + 1].offset - ponteiros[tc].offset;

                    }
                    if (size <= 1024)
                    {
                        ponteiros[tc].tipo = Types.CLT8bpp;
                        ponteiros[tc].bpp = 8;
                    }
                    if (size <= 64)
                    {
                        ponteiros[tc].tipo = Types.CLT4bpp;
                        ponteiros[tc].bpp = 4;
                    }
                    if (size > 1024)
                    {
                        ponteiros[tc].tipo = Types.TEX;
                    }

                    ponteiros[tc].Size = size;

                }


                #endregion
                #endregion

                #region Separar Dados das Texturas
                Entries = new List<byte[]>();
                //PixelDatas = new List<byte[]>();
                //ColorDatas = new List<byte[]>();
                foreach (var ponteiro in ponteiros)
                {
                    Entries.Add(Bin.ReadBlock(pointeranddata, (uint)ponteiro.offset, (uint)ponteiro.Size));
                }
                #endregion

                #region Texture Entries(segundos dados)
                texentries = new List<TextureEntries>();
                for (int t = 0; t < TextureCount; t++)
                {
                    texentries.Add(new TextureEntries(DadosTipo2[t], ponteiros));
                }
                #endregion

            }
            public static byte FindColorIndex(Color v, Color[] pal)
            {

                byte index = 0;
                for (byte i = 0; i < pal.Length; i++)
                    if (pal[i].R == v.R &&
                        pal[i].G == v.G &&
                        pal[i].B == v.B &&
                        pal[i].A == v.A)
                        return i;

                return index;
            }
            byte[] unswizzledPixelData;
            public Image GetImage(int textureindex)
            {
                Image image = null;
                List<Color> pal = new List<Color>();
                int pos = 0;
                byte r, g, b, a;
                int pixelindex = texentries[textureindex].pixelindex;
                int paletteidx = texentries[textureindex].paletteindex;
                int width = texentries[textureindex].Width;
                int height = texentries[textureindex].Height;
                int dbw = (width / 2 + 0x07) & ~0x07;
                int dbh = (height / 2 + 0x07) & ~0x07;
                //MessageBox.Show("índice de textura: " + textureindex.ToString() + "\n" +
                //   "Pixel Index: "+pixelindex.ToString() + "\n" + "Palette index: " + paletteidx.ToString());
                while (pos < Entries[paletteidx].Length)
                {
                    r = Entries[paletteidx][pos];
                    g = Entries[paletteidx][pos + 1];
                    b = Entries[paletteidx][pos + 2];
                    a = Entries[paletteidx][pos + 3];
                    if (a <= 128)
                        a = (byte)((a * 255) / 128);
                    pal.Add(Color.FromArgb(a, r, g, b));
                    pos += 4;
                }
                Color[] cores = unswizzlePalette(pal.ToArray());
                byte[] coresbyte = new byte[256 * 4];//1024 bytes = 256 cores
                for (int i = 0; i < coresbyte.Length; i += 4)
                {
                    if ((i / 4) < cores.Length)
                    {
                        coresbyte[i] = cores[i / 4].R;
                        coresbyte[i + 1] = cores[i / 4].G;
                        coresbyte[i + 2] = cores[i / 4].B;
                        coresbyte[i + 3] = cores[i / 4].A;
                    }

                }
                if (cores.Length <= 256 && cores.Length > 64)
                {
                    unswizzledPixelData = UnSwizzle8(width, height, Entries[pixelindex]);
                    ImageDecoderIndexed imageDecoder = new ImageDecoderIndexed(unswizzledPixelData, width, height, IndexCodec.FromNumberOfColors(cores.Length, Rainbow.ImgLib.Common.ByteOrder.BigEndian), cores);
                    image = imageDecoder.DecodeImage();
                    return image;
                }
                if (cores.Length <= 16)
                {
                    //unswizzledPixelData = PixelDatas[textureindex];
                    //unswizzledPixelData = new byte[width * height];
                    //Array.Copy(UnswizzleTexture(Entries[pixelindex], width, height, PixelFormat.Format4bppIndexed), unswizzledPixelData, UnswizzleTexture(Entries[pixelindex], width, height, PixelFormat.Format4bppIndexed).Length);
                    //cores = pal.ToArray();
                    //unswizzledPixelData = unswizzlePS2(Entries[pixelindex], width, height / 2);
                    //MessageBox.Show(PixelDatas[textureindex].Length.ToString("X2"));
                    //MessageBox.Show((ponteiros[textureindex * 2].Width * ponteiros[textureindex * 2].Height).ToString("X2"));
                    //InvertEndianess(PixelDatas[textureindex]);
                    //unswizzledPixelData = UnSwizzle8(texentries[textureindex].Width, texentries[textureindex].Height, Entries[pixelindex]);
                    //unswizzle4(PixelDatas[textureindex],unswizzledPixelData, ponteiros[textureindex * 2].Width, ponteiros[textureindex * 2].Height);
                    //unswizzledPixelData = Unswizzle(Entries[pixelindex], 25);
                    //unswizzledPixelData = From4to8bpp(Entries[pixelindex]);
                    //unswizzledPixelData = UnSwizzle8(256, 256, unswizzledPixelData);
                    //Color[] ncor = new Color[256];
                    //Array.Copy(cores, ncor, cores.Length);
                    //unswizzledPixelData = P6T_ProcessImg4(texentries[textureindex].Width, texentries[textureindex].Height, Entries[pixelindex]);
                    //MessageBox.Show(unswizzledPixelData.Length.ToString("X2"));
                    //unswizzledPixelData = UnSwizzle4(Entries[pixelindex], texentries[textureindex].Width, texentries[textureindex].Height,0);
                    ImageDecoderIndexed imageDecoder = new ImageDecoderIndexed(Entries[pixelindex], width, height, IndexCodec.FromNumberOfColors(16, Rainbow.ImgLib.Common.ByteOrder.BigEndian), cores);
                    image = imageDecoder.DecodeImage();
                    return image;
                }
                return image;
            }
            public void ImportTexture(int index, Image inpt)
            {
                int pixidx = texentries[index].pixelindex;
                int paletidx = texentries[index].paletteindex;
                var k = GetPixelandColorData(inpt);
                Entries[pixidx] = k[0];
                Entries[paletidx] = k[1];
                RebuildData();
            }
            HashSet<Color> colors;
            Color[] cores; //composta de 4 ou 3 valores, RGBA
            byte[] coresbyte;
            public List<byte[]> GetPixelandColorData(Image input)
            {
                colors = new HashSet<Color>();
                coresbyte = null;
                cores = null;
                var list = new List<byte[]>();
                Bitmap bit = new Bitmap(input);
                bit.RotateFlip(RotateFlipType.Rotate180FlipX);
                int colorcount = 0;
                #region Obter cores no eixo cartesiano 2D        
                for (int y = 0; y < bit.Height; y++)
                {
                    for (int x = 0; x < bit.Width; x++)
                    {
                        colors.Add(bit.GetPixel(x, y));
                    }
                }
                cores = new Color[256];
                //cores = colors.ToArray();
                Array.Copy(colors.ToArray(), cores, colors.Count);
                cores = swizzlePalette(cores);
                #region Calcular quantia de cores
                if (cores.Length <= 256)
                    colorcount = 256;
                else if (cores.Length <= 16)
                    colorcount = 16;
                #endregion
                #region Separar cores para array
                coresbyte = new byte[colorcount * 4];//1024 bytes = 256 cores
                for (int i = 0; i < coresbyte.Length; i += 4)
                {
                    if ((i / 4) < cores.Length)
                    {
                        coresbyte[i] = cores[i / 4].R;
                        coresbyte[i + 1] = cores[i / 4].G;
                        coresbyte[i + 2] = cores[i / 4].B;
                        coresbyte[i + 3] = cores[i / 4].A;
                        if (cores[i / 4].A <= 255)
                            coresbyte[i + 3] = (byte)((cores[i / 4].A * 128) / 255);
                    }

                }
                #endregion
                #endregion
                #region Obter índices de pixel no eixo cartesiano 2D
                var pixeldata = new List<byte>();
                Color c1;
                for (int y = 0; y < bit.Height; y++)
                    for (int x = 0; x < bit.Width; x++)
                    {
                        c1 = bit.GetPixel(x, bit.Height - y - 1);
                        pixeldata.Add(FindColorIndex(c1, colors.ToArray()));
                    }
                #endregion
                byte[] swizzledpixdata = Swizzle8(input.Width, input.Height, pixeldata.ToArray());
                list.Add(swizzledpixdata);
                list.Add(coresbyte);
                return list;

            }
            void RebuildData()
            {
                var data = new List<byte>();
                var pointers = new List<byte>();
                #region Header
                data.AddRange(BitConverter.GetBytes((UInt32)TextureCount));
                data.AddRange(BitConverter.GetBytes((UInt32)PointersCount));
                data.AddRange(BitConverter.GetBytes((UInt64)0));
                #endregion
                #region DadosTipo1
                foreach (var dat1 in DadosTipo1)
                    data.AddRange(dat1);
                #endregion
                #region TextureDatas
                foreach (var dat2 in DadosTipo2)
                    data.AddRange(dat2);
                #endregion
                #region Ponteiros
                int i = 0;
                foreach (var points in ponteiros)
                {
                    pointers.AddRange(BitConverter.GetBytes((UInt32)0));
                    pointers.AddRange(BitConverter.GetBytes((UInt32)(points.offset - (i * 0x10))));
                    pointers.AddRange(BitConverter.GetBytes((UInt16)points.Width));
                    pointers.AddRange(BitConverter.GetBytes((UInt16)points.Height));
                    pointers.AddRange(BitConverter.GetBytes((UInt16)points.Height1));
                    pointers.AddRange(BitConverter.GetBytes((UInt16)0));
                    i++;
                }
                while (pointers.Count < ponteiros[0].offset)
                    pointers.Add(0);
                data.AddRange(pointers.ToArray());
                #endregion
                #region Dados
                foreach (byte[] ed in Entries)
                    data.AddRange(ed);
                #endregion
                Data = data.ToArray();
                Save();
            }
            public void Save()
            {
                if (filex != null)
                {
                    filex.Size = Size;
                    filex.FileData = new byte[Size];
                    filex.FileData = Data;
                }
                if (folder != null)
                {
                    folder.Size = Size;
                    folder.FileData = new byte[Size];
                    folder.FileData = Data;
                }
            }

        }
        public class PointerEntry
        {
            //4 null bytes
            public int offset; //padded in 0x10 related to the index of this pointer
            public int Width, Height, Height1; //Multiplied for 2
            public int Size, bpp = 8; //Size of data
            public Types tipo;
        }
        public class TextureEntries
        {
            public int pixelindex, paletteindex; //padded in 0x10 related to the index of this pointer
            public int Width, Height; //Multiplied for 2
            public int Bpp;
            public TextureEntries(byte[] data, List<PointerEntry> entries)
            {
                pixelindex = (int)Bin.ReadUInt(data, 0, Bin.Int.UInt16);
                paletteindex = (int)Bin.ReadUInt(data, 8, Bin.Int.UInt16);
                Width = (int)Bin.ReadUInt(data, 0x10, Bin.Int.UInt16);
                Height = (int)Bin.ReadUInt(data, 0x12, Bin.Int.UInt16);
                Bpp = entries[paletteindex].bpp;
            }
        }
        public static byte[] From4to8bpp(byte[] input)
        {
            var exit = new List<byte>();

            for (int i = 0; i < input.Length; i++)
            {
                int one = (input[i] & 0XF0) >> 4;
                int two = input[i] & 0xF;
                exit.Add((byte)one);
                exit.Add((byte)two);
            }
            return exit.ToArray();

        }
        public static byte[] unswizzle8bpp(byte[] pixels, int w, int h)
        {
            byte[] unswizzled = new byte[pixels.Length];

            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {

                    int block_location = (y & (~0xf)) * w + (x & (~0xf)) * 2;
                    int swap_selector = (((y + 2) >> 2) & 0x1) * 4;
                    int posY = (((y & (~3)) >> 1) + (y & 1)) & 0x7;
                    int column_location = posY * w * 2 + ((x + swap_selector) & 0x7) * 4;

                    int byte_num = ((y >> 1) & 1) + ((x >> 2) & 2);     // 0,1,2,3

                    int idx = block_location + column_location + byte_num;
                    if (idx >= pixels.Length)
                    {
                        //System.out.println("x");
                    }
                    else
                    {
                        unswizzled[(y * w) + x] = pixels[idx];
                    }
                }
            }

            return unswizzled;
        }
        public static byte[] unswizzlePS2(byte[] bytes, int width, int height)
        {

            // Make a copy of the swizzled input
            int dataLength = bytes.Length;
            byte[] swizzled = new byte[dataLength];
            Array.Copy(bytes, 0, swizzled, 0, dataLength);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int block_location = (y & (~0xf)) * width + (x & (~0xf)) * 2;
                    int swap_selector = (((y + 2) >> 2) & 0x1) * 4;
                    int posY = (((y & (~3)) >> 1) + (y & 1)) & 0x7;
                    int column_location = posY * width * 2 + ((x + swap_selector) & 0x7) * 4;

                    int byte_num = ((y >> 1) & 1) + ((x >> 2) & 2);     // 0,1,2,3

                    bytes[(y * width) + x] = swizzled[block_location + column_location + byte_num];
                }
            }

            return bytes;
        }

        private static byte[] Unswizzle(byte[] data, int width)
        {
            var dst = new byte[data.Length];

            for (var i = 0; i < data.Length; i += 16)
            {
                var srcIndex = i;
                var dstIndex = srcIndex % 0x10;
                dstIndex += srcIndex / 0x10 % 8 * width;
                dstIndex += srcIndex / 0x80 % (width / 16) * 16;
                dstIndex += srcIndex / (width * 8) * width * 8;

                Array.Copy(data, srcIndex, dst, dstIndex, 16);
            }

            return dst;
        }

        public static byte[] UnSwizzle(byte[] source, int offset, int width, int height, int bpp)
        {
            int destinationOffset = 0;

            // Incorperate the bpp into the width
            width = (width * bpp) >> 3;

            byte[] destination = new byte[width * height];

            int rowblocks = (width / 16);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int blockX = x / 16;
                    int blockY = y / 8;

                    int blockIndex = blockX + ((blockY) * rowblocks);
                    int blockAddress = blockIndex * 16 * 8;

                    destination[destinationOffset] = source[offset + blockAddress + (x - blockX * 16) + ((y - blockY * 8) * 16)];
                    destinationOffset++;
                }
            }

            return destination;
        }
        private static void InvertEndianess(byte[] data)
        {
            for (var i = 0; i < data.Length; i++)
                data[i] = (byte)(((data[i] & 15) << 4) | (data[i] >> 4));
        }
        private static int Compact1By1(int x)
        {
            x &= 0x55555555;                 // x = -f-e -d-c -b-a -9-8 -7-6 -5-4 -3-2 -1-0
            x = (x ^ (x >> 1)) & 0x33333333; // x = --fe --dc --ba --98 --76 --54 --32 --10
            x = (x ^ (x >> 2)) & 0x0f0f0f0f; // x = ---- fedc ---- ba98 ---- 7654 ---- 3210
            x = (x ^ (x >> 4)) & 0x00ff00ff; // x = ---- ---- fedc ba98 ---- ---- 7654 3210
            x = (x ^ (x >> 8)) & 0x0000ffff; // x = ---- ---- ---- ---- fedc ba98 7654 3210
            return x;
        }
        public static void unswizzle4(byte[] src, byte[] dst, int width, int height)
        {
            int pos = 0;
            for (int y = 0; y < height; y++)
            {
                int yc0 = y * 16;
                int yc1 = y / 8 * (width * 4 - 128);

                for (int x = 0; x < width / 2; x++)
                {
                    int xc0 = x / 16 * 16;
                    int xc1 = x / 16 * 128;
                    int pixelPos = x - xc0 + xc1 + yc0 + yc1;

                    dst[pos] = (byte)(src[pixelPos] & 0xF);
                    pos++;
                    pixelPos = x - xc0 + xc1 + yc0 + yc1;
                    dst[pos] = (byte)(src[pixelPos] >> 4);
                    pos++;
                }

            }
        }
        private static int DecodeMorton2X(int code)
        {
            return Compact1By1(code >> 0);
        }

        private static int DecodeMorton2Y(int code)
        {
            return Compact1By1(code >> 1);
        }
        public static byte[] UnswizzleTexture(byte[] pixelData, int width, int height, PixelFormat pixelFormat)
        {
            int bytesPerPixel = (Bitmap.GetPixelFormatSize(pixelFormat) / 8);
            byte[] unswizzled = new byte[pixelData.Length];

            for (int i = 0; i < width * height; i++)
            {
                int min = width < height ? width : height;
                int k = (int)Math.Log(min, 2);

                int x, y;
                if (height < width)
                {
                    // XXXyxyxyx → XXXxxxyyy
                    int j = i >> (2 * k) << (2 * k)
                        | (DecodeMorton2Y(i) & (min - 1)) << k
                        | (DecodeMorton2X(i) & (min - 1)) << 0;
                    x = j / height;
                    y = j % height;
                }
                else
                {
                    // YYYyxyxyx → YYYyyyxxx
                    int j = i >> (2 * k) << (2 * k)
                        | (DecodeMorton2X(i) & (min - 1)) << k
                        | (DecodeMorton2Y(i) & (min - 1)) << 0;
                    x = j % width;
                    y = j / width;
                }

                if (y >= height || x >= width) continue;

                Buffer.BlockCopy(pixelData, i * bytesPerPixel, unswizzled, ((y * width) + x) * bytesPerPixel, bytesPerPixel);
            }

            return unswizzled;
        }
        public static byte[] UnSwizzle(byte[] data, int width, int height, int blockSize)
        {
            var unswizzled = new byte[data.Length];
            var dataIndex = 0;
            int heightTexels = height / 4;
            int widthTexels = width / 4;
            var texelCount = widthTexels * heightTexels;

            for (int texel = 0; texel < texelCount; ++texel)
            {
                int pixelIndex = SwizzleUtilities.Morton(texel, widthTexels, heightTexels);
                int destIndex = blockSize * pixelIndex;
                Array.Copy(data, dataIndex, unswizzled, destIndex, blockSize);
                dataIndex += blockSize;
            }

            return unswizzled;
        }
        public static byte[] P6T_ProcessImg4(int w, int h, byte[] inpixels)
        {
            byte[] outpixels = new byte[w * h];
            byte entry;
            //FILE *bindump;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    // get the pen
                    int index = (y * w) + x;


                    // swizzle
                    int pageX = x & (~0x7f);
                    int pageY = y & (~0x7f);

                    int pages_horz = (w + 127) / 128;
                    int pages_vert = (h + 127) / 128;

                    int page_number = (pageY / 128) * pages_horz + (pageX / 128);

                    int page32Y = (page_number / pages_vert) * 32;
                    int page32X = (page_number % pages_vert) * 64;

                    int page_location = page32Y * h * 2 + page32X * 4;

                    int locX = x & 0x7f;
                    int locY = y & 0x7f;

                    int block_location = ((locX & (~0x1f)) >> 1) * h + (locY & (~0xf)) * 2;
                    int swap_selector = (((y + 2) >> 2) & 0x1) * 4;
                    int posY = (((y & (~3)) >> 1) + (y & 1)) & 0x7;

                    int column_location = posY * h * 2 + ((x + swap_selector) & 0x7) * 4;

                    int byte_num = (x >> 3) & 3;     // 0,1,2,3

                    entry = inpixels[page_location + block_location + column_location + byte_num];
                    entry = (byte)((entry >> ((y >> 1) & 0x01) * 4) & 0x0F);
                    outpixels[index] = entry;

                }
            }
            return outpixels;
        }
        //public void ChangeAlfa(int alfa)
        //{
        //    CLUT = new byte[CL.Length];
        //    for (int i = 0; i < CL.Length; i += 4) //RGBA
        //    {
        //        CLUT[i] = CL[i]; //R
        //        CLUT[i + 1] = CL[i + 1]; //G
        //        CLUT[i + 2] = CL[i + 2]; //B
        //        CLUT[i + 3] = (byte)(alfa); //Alfa
        //    }
        //}
        #region Swizzlers/Unswizzlers
        public static byte[] UnSwizzle4(byte[] buffer, int width, int height, int where)
        {
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

#if UNSWIZZLE_TO_4BIT
            var result = new byte[width * height];

            s = 0;
            d = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < (width >> 1); x++)
                    result[d++] = (byte)((newPixels[s++] & 0xF) | (newPixels[s++] << 4));
            }
            return result;
#else
            // return an 8-bit texture
            return newPixels;
#endif
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
        public static Color[] swizzlePalette(Color[] palette)
        {
            if (palette.Length == 256)
            {
                Color[] unswizzled = new Color[palette.Length];

                int j = 0;
                for (int i = 0; i < 256; i += 32, j += 32)
                {
                    copySW(palette, i, unswizzled, j, 8);
                    copySW(palette, i + 16, unswizzled, j + 8, 8);
                    copySW(palette, i + 8, unswizzled, j + 16, 8);
                    copySW(palette, i + 24, unswizzled, j + 24, 8);
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
        private static void copySW(Color[] unswizzled, int i, Color[] swizzled, int j, int num)
        {
            for (int x = 0; x < num; ++x)
            {
                swizzled[j + x] = unswizzled[i + x];
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
            Cutscene,
            CLT8bpp,
            CLT4bpp,
            TEX,
            Empty,
            Link,
            Null,
            Pasta
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
            try
            {
                reader.BaseStream.Position = offset;
                switch (type)
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
            }
            catch (Exception) { }
            reader.Close();
            memory.Close();
            return retur;
        }

    }
    internal static class SwizzleUtilities
    {
        public static int Morton(int t, int sx, int sy)
        {
            int num1;
            int num2 = num1 = 1;
            int num3 = t;
            int num4 = sx;
            int num5 = sy;
            int num6 = 0;
            int num7 = 0;

            while (num4 > 1 || num5 > 1)
            {
                if (num4 > 1)
                {
                    num6 += num2 * (num3 & 1);
                    num3 >>= 1;
                    num2 *= 2;
                    num4 >>= 1;
                }
                if (num5 > 1)
                {
                    num7 += num1 * (num3 & 1);
                    num3 >>= 1;
                    num1 *= 2;
                    num5 >>= 1;
                }
            }

            return num7 * sx + num6;
        }
    }
}
