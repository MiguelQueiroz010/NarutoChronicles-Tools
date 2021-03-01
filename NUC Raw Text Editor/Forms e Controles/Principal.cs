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
        string[] recentes;
        static readonly string[] suffixes =
{ "Bytes", "KB", "MB", "GB", "TB", "PB" };
        byte[] raw;
        #endregion
        public Principal()
        {
            InitializeComponent();
            CheckRecent();
        }
        #region Elementos Funcionais e Visuais

        void ShowHide()
        {
            abrir.Visible = !abrir.Visible;
            label1.Visible = !label1.Visible;
            salvarToolStripMenuItem.Enabled = !salvarToolStripMenuItem.Enabled;
            salvarComoToolStripMenuItem.Enabled = !salvarComoToolStripMenuItem.Enabled;
            fecharToolStripMenuItem.Enabled = !fecharToolStripMenuItem.Enabled;
            groupBox1.Visible = !groupBox1.Visible;
            treeView1.Visible = !treeView1.Visible;
            label2.Visible = !label2.Visible;
            Button[] buttons = { Editar};
            foreach (var b in buttons)
                b.Visible = !b.Visible;
            if (groupBox1.Visible)
                this.BackgroundImage = Properties.Resources.PrincipalBGS;
            else
                this.BackgroundImage = Properties.Resources.PrincipalBG;
            if (filename.Text.Length > 20)
            {
                string p1, p2;
                p1 = filename.Text.Substring(0, 20);
                p2 = filename.Text.Substring(20);
                filename.Text = p1 + "\n" + p2;
            }
        }
        void CloseFile()
        {
            rawfile = null;
            raw = null;
            linkLabel1.ResetText();
            ShowHide();
            archivescount.Text = "Arquivos Comprimidos: ";
            size.Text = "Tamanho: ";
            filename.Text = "Arquivo: ";
            path.Text = "Local: ";
            exportarToolStripMenuItem.Enabled = false;
            importarToolStripMenuItem.Enabled = false;
            importarTexturaToolStripMenuItem.Enabled = false;
            exportarTexturaToolStripMenuItem.Enabled = false;
        }
        #region Design
        private void abrir_Click(object sender, EventArgs e)
        {

            abrirToolStripMenuItem.PerformClick();
        }

        private void abrir_MouseEnter(object sender, EventArgs e)
        {
            abrir.ForeColor = Color.DarkBlue;
        }

        private void abrir_MouseLeave(object sender, EventArgs e)
        {
            abrir.ForeColor = Color.Black;
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
            switch (treeView1.SelectedNode.Level)
            {
                case 1:
                    exportarToolStripMenuItem.Enabled = true;
                    importarToolStripMenuItem.Enabled = true;
                    exportarTexturaToolStripMenuItem.Enabled = false;
                    label2.Text = "Tamanho[HEX]: " + rawfile.Pastas[treeView1.SelectedNode.Index].Size.ToString("X2");
                    #region Verificar se é texto
                    if (rawfile.Pastas[treeView1.SelectedNode.Index].type == RAW.Types.Texto)
                        Editar.Enabled = true;
                    else
                        Editar.Enabled = false;
                    #endregion
                    break;

                case 2:
                    exportarToolStripMenuItem.Enabled = true;
                    importarToolStripMenuItem.Enabled = true;
                    exportarTexturaToolStripMenuItem.Enabled = false;
                    label2.Text = "Tamanho[HEX]: "+rawfile.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].Size.ToString("X2");
                    #region Verificar se é texto
                    if (rawfile.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].type == RAW.Types.Texto)
                        Editar.Enabled = true;
                    else
                        Editar.Enabled = false;
                    #endregion
                    break;

                case 3:
                    exportarTexturaToolStripMenuItem.Enabled = true;
                    exportarToolStripMenuItem.Enabled = false;
                    break;

                default:
                    exportarTexturaToolStripMenuItem.Enabled = false;
                    exportarToolStripMenuItem.Enabled = false;
                    importarToolStripMenuItem.Enabled = false;
                    importarTexturaToolStripMenuItem.Enabled = false;
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
            rawfile.Rebuild(rawfile, pathf, rawname);
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
        string Sizes(int decimal_size)
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
        void Abrir(bool isDrag, string fileName)
        {
            if (isDrag)
            {
                #region Se arquivo já aberto
                if (rawfile != null)
                    CloseFile();
                #endregion
                #region Arquivo
                raw = File.ReadAllBytes(fileName);
                rawfile = new RAW(raw, 128);
                #endregion
                #region Rótulos
                rawname = Path.GetFileName(fileName);
                pathf = Path.GetDirectoryName(fileName);
                linkLabel1.Text += Path.GetDirectoryName(fileName);
                filename.Text += Path.GetFileName(fileName);
                size.Text += Sizes(raw.Length);
                archivescount.Text += rawfile.folderCount - 1;
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

                    rawfile = new RAW(raw, 128);
                    #endregion
                    #region Rótulos
                    rawname = Path.GetFileName(opened.FileName);
                    pathf = Path.GetDirectoryName(opened.FileName);
                    linkLabel1.Text += Path.GetDirectoryName(opened.FileName);
                    filename.Text += Path.GetFileName(opened.FileName);
                    size.Text += Sizes(raw.Length);
                    archivescount.Text += rawfile.folderCount - 1;
                    #endregion
                    #region Funções
                    rawfile.ListInsert(treeView1, rawfile, opened.FileName);
                    ShowHide();
                    AddRecent(pathf + @"\" + rawname);
                    #endregion

                }
            }
            
        }
        void Export(TreeView list, RAW file)
        {
            var save = new FolderBrowserDialog();
            if (save.ShowDialog() == DialogResult.OK)
            {
                switch(treeView1.SelectedNode.Level)
                {
                    case 1:
                        File.WriteAllBytes(save.SelectedPath+"/"+treeView1.SelectedNode.Text+".sraw", file.Pastas[treeView1.SelectedNode.Index].FileData);
                        MessageBox.Show("Sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case 2:

                        File.WriteAllBytes(save.SelectedPath+"/"+treeView1.SelectedNode.Text + ".sraw", file.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].FileData);
                        MessageBox.Show("Sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case 3:
                        rawfile.Pastas[treeView1.SelectedNode.Parent.Parent.Index].Arquivos[treeView1.SelectedNode.Parent.Index].textura.images[treeView1.SelectedNode.Index].Save(save.SelectedPath+"/" + treeView1.SelectedNode.Text + ".png", System.Drawing.Imaging.ImageFormat.Png);
                        MessageBox.Show("Sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }
            }
        }
        void Import(TreeView list, RAW file)
        {
            var import = new OpenFileDialog();
            import.Filter = "Arquivo RAW|*.raw,*.sraw,*.bin";
            if (import.ShowDialog() == DialogResult.OK)
            {
                byte[] filebin = File.ReadAllBytes(import.FileName);
                switch (treeView1.SelectedNode.Level)
                {
                    case 1:
                        if (((int)RAW.ReadUInt(filebin, 8, RAW.Int.UInt32) / 0xF) != 0)
                        {
                            file.Pastas[treeView1.SelectedNode.Index].FileData = new byte[filebin.Length];
                            //file.Pastas[treeView1.SelectedNode.Index].Size = (uint)filebin.Length;
                            Array.Copy(filebin, file.Pastas[treeView1.SelectedNode.Index].FileData, filebin.Length);
                            MessageBox.Show("Importado!" + "\nLembre-se de salvar!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show("Arquivo incompatível!" + "\nVerifique a estrutura do container!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case 2:
                        if (filebin[0] != 0)
                        {
                            file.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].FileData = new byte[filebin.Length];
                            //file.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].Size = (uint)filebin.Length;
                            Array.Copy(filebin, file.Pastas[treeView1.SelectedNode.Parent.Index].Arquivos[treeView1.SelectedNode.Index].FileData, filebin.Length);
                            MessageBox.Show("Importado!" + "\nLembre-se de salvar!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show("Arquivo incompatível!" + "\nVerifique a estrutura do container!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
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
        }
        private void importarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Import(treeView1, rawfile);

        }
        void EditarTexto()
        {
            TextEditor editor = new TextEditor(this);
            editor.ShowDialog();
        }
        #endregion
    }
}