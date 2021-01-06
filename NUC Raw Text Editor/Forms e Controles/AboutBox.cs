using NUC_Raw_Tools.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Assemblies;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace NUC_Raw_Tools
{
    partial class AboutBox : Form
    {
        bool BlinkOn;
        int i;
        public AboutBox()
        {
            InitializeComponent();
            label4.Text = "Not a commercial tool, by fan to fan.\nFerramenta não comercial, de fã para fã.";
            label5.Text = "";
            label6.Text = Application.ProductVersion;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void AboutBox_Shown(object sender, EventArgs e)
        {
            //timer1.Enabled = true;
            //timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (i == 125)
                i = 0;
            if (BlinkOn)
            {

                label1.ForeColor = Color.FromArgb(255, 255, 255, i);

            }
            else
            {

                label1.ForeColor = Color.FromArgb(255, i, i, 255);

            }
            BlinkOn = !BlinkOn;
            i += 1;
        }
    }
}
