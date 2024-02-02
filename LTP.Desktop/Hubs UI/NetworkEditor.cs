using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LegoTrainProject
{
    public partial class NetworkEditor : Form
    {
        public NetworkEditor()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.MQTTClient = textBoxClient.Text;
            Properties.Settings.Default.MQTTServer = textBoxServer.Text;
            Properties.Settings.Default.MQTTPort = decimal.ToInt32(numericUpDownPort.Value);

            Properties.Settings.Default.MQTTPassword = textBoxPassword.Text.ToSecureString().EncryptString();
            Properties.Settings.Default.Save();
        }

        private void NetworkEditor_Load(object sender, EventArgs e)
        {
            textBoxClient.Text = Properties.Settings.Default.MQTTClient;
            textBoxServer.Text = Properties.Settings.Default.MQTTServer;
            numericUpDownPort.Value = Properties.Settings.Default.MQTTPort;
            textBoxUsername.Text = Properties.Settings.Default.MQTTUser;
            //textBoxPassword.Text = pass.ToSecureString().DecryptString().ToInsecureString();

            using (var secureString = Properties.Settings.Default.MQTTPassword.DecryptString())
            {
                textBoxPassword.Text = secureString.ToInsecureString();
            }
        }
    }
}
