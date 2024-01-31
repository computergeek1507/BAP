using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LegoTrainProject
{
	public partial class SerialHubs : Form
	{
		public delegate void NewOtherHubEventHandler(string name, Hub.Types hType);
		public static NewOtherHubEventHandler OtherHubUpdate;
		TrainProject m_train;

		public SerialHubs(TrainProject train)
		{
			InitializeComponent();

			m_train = train;
			RedrawItems();

		}
		private void RedrawItems()
		{
			listBoxSerialHubs.Items.Clear();
			foreach (var hub in m_train.RegisteredTrains)
			{
				if (hub.Type == Hub.Types.DACTA || hub.Type == Hub.Types.EV3 || hub.Type == Hub.Types.WIO)
				{
					listBoxSerialHubs.Items.Add($"{hub.Type}: {hub.DeviceId}");
				}
			}
		}

        private void toolStripButtonMTC4PU_Click(object sender, EventArgs e)
        {
			string result = Microsoft.VisualBasic.Interaction.InputBox("Enter WIO RocRail ID", "MTC4PU WIO ID", "1");
			if (result != string.Empty)
			{
				OnHubAdded(result, Hub.Types.WIO);
			}
			RedrawItems();
		}

		private void toolStripButtonDaca_Click(object sender, EventArgs e)
        {
			ListSelection dlg = new ListSelection("Select Serial Port", SerialPort.GetPortNames());
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				OnHubAdded(dlg.ListItemSelected, Hub.Types.DACTA);
			}
			RedrawItems();
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

		private void toolStripButtonEV3_Click(object sender, EventArgs e)
		{
			ListSelection dlg = new ListSelection("Select Serial Port", SerialPort.GetPortNames());
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				OnHubAdded(dlg.ListItemSelected, Hub.Types.EV3);
			}
			RedrawItems();
		}

		public static void OnHubAdded(string name, Hub.Types hType)
		{
			if (null == OtherHubUpdate)
				return;
			OtherHubUpdate.Invoke(name, hType);
		}

		void AddNewOtherHub(string name, Hub.Types hType)
		{
			OnHubAdded(name, hType);
		}
	}
}
