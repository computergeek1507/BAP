namespace LegoTrainProject
{
	partial class SectionsEditor
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.buttonStart = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAddSection = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEditPaths = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBoxTrains = new System.Windows.Forms.GroupBox();
            this.labelNoTrains = new System.Windows.Forms.Label();
            this.flowLayoutPanelTrains = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBoxSections = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanelSections = new System.Windows.Forms.FlowLayoutPanel();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxTrains.SuspendLayout();
            this.groupBoxSections.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonStart,
            this.toolStripButtonAddSection,
            this.toolStripButtonEditPaths});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.toolStrip.Size = new System.Drawing.Size(1408, 50);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip1";
            // 
            // buttonStart
            // 
            this.buttonStart.Image = global::LegoTrainProject.Properties.Resources.icons8_start_48;
            this.buttonStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(91, 36);
            this.buttonStart.Text = "Start";
            this.buttonStart.Click += new System.EventHandler(this.toolStripButtonStart_Click);
            // 
            // toolStripButtonAddSection
            // 
            this.toolStripButtonAddSection.Image = global::LegoTrainProject.Properties.Resources.icons8_add_tracks_48;
            this.toolStripButtonAddSection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddSection.Name = "toolStripButtonAddSection";
            this.toolStripButtonAddSection.Size = new System.Drawing.Size(172, 36);
            this.toolStripButtonAddSection.Text = "Add Section";
            this.toolStripButtonAddSection.Click += new System.EventHandler(this.toolStripButtonAddSection_Click);
            // 
            // toolStripButtonEditPaths
            // 
            this.toolStripButtonEditPaths.Image = global::LegoTrainProject.Properties.Resources.icons8_navigator_48;
            this.toolStripButtonEditPaths.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEditPaths.Name = "toolStripButtonEditPaths";
            this.toolStripButtonEditPaths.Size = new System.Drawing.Size(214, 36);
            this.toolStripButtonEditPaths.Text = "Edit Trains Paths";
            this.toolStripButtonEditPaths.Click += new System.EventHandler(this.toolStripButtonEditPaths_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 62);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxTrains);
            this.splitContainer1.Panel1MinSize = 285;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxSections);
            this.splitContainer1.Size = new System.Drawing.Size(1406, 638);
            this.splitContainer1.SplitterDistance = 285;
            this.splitContainer1.SplitterWidth = 15;
            this.splitContainer1.TabIndex = 3;
            // 
            // groupBoxTrains
            // 
            this.groupBoxTrains.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTrains.Controls.Add(this.labelNoTrains);
            this.groupBoxTrains.Controls.Add(this.flowLayoutPanelTrains);
            this.groupBoxTrains.Location = new System.Drawing.Point(6, 5);
            this.groupBoxTrains.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxTrains.Name = "groupBoxTrains";
            this.groupBoxTrains.Padding = new System.Windows.Forms.Padding(4, 3, 4, 0);
            this.groupBoxTrains.Size = new System.Drawing.Size(275, 616);
            this.groupBoxTrains.TabIndex = 1;
            this.groupBoxTrains.TabStop = false;
            this.groupBoxTrains.Text = "Trains Position";
            // 
            // labelNoTrains
            // 
            this.labelNoTrains.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labelNoTrains.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNoTrains.Location = new System.Drawing.Point(104, 98);
            this.labelNoTrains.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNoTrains.Name = "labelNoTrains";
            this.labelNoTrains.Size = new System.Drawing.Size(280, 141);
            this.labelNoTrains.TabIndex = 0;
            this.labelNoTrains.Text = "Select the \'TRAIN MOTOR\' type on your Motors to make them appear as a Train here." +
    "";
            this.labelNoTrains.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanelTrains
            // 
            this.flowLayoutPanelTrains.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelTrains.AutoScroll = true;
            this.flowLayoutPanelTrains.BackColor = System.Drawing.SystemColors.ControlLight;
            this.flowLayoutPanelTrains.Location = new System.Drawing.Point(8, 34);
            this.flowLayoutPanelTrains.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.flowLayoutPanelTrains.Name = "flowLayoutPanelTrains";
            this.flowLayoutPanelTrains.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.flowLayoutPanelTrains.Size = new System.Drawing.Size(253, 595);
            this.flowLayoutPanelTrains.TabIndex = 2;
            // 
            // groupBoxSections
            // 
            this.groupBoxSections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSections.BackColor = System.Drawing.SystemColors.Control;
            this.groupBoxSections.Controls.Add(this.flowLayoutPanelSections);
            this.groupBoxSections.Location = new System.Drawing.Point(4, 5);
            this.groupBoxSections.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxSections.Name = "groupBoxSections";
            this.groupBoxSections.Padding = new System.Windows.Forms.Padding(4, 3, 4, 0);
            this.groupBoxSections.Size = new System.Drawing.Size(739, 638);
            this.groupBoxSections.TabIndex = 2;
            this.groupBoxSections.TabStop = false;
            this.groupBoxSections.Text = "Sections";
            // 
            // flowLayoutPanelSections
            // 
            this.flowLayoutPanelSections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelSections.AutoScroll = true;
            this.flowLayoutPanelSections.BackColor = System.Drawing.SystemColors.ControlLight;
            this.flowLayoutPanelSections.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelSections.Location = new System.Drawing.Point(10, 34);
            this.flowLayoutPanelSections.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.flowLayoutPanelSections.Name = "flowLayoutPanelSections";
            this.flowLayoutPanelSections.Size = new System.Drawing.Size(831, 599);
            this.flowLayoutPanelSections.TabIndex = 1;
            // 
            // SectionsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            this.Name = "SectionsEditor";
            this.Size = new System.Drawing.Size(1408, 698);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxTrains.ResumeLayout(false);
            this.groupBoxSections.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton toolStripButtonAddSection;
		private System.Windows.Forms.ToolStripButton buttonStart;
		private System.Windows.Forms.ToolStripButton toolStripButtonEditPaths;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.GroupBox groupBoxTrains;
		private System.Windows.Forms.Label labelNoTrains;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelTrains;
		private System.Windows.Forms.GroupBox groupBoxSections;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelSections;
	}
}
