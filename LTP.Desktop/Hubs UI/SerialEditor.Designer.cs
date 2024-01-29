
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
			this.listBoxSerialHubs = new System.Windows.Forms.ListBox();
			this.toolStripButtonDaca = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonMTC4PU = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonEV3 = new System.Windows.Forms.ToolStripButton();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonDaca,
            this.toolStripButtonMTC4PU,
            this.toolStripButtonEV3,
            this.toolStripButton1});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(574, 38);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// listBoxSerialHubs
			// 
			this.listBoxSerialHubs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listBoxSerialHubs.FormattingEnabled = true;
			this.listBoxSerialHubs.Location = new System.Drawing.Point(9, 37);
			this.listBoxSerialHubs.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.listBoxSerialHubs.Name = "listBoxSerialHubs";
			this.listBoxSerialHubs.Size = new System.Drawing.Size(559, 329);
			this.listBoxSerialHubs.TabIndex = 1;
			// 
			// toolStripButtonDaca
			// 
			this.toolStripButtonDaca.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButtonDaca.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDaca.Image")));
			this.toolStripButtonDaca.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonDaca.Name = "toolStripButtonDaca";
			this.toolStripButtonDaca.Size = new System.Drawing.Size(111, 33);
			this.toolStripButtonDaca.Text = "Add DACTA";
			this.toolStripButtonDaca.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.toolStripButtonDaca.Click += new System.EventHandler(this.toolStripButtonDaca_Click);
			// 
			// toolStripButtonMTC4PU
			// 
			this.toolStripButtonMTC4PU.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButtonMTC4PU.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMTC4PU.Image")));
			this.toolStripButtonMTC4PU.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonMTC4PU.Name = "toolStripButtonMTC4PU";
			this.toolStripButtonMTC4PU.Size = new System.Drawing.Size(122, 29);
			this.toolStripButtonMTC4PU.Text = "Add MTC4PU";
			this.toolStripButtonMTC4PU.Click += new System.EventHandler(this.toolStripButtonMTC4PU_Click);
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.Image = global::LegoTrainProject.Properties.Resources.icons8_cancel_48;
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(100, 29);
			this.toolStripButton1.Text = "Remove";
			this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
			// 
			// toolStripButtonEV3
			// 
			this.toolStripButtonEV3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButtonEV3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEV3.Image")));
			this.toolStripButtonEV3.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonEV3.Name = "toolStripButtonEV3";
			this.toolStripButtonEV3.RightToLeftAutoMirrorImage = true;
			this.toolStripButtonEV3.Size = new System.Drawing.Size(85, 29);
			this.toolStripButtonEV3.Text = "Add EV3";
			this.toolStripButtonEV3.Click += new System.EventHandler(this.toolStripButtonEV3_Click);
			// 
			// SerialHubs
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(574, 382);
			this.Controls.Add(this.listBoxSerialHubs);
			this.Controls.Add(this.toolStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
		private System.Windows.Forms.ToolStripButton toolStripButtonEV3;
	}
}