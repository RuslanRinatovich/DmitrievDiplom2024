namespace Kvartirs
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMaterial = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.кадастровыйУчётToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbMaterial = new System.Windows.Forms.ToolStripButton();
            this.tsbBuilding = new System.Windows.Forms.ToolStripButton();
            this.tsbFlat = new System.Windows.Forms.ToolStripButton();
            this.tsbExit = new System.Windows.Forms.ToolStripButton();
            this.mnuFlats = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBuilding = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.кадастровыйУчётToolStripMenuItem,
            this.mnuExit});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(759, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuMaterial,
            this.toolStripMenuItem2});
            this.toolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem1.Image")));
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(110, 20);
            this.toolStripMenuItem1.Text = "Справочники";
            // 
            // mnuMaterial
            // 
            this.mnuMaterial.Image = ((System.Drawing.Image)(resources.GetObject("mnuMaterial.Image")));
            this.mnuMaterial.Name = "mnuMaterial";
            this.mnuMaterial.Size = new System.Drawing.Size(152, 22);
            this.mnuMaterial.Text = "Виды спорта";
            this.mnuMaterial.Click += new System.EventHandler(this.mnuMaterial_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem2.Image")));
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem2.Text = "Клиенты";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.mnuCustomer_Click);
            // 
            // кадастровыйУчётToolStripMenuItem
            // 
            this.кадастровыйУчётToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuBuilding,
            this.mnuFlats});
            this.кадастровыйУчётToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("кадастровыйУчётToolStripMenuItem.Image")));
            this.кадастровыйУчётToolStripMenuItem.Name = "кадастровыйУчётToolStripMenuItem";
            this.кадастровыйУчётToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.кадастровыйУчётToolStripMenuItem.Text = "Ставки";
            // 
            // mnuExit
            // 
            this.mnuExit.Image = ((System.Drawing.Image)(resources.GetObject("mnuExit.Image")));
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(69, 20);
            this.mnuExit.Text = "Выход";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbMaterial,
            this.toolStripButton1,
            this.tsbBuilding,
            this.tsbFlat,
            this.tsbExit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(759, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbMaterial
            // 
            this.tsbMaterial.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMaterial.Image = ((System.Drawing.Image)(resources.GetObject("tsbMaterial.Image")));
            this.tsbMaterial.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMaterial.Name = "tsbMaterial";
            this.tsbMaterial.Size = new System.Drawing.Size(23, 22);
            this.tsbMaterial.Text = "toolStripButton1";
            this.tsbMaterial.ToolTipText = "Виды спорта";
            this.tsbMaterial.Click += new System.EventHandler(this.mnuMaterial_Click);
            // 
            // tsbBuilding
            // 
            this.tsbBuilding.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbBuilding.Image = ((System.Drawing.Image)(resources.GetObject("tsbBuilding.Image")));
            this.tsbBuilding.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbBuilding.Name = "tsbBuilding";
            this.tsbBuilding.Size = new System.Drawing.Size(23, 22);
            this.tsbBuilding.Text = "toolStripButton3";
            this.tsbBuilding.ToolTipText = "События";
            this.tsbBuilding.Click += new System.EventHandler(this.mnuBuilding_Click);
            // 
            // tsbFlat
            // 
            this.tsbFlat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbFlat.Image = ((System.Drawing.Image)(resources.GetObject("tsbFlat.Image")));
            this.tsbFlat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFlat.Name = "tsbFlat";
            this.tsbFlat.Size = new System.Drawing.Size(23, 22);
            this.tsbFlat.Text = "toolStripButton4";
            this.tsbFlat.ToolTipText = "Исходы";
            this.tsbFlat.Click += new System.EventHandler(this.mnuFlats_Click);
            // 
            // tsbExit
            // 
            this.tsbExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExit.Image = ((System.Drawing.Image)(resources.GetObject("tsbExit.Image")));
            this.tsbExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExit.Name = "tsbExit";
            this.tsbExit.Size = new System.Drawing.Size(23, 22);
            this.tsbExit.Text = "toolStripButton5";
            this.tsbExit.ToolTipText = "Выход";
            this.tsbExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuFlats
            // 
            this.mnuFlats.Image = ((System.Drawing.Image)(resources.GetObject("mnuFlats.Image")));
            this.mnuFlats.Name = "mnuFlats";
            this.mnuFlats.Size = new System.Drawing.Size(152, 22);
            this.mnuFlats.Text = "Исходы";
            this.mnuFlats.Click += new System.EventHandler(this.mnuFlats_Click);
            // 
            // mnuBuilding
            // 
            this.mnuBuilding.Image = ((System.Drawing.Image)(resources.GetObject("mnuBuilding.Image")));
            this.mnuBuilding.Name = "mnuBuilding";
            this.mnuBuilding.Size = new System.Drawing.Size(152, 22);
            this.mnuBuilding.Text = "События";
            this.mnuBuilding.Click += new System.EventHandler(this.mnuBuilding_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.ToolTipText = "Клиенты";
            this.toolStripButton1.Click += new System.EventHandler(this.mnuCustomer_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 398);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Букмекерская контора ОК";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem кадастровыйУчётToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem mnuMaterial;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbMaterial;
        private System.Windows.Forms.ToolStripButton tsbBuilding;
        private System.Windows.Forms.ToolStripButton tsbFlat;
        private System.Windows.Forms.ToolStripButton tsbExit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem mnuBuilding;
        private System.Windows.Forms.ToolStripMenuItem mnuFlats;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}