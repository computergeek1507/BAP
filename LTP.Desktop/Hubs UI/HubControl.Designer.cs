using System.Drawing;

namespace LegoTrainProject
{
	partial class HubControl
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
            this.richTextBoxLabelHub = new System.Windows.Forms.RichTextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ButtonConfigure = new System.Windows.Forms.Button();
            this.pictureBoxStatus = new System.Windows.Forms.PictureBox();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.lblVolts = new System.Windows.Forms.Label();
            this.lblAmps = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBoxLabelHub
            // 
            this.richTextBoxLabelHub.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxLabelHub.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.richTextBoxLabelHub.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxLabelHub.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxLabelHub.Location = new System.Drawing.Point(56, 12);
            this.richTextBoxLabelHub.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.richTextBoxLabelHub.Name = "richTextBoxLabelHub";
            this.richTextBoxLabelHub.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBoxLabelHub.Size = new System.Drawing.Size(262, 30);
            this.richTextBoxLabelHub.TabIndex = 0;
            this.richTextBoxLabelHub.Text = "Red Train - 87%\n";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 111);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(308, 430);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // ButtonConfigure
            // 
            this.ButtonConfigure.Location = new System.Drawing.Point(10, 53);
            this.ButtonConfigure.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ButtonConfigure.Name = "ButtonConfigure";
            this.ButtonConfigure.Size = new System.Drawing.Size(147, 42);
            this.ButtonConfigure.TabIndex = 2;
            this.ButtonConfigure.Text = "Configure";
            this.ButtonConfigure.UseVisualStyleBackColor = true;
            this.ButtonConfigure.Click += new System.EventHandler(this.ButtonConfigure_Click);
            // 
            // pictureBoxStatus
            // 
            this.pictureBoxStatus.Image = global::LegoTrainProject.Properties.Resources.disconnected;
            this.pictureBoxStatus.Location = new System.Drawing.Point(8, 5);
            this.pictureBoxStatus.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBoxStatus.Name = "pictureBoxStatus";
            this.pictureBoxStatus.Size = new System.Drawing.Size(39, 41);
            this.pictureBoxStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxStatus.TabIndex = 3;
            this.pictureBoxStatus.TabStop = false;
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Image = global::LegoTrainProject.Properties.Resources.icons8_close_window_48;
            this.buttonDisconnect.Location = new System.Drawing.Point(164, 53);
            this.buttonDisconnect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(48, 42);
            this.buttonDisconnect.TabIndex = 4;
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // lblVolts
            // 
            this.lblVolts.AutoSize = true;
            this.lblVolts.Font = new System.Drawing.Font("Segoe UI Semibold", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolts.Location = new System.Drawing.Point(215, 52);
            this.lblVolts.Name = "lblVolts";
            this.lblVolts.Size = new System.Drawing.Size(20, 21);
            this.lblVolts.TabIndex = 5;
            this.lblVolts.Text = "V";
            // 
            // lblAmps
            // 
            this.lblAmps.AutoSize = true;
            this.lblAmps.Font = new System.Drawing.Font("Segoe UI Semibold", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmps.Location = new System.Drawing.Point(215, 75);
            this.lblAmps.Name = "lblAmps";
            this.lblAmps.Size = new System.Drawing.Size(35, 21);
            this.lblAmps.TabIndex = 6;
            this.lblAmps.Text = "mA";
            // 
            // HubControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.lblAmps);
            this.Controls.Add(this.lblVolts);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.pictureBoxStatus);
            this.Controls.Add(this.ButtonConfigure);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.richTextBoxLabelHub);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "HubControl";
            this.Size = new System.Drawing.Size(320, 536);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RichTextBox richTextBoxLabelHub;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button ButtonConfigure;
		private System.Windows.Forms.PictureBox pictureBoxStatus;
		private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.Label lblVolts;
        private System.Windows.Forms.Label lblAmps;
    }
}
