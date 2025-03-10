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
            rtb1.Text = seqs[actual - 1];
            rtb1.Draw(rtb1);
            rtb2.Text = seqs[actual - 1];
            //nrtb2.RichTextBox.MaxLength = nrtb2.RichTextBox.Text.Length;
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
            TextRender();
            if(actual>=1|actual==seqs.Count)
            {
                button4.Enabled = true;
                
            }
            if (actual == seqs.Count-1)
                button5.Enabled = false;
            actual++;
            rtb2.Text = seqs[actual - 1];
            rtb1.Draw(rtb1);
            rtb1.Text = seqs[actual-1];
            label2.Text = "Sequência: "+actual + " de " + seqs.Count;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            TextRender();
            if (actual == 2)
                button4.Enabled = false;
            if (actual >= 1)
                button5.Enabled = true;
            actual--;
            rtb1.Text = seqs[actual - 1];
            rtb1.Draw(rtb1);
            rtb2.Text = seqs[actual-1];
            label2.Text = "Sequência: " + actual + " de " + seqs.Count;
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
                    p01.rawfile.Pastas[p01.treeView1.SelectedNode.Index].texto.sequences[actual - 1] = Encodings.Naruto.UzumakiChronicles2.GetBytes(rtb2.Text);
                    p01.rawfile.Pastas[p01.treeView1.SelectedNode.Index].texto.Save();

                    break;
                case 2:
                    p01.rawfile.Pastas[p01.treeView1.SelectedNode.Parent.Index].Arquivos[p01.treeView1.SelectedNode.Index].texto.sequences[actual-1] = Encodings.Naruto.UzumakiChronicles2.GetBytes(rtb2.Text);
                    p01.rawfile.Pastas[p01.treeView1.SelectedNode.Parent.Index].Arquivos[p01.treeView1.SelectedNode.Index].texto.Save();
                    break;
            }
            //rtb2.Text = rtb1.Text;
            rtb1.Draw(rtb1);
        }
        #region Synchronize ScrollBars code from: https://stackoverflow.com/questions/1827323/c-synchronize-scroll-position-of-two-richtextboxes (Sudhakar MuthuKrishnan)
        const int WM_USER = 0x400;

        const int EM_GETSCROLLPOS = WM_USER + 221;

        const int EM_SETSCROLLPOS = WM_USER + 222;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int msg, int wParam, ref Point lParam);

        private void RichTextBox1_VScroll(object sender, EventArgs e)
        {
            Point pt = new Point();

            SendMessage(rtb2.Handle, EM_GETSCROLLPOS, 0, ref pt);

            SendMessage(rtb1.Handle, EM_SETSCROLLPOS, 0, ref pt);
        }


        private void RichTextBox2_VScroll(object sender, EventArgs e)
        {
            Point pt = new Point();

            SendMessage(rtb2.Handle, EM_GETSCROLLPOS, 0, ref pt);

            SendMessage(rtb1.Handle, EM_SETSCROLLPOS, 0, ref pt);
        }
        #endregion
        #endregion
    }
}
