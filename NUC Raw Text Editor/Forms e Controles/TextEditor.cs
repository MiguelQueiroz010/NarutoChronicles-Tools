using System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NUC_Raw_Tools
{
    public partial class TextEditor : Form
    {
        public List<string> seqs;
        public Principal p01;
        Point lastPoint;
        public int actual = 1;
        public TextEditor(Principal p1)
        {
            p01 = p1;
            InitializeComponent();
            
        }
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
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        #endregion
        #region Movimento do Form
        private void Principal_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void TextEditor_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
        #endregion
        #region Métodos
        void TextRender()
        {
            seqs = new List<string>();
            switch(p01.treeView1.SelectedNode.Level)
            {
                case 1:
                        foreach (var s in p01.rawfile.Pastas[p01.treeView1.SelectedNode.Index].texto.sequences)
                        {
                            seqs.Add(Encodings.Naruto.UzumakiChronicles2.GetString(s));
                        }
                    
                    break;

                case 2:
                    foreach (var s in p01.rawfile.Pastas[p01.treeView1.SelectedNode.Parent.Index].Arquivos[p01.treeView1.SelectedNode.Index].texto.sequences)
                    {
                        seqs.Add(Encodings.Naruto.UzumakiChronicles2.GetString(s));
                    }
                    break;
            }
            nrtb1.RichTextBox.Text = seqs[actual - 1];
            nrtb1.Draw(nrtb1.RichTextBox);
            nrtb2.RichTextBox.Text = seqs[actual - 1];
            filename.Text += p01.treeView1.SelectedNode.Text;
            if (filename.Text.Length > 27)
            {
                string p1, p2;
                p1 = filename.Text.Substring(0, 27);
                p2 = filename.Text.Substring(27);
                filename.Text = p1 + "\n" + p2;
            }
            label2.Text += actual + " de " + seqs.Count;
            if (seqs.Count > 1)
            {
                button5.Enabled = true;
            }
        }
        #endregion
        #region Botões e Início
        private void button5_Click(object sender, EventArgs e)
        {
            if(actual>=1|actual==seqs.Count)
            {
                button4.Enabled = true;
                
            }
            if (actual == seqs.Count-1)
                button5.Enabled = false;
            actual++;
            nrtb1.RichTextBox.Text = seqs[actual - 1];
            nrtb1.Draw(nrtb1.RichTextBox);
            nrtb2.RichTextBox.Text = seqs[actual-1];
            label2.Text = "Sequência: "+actual + " de " + seqs.Count;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (actual == 2)
                button4.Enabled = false;
            if (actual >= 1)
                button5.Enabled = true;
            actual--;
            nrtb1.RichTextBox.Text = seqs[actual - 1];
            nrtb1.Draw(nrtb1.RichTextBox);
            nrtb2.RichTextBox.Text = seqs[actual-1];
            label2.Text = "Sequência: " + actual + " de " + seqs.Count;
        }

        private void TextEditor_Load(object sender, EventArgs e)
        {
            TextRender();
        }
        public void save()
        {
            File.WriteAllBytes("text" + actual.ToString() + ".bin", Encodings.Naruto.UzumakiChronicles2.GetBytes(nrtb2.RichTextBox.Text));
            nrtb1.RichTextBox.Text = nrtb2.RichTextBox.Text;
            nrtb1.Draw(nrtb1.RichTextBox);
        }
        #endregion
    }
}
