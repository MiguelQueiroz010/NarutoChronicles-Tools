using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;
using NUC_Raw_Tools;

namespace NUC_Raw_Tools
{
    partial class AboutBox1 : Form
    {
        private Timer timer;
        private Color[] cores = { Color.Orange ,Color.Blue, Color.Pink, Color.Orange };
        private int indiceCor = 0;
        SoundPlayer ss;
        Principal f01;
        public AboutBox1(Principal f1)
        {
            InitializeComponent();
            f01 = f1;
            f01.Visible = false;
            Stream str = Properties.Resources.sobre;
            ss = new SoundPlayer(str);
            ss.Play();

            timer = new Timer()
            {
                Interval = 30 // Tempo em milissegundos
            };
            timer.Tick += TrocarCor;
            timer.Start();

        }
        private void TrocarCor(object sender, EventArgs e)
        {
            indiceCor = (indiceCor + 1) % cores.Length;
            label1.ForeColor = cores[indiceCor];
        }
        void closex()
        {
            ss.Stop();
            ss.Dispose();
            Close();
            f01.Visible = true;
        }

        

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            closex();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            closex();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            closex();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://bitmundo.xyz");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/MiguelQueiroz010/NarutoChronicles-Tools");
        }
    }
}
