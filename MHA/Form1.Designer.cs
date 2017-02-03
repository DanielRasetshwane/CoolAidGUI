namespace MHA
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.Buttons_panel = new System.Windows.Forms.Panel();
            this.generateGHAbutton = new System.Windows.Forms.Button();
            this.loadDLbutton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadDSLPrescriptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateGHADatafileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Figure_panel = new System.Windows.Forms.Panel();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.Tables_panel = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Buttons_panel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.Figure_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Buttons_panel
            // 
            this.Buttons_panel.BackColor = System.Drawing.Color.Cornsilk;
            this.Buttons_panel.Controls.Add(this.generateGHAbutton);
            this.Buttons_panel.Controls.Add(this.loadDLbutton);
            this.Buttons_panel.Location = new System.Drawing.Point(0, 27);
            this.Buttons_panel.Name = "Buttons_panel";
            this.Buttons_panel.Size = new System.Drawing.Size(779, 87);
            this.Buttons_panel.TabIndex = 0;
            // 
            // generateGHAbutton
            // 
            this.generateGHAbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.generateGHAbutton.Location = new System.Drawing.Point(252, 23);
            this.generateGHAbutton.Name = "generateGHAbutton";
            this.generateGHAbutton.Size = new System.Drawing.Size(184, 39);
            this.generateGHAbutton.TabIndex = 1;
            this.generateGHAbutton.Text = "Generate GHA Datafile";
            this.generateGHAbutton.UseVisualStyleBackColor = true;
            this.generateGHAbutton.Click += new System.EventHandler(this.generateGHAbutton_Click);
            // 
            // loadDLbutton
            // 
            this.loadDLbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadDLbutton.Location = new System.Drawing.Point(46, 23);
            this.loadDLbutton.Name = "loadDLbutton";
            this.loadDLbutton.Size = new System.Drawing.Size(184, 39);
            this.loadDLbutton.TabIndex = 0;
            this.loadDLbutton.Text = "Load DSL Prescription";
            this.loadDLbutton.UseVisualStyleBackColor = true;
            this.loadDLbutton.Click += new System.EventHandler(this.loadDLbutton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Cornsilk;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(781, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadDSLPrescriptionToolStripMenuItem,
            this.generateGHADatafileToolStripMenuItem,
            this.clearAllToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadDSLPrescriptionToolStripMenuItem
            // 
            this.loadDSLPrescriptionToolStripMenuItem.Name = "loadDSLPrescriptionToolStripMenuItem";
            this.loadDSLPrescriptionToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.loadDSLPrescriptionToolStripMenuItem.Text = "Load DSL Prescription...";
            this.loadDSLPrescriptionToolStripMenuItem.Click += new System.EventHandler(this.loadDSLPrescriptionToolStripMenuItem_Click);
            // 
            // generateGHADatafileToolStripMenuItem
            // 
            this.generateGHADatafileToolStripMenuItem.Name = "generateGHADatafileToolStripMenuItem";
            this.generateGHADatafileToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.generateGHADatafileToolStripMenuItem.Text = "Generate GHA Datafile";
            this.generateGHADatafileToolStripMenuItem.Click += new System.EventHandler(this.generateGHADatafileToolStripMenuItem_Click);
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.clearAllToolStripMenuItem.Text = "Clear all";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // Figure_panel
            // 
            this.Figure_panel.BackColor = System.Drawing.Color.Cornsilk;
            this.Figure_panel.Controls.Add(this.zedGraphControl1);
            this.Figure_panel.Location = new System.Drawing.Point(0, 120);
            this.Figure_panel.Name = "Figure_panel";
            this.Figure_panel.Size = new System.Drawing.Size(779, 375);
            this.Figure_panel.TabIndex = 2;
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Location = new System.Drawing.Point(46, 3);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0;
            this.zedGraphControl1.ScrollMaxX = 0;
            this.zedGraphControl1.ScrollMaxY = 0;
            this.zedGraphControl1.ScrollMaxY2 = 0;
            this.zedGraphControl1.ScrollMinX = 0;
            this.zedGraphControl1.ScrollMinY = 0;
            this.zedGraphControl1.ScrollMinY2 = 0;
            this.zedGraphControl1.Size = new System.Drawing.Size(684, 369);
            this.zedGraphControl1.TabIndex = 0;
            // 
            // Tables_panel
            // 
            this.Tables_panel.BackColor = System.Drawing.Color.Cornsilk;
            this.Tables_panel.Location = new System.Drawing.Point(0, 501);
            this.Tables_panel.Name = "Tables_panel";
            this.Tables_panel.Size = new System.Drawing.Size(779, 167);
            this.Tables_panel.TabIndex = 3;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 671);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(781, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 693);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.Tables_panel);
            this.Controls.Add(this.Figure_panel);
            this.Controls.Add(this.Buttons_panel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "MHA User Interface";
            this.Buttons_panel.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.Figure_panel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel Buttons_panel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.Panel Figure_panel;
        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.Panel Tables_panel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button generateGHAbutton;
        private System.Windows.Forms.Button loadDLbutton;
        private System.Windows.Forms.ToolStripMenuItem loadDSLPrescriptionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateGHADatafileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}

