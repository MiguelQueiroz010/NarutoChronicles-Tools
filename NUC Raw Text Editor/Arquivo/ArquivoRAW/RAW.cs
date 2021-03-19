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
            folderCount = (int)ReadUInt(Data, 8, Int.UInt32)/0x10;
            if(folderCount==0)
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
                if (folder.type==Types.Texto)
                    Raiz2 = new TreeNode(Path.GetFileName(filename) + "_" + i);
                else if (folder.type == Types.Textura)
                    Raiz2 = new TreeNode(Path.GetFileName(filename) + "_" + i);
                else if (folder.type==Types.Unknow)
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
                                foreach (var image in file.textura.images)
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
            foreach(TreeNode node in tree.Nodes)
            {
                int k = 0;
                foreach(TreeNode treeNode in node.Nodes)
                {
                    int c = 0;
                    foreach(TreeNode node1 in treeNode.Nodes)
                    {
                        switch(raw.Pastas[k].Arquivos[c].type)
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
                if (folder.Arquivos !=null)
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
            #region Salvar o arquivo
            System.IO.File.WriteAllBytes(path + @"/" + name, root.ToArray());
            #endregion
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
            public Texture textura;
            public Types type = Types.Unknow;
            public Folder(byte[] Data, int index)
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
                        texto = new Text(null,this, index);
                    }
                    if (ReadUInt(FileData, 16, Int.UInt32).ToString("X2") == "8004")
                    {
                        type = Types.Textura;
                        textura = new Texture(null,this,FileData, index);
                    }
                    return;
                }
                int test = (int)ReadUInt(FileData, 4, Int.UInt32) - ((int)ReadUInt(FileData, 0, Int.UInt32) * 8);
                int soma = 0;
                if (test < (int)ReadUInt(FileData, 4, Int.UInt32)&&test>0)
                    soma = test + ((int)ReadUInt(FileData, 0, Int.UInt32) * 8);
                int ste = filescount * 8+4;
                while (ste % 0x10 != 0)
                    ste++;
                if ((int)ReadUInt(FileData, 4, Int.UInt32)!=0&&test< (int)ReadUInt(FileData, 4, Int.UInt32) && soma == (int)ReadUInt(FileData, 4, Int.UInt32)&& (int)ReadUInt(FileData, 4, Int.UInt32)==ste)
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
                    foreach(var fi in Arquivos)
                    {
                        if (fi.Position!=0&&fi.type!=Types.Null&&fi.type!=Types.Link&&fi.type!=Types.Empty)
                        {
                            verifier.Add(fi);
                        }
                    }
                    int padd = 0;
                    for (int l =0;l< verifier.Count;l++)
                    {
                        
                            if (l == verifier.Count-1)
                            {
                                padd = FileData.Length - (int)verifier[l].Position;
                            }
                            else if(l!=verifier.Count)
                            {
                            padd = (int)(verifier[l + 1].Position - verifier[l].Position);
                            }
                            verifier[l].Padding = padd - (int)verifier[l].Size;
                    }
                    for(int k = 0;k<Arquivos.Count;k++)
                    {
                        foreach(var f in verifier)
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
                    if(file.type!=Types.Null&&file.type!=Types.Link&&file.type!=Types.Empty)
                    {
                        int padd = 0;
                        file.Position = (uint)offset;
                        offset += (int)file.Size;
                        while(offset%0x10!=0)
                        {
                            offset++;
                            padd++;
                        }
                        Array.Resize(ref file.FileData, ((int)file.Size)+padd);
                    }
                }
                foreach(var f in Arquivos)
                {
                    if(f.type==Types.Link)
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
                for (int i =0;i<filescount;i++)
                {
                    ptable.AddRange(BitConverter.GetBytes((UInt32)Arquivos[i].Position));
                    ptable.AddRange(BitConverter.GetBytes((UInt32)Arquivos[i].Size));
                }
                #endregion
                while(ptable.Count!=foffset)
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
            public int Index, Padding,LinkerIndex;
            public uint Position;
            public uint Size;
            public Types type = Types.Unknow;
            public byte[] FileData;
            public Text texto;
            public Texture textura;
            public File(Folder folder, int index)
            {
                #region Arquivo
                Index = index;
                Position = (uint)ReadUInt(folder.FileData, 4 + (8 * (index)) , Int.UInt32);
                Size = (uint)ReadUInt(folder.FileData, 8 + (8 * (index)), Int.UInt32);
                FileData = Bin.ReadBlock(folder.FileData, Position, Size);
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
                        texto = new Text(this,null, index);
                    }
                    if (ReadUInt(FileData, 16, Int.UInt32).ToString("X2") == "8004")
                    {
                        type = Types.Textura;
                        textura = new Texture(this,null,FileData, index);
                    }
                }
                else
                {
                    type = Types.Null;
                }
                #endregion
                //MessageBox.Show("Índice: " + Index.ToString() + "\n" +
                //    "Posição: " + Position.ToString("X2") + "\n" +
                //    "Tamanho: " + Size.ToString("X2") + "\n" +
                //    "Padding: " + Padding.ToString() + "\n" +
                //    "Tipo: " + type.ToString());
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
                if (intfile != null)
                {
                    intfile.Size = Size;
                    intfile.FileData = new byte[Size];
                    intfile.FileData = Data;
                }
                if(intfold!=null)
                {
                    intfold.Size = Size;
                    intfold.FileData = new byte[Size];
                    intfold.FileData = Data;
                }
                
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
            public int Index,errcount=0;
            public uint Position;
            public uint Size;
            public uint Count;
            public List<Image> images;
            public List<TextureDATA.TEXs> TEXs;
            public List<TextureDATA.CLUTs> CLUTs;
            private Folder folder;
            private File filex;
            HashSet<Color> cores;
            int lastColorCount = -1;
            int expectedCount = 256;
            int expectedSizeX;
            int expectedSizeY;
            Image currOutput, currInput;

            public Texture(File fil, Folder fold,byte[] file, int index)
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
                            TEXs.Add(new TextureDATA.TEXs(TEX, width, height,(int)texoffset));
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
                            CLUTs.Add(new TextureDATA.CLUTs(CLT, bpp, (int)cltoffset));
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
                bool argument=false;
                errcount = 0;
                foreach(TextureDATA.CLUTs clut in CLUTs) {
                    if (clut.bpp == 8)
                    {
                        byte[] unswizzled = null;
                        List<Color> pal = new List<Color>();
                        int pos = 0;
                        byte r, g, b, a;
                        while (pos < CLUTs[i].CLUT.Length)
                        {
                            r = CLUTs[i].CLUT[pos];
                            g = CLUTs[i].CLUT[pos + 1];
                            b = CLUTs[i].CLUT[pos + 2];
                            a = CLUTs[i].CLUT[pos + 3];
                            if (a <= 128)
                                a = (byte)((a * 255) / 128);
                            pal.Add(Color.FromArgb(a, r, g, b));
                            pos += 4;
                        }
                        //clut.ChangeAlfa(255);
                        Color[] colors = unswizzlePalette(pal.ToArray());
                        try
                        {
                            unswizzled = UnSwizzle8(TEXs[i].width, TEXs[i].height, TEXs[i].TEX);
                            Rainbow.ImgLib.Encoding.ImageDecoderIndexed imageDecoder = new Rainbow.ImgLib.Encoding.ImageDecoderIndexed(unswizzled, TEXs[i].width, TEXs[i].height, Rainbow.ImgLib.Encoding.IndexCodec.FromNumberOfColors(256, Rainbow.ImgLib.Common.ByteOrder.BigEndian), colors);
                            images.Add(imageDecoder.DecodeImage());
                            
                        }
                        catch (ArgumentOutOfRangeException) { errcount++; argument = true; }
                    }
                    i++;
                }
                //if(argument)
                //    MessageBox.Show("Contém "+errcount+" texturas não processadas corretamente!",
                //        "Aviso!",MessageBoxButtons.OK, MessageBoxIcon.Warning); 
                #endregion
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
            public void Importar(int Iindex)
            {
                //currInput = currOutput = images[Iindex];
                //CountColors();
                //Color[] list = cores.ToArray();
                //for (int i = 0; i < expectedCount; i++)
                //{
                //    if (i < list.Length)
                //    {
                //        CLUTs[Iindex].CLUT[i * 4 + 0x10] = list[i].R;
                //        CLUTs[Iindex].CLUT[i * 4 + 0x11] = list[i].G;
                //        CLUTs[Iindex].CLUT[i * 4 + 0x12] = list[i].B;
                //        CLUTs[Iindex].CLUT[i * 4 + 0x13] = list[i].A;

                //    }
                //    else
                //    {
                //        CLUTs[Iindex].CLUT[i * 4 + 0x10] = 0;
                //        CLUTs[Iindex].CLUT[i * 4 + 0x11] = 0;
                //        CLUTs[Iindex].CLUT[i * 4 + 0x12] = 0;
                //        CLUTs[Iindex].CLUT[i * 4 + 0x13] = 0;
                //    }
                //}
                //Bitmap bmp = new Bitmap(currOutput);
                //int pos = 0;
                //Color c1, c2;
                //byte[] tex = UnSwizzle8(TEXs[Iindex].width, TEXs[Iindex].height, TEXs[Iindex].TEX);
                //if (expectedCount == 16)
                //    for (int y = 0; y < expectedSizeY; y++)
                //        for (int x = 0; x < expectedSizeX / 2; x++)
                //        {
                //            c1 = bmp.GetPixel(x * 2 + 1, expectedSizeY - y - 1);
                //            c2 = bmp.GetPixel(x * 2, expectedSizeY - y - 1);
                //            tex[0x18 + pos++] = (byte)((FindColorIndex(c1, list) << 4) + FindColorIndex(c2, list));
                //        }
                //else if (expectedCount == 256)
                //    for (int y = 0; y < expectedSizeY; y++)
                //        for (int x = 0; x < expectedSizeX; x++)
                //        {
                //            c1 = bmp.GetPixel(x, expectedSizeY - y - 1);
                //            tex[0x18 + pos++] = FindColorIndex(c1, list);
                //        }
                //TEXs[Iindex].TEX = new byte[tex.Length];
                //TEXs[Iindex].TEX = Swizzle8(TEXs[Iindex].width, TEXs[Iindex].height,tex);
                //List<byte> palc = new List<byte>();
                //for(int i =0;i<list.Length;i++)
                //{
                //    palc.Add(list[i].R);
                //    palc.Add(list[i].G);
                //    palc.Add(list[i].B);
                //    palc.Add(list[i].A);
                //}
                //CLUTs[Iindex].CLUT = palc.ToArray();
                #region Salvar na DATA
                Array.Copy(TEXs[Iindex].TEX, 0, Data, TEXs[Iindex].offset, TEXs[Iindex].TEX.Length);//TEX
                Array.Copy(CLUTs[Iindex].CLUT, 0, Data, CLUTs[Iindex].offset, CLUTs[Iindex].CLUT.Length);//CLT
                Save();
                #endregion
            }
            private void CountColors()
            {
                Bitmap bmp = new Bitmap(currOutput);
                cores = new HashSet<Color>();
                for (int y = 0; y < bmp.Size.Height; y++)
                    for (int x = 0; x < bmp.Size.Width; x++)
                        cores.Add(bmp.GetPixel(x, y));
                lastColorCount = cores.Count();
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
        }
        public class TextureDATA
        {
            public class TEXs
            {
                public byte[] TEX;
                public int width = 0;
                public int height = 0;
                public int offset;
                public TEXs(byte[] TEXs, uint Width, uint Height, int Offset)
                {
                    TEX = TEXs;
                    width = (int)Width;
                    height = (int)Height;
                    offset = Offset;
                }
            }
            public class CLUTs
            {
                public byte[] CLUT,CL;
                public int bpp;
                public int offset;
                public CLUTs(byte[] CLT,int Bpp, int Offset)
                {
                    CL = CLT;
                    CLUT = CLT;
                    bpp = Bpp;
                    offset = Offset;
                }
                public void ChangeAlfa(int alfa)
                {
                    CLUT = new byte[CL.Length];
                    for (int i = 0; i < CL.Length; i += 4) //RGBA
                    {
                        CLUT[i] = CL[i]; //R
                        CLUT[i + 1] = CL[i + 1]; //G
                        CLUT[i + 2] = CL[i + 2]; //B
                        CLUT[i + 3] = (byte)(alfa); //Alfa
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
            Cutscene,
            CLT,
            TEX,
            Empty,
            Link,
            Null
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
}
