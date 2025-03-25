using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Diagnostics;
using NUC_Raw_Tools.ArquivoRAW;
using NUC_Raw_Tools.Arquivo.ArquivoRAW;

namespace NUC_Raw_Tools
{
    public partial class Principal : Form
    {
        #region Variáveis
        public RAW rawfile;
        public List<int> groupSelected;

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
            layout.Visible = !layout.Visible;
            salvarToolStripMenuItem.Enabled = !salvarToolStripMenuItem.Enabled;
            salvarComoToolStripMenuItem.Enabled = !salvarComoToolStripMenuItem.Enabled;
            fecharToolStripMenuItem.Enabled = !fecharToolStripMenuItem.Enabled;
            Editar.Visible = !Editar.Visible;
            if (layout.Visible)
            {
                this.BackgroundImage = Properties.Resources.PrincipalBGS;
                this.Size = new Size(890, 493);
            }
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
            exporttexbt.Visible = false;
            exportbt.Visible = false;
            importbt.Visible = false;
            this.Size = new Size(520, 460);
        }
        #region Design
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
                                Editar.Visible = true;
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
                    size.Text = "Tamanho: " + "0x" + rawfile.Pastas[treeView1.SelectedNode.Index].Size.ToString("X2");
                    label3.Text = "Tipo: " + rawfile.Pastas[treeView1.SelectedNode.Index].type.ToString();

                    break;

                case 2:
                    exportarToolStripMenuItem.Enabled = true;
                    importarToolStripMenuItem.Enabled = true;
                    label6.Visible = false;
                    importarTexturaToolStripMenuItem.Enabled = false;
                    pictureBox1.Visible = false;
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
                                Editar.Visible = true;
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
                    size.Text = "Tamanho: "+"0x"+rawfile.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].Size.ToString("X2");
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
                    var tex = rawfile.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.texentries[treeView1.SelectedNode.Index];
                    rawfile.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.GetImage(treeView1.SelectedNode.Index, out Image mage);
                    pictureBox1.Image = mage;
                    label3.Text = "Tipo: " + rawfile.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].type.ToString();
                    label6.Text = "Resolução: " + pictureBox1.Image.Width + "x" + pictureBox1.Image.Height + " " + rawfile.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.texentries[treeView1.SelectedNode.Index].Bpp + " Bpp " + rawfile.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.Entries[rawfile.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.texentries[treeView1.SelectedNode.Index].paletteindex].Length / 4 + " cores\nÍndice textura linkada: " +
                        rawfile.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.texentries[treeView1.SelectedNode.Index].linkedidx;
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
                    Editar.Enabled = false;
                    break;
            }


        }
        #endregion

        #endregion
        
        #region Créditos e Etc
        private void sobreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 sobre = new AboutBox1(this);
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
            rawfile.Rebuild(rawfile, pathf, rawname);
            #region Salvar o arquivo
            System.IO.File.WriteAllBytes(pathf+@"\"+rawname, rawfile.Data);
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
                rawfile.Rebuild(rawfile,Path.GetDirectoryName(save.FileName), Path.GetFileName(save.FileName));
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
                        var tex = rawfile.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.GetImage(treeView1.SelectedNode.Index, out Image mage);
                        File.WriteAllBytes(save.SelectedPath + "/" + treeView1.SelectedNode.Text +".tm2", tex);
                        //tex.Save(save.SelectedPath+"/" + treeView1.SelectedNode.Text + ".png", System.Drawing.Imaging.ImageFormat.Png);
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
                        File.WriteAllBytes(save.SelectedPath + @"/" + treeView1.SelectedNode.Text + @"/" + k.ToString() + ".tm2", rawfile.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].textura.GetImage(i, out Image mage));
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
        public void ImportTextura(RAW file)
        {
            if (treeView1.SelectedNode.Level != 3)
                return;

            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "TM2 Graphics(*.tm2)|*.tm2"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            if (Path.GetExtension(ofd.FileName).ToLower() != ".tm2")
            {
                MessageBox.Show("Formato de Imagem não suportado!!");
                return;
            }

            byte[] input = File.ReadAllBytes(ofd.FileName);
            var timdata = TM2.GetClutandTex(input);

            int colorCount = timdata.Bpp == 4 ? 16 : 256;

            var selectedNode = treeView1.SelectedNode;
            var parentNode = selectedNode.Parent;
            var grandParentNode = parentNode.Parent;

            var pasta = file.Pastas[grandParentNode.Index];
            var arquivo = pasta.Arquivos[parentNode.Index];
            var textura = arquivo.textura;
            var texEntry = textura.texentries[selectedNode.Index];

            int width = texEntry.Width;
            int height = texEntry.Height;
            int bpp = texEntry.Bpp;

            bool isLinked = texEntry.linkedidx != -1;
            bool isValidSize = timdata.Width == width && timdata.Height == height;
            bool isValidColorCount = colorCount <= 256;

            if (!isLinked && isValidSize && isValidColorCount)
            {
                textura.ImportTexture(selectedNode.Index, input);
                ShowSuccessMessage();
                return;
            }

            if (isLinked)
            {
                (width, height) = CalculateLinkedTextureSize(textura, texEntry);
                if (timdata.Width == width && timdata.Height == height && timdata.Bpp == 8 && isValidColorCount)
                {
                    textura.ImportTexture(selectedNode.Index, input);
                    ShowSuccessMessage();
                }
                else
                {
                    ShowErrorMessage(width, height, bpp, colorCount);
                }
                return;
            }

            ShowErrorMessage(width, height, bpp, colorCount);
        }

        public void ShowSuccessMessage()
        {
            MessageBox.Show("Importado!\nLembre-se de salvar!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshUI();
        }

        public void ShowErrorMessage(int width, int height, int bpp, int colorCount)
        {
            MessageBox.Show($"A imagem deve ter a mesma resolução da original\n[e atualmente suporto apenas texturas 8bpp]!!\n\n" +
                            $"Resolução esperada: {width}x{height}\n" +
                            $"{bpp} bpp 256 cores\n\n" +
                            $"Quantia de cores da textura tentada: {colorCount} cores\n" +
                            "Recomendo usar o OptiPix ImageStudio para reduzir as cores!!", "Erro");
        }

        public void RefreshUI()
        {
            var node = treeView1.SelectedNode;
            treeView1.SelectedNode = node;
            treeView1.Focus();
            treeView1.Select();
            pictureBox1.Visible = true;
            label5.Visible = false;
            this.Size = new Size(890, 493);
        }

        public (int, int) CalculateLinkedTextureSize(RAW.TextureArray textura, RAW.TextureEntries texEntry)
        {
            int width = 0, height = 0;
            foreach (int idx in texEntry.linkedwith)
            {
                var entry = textura.texentries[idx];
                width += entry.Width;
                height += entry.Height;
            }
            return (width / 2, height / 2);
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