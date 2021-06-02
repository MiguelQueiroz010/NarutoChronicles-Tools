using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Diagnostics;
using NUC_Raw_Tools.ArquivoRAW;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace NUC_Raw_Tools
{
    public partial class Principal : Form
    {
        #region Variáveis
        public RAW rawfile;
        public List<int> groupSelected;
        Point lastPoint;
        OpenFileDialog opened;
        public string rawname, pathf;
        static readonly string[] suffixes =
{ "Bytes", "KB", "MB", "GB", "TB", "PB" };
        byte[] raw;
        #endregion
        public Principal()
        {
            InitializeComponent();
            this.Size = new Size(520, 460);
            CheckRecent();
        }
        #region Elementos Funcionais e Visuais
        public void ShowHide()
        {
            abrir.Visible = !abrir.Visible;
            label1.Visible = !label1.Visible;
            salvarToolStripMenuItem.Enabled = !salvarToolStripMenuItem.Enabled;
            salvarComoToolStripMenuItem.Enabled = !salvarComoToolStripMenuItem.Enabled;
            fecharToolStripMenuItem.Enabled = !fecharToolStripMenuItem.Enabled;
            groupBox1.Visible = !groupBox1.Visible;
            treeView1.Visible = !treeView1.Visible;
            Editar.Visible = !Editar.Visible;
            panel1.Visible=!panel1.Visible;
            if (groupBox1.Visible)
                this.BackgroundImage = Properties.Resources.PrincipalBGS;
            else
                this.BackgroundImage = Properties.Resources.PrincipalBG;
            if (filename.Text.Length > 40)
            {
                string p1, p2;
                p1 = filename.Text.Substring(0, 40);
                p2 = filename.Text.Substring(40);
                filename.Text = p1 + "\n" + p2;
            }
        }
        public void CloseFile()
        {
            rawfile = null;
            raw = null;
            linkLabel1.ResetText();
            ShowHide();
            archivescount.Text = "Pastas: ";
            size.Text = "Tamanho: ";
            filename.Text = "Arquivo: ";
            path.Text = "Local: ";
            exportarToolStripMenuItem.Enabled = false;
            importarToolStripMenuItem.Enabled = false;
            importarTexturaToolStripMenuItem.Enabled = false;
            exportarTexturaToolStripMenuItem.Enabled = false;
            groupBox2.Visible = false;
            exporttexbt.Visible = false;
            exportbt.Visible = false;
            importbt.Visible = false;
            label4.Visible = false;
            pictureBox1.Visible = false;
            this.Size = new Size(520, 460);
        }
        #region Design
        private void abrir_Click(object sender, EventArgs e)
        {

            abrirToolStripMenuItem.PerformClick();
        }

        private void abrir_MouseEnter(object sender, EventArgs e)
        {
            abrir.ForeColor = Color.DarkBlue;
            abrir.Font = new Font(abrir.Font.FontFamily,20, FontStyle.Bold);
        }

        private void abrir_MouseLeave(object sender, EventArgs e)
        {
            abrir.ForeColor = Color.Black;
            abrir.Font = new Font(abrir.Font.FontFamily, 18, FontStyle.Bold);
        }

        private void abrir_MouseClick(object sender, MouseEventArgs e)
        {
            abrir.ForeColor = Color.Blue;
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void exportarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Export(treeView1, rawfile);
        }

        private void exportarTexturaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Export(treeView1, rawfile);
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if(treeView1.SelectedNode.Level>0)
            {
                groupBox2.Visible = true;
            }
            else
            {
                groupBox2.Visible = false;
            }
            switch (treeView1.SelectedNode.Level)
            {
                case 1:
                    exportarToolStripMenuItem.Enabled = true;
                    if (treeView1.SelectedNode.Nodes.Count != 0)
                    {
                        exportbt.Visible = false;
                        importbt.Visible = false;
                    }
                    pictureBox1.Visible = false;
                    label6.Visible = false;
                    this.Size = new Size(520, 460);
                    if (rawfile.Pastas[treeView1.SelectedNode.Index].type != RAW.Types.Link && rawfile.Pastas[treeView1.SelectedNode.Index].type != RAW.Types.Null)
                    {
                        switch (rawfile.Pastas[treeView1.SelectedNode.Index].type)
                        {
                            case RAW.Types.Textura:
                                exporttexbt.Visible = true;
                                label5.Visible = false;
                                break;
                            case RAW.Types.Texto:
                                Editar.Enabled = true;
                                label5.Visible = true;
                                label5.Text = "Strings: " + rawfile.Pastas[treeView1.SelectedNode.Index].texto.SeqCount;
                                break;
                            default:
                                exporttexbt.Visible = false;
                                exportbt.Enabled = true;
                                Editar.Enabled = false;
                                label5.Visible = false;
                                importbt.Enabled = true;
                                break;
                        }
                    }
                    else
                    {
                        exportbt.Visible = false;
                        importbt.Visible = false;
                        label5.Visible = false;
                    }
                    importarToolStripMenuItem.Enabled = true;
                    exportarTexturaToolStripMenuItem.Enabled = false;
                    label2.Text = "Tamanho: " + "0x" + rawfile.Pastas[treeView1.SelectedNode.Index].Size.ToString("X2");
                    label3.Text = "Tipo: " + rawfile.Pastas[treeView1.SelectedNode.Index].type.ToString();
                    //#region Verificar se é texto
                    //if (rawfile.Pastas[treeView1.SelectedNode.Index].type == RAW.Types.Texto)
                    //    Editar.Enabled = true;
                    //else
                    //    Editar.Enabled = false;
                    //#endregion
                    break;

                case 2:
                    exportarToolStripMenuItem.Enabled = true;
                    importarToolStripMenuItem.Enabled = true;
                    label6.Visible = false;
                    importarTexturaToolStripMenuItem.Enabled = false;
                    pictureBox1.Visible = false;
                    this.Size = new Size(520, 460);
                    if (rawfile.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].type != RAW.Types.Link && rawfile.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].type != RAW.Types.Null)
                    {
                        exportbt.Visible = true;
                        importbt.Visible = true;
                        switch (rawfile.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].type)
                        {
                            case RAW.Types.Textura:
                                label5.Visible = false;
                                exporttexbt.Visible = true;
                                break;
                            case RAW.Types.Texto:
                                Editar.Enabled = true;
                                label5.Visible = true;
                                label5.Text = "Strings: " + rawfile.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].texto.SeqCount;
                                break;
                            default:
                                exporttexbt.Visible = false;
                                exportbt.Visible = true;
                                Editar.Enabled = false;
                                label5.Visible = false;
                                importbt.Visible = true;
                                break;
                        }
                        
                    }
                    else
                    {
                        exportbt.Visible = false;
                        label5.Visible = false;
                        importbt.Visible = false;
                    }
                    exportarTexturaToolStripMenuItem.Enabled = false;
                    label2.Text = "Tamanho: "+"0x"+rawfile.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].Size.ToString("X2");
                    label3.Text = "Tipo: " + rawfile.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].type.ToString();
                    #region Verificar se é texto
                    if (rawfile.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].type == RAW.Types.Texto)
                        Editar.Enabled = true;
                    else
                        Editar.Enabled = false;
                    #endregion
                    break;

                case 3:
                    exportarTexturaToolStripMenuItem.Enabled = true;
                    exportarToolStripMenuItem.Enabled = true;
                    exportbt.Visible = true;
                    importbt.Visible = true;
                    importarTexturaToolStripMenuItem.Enabled = true;
                    exporttexbt.Visible = false;
                    pictureBox1.Visible = true;
                    label5.Visible = false;
                    label6.Visible = true;
                    this.Size = new Size(882, 460);
                    pictureBox1.Image = rawfile.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.GetImage(treeView1.SelectedNode.Index);
                    label3.Text = "Tipo: " + rawfile.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].type.ToString();
                    label6.Text = "Resolução: " + pictureBox1.Image.Width + "x" + pictureBox1.Image.Height+" "+ rawfile.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.texentries[treeView1.SelectedNode.Index].Bpp+" Bpp "+ rawfile.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.Entries[rawfile.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.texentries[treeView1.SelectedNode.Index].paletteindex].Length/4+" cores";
                    break;

                default:
                    exportarTexturaToolStripMenuItem.Enabled = false;
                    exportarToolStripMenuItem.Enabled = false;
                    exportbt.Visible = false;
                    label6.Visible = false;
                    importbt.Visible = false;
                    importarToolStripMenuItem.Enabled = false;
                    importarTexturaToolStripMenuItem.Enabled = false;
                    pictureBox1.Visible = false;
                    this.Size = new Size(520, 460);
                    Editar.Enabled = false;
                    break;
            }


        }
        #endregion

        #endregion
        #region Efeito dos Botões(Fechar e Minimizar)
        private void button1_MouseEnter(object sender, EventArgs e)
        {
            var bt = sender as Button;
            bt.BackColor = Color.Red;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            var bt = sender as Button;
            bt.BackColor = Color.LightGray;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            var bt = sender as Button;
            bt.BackColor = Color.Green;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            var bt = sender as Button;
            bt.BackColor = Color.LightGray;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        #endregion
        #region Movimento do Form
        private void Principal_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
        private void Principal_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }
        #endregion
        #region Créditos e Etc
        private void sobreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox sobre = new AboutBox();
            sobre.ShowDialog();
        }

        #endregion
        #region Métodos e Funções
        #region Botões
        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Abrir(false, null);
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var path = Path.GetFullPath(linkLabel1.Text);
            Process.Start("explorer.exe", path);
        }

        private void fecharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseFile();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            EditarTexto();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Export(treeView1, rawfile);
        }
        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //rawfile.Rebuild(rawfile, pathf, rawname);
            #region Salvar o arquivo
            System.IO.File.WriteAllBytes(path + @"/" + rawname, rawfile.Data);
            #endregion
            Refresh();
        }
        private void button4_Click(object sender, EventArgs e)
        {

        }
        private void salvarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Arquivo RAW|*.raw";
            save.FileName = rawname.Substring(0, rawname.Length - 4);
            if(save.ShowDialog()==DialogResult.OK)
            {
                //rawfile.Rebuild(rawfile,Path.GetDirectoryName(save.FileName), Path.GetFileName(save.FileName));
                #region Salvar o arquivo
                System.IO.File.WriteAllBytes(Path.GetDirectoryName(save.FileName) + @"/" + Path.GetFileName(save.FileName), rawfile.Data);
                #endregion
                MessageBox.Show("Concluído!", "Êxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void Principal_DragDrop(object sender, DragEventArgs e)
        {
            var dropped = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (dropped.Length > 1)
            {
                MessageBox.Show("Apenas 1 por vez!!", "Nope", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
                Abrir(true, dropped[0]);

        }

        private void Principal_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;

        }
        #endregion
        public string Sizes(int decimal_size)
        {
            var sizes = String.Empty;

            if (decimal_size < 1024)
            {
                sizes = decimal_size.ToString() + "B";
            }
            else if (decimal_size >= 1024 && decimal_size < 1050000 && decimal_size < 1048600000)
            {
                sizes = (decimal_size / 1024).ToString() + "KB";
            }
            else if (decimal_size >= 1050000 && decimal_size < 1048600000)
            {
                sizes = (decimal_size / 1050000).ToString() + "MB";
            }
            else if (decimal_size >= 1048600000)
            {
                sizes = (decimal_size / 1048600000).ToString() + "GB";
            }

            return sizes;
        }
        public void Atualizar()
        {
            rawfile.Rebuild(rawfile,"","");
            string path = pathf + @"\" + rawname;
            rawfile = new RAW(rawfile.Data);
            rawfile.ListInsert(treeView1, rawfile, path);
        }
        public void Refresh()
        {
            if (rawfile.Data != null)
            {
                CloseFile();
                ShowHide();
                string path = pathf + @"\" + rawname;
                #region Arquivo
                raw = File.ReadAllBytes(path);
                rawfile = new RAW(raw);
                #endregion
                #region Rótulos
                rawname = Path.GetFileName(path);
                pathf = Path.GetDirectoryName(path);
                linkLabel1.Text += Path.GetDirectoryName(path);
                filename.Text += Path.GetFileName(path);
                size.Text += Sizes(raw.Length);
                archivescount.Text += rawfile.folderCount;
                foreach (var file in rawfile.Pastas)
                {
                    if (file.type == RAW.Types.Textura)
                    {
                        //if (file.textura.errcount != 0)
                        //{
                        //    label4.Visible = true;
                        //    label4.Text = "Erros: " + file.textura.errcount;
                        //}
                    }
                    if (file.Arquivos != null)
                        foreach (var fi in file.Arquivos)
                        {
                            if (fi.type == RAW.Types.Textura)
                            {
                                //if (fi.textura.errcount != 0)
                                //{
                                //    label4.Visible = true;
                                //    label4.Text = "Erros: " + fi.textura.errcount;
                                //}
                            }
                        }
                }
                #endregion
                #region Funções
                AddRecent(pathf + @"\" + rawname);
                #endregion
                timerSALVO.Enabled = true;
                timerSALVO.Start();
                Salvo.Visible = true;
            }
        }
        public void Abrir(bool isDrag, string fileName)
        {
            if (isDrag)
            {
                #region Se arquivo já aberto
                if (rawfile != null)
                    CloseFile();
                #endregion
                #region Arquivo
                raw = File.ReadAllBytes(fileName);
                rawfile = new RAW(raw);
                #endregion
                #region Rótulos
                rawname = Path.GetFileName(fileName);
                pathf = Path.GetDirectoryName(fileName);
                linkLabel1.Text += Path.GetDirectoryName(fileName);
                filename.Text += Path.GetFileName(fileName);
                size.Text += Sizes(raw.Length);
                archivescount.Text += rawfile.folderCount;
                foreach (var file in rawfile.Pastas)
                    {
                        if (file.type == RAW.Types.Textura)
                        {
                            //if (file.textura.errcount != 0)
                            //{
                            //    label4.Visible = true;
                            //    label4.Text = "Erros: " + file.textura.errcount;
                            //}
                        }
                        if(file.Arquivos!=null)
                          foreach (var fi in file.Arquivos)
                        {
                            if (fi.type == RAW.Types.Textura)
                            {
                                //if (fi.textura.errcount != 0)
                                //{
                                //    label4.Visible = true;
                                //    label4.Text = "Erros: " + fi.textura.errcount;
                                //}
                            }
                        }
                    }
                #endregion
                #region Funções
                rawfile.ListInsert(treeView1, rawfile, fileName);
                ShowHide();
                AddRecent(pathf + @"\" + rawname);
                #endregion
            }
            else
            {
                opened = new OpenFileDialog();
                opened.Filter = "Arquivo RAW NUC|*.raw";
                if (opened.ShowDialog() == DialogResult.OK)
                {
                    #region Se arquivo já aberto
                    if (rawfile != null)
                        CloseFile();
                    #endregion
                    #region Arquivo
                    raw = File.ReadAllBytes(opened.FileName);

                    rawfile = new RAW(raw);
                    #endregion
                    #region Rótulos
                    rawname = Path.GetFileName(opened.FileName);
                    pathf = Path.GetDirectoryName(opened.FileName);
                    linkLabel1.Text += Path.GetDirectoryName(opened.FileName);
                    filename.Text += Path.GetFileName(opened.FileName);
                    size.Text += Sizes(raw.Length);
                    archivescount.Text += rawfile.folderCount;
                    foreach (var file in rawfile.Pastas)
                    {
                        if (file.type == RAW.Types.Textura)
                        {
                            //if (file.textura.errcount != 0)
                            //{
                            //    label4.Visible = true;
                            //    label4.Text = "Erros: " + file.textura.errcount;
                            //}
                        }
                        if (file.Arquivos != null)
                            foreach (var fi in file.Arquivos)
                            {
                                if (fi.type == RAW.Types.Textura)
                                {
                                    //if (fi.textura.errcount != 0)
                                    //{
                                    //    label4.Visible = true;
                                    //    label4.Text = "Erros: " + fi.textura.errcount;
                                    //}
                                }
                            }
                    }
                    #endregion
                    #region Funções
                    rawfile.ListInsert(treeView1, rawfile, opened.FileName);
                    ShowHide();
                    AddRecent(pathf + @"\" + rawname);
                    #endregion

                }
            }
            
        }
        public void Export(TreeView list, RAW file)
        {
            var save = new FolderBrowserDialog();
            if (save.ShowDialog() == DialogResult.OK)
            {
                switch(treeView1.SelectedNode.Level)
                {
                    case 1:
                        if (file.Pastas[treeView1.SelectedNode.Index].type == RAW.Types.Texto&& MessageBox.Show("Quer exportar o texto em formato TXT?", "Perguntinha", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            StringBuilder writer = new StringBuilder();
                            writer.AppendLine("#NUC#" + file.Pastas[treeView1.SelectedNode.Index].texto.SeqCount);
                            writer.AppendLine("---------------------------");
                            foreach (var str in file.Pastas[treeView1.SelectedNode.Index].texto.sequences)
                            {
                                string texto = Encodings.Naruto.UzumakiChronicles2.GetString(str);
                                writer.AppendLine(texto);
                                writer.AppendLine("#--------------------#");
                            }
                            File.WriteAllText(save.SelectedPath + "/" + treeView1.SelectedNode.Text + ".txt", writer.ToString(),Encoding.Default);
                        }
                        else
                        {
                            File.WriteAllBytes(save.SelectedPath + "/" + treeView1.SelectedNode.Text + ".sraw", file.Pastas[treeView1.SelectedNode.Index].FileData);
                        }
                        MessageBox.Show("Sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case 2:
                        if (file.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].type == RAW.Types.Texto && MessageBox.Show("Quer exportar o texto em formato TXT?", "Perguntinha", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            StringBuilder writer = new StringBuilder();
                            writer.AppendLine("#NUC#" + file.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].texto.SeqCount);
                            writer.AppendLine("---------------------------");
                            foreach (var str in file.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].texto.sequences)
                            {
                                string texto = Encodings.Naruto.UzumakiChronicles2.GetString(str);
                                writer.AppendLine(texto);
                               writer.AppendLine("#--------------------#");
                            }
                            File.WriteAllText(save.SelectedPath + "/" + treeView1.SelectedNode.Text + ".txt", writer.ToString(), Encoding.Default);

                        }
                        else
                        {
                            File.WriteAllBytes(save.SelectedPath + "/" + treeView1.SelectedNode.Text + ".sraw", file.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].FileData);
                        }
                        MessageBox.Show("Sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case 3:
                        rawfile.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.GetImage(treeView1.SelectedNode.Index).Save(save.SelectedPath+"/" + treeView1.SelectedNode.Text + ".png", System.Drawing.Imaging.ImageFormat.Png);
                        MessageBox.Show("Sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }
            }
        }
        public void ExportTexturas(TreeView list, RAW file)
        {
            var save = new FolderBrowserDialog();
            if (save.ShowDialog() == DialogResult.OK)
            {
                
                if (treeView1.SelectedNode.Level==2)
                {
                        Directory.CreateDirectory(save.SelectedPath + @"/" + treeView1.SelectedNode.Text);
                        int k = 0;
                        for(int i =0;i< rawfile.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].textura.TextureCount;i++)
                        {
                        rawfile.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].textura.GetImage(i).Save(save.SelectedPath + @"/" + treeView1.SelectedNode.Text + @"/" + k.ToString() + ".png", ImageFormat.Png);
                                k++;
                        }
                        MessageBox.Show("Sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        public void ExportTEXCLT(RAW file)
        {
            if (MessageBox.Show("Quer exportar a textura em formato RAW?","Perguntinha",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                FolderBrowserDialog folder = new FolderBrowserDialog();
                if (folder.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(folder.SelectedPath + @"\" + treeView1.SelectedNode.Text + ".tex", file.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.Entries[file.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.texentries[treeView1.SelectedNode.Index].pixelindex]);
                    File.WriteAllBytes(folder.SelectedPath + @"\" + treeView1.SelectedNode.Text + ".clt", file.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.Entries[file.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.texentries[treeView1.SelectedNode.Index].paletteindex]); 
                    MessageBox.Show("Sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                    Export(treeView1, file);
            }
           
        }
        public void Import(TreeView list, RAW file)
        {
            var import = new OpenFileDialog();
            import.Filter = "Arquivo RAW|*.raw;*.sraw;*.bin;|Arquivo de Texto|*.txt";
            if (import.ShowDialog() == DialogResult.OK)
            {
                bool done = false;
                byte[] filebin = File.ReadAllBytes(import.FileName);
                switch (treeView1.SelectedNode.Level)
                {
                    case 1:
                        if (filebin.Length!=0)
                        {
                            if (file.Pastas[treeView1.SelectedNode.Index].type == RAW.Types.Texto && Path.GetExtension(import.FileName).ToLower()==".txt")
                            {
                                string input = File.ReadAllText(import.FileName,Encoding.Default);
                                ImportTXT(input, file.Pastas[treeView1.SelectedNode.Index].texto);
                            }
                            else
                            {
                                file.Pastas[treeView1.SelectedNode.Index].FileData = new byte[filebin.Length];
                                file.Pastas[treeView1.SelectedNode.Index].FileData = filebin;
                                file.Pastas[treeView1.SelectedNode.Index].Size = (uint)filebin.Length;
                            }
                            MessageBox.Show("Importado!" + "\nLembre-se de salvar!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            done = true;
                        }
                        else
                            MessageBox.Show("Arquivo incompatível!" + "\nVerifique a estrutura do container!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case 2:
                        if (filebin.Length!=0)
                        {
                            if (file.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].type == RAW.Types.Texto && Path.GetExtension(import.FileName).ToLower() == ".txt")
                            {
                                string input = File.ReadAllText(import.FileName,Encoding.Default);
                                ImportTXT(input, file.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].texto);
                            }
                            else
                            {
                                file.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].FileData = new byte[filebin.Length];
                                file.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].FileData = filebin;
                                file.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].Size = (uint)filebin.Length;
                            }
                            MessageBox.Show("Importado!" + "\nLembre-se de salvar!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            done = true;
                        }
                        else
                            MessageBox.Show("Arquivo incompatível!" + "\nVerifique a estrutura do container!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
                if (done)
                {
                    var node = treeView1.SelectedNode;
                    //Refresh();
                    
                    Atualizar();
                    treeView1.Focus();
                    treeView1.Select();
                    treeView1.SelectedNode = node;
                    
                }
            }
        }
        void ImportTextura(RAW file)
        {
            switch (treeView1.SelectedNode.Level)
            {
                case 3:
                    if (file.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.texentries[treeView1.SelectedNode.Index].Bpp == 4)
                    {
                        MessageBox.Show("Atualmente suporto apenas texturas 8bpp!!", "Erro");
                        return;
                    }
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "Portable Network Graphics(*.png)|*.png";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        if (Path.GetExtension(ofd.FileName).ToLower() != ".png")
                        {
                            MessageBox.Show("Formato de Imagem não suportado!!");
                            return;
                        }
                        Image input = Image.FromFile(ofd.FileName);
                        Bitmap bit = new Bitmap(input);
                        HashSet<Color> colors = new HashSet<Color>();
                        for (int y = 0; y < bit.Height; y++)
                        {
                            for (int x = 0; x < bit.Width; x++)
                            {
                                colors.Add(bit.GetPixel(x, y));
                            }
                        }
                        Color[] cores = colors.ToArray();
                        int colorcoutn = cores.Length;
                        int bpp = file.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.texentries[treeView1.SelectedNode.Index].Bpp;
                        int width = file.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.texentries[treeView1.SelectedNode.Index].Width;
                        int height = file.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.texentries[treeView1.SelectedNode.Index].Height;
                        if (input.Width == width && input.Height == height &&bpp==8&&colorcoutn<=256)
                        {
                            file.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.ImportTexture(treeView1.SelectedNode.Index, input);
                            MessageBox.Show("Importado!" + "\nLembre-se de salvar!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            var node = treeView1.SelectedNode;
                            treeView1.SelectedNode = node;
                            treeView1.Focus();
                            treeView1.Select();
                            pictureBox1.Visible = true;
                            label5.Visible = false;
                            this.Size = new Size(882, 460);
                            pictureBox1.Image = rawfile.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.GetImage(treeView1.SelectedNode.Index);
                        }
                        else
                          MessageBox.Show("A imagem deve ter a mesma resolução da original\n[e atualmente suporto apenas texturas 8bpp]!!\n\nResolução esperada: " + width + "x" + height+"\n"+bpp.ToString()+" bpp 256 cores\n\nQuantia de cores da textura tentada: "+ colorcoutn+" cores\nRecomendo usar o OptiPix ImageStudio para reduzir as cores!!", "Erro");
                        
                    }
                    break;
            }

        }
        void ImportTXT(string input,RAW.Text texto)
        {
            string[] strs = input.Split(new string[] { "\r\n", "#--------------------#", "---------------------------" }, StringSplitOptions.RemoveEmptyEntries);
            if (strs[0]=="#NUC#"+texto.SeqCount)
            {
                for(int i =1;i<texto.SeqCount+1;i++)
                {
                    texto.sequences[i - 1] = Encodings.Naruto.UzumakiChronicles2.GetBytes(strs[i]);
                }
                texto.Save();
            }

        }
        public void CheckRecent()
        {
            recenteToolStripMenuItem.Enabled = false;
            if (recenteToolStripMenuItem.HasDropDownItems)
                recenteToolStripMenuItem.DropDownItems.Clear();
            if (File.Exists("recent.txt"))
            {
                string[] lines = File.ReadAllLines("recent.txt");
                foreach (string line in lines)
                    if (line.Trim() != "")
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem(line.Trim());
                        item.Click += recentClick;
                        recenteToolStripMenuItem.DropDownItems.Add(item);
                        recenteToolStripMenuItem.Enabled = true;
                    }
            }
        }
        public void recentClick(object sender, EventArgs e)
        {
            if ((sender as ToolStripMenuItem).Text.EndsWith(".RAW") || ((sender as ToolStripMenuItem).Text.EndsWith(".raw")))
            {
                Abrir(true, (sender as ToolStripMenuItem).Text);
            }
            
        }
        public void AddRecent(string path)
        {
            if (!File.Exists("recent.txt"))
                File.WriteAllLines("recent.txt", new string[0]);
            string[] lines = File.ReadAllLines("recent.txt");
            List<string> result = new List<string>();
            result.Add(path);
            int index = 0;
            while (result.Count < 10 && index < lines.Length && lines[index] != path)
                result.Add(lines[index++]);
            File.WriteAllLines("recent.txt", result.ToArray());
            CheckRecent();
        }
        private void importarTexturaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportTextura(rawfile);
        }
        private void importarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            importbt.PerformClick();

        }
        private void exportbt_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Level == 3)
            {
                
                ExportTEXCLT(rawfile);
            }
            else
                Export(treeView1, rawfile);
        }
        private void importbt_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Level == 3)
            {
                ImportTextura(rawfile);
            }
            else
                Import(treeView1, rawfile);
        }

        private void exporttexbt_Click(object sender, EventArgs e)
        {
            ExportTexturas(treeView1, rawfile);
        }

        private void timerSALVO_Tick(object sender, EventArgs e)
        {
            timerSALVO.Stop();
            Salvo.Visible = false;
        }

        void EditarTexto()
        {
            TextEditor editor = new TextEditor(this);
            editor.ShowDialog();
        }
        #endregion
    }
    static class ByteArrayRocks
    {
        static readonly int[] Empty = new int[0];

        public static int[] Locate(this byte[] self, byte[] candidate)
        {
            if (IsEmptyLocate(self, candidate))
                return Empty;

            var list = new List<int>();

            for (int i = 0; i < self.Length; i++)
            {
                if (!IsMatch(self, i, candidate))
                    continue;

                list.Add(i);
            }

            return list.Count == 0 ? Empty : list.ToArray();
        }

        static bool IsMatch(byte[] array, int position, byte[] candidate)
        {
            if (candidate.Length > (array.Length - position))
                return false;

            for (int i = 0; i < candidate.Length; i++)
                if (array[position + i] != candidate[i])
                    return false;

            return true;
        }

        static bool IsEmptyLocate(byte[] array, byte[] candidate)
        {
            return array == null
                || candidate == null
                || array.Length == 0
                || candidate.Length == 0
                || candidate.Length > array.Length;
        }

    }
}