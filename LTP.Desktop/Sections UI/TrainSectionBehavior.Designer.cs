namespace LegoTrainProject.Sections_UI
{
	partial class TrainSectionBehavior
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
            this.checkBox2Ahead = new System.Windows.Forms.CheckBox();
            this.textBoxMaxSpeed = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonEditCode = new System.Windows.Forms.Button();
            this.radioButtonExecute = new System.Windows.Forms.RadioButton();
            this.radioButtonResume = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox2Ahead
            // 
            this.checkBox2Ahead.AutoSize = true;
            this.checkBox2Ahead.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox2Ahead.Location = new System.Drawing.Point(9, 52);
            this.checkBox2Ahead.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox2Ahead.Name = "checkBox2Ahead";
            this.checkBox2Ahead.Size = new System.Drawing.Size(314, 34);
            this.checkBox2Ahead.TabIndex = 13;
            this.checkBox2Ahead.Text = "Need next 2 Sections cleared";
            this.checkBox2Ahead.UseVisualStyleBackColor = true;
            this.checkBox2Ahead.CheckedChanged += new System.EventHandler(this.checkBox2Ahead_CheckedChanged);
            // 
            // textBoxMaxSpeed
            // 
            this.textBoxMaxSpeed.Location = new System.Drawing.Point(44, 77);
            this.textBoxMaxSpeed.Name = "textBoxMaxSpeed";
            this.textBoxMaxSpeed.Size = new System.Drawing.Size(187, 31);
            this.textBoxMaxSpeed.TabIndex = 12;
            this.textBoxMaxSpeed.TextChanged += new System.EventHandler(this.textBoxMaxSpeed_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox2Ahead);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(26, 275);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(364, 116);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "When Leaving Section";
            // 
            // buttonEditCode
            // 
            this.buttonEditCode.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonEditCode.Location = new System.Drawing.Point(44, 166);
            this.buttonEditCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonEditCode.Name = "buttonEditCode";
            this.buttonEditCode.Size = new System.Drawing.Size(200, 50);
            this.buttonEditCode.TabIndex = 16;
            this.buttonEditCode.Text = "Edit Code";
            this.buttonEditCode.UseVisualStyleBackColor = true;
            this.buttonEditCode.Click += new System.EventHandler(this.buttonEditCode_Click);
            // 
            // radioButtonExecute
            // 
            this.radioButtonExecute.AutoSize = true;
            this.radioButtonExecute.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonExecute.Location = new System.Drawing.Point(9, 123);
            this.radioButtonExecute.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButtonExecute.Name = "radioButtonExecute";
            this.radioButtonExecute.Size = new System.Drawing.Size(247, 34);
            this.radioButtonExecute.TabIndex = 15;
            this.radioButtonExecute.TabStop = true;
            this.radioButtonExecute.Text = "Execute Custom Code";
            this.radioButtonExecute.UseVisualStyleBackColor = true;
            this.radioButtonExecute.CheckedChanged += new System.EventHandler(this.radioButtonExecute_CheckedChanged);
            // 
            // radioButtonResume
            // 
            this.radioButtonResume.AutoSize = true;
            this.radioButtonResume.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonResume.Location = new System.Drawing.Point(9, 33);
            this.radioButtonResume.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButtonResume.Name = "radioButtonResume";
            this.radioButtonResume.Size = new System.Drawing.Size(261, 34);
            this.radioButtonResume.TabIndex = 14;
            this.radioButtonResume.TabStop = true;
            this.radioButtonResume.Text = "Resume At Fixed Speed";
            this.radioButtonResume.UseVisualStyleBackColor = true;
            this.radioButtonResume.CheckedChanged += new System.EventHandler(this.radioButtonResume_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonEditCode);
            this.groupBox2.Controls.Add(this.radioButtonExecute);
            this.groupBox2.Controls.Add(this.radioButtonResume);
            this.groupBox2.Controls.Add(this.textBoxMaxSpeed);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(26, 19);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(364, 247);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "When Entering Section";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(136, 413);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 52);
            this.button1.TabIndex = 16;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TrainSectionBehavior
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(408, 496);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TrainSectionBehavior";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Train Behavior";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.CheckBox checkBox2Ahead;
		private System.Windows.Forms.TextBox textBoxMaxSpeed;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton radioButtonExecute;
		private System.Windows.Forms.RadioButton radioButtonResume;
		private System.Windows.Forms.Button buttonEditCode;
        private System.Windows.Forms.Button button1;
    }
}