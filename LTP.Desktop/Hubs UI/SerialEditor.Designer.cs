
namespace LegoTrainProject
{
    partial class SerialHubs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SerialHubs));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonDaca = new System.Windows.Forms.ToolStripButton();
            this.listBoxSerialHubs = new System.Windows.Forms.ListBox();
            this.toolStripButtonMTC4PU = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonDaca,
            this.toolStripButtonMTC4PU,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(556, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonDaca
            // 
            this.toolStripButtonDaca.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonDaca.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDaca.Image")));
            this.toolStripButtonDaca.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDaca.Name = "toolStripButtonDaca";
            this.toolStripButtonDaca.Size = new System.Drawing.Size(92, 24);
            this.toolStripButtonDaca.Text = "Add DACTA";
            this.toolStripButtonDaca.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.toolStripButtonDaca.Click += new System.EventHandler(this.toolStripButtonDaca_Click);
            // 
            // listBoxSerialHubs
            // 
            this.listBoxSerialHubs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxSerialHubs.FormattingEnabled = true;
            this.listBoxSerialHubs.ItemHeight = 16;
            this.listBoxSerialHubs.Location = new System.Drawing.Point(12, 30);
            this.listBoxSerialHubs.Name = "listBoxSerialHubs";
            this.listBoxSerialHubs.Size = new System.Drawing.Size(534, 308);
            this.listBoxSerialHubs.TabIndex = 1;
            // 
            // toolStripButtonMTC4PU
            // 
            this.toolStripButtonMTC4PU.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonMTC4PU.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMTC4PU.Image")));
            this.toolStripButtonMTC4PU.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMTC4PU.Name = "toolStripButtonMTC4PU";
            this.toolStripButtonMTC4PU.Size = new System.Drawing.Size(100, 24);
            this.toolStripButtonMTC4PU.Text = "Add MTC4PU";
            this.toolStripButtonMTC4PU.Click += new System.EventHandler(this.toolStripButtonMTC4PU_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::LegoTrainProject.Properties.Resources.icons8_cancel_48;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(87, 24);
            this.toolStripButton1.Text = "Remove";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // SerialHubs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 347);
            this.Controls.Add(this.listBoxSerialHubs);
            this.Controls.Add(this.toolStrip1);
            this.Name = "SerialHubs";
            this.Text = "Serial Hubs";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonDaca;
        private System.Windows.Forms.ListBox listBoxSerialHubs;
        private System.Windows.Forms.ToolStripButton toolStripButtonMTC4PU;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}