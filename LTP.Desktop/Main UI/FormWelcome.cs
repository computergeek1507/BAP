using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LegoTrainProject.Main_UI
{
	public partial class FormWelcome : Form
	{
		public FormWelcome()
		{
			InitializeComponent();

			Version appVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			richTextBox2.Text =
				$"The Brick Automation Project v{appVersion.Major}.{appVersion.Minor}\n" +
				$"Running on {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}\n\n" +
				richTextBox2.Text;
		}

		private void buttonDonate_Click(object sender, EventArgs e)
		{
			//MainBoard.OpenUrl("https://www.paypal.me/vincentvergonjeanne");
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void richTextBox1_TextChanged(object sender, EventArgs e)
		{

		}

        private void richTextBox2_LinkClicked(object sender, LinkClickedEventArgs e)
        {
			MainBoard.OpenUrl(e.LinkText);
		}
    }
}
