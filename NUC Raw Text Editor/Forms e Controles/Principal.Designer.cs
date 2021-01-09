﻿namespace NUC_Raw_Tools
{
    partial class Principal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Principal));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.arquivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salvarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salvarComoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fecharToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recenteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.sairToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportarTexturaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importarTexturaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sobreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Editar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.path = new System.Windows.Forms.Label();
            this.archivescount = new System.Windows.Forms.Label();
            this.size = new System.Windows.Forms.Label();
            this.filename = new System.Windows.Forms.Label();
            this.abrir = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(18, 18);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.arquivoToolStripMenuItem,
            this.editarToolStripMenuItem,
            this.sobreToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(518, 27);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Principal_MouseDown);
            this.menuStrip1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Principal_MouseMove);
            // 
            // arquivoToolStripMenuItem
            // 
            this.arquivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.abrirToolStripMenuItem,
            this.salvarToolStripMenuItem,
            this.salvarComoToolStripMenuItem,
            this.fecharToolStripMenuItem,
            this.recenteToolStripMenuItem,
            this.sairToolStripMenuItem});
            this.arquivoToolStripMenuItem.Name = "arquivoToolStripMenuItem";
            this.arquivoToolStripMenuItem.Size = new System.Drawing.Size(75, 23);
            this.arquivoToolStripMenuItem.Text = "Arquivo";
            // 
            // abrirToolStripMenuItem
            // 
            this.abrirToolStripMenuItem.Name = "abrirToolStripMenuItem";
            this.abrirToolStripMenuItem.Size = new System.Drawing.Size(170, 24);
            this.abrirToolStripMenuItem.Text = "Abrir";
            this.abrirToolStripMenuItem.Click += new System.EventHandler(this.abrirToolStripMenuItem_Click);
            // 
            // salvarToolStripMenuItem
            // 
            this.salvarToolStripMenuItem.Enabled = false;
            this.salvarToolStripMenuItem.Name = "salvarToolStripMenuItem";
            this.salvarToolStripMenuItem.Size = new System.Drawing.Size(170, 24);
            this.salvarToolStripMenuItem.Text = "Salvar";
            this.salvarToolStripMenuItem.Click += new System.EventHandler(this.salvarToolStripMenuItem_Click);
            // 
            // salvarComoToolStripMenuItem
            // 
            this.salvarComoToolStripMenuItem.Enabled = false;
            this.salvarComoToolStripMenuItem.Name = "salvarComoToolStripMenuItem";
            this.salvarComoToolStripMenuItem.Size = new System.Drawing.Size(170, 24);
            this.salvarComoToolStripMenuItem.Text = "Salvar Como";
            this.salvarComoToolStripMenuItem.Click += new System.EventHandler(this.salvarComoToolStripMenuItem_Click);
            // 
            // fecharToolStripMenuItem
            // 
            this.fecharToolStripMenuItem.Enabled = false;
            this.fecharToolStripMenuItem.Name = "fecharToolStripMenuItem";
            this.fecharToolStripMenuItem.Size = new System.Drawing.Size(170, 24);
            this.fecharToolStripMenuItem.Text = "Fechar";
            this.fecharToolStripMenuItem.Click += new System.EventHandler(this.fecharToolStripMenuItem_Click);
            // 
            // recenteToolStripMenuItem
            // 
            this.recenteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.recenteToolStripMenuItem.Enabled = false;
            this.recenteToolStripMenuItem.Name = "recenteToolStripMenuItem";
            this.recenteToolStripMenuItem.Size = new System.Drawing.Size(170, 24);
            this.recenteToolStripMenuItem.Text = "Recente";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Enabled = false;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(96, 24);
            this.toolStripMenuItem2.Text = "...";
            this.toolStripMenuItem2.Visible = false;
            // 
            // sairToolStripMenuItem
            // 
            this.sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            this.sairToolStripMenuItem.Size = new System.Drawing.Size(170, 24);
            this.sairToolStripMenuItem.Text = "Sair";
            this.sairToolStripMenuItem.Click += new System.EventHandler(this.sairToolStripMenuItem_Click);
            // 
            // editarToolStripMenuItem
            // 
            this.editarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportarToolStripMenuItem,
            this.exportarTexturaToolStripMenuItem,
            this.importarToolStripMenuItem,
            this.importarTexturaToolStripMenuItem});
            this.editarToolStripMenuItem.Name = "editarToolStripMenuItem";
            this.editarToolStripMenuItem.Size = new System.Drawing.Size(60, 23);
            this.editarToolStripMenuItem.Text = "Editar";
            // 
            // exportarToolStripMenuItem
            // 
            this.exportarToolStripMenuItem.Enabled = false;
            this.exportarToolStripMenuItem.Name = "exportarToolStripMenuItem";
            this.exportarToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.exportarToolStripMenuItem.Text = "Exportar";
            this.exportarToolStripMenuItem.Click += new System.EventHandler(this.exportarToolStripMenuItem_Click);
            // 
            // exportarTexturaToolStripMenuItem
            // 
            this.exportarTexturaToolStripMenuItem.Enabled = false;
            this.exportarTexturaToolStripMenuItem.Name = "exportarTexturaToolStripMenuItem";
            this.exportarTexturaToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.exportarTexturaToolStripMenuItem.Text = "Exportar Textura";
            this.exportarTexturaToolStripMenuItem.Click += new System.EventHandler(this.exportarTexturaToolStripMenuItem_Click);
            // 
            // importarToolStripMenuItem
            // 
            this.importarToolStripMenuItem.Enabled = false;
            this.importarToolStripMenuItem.Name = "importarToolStripMenuItem";
            this.importarToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.importarToolStripMenuItem.Text = "Importar";
            this.importarToolStripMenuItem.Visible = false;
            this.importarToolStripMenuItem.Click += new System.EventHandler(this.importarToolStripMenuItem_Click);
            // 
            // importarTexturaToolStripMenuItem
            // 
            this.importarTexturaToolStripMenuItem.Enabled = false;
            this.importarTexturaToolStripMenuItem.Name = "importarTexturaToolStripMenuItem";
            this.importarTexturaToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.importarTexturaToolStripMenuItem.Text = "Importar Textura";
            this.importarTexturaToolStripMenuItem.Visible = false;
            this.importarTexturaToolStripMenuItem.Click += new System.EventHandler(this.importarTexturaToolStripMenuItem_Click);
            // 
            // sobreToolStripMenuItem
            // 
            this.sobreToolStripMenuItem.Name = "sobreToolStripMenuItem";
            this.sobreToolStripMenuItem.Size = new System.Drawing.Size(61, 23);
            this.sobreToolStripMenuItem.Text = "Sobre";
            this.sobreToolStripMenuItem.Click += new System.EventHandler(this.sobreToolStripMenuItem_Click);
            // 
            // Editar
            // 
            this.Editar.Enabled = false;
            this.Editar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Editar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.150944F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Editar.Location = new System.Drawing.Point(36, 357);
            this.Editar.Name = "Editar";
            this.Editar.Size = new System.Drawing.Size(100, 38);
            this.Editar.TabIndex = 4;
            this.Editar.Text = "Editar Texto";
            this.Editar.UseVisualStyleBackColor = true;
            this.Editar.Visible = false;
            this.Editar.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox1.Controls.Add(this.linkLabel1);
            this.groupBox1.Controls.Add(this.path);
            this.groupBox1.Controls.Add(this.archivescount);
            this.groupBox1.Controls.Add(this.size);
            this.groupBox1.Controls.Add(this.filename);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(478, 78);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Informações";
            this.groupBox1.Visible = false;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoEllipsis = true;
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(65, 16);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(0, 16);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // path
            // 
            this.path.AutoSize = true;
            this.path.BackColor = System.Drawing.Color.LightSteelBlue;
            this.path.Location = new System.Drawing.Point(26, 16);
            this.path.Name = "path";
            this.path.Size = new System.Drawing.Size(54, 16);
            this.path.TabIndex = 3;
            this.path.Text = "Local: ";
            // 
            // archivescount
            // 
            this.archivescount.AutoSize = true;
            this.archivescount.BackColor = System.Drawing.Color.LightSteelBlue;
            this.archivescount.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.archivescount.ForeColor = System.Drawing.Color.Black;
            this.archivescount.Location = new System.Drawing.Point(300, 53);
            this.archivescount.Name = "archivescount";
            this.archivescount.Size = new System.Drawing.Size(64, 16);
            this.archivescount.TabIndex = 2;
            this.archivescount.Text = "Pastas: ";
            // 
            // size
            // 
            this.size.AutoSize = true;
            this.size.BackColor = System.Drawing.Color.LightSteelBlue;
            this.size.ForeColor = System.Drawing.Color.Black;
            this.size.Location = new System.Drawing.Point(300, 35);
            this.size.Name = "size";
            this.size.Size = new System.Drawing.Size(81, 16);
            this.size.TabIndex = 1;
            this.size.Text = "Tamanho: ";
            // 
            // filename
            // 
            this.filename.AutoSize = true;
            this.filename.BackColor = System.Drawing.Color.LightSteelBlue;
            this.filename.ForeColor = System.Drawing.Color.Black;
            this.filename.Location = new System.Drawing.Point(55, 44);
            this.filename.Name = "filename";
            this.filename.Size = new System.Drawing.Size(69, 16);
            this.filename.TabIndex = 0;
            this.filename.Text = "Arquivo: ";
            // 
            // abrir
            // 
            this.abrir.AutoSize = true;
            this.abrir.BackColor = System.Drawing.Color.Transparent;
            this.abrir.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.abrir.Location = new System.Drawing.Point(65, 212);
            this.abrir.Name = "abrir";
            this.abrir.Size = new System.Drawing.Size(183, 31);
            this.abrir.TabIndex = 9;
            this.abrir.Text = "Abrir Arquivo";
            this.abrir.Click += new System.EventHandler(this.abrir_Click);
            this.abrir.MouseClick += new System.Windows.Forms.MouseEventHandler(this.abrir_MouseClick);
            this.abrir.MouseEnter += new System.EventHandler(this.abrir_MouseEnter);
            this.abrir.MouseLeave += new System.EventHandler(this.abrir_MouseLeave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(95, 241);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "(Ou arraste e solte aqui)";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 126);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(478, 213);
            this.treeView1.TabIndex = 11;
            this.treeView1.Visible = false;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // Principal
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(518, 422);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Editar);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.abrir);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Principal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NUC RAW Tools";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Principal_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Principal_DragEnter);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem arquivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sobreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abrirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fecharToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sairToolStripMenuItem;
        private System.Windows.Forms.Label path;
        private System.Windows.Forms.Label archivescount;
        private System.Windows.Forms.Label size;
        private System.Windows.Forms.Label filename;
        private System.Windows.Forms.LinkLabel linkLabel1;
        public System.Windows.Forms.Button Editar;
        public System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStripMenuItem salvarComoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salvarToolStripMenuItem;
        private System.Windows.Forms.Label abrir;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ToolStripMenuItem exportarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportarTexturaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importarTexturaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recenteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}