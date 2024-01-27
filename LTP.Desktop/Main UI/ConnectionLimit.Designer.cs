namespace LegoTrainProject.LTP.Desktop.Main_UI
{
	partial class ConnectionLimit
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionLimit));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.richTextBoxDevices = new System.Windows.Forms.RichTextBox();
			this.radioButton3 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.richTextBoxDevices);
			this.groupBox1.Controls.Add(this.radioButton3);
			this.groupBox1.Controls.Add(this.radioButton2);
			this.groupBox1.Controls.Add(this.radioButton1);
			this.groupBox1.Location = new System.Drawing.Point(10, 5);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.groupBox1.Size = new System.Drawing.Size(454, 232);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// richTextBoxDevices
			// 
			this.richTextBoxDevices.Location = new System.Drawing.Point(5, 79);
			this.richTextBoxDevices.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.richTextBoxDevices.Name = "richTextBoxDevices";
			this.richTextBoxDevices.Size = new System.Drawing.Size(445, 150);
			this.richTextBoxDevices.TabIndex = 5;
			this.richTextBoxDevices.Text = "; Enter Mac Addresses separated by a comma\n";
			this.richTextBoxDevices.TextChanged += new System.EventHandler(this.richTextBoxDevices_TextChanged);
			// 
			// radioButton3
			// 
			this.radioButton3.AutoSize = true;
			this.radioButton3.Location = new System.Drawing.Point(4, 56);
			this.radioButton3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.Size = new System.Drawing.Size(356, 20);
			this.radioButton3.TabIndex = 2;
			this.radioButton3.Text = "Connect only to this specifc list of devices (one mac address per line)";
			this.radioButton3.UseVisualStyleBackColor = true;
			this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
			// 
			// radioButton2
			// 
			this.radioButton2.AutoSize = true;
			this.radioButton2.Location = new System.Drawing.Point(5, 34);
			this.radioButton2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(274, 20);
			this.radioButton2.TabIndex = 1;
			this.radioButton2.Text = "Connect only to pre-connected devices of a project";
			this.radioButton2.UseVisualStyleBackColor = true;
			this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
			// 
			// radioButton1
			// 
			this.radioButton1.AutoSize = true;
			this.radioButton1.Checked = true;
			this.radioButton1.Location = new System.Drawing.Point(5, 13);
			this.radioButton1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(175, 20);
			this.radioButton1.TabIndex = 0;
			this.radioButton1.TabStop = true;
			this.radioButton1.Text = "Do not limit device connection";
			this.radioButton1.UseVisualStyleBackColor = true;
			this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(15, 243);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(418, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Enter one Mac Address per line. Only 12 numbers and letters. Exemple: 90842B04349" +
    "A";
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button1.Location = new System.Drawing.Point(200, 292);
			this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(80, 27);
			this.button1.TabIndex = 2;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// ConnectionLimit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.button1;
			this.ClientSize = new System.Drawing.Size(478, 335);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConnectionLimit";
			this.Text = "Connection Limitation Options";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radioButton3;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RichTextBox richTextBoxDevices;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}