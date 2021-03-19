using System.Drawing;

namespace NUC_Raw_Tools
{
    partial class TextEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextEditor));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.filename = new System.Windows.Forms.Label();
            //this.nrtb1 = new NUC_Raw_Tools.NRTB(this);
            //this.nrtb2 = new NUC_Raw_Tools.NRTB(this);
            this.rtb1 = new RichTextBoxEx(this);
            this.rtb2 = new RichTextBoxEx(this);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(0, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Editor de Texto";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(10, 277);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Sequência:";
            // 
            // button4
            // 
            this.button4.Enabled = false;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(253, 259);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 11;
            this.button4.Text = "Anterior";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Enabled = false;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Location = new System.Drawing.Point(343, 259);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 12;
            this.button5.Text = "Próximo";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.filename);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(430, 23);
            this.panel1.TabIndex = 13;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TextEditor_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Principal_MouseMove);
            // 
            // filename
            // 
            this.filename.AutoSize = true;
            this.filename.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filename.ForeColor = System.Drawing.Color.DarkBlue;
            this.filename.Location = new System.Drawing.Point(140, 6);
            this.filename.Name = "filename";
            this.filename.Size = new System.Drawing.Size(13, 17);
            this.filename.TabIndex = 7;
            this.filename.Text = " ";
            // 
            // nrtb1
            // 
            this.rtb1.BackColor = System.Drawing.SystemColors.Window;
            this.rtb1.BackColor = System.Drawing.SystemColors.Control;
            //this.nrtb1.Strip.BackColor = System.Drawing.SystemColors.ControlDark;
            this.rtb1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rtb1.Location = new System.Drawing.Point(14, 46);
            this.rtb1.Name = "nrtb1";
            this.rtb1.Size = new System.Drawing.Size(395, 89);
            this.rtb1.TabIndex = 14;
            this.rtb1.NumberColor = Color.Black;
            this.rtb1.NumberFont = new Font(this.rtb1.Font.FontFamily, 11);
            this.rtb1.Font = new System.Drawing.Font(this.rtb2.Font.FontFamily, 14);
            this.rtb1.ShowLineNumbers = true;
            this.rtb1.VScroll += RichTextBox1_VScroll;
            //// 
            //// nrtb2
            //// 
            //this.nrtb2.BackColor = System.Drawing.SystemColors.Window;
            //this.nrtb2.Strip.BackColor = System.Drawing.SystemColors.Control;
            //this.nrtb2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            //this.nrtb2.Location = new System.Drawing.Point(14, 152);
            //this.nrtb2.Name = "nrtb2";
            //this.nrtb2.Size = new System.Drawing.Size(395, 89);
            //this.nrtb2.TabIndex = 15;
            this.rtb2.BackColor = System.Drawing.SystemColors.Window;
            this.rtb2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rtb2.Location = new System.Drawing.Point(14, 152);
            this.rtb2.Name = "nrtb2";
            this.rtb2.NumberFont = new Font(this.rtb1.Font.FontFamily, 11);
            this.rtb2.Size = new System.Drawing.Size(395, 89);
            this.rtb2.Font = new System.Drawing.Font(this.rtb2.Font.FontFamily, 14);
            this.rtb2.TabIndex = 15;
            this.rtb2.NumberColor = Color.Black;
            this.rtb2.VScroll += RichTextBox2_VScroll;
            this.rtb2.ShowLineNumbers = true;
            // 
            // TextEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::NUC_Raw_Tools.Properties.Resources.PrincipalBGS;
            this.ClientSize = new System.Drawing.Size(430, 304);
            this.Controls.Add(rtb1);
            this.Controls.Add(rtb2);
            //this.Controls.Add(this.nrtb2);
            //this.Controls.Add(this.nrtb1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TextEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Editor de Texto";
            this.Load += new System.EventHandler(this.TextEditor_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label filename;
        private RichTextBoxEx rtb1, rtb2;
        //private NRTB nrtb1;
        //private NRTB nrtb2;
    }
}