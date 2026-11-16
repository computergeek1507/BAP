namespace LegoTrainProject.Main_UI
{
	partial class FormWelcome
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWelcome));
			richTextBox1 = new System.Windows.Forms.RichTextBox();
			button1 = new System.Windows.Forms.Button();
			richTextBox2 = new System.Windows.Forms.RichTextBox();
			pictureBox1 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			SuspendLayout();
			// 
			// richTextBox1
			// 
			richTextBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			richTextBox1.BackColor = System.Drawing.SystemColors.Window;
			richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			richTextBox1.Location = new System.Drawing.Point(16, 272);
			richTextBox1.Margin = new System.Windows.Forms.Padding(14, 16, 14, 16);
			richTextBox1.Name = "richTextBox1";
			richTextBox1.ReadOnly = true;
			richTextBox1.RightMargin = 500;
			richTextBox1.ShowSelectionMargin = true;
			richTextBox1.Size = new System.Drawing.Size(805, 508);
			richTextBox1.TabIndex = 0;
			richTextBox1.Text = resources.GetString("richTextBox1.Text");
			richTextBox1.TextChanged += richTextBox1_TextChanged;
			// 
			// button1
			// 
			button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			button1.Location = new System.Drawing.Point(679, 22);
			button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(126, 52);
			button1.TabIndex = 2;
			button1.Text = "Close";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// richTextBox2
			// 
			richTextBox2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			richTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
			richTextBox2.Location = new System.Drawing.Point(255, 22);
			richTextBox2.Margin = new System.Windows.Forms.Padding(14, 16, 14, 16);
			richTextBox2.Name = "richTextBox2";
			richTextBox2.ShowSelectionMargin = true;
			richTextBox2.Size = new System.Drawing.Size(406, 228);
			richTextBox2.TabIndex = 4;
			richTextBox2.Text = resources.GetString("richTextBox2.Text");
			richTextBox2.LinkClicked += richTextBox2_LinkClicked;
			// 
			// pictureBox1
			// 
			pictureBox1.Image = Properties.Resources._2019_02_09_17_51_12;
			pictureBox1.Location = new System.Drawing.Point(16, 22);
			pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new System.Drawing.Size(224, 228);
			pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			pictureBox1.TabIndex = 3;
			pictureBox1.TabStop = false;
			// 
			// FormWelcome
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(835, 808);
			Controls.Add(richTextBox2);
			Controls.Add(pictureBox1);
			Controls.Add(button1);
			Controls.Add(richTextBox1);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			Name = "FormWelcome";
			SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "About BAP";
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.RichTextBox richTextBox2;
	}
}