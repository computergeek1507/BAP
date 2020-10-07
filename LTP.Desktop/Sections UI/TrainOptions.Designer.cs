namespace LegoTrainProject.Sections_UI
{
	partial class TrainOptions
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSpeed = new System.Windows.Forms.TextBox();
            this.textBoxClearing = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxCoefficient = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 70);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(357, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Speed when danger ahead (30-100)";
            // 
            // textBoxSpeed
            // 
            this.textBoxSpeed.Location = new System.Drawing.Point(396, 66);
            this.textBoxSpeed.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxSpeed.Name = "textBoxSpeed";
            this.textBoxSpeed.Size = new System.Drawing.Size(148, 31);
            this.textBoxSpeed.TabIndex = 1;
            this.textBoxSpeed.TextChanged += new System.EventHandler(this.textBoxSpeed_TextChanged);
            // 
            // textBoxClearing
            // 
            this.textBoxClearing.Location = new System.Drawing.Point(396, 12);
            this.textBoxClearing.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxClearing.Name = "textBoxClearing";
            this.textBoxClearing.Size = new System.Drawing.Size(148, 31);
            this.textBoxClearing.TabIndex = 4;
            this.textBoxClearing.TextChanged += new System.EventHandler(this.textBoxClearing_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 17);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(360, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Time needed to leave a section (ms)";
            // 
            // textBoxCoefficient
            // 
            this.textBoxCoefficient.Location = new System.Drawing.Point(396, 119);
            this.textBoxCoefficient.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxCoefficient.Name = "textBoxCoefficient";
            this.textBoxCoefficient.Size = new System.Drawing.Size(148, 31);
            this.textBoxCoefficient.TabIndex = 7;
            this.textBoxCoefficient.TextChanged += new System.EventHandler(this.textBoxCoefficient_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 123);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(269, 25);
            this.label3.TabIndex = 6;
            this.label3.Text = "Section\'s Speed Coeficient";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(203, 210);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 52);
            this.button1.TabIndex = 8;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // TrainOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(564, 289);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxCoefficient);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxClearing);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxSpeed);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TrainOptions";
            this.Text = "Train Options";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxSpeed;
		private System.Windows.Forms.TextBox textBoxClearing;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBoxCoefficient;
		private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
    }
}