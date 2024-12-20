﻿using Windows.UI.Xaml.Controls;

namespace LegoTrainProject
{
	partial class HubEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HubEditor));
			this.label1 = new System.Windows.Forms.Label();
			this.comboBoxType = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.flowLayoutPanelPort = new System.Windows.Forms.FlowLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.textBoxName = new System.Windows.Forms.TextBox();
			this.comboBoxColors = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.btnRename = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 65);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Hardware:";
			// 
			// comboBoxType
			// 
			this.comboBoxType.FormattingEnabled = true;
			this.comboBoxType.Location = new System.Drawing.Point(66, 62);
			this.comboBoxType.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.comboBoxType.Name = "comboBoxType";
			this.comboBoxType.Size = new System.Drawing.Size(175, 21);
			this.comboBoxType.TabIndex = 1;
			this.comboBoxType.SelectedIndexChanged += new System.EventHandler(this.comboBoxType_SelectedIndexChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.flowLayoutPanelPort);
			this.groupBox1.Location = new System.Drawing.Point(9, 89);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.groupBox1.Size = new System.Drawing.Size(398, 368);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Port Setup";
			// 
			// flowLayoutPanelPort
			// 
			this.flowLayoutPanelPort.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanelPort.Location = new System.Drawing.Point(2, 16);
			this.flowLayoutPanelPort.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.flowLayoutPanelPort.Name = "flowLayoutPanelPort";
			this.flowLayoutPanelPort.Size = new System.Drawing.Size(394, 349);
			this.flowLayoutPanelPort.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(7, 7);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(38, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Name:";
			// 
			// textBoxName
			// 
			this.textBoxName.Location = new System.Drawing.Point(66, 6);
			this.textBoxName.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.textBoxName.Name = "textBoxName";
			this.textBoxName.Size = new System.Drawing.Size(92, 20);
			this.textBoxName.TabIndex = 4;
			this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
			// 
			// comboBoxColors
			// 
			this.comboBoxColors.FormattingEnabled = true;
			this.comboBoxColors.Location = new System.Drawing.Point(66, 33);
			this.comboBoxColors.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.comboBoxColors.Name = "comboBoxColors";
			this.comboBoxColors.Size = new System.Drawing.Size(175, 21);
			this.comboBoxColors.TabIndex = 6;
			this.comboBoxColors.SelectedIndexChanged += new System.EventHandler(this.comboBoxColors_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 36);
			this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(34, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Color:";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button1.Location = new System.Drawing.Point(317, 462);
			this.button1.Margin = new System.Windows.Forms.Padding(2);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(80, 27);
			this.button1.TabIndex = 7;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// btnRename
			// 
			this.btnRename.Location = new System.Drawing.Point(164, 6);
			this.btnRename.Margin = new System.Windows.Forms.Padding(2);
			this.btnRename.Name = "btnRename";
			this.btnRename.Size = new System.Drawing.Size(79, 22);
			this.btnRename.TabIndex = 8;
			this.btnRename.Text = "Rename Hub";
			this.btnRename.UseVisualStyleBackColor = true;
			this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
			// 
			// HubEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.button1;
			this.ClientSize = new System.Drawing.Size(408, 500);
			this.Controls.Add(this.btnRename);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.comboBoxColors);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textBoxName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.comboBoxType);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "HubEditor";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Hub Editor";
			this.Shown += new System.EventHandler(this.HubEditor_Shown);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxType;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelPort;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBoxName;
		private System.Windows.Forms.ComboBox comboBoxColors;
		private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnRename;
    }
}