using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.UI;

namespace LegoTrainProject
{
	public partial class HubEditor : Form
	{
		public Hub CurrentHub;

		// added by Tom Cook to have a project of all connected hubs for MU function
		private TrainProject Project;

		//modified by Tom Cook to add project of connected hubs for MU function
		//public HubEditor(Hub hub)
		public HubEditor(Hub hub, TrainProject project)
		{
			//added by Tom Cook, see above
			Project = project;

			CurrentHub = hub;
			InitializeComponent();

			foreach (Hub.Types ev in (Hub.Types[])Enum.GetValues(typeof(Hub.Types)))
				comboBoxType.Items.Add(Enum.GetName(typeof(Hub.Types), ev));

			foreach (Port.Colors ev in (Port.Colors[])Enum.GetValues(typeof(Port.Colors)))
				comboBoxColors.Items.Add(Enum.GetName(typeof(Port.Colors), ev));

			if (hub.Type == Hub.Types.SBRICK)
				comboBoxType.Enabled = false;

			btnRename.Enabled = CurrentHub.Type != Hub.Types.WEDO_2_SMART_HUB;

		}

		public void InitPortComponents()
		{
			flowLayoutPanelPort.Controls.Clear();

			foreach (Port p in CurrentHub.RegistredPorts)
			{
				Label labelPortName = new Label();
				labelPortName.Text = "Port " + p.Id;
				labelPortName.Padding = new Padding(0, 6, 0, 0);
				labelPortName.Width = 65;
				flowLayoutPanelPort.Controls.Add(labelPortName);

				ComboBox comboBoxPort = new ComboBox();
				comboBoxPort.Width = 150;
				comboBoxPort.DropDownStyle = ComboBoxStyle.DropDownList;
				comboBoxPort.Tag = p;

				foreach (Port.Functions t in (Port.Functions[])Enum.GetValues(typeof(Port.Functions)))
				{
					comboBoxPort.Items.Add(Enum.GetName(typeof(Port.Functions), t).Replace('_', ' '));

					if (p.Function == t)
						comboBoxPort.SelectedIndex = (int)t;
				}

				comboBoxPort.SelectedIndexChanged += ComboBoxPort_SelectedIndexChanged;

				//added by Tom Cook for invert and MU functions
				if (p.Function == Port.Functions.MOTOR ||
					p.Function == Port.Functions.TRAIN_MOTOR ||
					p.Function == Port.Functions.SWITCH_DOUBLECROSS ||
					p.Function == Port.Functions.SWITCH_STANDARD ||
					p.Function == Port.Functions.SWITCH_TRIXBRIX ||
					p.Function == Port.Functions.SWITCH_INFINITE)
				{
					Label labelHeaderMotor = new Label();
					labelHeaderMotor.Text = $"Invert Direction:";
					labelHeaderMotor.Font = new Font(new FontFamily("Segoe UI"), 7.1f, FontStyle.Bold);
					labelHeaderMotor.Padding = new Padding(0, 7, 0, 0);
					labelHeaderMotor.Width = 90;
					labelHeaderMotor.Height = 27;

					CheckBox checkBoxInvert = new CheckBox();
					checkBoxInvert.Width = 30;
					checkBoxInvert.Checked = p.Inverted;
					checkBoxInvert.CheckedChanged += (e, f) => p.Inverted = checkBoxInvert.Checked;

					Label labelMU = new Label();
					labelMU.Text = $"MU:";
					labelMU.Font = new Font(new FontFamily("Segoe UI"), 7.1f, FontStyle.Bold);
					labelMU.Padding = new Padding(0, 7, 0, 0);
					labelMU.Width = 30;
					labelMU.Height = 27;

					ComboBox comboBoxMUcolor = new ComboBox();
					comboBoxMUcolor.Width = 70;
					comboBoxMUcolor.DropDownStyle = ComboBoxStyle.DropDownList;
					comboBoxMUcolor.Tag = p;
					comboBoxMUcolor.Items.Add("");
					comboBoxMUcolor.Items.Add("BLACK");
					comboBoxMUcolor.Items.Add("PINK");
					comboBoxMUcolor.Items.Add("PURPLE");
					comboBoxMUcolor.Items.Add("BLUE");
					comboBoxMUcolor.Items.Add("LBLUE");
					comboBoxMUcolor.Items.Add("CYAN");
					comboBoxMUcolor.Items.Add("GREEN");
					comboBoxMUcolor.Items.Add("YELLOW");
					comboBoxMUcolor.Items.Add("ORANGE");
					comboBoxMUcolor.Items.Add("RED");
					comboBoxMUcolor.Items.Add("WHITE");
					//comboBoxMUcolor.SelectedIndexChanged += (e, f) => p.MUcolor = comboBoxMUcolor.SelectedIndex;
					comboBoxMUcolor.SelectedIndexChanged += ComboBoxMUcolor_SelectedIndexChanged;
					comboBoxMUcolor.SelectedIndex = p.MUcolor;

					flowLayoutPanelPort.Controls.Add(comboBoxPort);
					flowLayoutPanelPort.Controls.Add(labelHeaderMotor);
					flowLayoutPanelPort.Controls.Add(checkBoxInvert);
					flowLayoutPanelPort.Controls.Add(labelMU);
					flowLayoutPanelPort.Controls.Add(comboBoxMUcolor);
					flowLayoutPanelPort.SetFlowBreak(comboBoxMUcolor, true);

					this.Width = 520;
				}

				//added by Tom Cook	see above
				else

				if (p.Function == Port.Functions.SENSOR)
				{
					Label labelHeader = new Label();
					labelHeader.Text = $"Trigger Cooldown (ms)";
					labelHeader.Font = new Font(new FontFamily("Segoe UI"), 7.1f, FontStyle.Bold);

					labelHeader.Padding = new Padding(0, 0, 0, 0);
					labelHeader.Width = 80;
					labelHeader.Height = 27;

					TextBox textBoxDistanceTimer = new TextBox();
					textBoxDistanceTimer.Width = 60;
					textBoxDistanceTimer.Text = p.DistanceColorCooldownMs.ToString();
					textBoxDistanceTimer.TextChanged += (e, f) =>
					{
						if (Int32.TryParse(textBoxDistanceTimer.Text, out int value))
							p.DistanceColorCooldownMs = value;
					};

					flowLayoutPanelPort.Controls.Add(comboBoxPort);
					flowLayoutPanelPort.Controls.Add(labelHeader);
					flowLayoutPanelPort.Controls.Add(textBoxDistanceTimer);
					flowLayoutPanelPort.SetFlowBreak(textBoxDistanceTimer, true);

					this.Width = 520;
				}
				else
				{
					flowLayoutPanelPort.Controls.Add(comboBoxPort);
					flowLayoutPanelPort.SetFlowBreak(comboBoxPort, true);
				}

			}
		}


		private void ComboBoxPort_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox portBox = (ComboBox)sender;
			Port portToChange = (Port)portBox.Tag;
			portToChange.Function = (Port.Functions)portBox.SelectedIndex;

			CurrentHub.TrainMotorPort = null;
			foreach (Port port in CurrentHub.RegistredPorts)
				if (port.Function == Port.Functions.TRAIN_MOTOR)
					CurrentHub.TrainMotorPort = port.Id;

			InitPortComponents();
		}

		//added by Tom Cook for MU function
		private void ComboBoxMUcolor_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox MUcolorBox = (ComboBox)sender;
			Port portToChange = (Port)MUcolorBox.Tag;
			portToChange.MUcolor = MUcolorBox.SelectedIndex;
			switch (MUcolorBox.SelectedIndex)
            {
				case 0:
					//BLANK
					portToChange.MUbackcolor = System.Drawing.Color.Empty;
					break;
				case 1:
					//BLACK
					portToChange.MUbackcolor = System.Drawing.Color.Black;
					break;
				case 2:
					//PINK
					portToChange.MUbackcolor = System.Drawing.Color.Pink;
					break;
				case 3:
					//PURPLE
					portToChange.MUbackcolor = System.Drawing.Color.Purple;
					break;
				case 4:
					//BLUE
					portToChange.MUbackcolor = System.Drawing.Color.Blue;
					break;
				case 5:
					//LBLUE
					portToChange.MUbackcolor = System.Drawing.Color.LightBlue;
					break;
				case 6:
					//CYAN
					portToChange.MUbackcolor = System.Drawing.Color.Cyan;
					break;
				case 7:
					//GREEN
					portToChange.MUbackcolor = System.Drawing.Color.Green;
					break;
				case 8:
					//YELLOW
					portToChange.MUbackcolor = System.Drawing.Color.Yellow;
					break;
				case 9:
					//ORANGE
					portToChange.MUbackcolor = System.Drawing.Color.Orange;
					break;
				case 10:
					//RED
					portToChange.MUbackcolor = System.Drawing.Color.Red;
					break;
				case 11:
					//WHITE
					portToChange.MUbackcolor = System.Drawing.Color.White;
					break;
				default:
					portToChange.MUbackcolor = System.Drawing.Color.Empty;
					break;
			}
		}

		private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
		{
			// We change port only if the type is changed from the current one
			if (CurrentHub.Type != (Hub.Types)comboBoxType.SelectedIndex)
			{
				CurrentHub.Type = (Hub.Types)comboBoxType.SelectedIndex;
				CurrentHub.InitPorts();
				InitPortComponents();
			}
		}

		private void textBoxName_TextChanged(object sender, EventArgs e)
		{
			// Change the Hub name if needed
			if (CurrentHub.Name != textBoxName.Text)
				CurrentHub.Name = textBoxName.Text;
		}

		private void HubEditor_Shown(object sender, EventArgs e)
		{
			comboBoxType.DropDownStyle = ComboBoxStyle.DropDownList;
			comboBoxType.SelectedIndex = (int)CurrentHub.Type;
			comboBoxColors.DropDownStyle = ComboBoxStyle.DropDownList;
			comboBoxColors.SelectedIndex = (int)CurrentHub.LEDColor;

			textBoxName.Text = CurrentHub.Name;
			InitPortComponents();
		}

		private void comboBoxColors_SelectedIndexChanged(object sender, EventArgs e)
		{
			// We change port only if the type is changed from the current one
			if (CurrentHub.LEDColor != (Port.Colors)comboBoxColors.SelectedIndex)
			{
				CurrentHub.LEDColor = (Port.Colors)comboBoxColors.SelectedIndex;
				CurrentHub.RestoreLEDColor();
			}
		}

        //added by Tom Cook to rename hub and send via bluetooth
		private void btnRename_Click(object sender, EventArgs e)
        {
			string name = textBoxName.Text;
			//if (CurrentHub.Name != name)
			{
				if (!CurrentHub.Rename(name))
					MessageBox.Show(this, "Name must be 14 characters or less.", "Name is invalid", MessageBoxButtons.OK);
				else
					MainBoard.WriteLine(string.Format("Hub with MAC Address {0:X} name was changed to '{1:X}'.", CurrentHub.BluetoothAddress, name), System.Drawing.Color.Blue);
			}
		}
    }
}
