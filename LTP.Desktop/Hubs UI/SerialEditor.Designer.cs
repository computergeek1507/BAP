
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
            this.toolStripButtonWIO = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEV3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonJRMI = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCRMI = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.listBoxSerialHubs = new System.Windows.Forms.ListBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonDaca,
            this.toolStripButtonWIO,
            this.toolStripButtonEV3,
            this.toolStripButtonJRMI,
            this.toolStripButtonCRMI,
            this.toolStripButtonDelete});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(765, 27);
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
            // toolStripButtonWIO
            // 
            this.toolStripButtonWIO.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonWIO.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonWIO.Image")));
            this.toolStripButtonWIO.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonWIO.Name = "toolStripButtonWIO";
            this.toolStripButtonWIO.Size = new System.Drawing.Size(74, 24);
            this.toolStripButtonWIO.Text = "Add WIO";
            this.toolStripButtonWIO.Click += new System.EventHandler(this.toolStripButtonMTC4PU_Click);
            // 
            // toolStripButtonEV3
            // 
            this.toolStripButtonEV3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonEV3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEV3.Image")));
            this.toolStripButtonEV3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEV3.Name = "toolStripButtonEV3";
            this.toolStripButtonEV3.RightToLeftAutoMirrorImage = true;
            this.toolStripButtonEV3.Size = new System.Drawing.Size(70, 24);
            this.toolStripButtonEV3.Text = "Add EV3";
            this.toolStripButtonEV3.Click += new System.EventHandler(this.toolStripButtonEV3_Click);
            // 
            // toolStripButtonJRMI
            // 
            this.toolStripButtonJRMI.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonJRMI.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonJRMI.Image")));
            this.toolStripButtonJRMI.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonJRMI.Name = "toolStripButtonJRMI";
            this.toolStripButtonJRMI.Size = new System.Drawing.Size(76, 24);
            this.toolStripButtonJRMI.Text = "Add JRMI";
            this.toolStripButtonJRMI.Click += new System.EventHandler(this.toolStripButtonJRMI_Click);
            // 
            // toolStripButtonCRMI
            // 
            this.toolStripButtonCRMI.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonCRMI.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCRMI.Image")));
            this.toolStripButtonCRMI.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCRMI.Name = "toolStripButtonCRMI";
            this.toolStripButtonCRMI.Size = new System.Drawing.Size(80, 24);
            this.toolStripButtonCRMI.Text = "Add CRMI";
            this.toolStripButtonCRMI.Click += new System.EventHandler(this.toolStripButtonCRMI_Click);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.Enabled = false;
            this.toolStripButtonDelete.Image = global::LegoTrainProject.Properties.Resources.icons8_cancel_48;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(87, 24);
            this.toolStripButtonDelete.Text = "Remove";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // listBoxSerialHubs
            // 
            this.listBoxSerialHubs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxSerialHubs.FormattingEnabled = true;
            this.listBoxSerialHubs.ItemHeight = 16;
            this.listBoxSerialHubs.Location = new System.Drawing.Point(12, 46);
            this.listBoxSerialHubs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listBoxSerialHubs.Name = "listBoxSerialHubs";
            this.listBoxSerialHubs.Size = new System.Drawing.Size(744, 404);
            this.listBoxSerialHubs.TabIndex = 1;
            this.listBoxSerialHubs.SelectedIndexChanged += new System.EventHandler(this.listBoxSerialHubs_SelectedIndexChanged);
            // 
            // SerialHubs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 470);
            this.Controls.Add(this.listBoxSerialHubs);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SerialHubs";
            this.Text = "Serial/Ethernet Hubs";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonDaca;
        private System.Windows.Forms.ListBox listBoxSerialHubs;
        private System.Windows.Forms.ToolStripButton toolStripButtonWIO;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
		private System.Windows.Forms.ToolStripButton toolStripButtonEV3;
		private System.Windows.Forms.ToolStripButton toolStripButtonJRMI;
		private System.Windows.Forms.ToolStripButton toolStripButtonCRMI;
	}
}