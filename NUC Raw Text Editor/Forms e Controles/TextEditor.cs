using System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NUC_Raw_Tools.Forms_e_Controles;

namespace NUC_Raw_Tools
{
    public partial class TextEditor : Form
    {
        public List<string> seqs;
        public string All;
        public Principal p01;
        Point lastPoint;
        public CustomRTB RTB = new CustomRTB(Properties.Resources.buttons);
        public TextEditor(Principal p1)
        {
            p01 = p1;
            InitializeComponent();
            this.tableLayoutPanel1.Controls.Add(RTB,0,0);
        }
        #region Métodos
        void TextRender()
        {
            seqs = new List<string>();
            switch(p01.treeView1.SelectedNode.Level)
            {
                case 1:
                    seqs = p01.rawfile.Pastas[p01.treeView1.SelectedNode.Index].texto.GetStrings();
                    break;

                case 2:
                    seqs = p01.rawfile.Pastas[p01.treeView1.SelectedNode.Index].texto.GetStrings();
                    break;
            }

            foreach (string s in seqs)
                All += s + "\r\n\r\n";

            RTB.Editor.Text = All;
            //rtb1.Draw(rtb1);

            filename.Text += p01.treeView1.SelectedNode.Text;
            if (filename.Text.Length > 27)
            {
                string p1, p2;
                p1 = filename.Text.Substring(0, 27);
                p2 = filename.Text.Substring(27);
                filename.Text = p1 + "\n" + p2;
            }
        }
        #endregion
        #region Botões e Início
        private void button5_Click(object sender, EventArgs e)
        {
            TextRender();
            //rtb1.Draw(rtb1);
            RTB.Editor.Text = All;
        }
        private void TextEditor_Load(object sender, EventArgs e)
        {
            TextRender();
        }
        public void save()
        {
            switch (p01.treeView1.SelectedNode.Level)
            {
                case 1:
                    //p01.rawfile.Pastas[p01.treeView1.SelectedNode.Index].texto.sequences[actual - 1] = Encodings.Naruto.UzumakiChronicles2.GetBytes(rtb2.Text);



                    //p01.rawfile.Pastas[p01.treeView1.SelectedNode.Index].texto.Save();

                    break;
                case 2:
                   // p01.rawfile.Pastas[p01.treeView1.SelectedNode.Parent.Index].Arquivos[p01.treeView1.SelectedNode.Index].texto.sequences[actual-1] = Encodings.Naruto.UzumakiChronicles2.GetBytes(rtb2.Text);
                   // p01.rawfile.Pastas[p01.treeView1.SelectedNode.Parent.Index].Arquivos[p01.treeView1.SelectedNode.Index].texto.Save();
                    break;
            }
            //rtb2.Text = rtb1.Text;
            //rtb1.Draw(rtb1);
        }
        
        #endregion
    }
}
