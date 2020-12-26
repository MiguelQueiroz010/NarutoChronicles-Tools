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
using NUC_Raw_Tools.Arquivo;

namespace NUC_Raw_Tools
{
    public partial class Principal : Form
    {
        #region Variáveis
        public Arquivo.Raw rawfile;
        public List<int> groupSelected;
        Point lastPoint;
        OpenFileDialog opened;
        public string rawname;
        static readonly string[] suffixes =
{ "Bytes", "KB", "MB", "GB", "TB", "PB" };
        byte[] raw;
        #endregion
        public Principal()
        {
            InitializeComponent();
        }

        #region Elementos Funcionais e Visuais

        void ShowHide()
        {
            abrir.Visible = !abrir.Visible;
            label1.Visible = !label1.Visible;
            fecharToolStripMenuItem.Enabled = !fecharToolStripMenuItem.Enabled;
            groupBox1.Visible = !groupBox1.Visible;
            treeView1.Visible = !treeView1.Visible;
            Button[] buttons = { button3, button4, button5, button6 };
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
            //EditarTexto();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Export(treeView1, rawfile);
        }

        private void button4_Click(object sender, EventArgs e)
        {

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
        void ListInsert(TreeView list, Raw files, string name)
        {
            groupSelected = new List<int>();
            list.Nodes.Clear();
            for (int i = 0; i < files.Root.Count; i++)
            {
                groupSelected.Add(i + 1);
                TreeNode group = new TreeNode(Path.GetFileName(name) + "_" + i);
                treeView1.Nodes.Add(group);
            }
        }
        void Abrir(bool isDrag, string fileName)
        {
            //if (isDrag)
            //{
            //    #region Se arquivo já aberto
            //    if (rawfile != null)
            //        CloseFile();
            //    #endregion
            //    #region Arquivo
            //    raw = File.ReadAllBytes(fileName);
            //    rawfile = new RawFile(raw);
            //    #endregion
            //    #region Rótulos
            //    rawname = Path.GetFileName(fileName);
            //    linkLabel1.Text += Path.GetDirectoryName(fileName);
            //    filename.Text += Path.GetFileName(fileName);
            //    size.Text += Sizes(rawfile.raw.Length);
            //    archivescount.Text += rawfile.files.Count;
            //    #endregion
            //    #region Funções
            //    ShowHide();
            //    ListInsert(this.treeView1, rawfile, fileName);
            //    #endregion
            //}
            //else
            //{
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
                rawfile = new Raw();
                rawfile.RawR(raw);
                    
                #endregion
                #region Rótulos
                rawname = Path.GetFileName(opened.FileName);
                linkLabel1.Text += Path.GetDirectoryName(opened.FileName);
                filename.Text += Path.GetFileName(opened.FileName);
                size.Text += Sizes(raw.Length);
                archivescount.Text += rawfile.Root.Count-1;
                #endregion
                #region Funções
                ShowHide();
                ListInsert(this.treeView1, rawfile, opened.FileName);
                    #endregion
                
            }
        }
        //void Export(TreeView list, RawFile file)
        //{
        //    var save = new FolderBrowserDialog();
        //    if(save.ShowDialog()==DialogResult.OK)
        //    {
        //        if(treeView1.SelectedNode.Level==1)
        //        {
        //            File.WriteAllBytes(save.SelectedPath + "/" + treeView1.SelectedNode.Text + ".raw", file.files[treeView1.SelectedNode.Parent.Index].subFiles[treeView1.SelectedNode.Index].Data);
        //            MessageBox.Show("Sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            return;
        //        }
        //        string[] separator = { "|" };
        //        string[] properties = treeView1.SelectedNode.Text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        //        File.WriteAllBytes(save.SelectedPath + "/" + properties[1] + ".raw", file.files[treeView1.SelectedNode.Index].FileData);
        //        MessageBox.Show("Sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }

        //}

        //void EditarTexto()
        //{
        //    TextEditor editor = new TextEditor(this);
        //    editor.ShowDialog();
        //}
        #endregion

        //private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        //{
        //        if (rawfile.files[treeView1.SelectedNode.Parent.Index].subFiles[treeView1.SelectedNode.Index].Type == "Texto")
        //        {
        //            button6.Enabled = true;
        //            button3.Enabled = true;
        //        }
        //        if(rawfile.files[treeView1.SelectedNode.Parent.Index].subFiles[treeView1.SelectedNode.Index].Type != "Texto")
        //        {
        //            button6.Enabled = false;
        //            button3.Enabled = false;
        //        }
        //}
    }
}