using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LegoTrainProject.PFxHub;
using System.Linq.Expressions;
using Microsoft.VisualBasic.Devices;
using System.Media;
using System.Drawing.Drawing2D;

namespace LegoTrainProject
{
	public partial class HubControl : UserControl
	{
		private Hub Hub;

		// added by Tom Cook to have a project of all connected hubs for MU function
		private TrainProject Project;
		
		HubEditor Editor;
		bool hubIsTrain = false;

		public delegate void RefreshTrainLabelThreadSafeDelegate(Hub train);
		public delegate void RefreshUIThreadSafeDelegate();
		public event RefreshUIThreadSafeDelegate PortTypeRefreshed;

		//modified by Tom Cook for MU function to add TrainProject project to class where need to loop thru all hubs and ports
		//public HubControl(Hub hub)
		public HubControl(Hub hub, TrainProject project)
		{
			Hub = hub;
			Hub.PortTypeUpdated += RefreshUI;
			Hub.DataUpdated += UpdateLabels;

			//modified by Tom Cook, see above
			//Editor = new HubEditor(Hub);
			Editor = new HubEditor(Hub, project);

			//added by Tom Cook, see above
			Project = project;

			InitializeComponent();
			InitControl();
		}

		private void InitControl()
		{
			int width = 0;

			foreach (Port p in Hub.RegistredPorts)
			{
				switch (p.Function)
				{
					case Port.Functions.MOTOR:
					case Port.Functions.TRAIN_MOTOR:
						{
							width += 65;

							//added by Tom Cook to increase the width to accomadate larger trackbar
							width += 5;

							PictureBox pb = new PictureBox();
							pb.Width = 30;
							pb.Height = 30;
							pb.Margin = new Padding(10, 0, 0, 0);
							pb.SizeMode = PictureBoxSizeMode.StretchImage;
							pb.Image = Properties.Resources.icons8_train_48;

							Label labelSpeed = new Label();
							labelSpeed.Text = $"- Port {p.Id} - " + Environment.NewLine + "Speed: " + p.Speed;
							labelSpeed.Padding = new Padding(0, 0, 0, 0);
							labelSpeed.Width = 60;
							labelSpeed.Height = 24;
							labelSpeed.Font = new Font(new FontFamily("Segoe UI"), 7.1f);

							TrackBar tb = new TrackBar();
							tb.Width = 50;
							tb.Height = 142;
							tb.Minimum = -100;
							tb.Maximum = 100;
							tb.TickFrequency = 10;
							tb.Orientation = Orientation.Vertical;
							tb.SmallChange = 10;
							tb.LargeChange = 20;
							//modified by Tom Cook for different style
							//tb.Margin = new Padding(15, 0, 0, 0);
							tb.Margin = new Padding(3, 0, 0, 0);
							tb.Value = 0;
							//added by Tom Cook to set color to match MU function
							tb.TickStyle = TickStyle.Both;
							tb.BackColor = p.MUbackcolor;
							
							// Connect the trackbar to the label
							labelSpeed.Tag = tb;
							p.label = labelSpeed;
							tb.Tag = new object[] { Hub, labelSpeed, p.Id };
							tb.Scroll += Tb_Scrolled;

							Button buttonStop = new Button();
							buttonStop.Text = "Stop";
							buttonStop.Tag = new object[] { Hub, p.Id }; ;
							buttonStop.Click += ButtonStopTrain_Click;
							buttonStop.Width = 50;
							//added by Tom Cook to pull up Stop button a little
							buttonStop.Margin = new Padding(0, 0, 0, 0);
							
							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, pb, false);
							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, labelSpeed, false);
							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, tb, false);
							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, buttonStop, true);
		
							break;
						}
					case Port.Functions.SWITCH_DOUBLECROSS:
					case Port.Functions.SWITCH_STANDARD:
					case Port.Functions.SWITCH_TRIXBRIX:
						{
							width += 70;

							PictureBox pb = new PictureBox();
							pb.Width = 30;
							pb.Height = 30;
							pb.Margin = new Padding(10, 0, 0, 0);
							pb.SizeMode = PictureBoxSizeMode.StretchImage;
							pb.Image = Properties.Resources.icons8_train_track_48;

							Label labelSpeed = new Label();
							labelSpeed.Text = $"- Port {p.Id} - " + Environment.NewLine + "Pos: " + ((p.TargetSpeed == 0) ? "Unknown" : (p.TargetSpeed < 0) ? "Left" : "Right");
							labelSpeed.Padding = new Padding(0, 0, 0, 0);
							labelSpeed.Width = 60;
							labelSpeed.Height = 24;
							labelSpeed.Font = new Font(new FontFamily("Segoe UI"), 7.1f);
							p.label = labelSpeed;

							Button buttonLeft = new Button();
							buttonLeft.Text = "Left";
							buttonLeft.Tag = new object[] { Hub, p.Id, -100 }; ;
							buttonLeft.Click += ButtonActivateSwitch_Click;
							buttonLeft.Width = 50;
							//added by Tom Cook to offset button
							buttonLeft.Padding = new Padding(0, 0, 15, 0);
							//added by Tom Cook for MU function
							buttonLeft.BackColor = p.MUbackcolor;
							if (p.MUbackcolor == Color.Black
							 || p.MUbackcolor == Color.Blue
							 || p.MUbackcolor == Color.Purple
							 || p.MUbackcolor == Color.Green
							 || p.MUbackcolor == Color.Red
							) buttonLeft.ForeColor = Color.White;

							Button buttonRight = new Button();
							buttonRight.Text = "Right";
							buttonRight.Tag = new object[] { Hub, p.Id, 100 }; ;
							buttonRight.Click += ButtonActivateSwitch_Click;
							buttonRight.Width = 50;
							//added by Tom Cook to offset button
							buttonRight.Padding = new Padding(10, 0, 0, 0);
							//added by Tom Cook for MU function
							buttonRight.BackColor = p.MUbackcolor;
							if (p.MUbackcolor == Color.Black
							 || p.MUbackcolor == Color.Blue
							 || p.MUbackcolor == Color.Purple
							 || p.MUbackcolor == Color.Green
							 || p.MUbackcolor == Color.Red
							 ) buttonRight.ForeColor = Color.White;

							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, pb, false);
							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, labelSpeed, false);
							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, buttonLeft, false);
							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, buttonRight, true);

							break;
						}
					case Port.Functions.SENSOR:
						{
							width += 70;

							//added by Tom Cook to widen for COLOR name
							width += 10;

							PictureBox pb = new PictureBox();
							pb.Width = 30;
							pb.Height = 30;
							pb.Margin = new Padding(10, 0, 0, 0);
							pb.SizeMode = PictureBoxSizeMode.StretchImage;
							pb.Image = Properties.Resources.icons8_motion_sensor_48;

							Label labelColor = new Label();
							labelColor.Text = $"- Port {p.Id} - " + Environment.NewLine + "Color:" + Enum.GetName(typeof(Port.Colors), p.LatestColor) + Environment.NewLine + "Distance: 0";
							labelColor.Font = new Font(new FontFamily("Segoe UI"), 7.1f);

							labelColor.Padding = new Padding(0, 0, 0, 0);
							labelColor.Width = 71;

							//modified by Tom Cook to format more data in box
							//labelColor.Height = 80;
							labelColor.Height = 47;

							// Connect the trackbar to the label
							p.label = labelColor;

							//added by Tom Cook to add 'Black' buton
							Button buttonBlack = new Button();
							buttonBlack.Text = "Off";
							buttonBlack.Tag = new object[] { Hub, p.Id, Color.Black }; ;
							buttonBlack.Click += ButtonColor_Click;
							buttonBlack.Width = 50;
							//buttonBlack.Visible = Hub.Type != Hub.Types.WEDO_2_SMART_HUB;

							//added by Tom Cook to add 'Blue' button
							Button buttonBlue = new Button();
							buttonBlue.Text = "Blue";
							buttonBlue.Tag = new object[] { Hub, p.Id, Color.Blue }; ;
							buttonBlue.Click += ButtonColor_Click;
							buttonBlue.Width = 50;
							//buttonBlue.Visible = Hub.Type != Hub.Types.WEDO_2_SMART_HUB;

							//added by Tom Cook to add 'Green' button
							Button buttonGreen = new Button();
							buttonGreen.Text = "Green";
							buttonGreen.Tag = new object[] { Hub, p.Id, Color.Green }; ;
							buttonGreen.Click += ButtonColor_Click;
							buttonGreen.Width = 50;
							//buttonGreen.Visible = Hub.Type != Hub.Types.WEDO_2_SMART_HUB;

							//added by Tom Cook to add 'Red' button
							Button buttonRed = new Button();
							buttonRed.Text = "Red";
							buttonRed.Tag = new object[] { Hub, p.Id, Color.Red }; ;
							buttonRed.Click += ButtonColor_Click;
							buttonRed.Width = 50;
							//buttonRed.Visible = Hub.Type != Hub.Types.WEDO_2_SMART_HUB;

							//added by Tom Cook to add 'White' button
							Button buttonWhite = new Button();
							buttonWhite.Text = "White";
							buttonWhite.Tag = new object[] { Hub, p.Id, Color.White }; ;
							buttonWhite.Click += ButtonColor_Click;
							buttonWhite.Width = 50;
							//buttonWhite.Visible = Hub.Type != Hub.Types.WEDO_2_SMART_HUB;

							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, pb, false);
							//modified by Tom Cook to change 'true' to 'false' so could add buttons then enter 'true'
							//MainBoard.AddControlToFlowPanel(flowLayoutPanel1, labelColor, true);

							//added by Tom Cook, see above - 'true' for end of column of elements
							if (p.Device == Port.Devices.BOOST_DISTANCE && Hub.Type != Hub.Types.WEDO_2_SMART_HUB)
							{
								MainBoard.AddControlToFlowPanel(flowLayoutPanel1, labelColor, false);
								MainBoard.AddControlToFlowPanel(flowLayoutPanel1, buttonBlack, false);
								MainBoard.AddControlToFlowPanel(flowLayoutPanel1, buttonBlue, false);
								MainBoard.AddControlToFlowPanel(flowLayoutPanel1, buttonGreen, false);
								MainBoard.AddControlToFlowPanel(flowLayoutPanel1, buttonRed, false);
								MainBoard.AddControlToFlowPanel(flowLayoutPanel1, buttonWhite, true);
							}
							else
								MainBoard.AddControlToFlowPanel(flowLayoutPanel1, labelColor, true);


								break;
						}
					case Port.Functions.LIGHT:
						{
							width += 70;

							PictureBox pb = new PictureBox();
							pb.Width = 30;
							pb.Height = 30;
							pb.Margin = new Padding(10, 0, 0, 0);
							pb.SizeMode = PictureBoxSizeMode.StretchImage;
							pb.Image = Properties.Resources.icons8_light_48;

							Label labelLight = new Label();
							labelLight.Text = $"- Port {p.Id} - " + Environment.NewLine + "Light is " + ((p.TargetSpeed == 0) ? "Off" : "On");
							labelLight.Padding = new Padding(0, 0, 0, 0);
							labelLight.Width = 60;
							labelLight.Height = 36;
							labelLight.Font = new Font(new FontFamily("Segoe UI"), 7.1f);

							// Connect the trackbar to the label
							p.label = labelLight;

							Button buttonOn = new Button();
							buttonOn.Text = "On";
							buttonOn.Tag = new object[] { Hub, p.Id, 100 }; ;
							buttonOn.Click += ButtonOnOff_Click;
							buttonOn.Width = 50;

							//added by Tom Cook for 'Bright' button
							Button buttonBright = new Button();
							buttonBright.Text = "Bright";
							buttonBright.Tag = new object[] { Hub, p.Id, 80 }; ;
							buttonBright.Click += ButtonOnOff_Click;
							buttonBright.Width = 50;

							//added by Tom Cook for 'Normal' button
							Button buttonNormal = new Button();
							buttonNormal.Text = "Normal";
							buttonNormal.Tag = new object[] { Hub, p.Id, 50 }; ;
							buttonNormal.Click += ButtonOnOff_Click;
							buttonNormal.Width = 50;

							//added by Tom Cook for 'Dim' button
							Button buttonDim = new Button();
							buttonDim.Text = "Dim";
							buttonDim.Tag = new object[] { Hub, p.Id, 20 }; ;
							buttonDim.Click += ButtonOnOff_Click;
							buttonDim.Width = 50;

							Button buttonOff = new Button();
							buttonOff.Text = "Off";
							buttonOff.Tag = new object[] { Hub, p.Id, 0 }; ;
							buttonOff.Click += ButtonOnOff_Click;
							buttonOff.Width = 50;

							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, pb, false);
							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, labelLight, false);
							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, buttonOn, false);

							//added by Tom Cook add button and make last button 'true' for end of column of elements
							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, buttonBright, false);
							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, buttonNormal, false);
							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, buttonDim, false);

							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, buttonOff, true);

							break;
						}
					case Port.Functions.BUTTON:
						{
							width += 75;

							PictureBox pb = new PictureBox();
							pb.Width = 30;
							pb.Height = 30;
							pb.Margin = new Padding(15, 0, 0, 0);
							pb.SizeMode = PictureBoxSizeMode.StretchImage;
							pb.Image = (p.Id == "A") ? Properties.Resources.icons8_xbox_a_48 : Properties.Resources.icons8_xbox_b_48;

							Label labelLight = new Label();
							labelLight.Text = $"  - Port {p.Id} - " + Environment.NewLine + ((p.Id == "A") ? "Left Buttons" : "Right Buttons");
							labelLight.Padding = new Padding(0, 0, 0, 0);
							labelLight.Width = 65;
							labelLight.Height = 36;
							labelLight.Font = new Font(new FontFamily("Segoe UI"), 7.1f);

							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, labelLight, false);
							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, pb, true);

							break;
						}
					case Port.Functions.PFX_SPEAKER:
						{
							width += 100;

							PictureBox pb = new PictureBox();
							pb.Width = 30;
							pb.Height = 30;
							pb.Margin = new Padding(10, 0, 0, 0);
							pb.SizeMode = PictureBoxSizeMode.StretchImage;
							pb.Image = Properties.Resources.icons8_speaker_48;

							Label labelLight = new Label();
							labelLight.Text = $"  - Port {p.Id} - " + Environment.NewLine + "PFx - Speaker";
							labelLight.Padding = new Padding(0, 0, 0, 0);
							labelLight.Width = 75;
							labelLight.Height = 36;
							labelLight.Font = new Font(new FontFamily("Segoe UI"), 7.1f);


							ComboBox comboFiles = new ComboBox();
							comboFiles.Width = 65;
							comboFiles.DropDownWidth = 100;
							comboFiles.DropDownStyle = ComboBoxStyle.DropDownList;

							for (int i = 0; i < 64; i++)
								comboFiles.Items.Add("File id " + i);

							Button buttonPlay = new Button();
							buttonPlay.Text = "Play";
							buttonPlay.Tag = comboFiles;
							buttonPlay.Click += ButtonPlay_Click; ;
							buttonPlay.Width = 50;

							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, pb, false);
							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, labelLight, false);
							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, comboFiles, false);
							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, buttonPlay, true);

							break;
						}

					case Port.Functions.PFX_LIGHT_CONTROLER:
						{
							width += 150;

							PictureBox pb = new PictureBox();
							pb.Width = 30;
							pb.Height = 30;
							pb.Margin = new Padding(10, 0, 0, 0);
							pb.SizeMode = PictureBoxSizeMode.StretchImage;
							pb.Image = Properties.Resources.icons8_light_automation_48;

							Label labelLight = new Label();
							labelLight.Text = $"  - Port {p.Id} - " + Environment.NewLine + "PFx - 8x Light Hub";
							labelLight.Padding = new Padding(0, 0, 0, 0);
							labelLight.Width = 95;
							labelLight.Height = 36;
							labelLight.Font = new Font(new FontFamily("Segoe UI"), 7.1f);

							ComboBox comboFiles = new ComboBox();
							comboFiles.Width = 135;
							comboFiles.DropDownWidth = 170;
							comboFiles.DropDownStyle = ComboBoxStyle.DropDownList;

							foreach (PFxLightFx fx in Enum.GetValues(typeof(PFxLightFx)))
								comboFiles.Items.Add(fx.ToString());

							Button buttonPlay = new Button();
							buttonPlay.Text = "Test Light";
							buttonPlay.Tag = comboFiles;
							buttonPlay.Click += ButtonPlayLight_Click;
							buttonPlay.Width = 50;

							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, pb, false);
							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, labelLight, false);
							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, comboFiles, false);
							MainBoard.AddControlToFlowPanel(flowLayoutPanel1, buttonPlay, true);

							break;

						}

				}
			}

			//modified by Tom Cook to increase width to show Volts and mAmps even when no devices attached to hub
			//MainBoard.SetControlPropertyThreadSafe(this, "Width", (width < 110) ? 110 : width);
			MainBoard.SetControlPropertyThreadSafe(this, "Width", (width < 140) ? 140 : width);
		}

		private void ButtonPlayLight_Click(object sender, EventArgs e)
		{
			ComboBox files = (ComboBox)((Control)sender).Tag;
			PFxHub pFxHub = (PFxHub)Hub;

			Enum.TryParse((string)files.SelectedItem, out PFxLightFx light);
			pFxHub.LightFx("1,2,3,4,5,6,7,8", (byte)light, new byte[0]);
		}

		private void ButtonPlay_Click(object sender, EventArgs e)
		{
			ComboBox files = (ComboBox)((Control)sender).Tag;
			PFxHub pFxHub = (PFxHub)Hub;
			pFxHub.PlayAudioFile((byte)files.SelectedIndex);
		}

		private void RefreshUI()
		{
			if (!hubIsTrain && Hub.IsTrain())
			{
				hubIsTrain = true;
				// Let the MainBoard know we have a new train
				PortTypeRefreshed?.Invoke();
			}

			//We clear all elements 
			MainBoard.AddControlToFlowPanel(flowLayoutPanel1, null, false);
			// And rebuild the control
			InitControl();
		}

		public void UpdateLabels()
		{
			UpdateHubLabel(Hub);
		}

		public void UpdateHubLabel(Hub train)
		{
			if (richTextBoxLabelHub.InvokeRequired)
			{
				richTextBoxLabelHub.Invoke(new RefreshTrainLabelThreadSafeDelegate(UpdateHubLabel),
					new object[] { train });
			}
			else
			{
				foreach (Port p in Hub.RegistredPorts)
				// Update the speed on the trackbar for this train if necessary
					if (p.label != null)
					{
						if (p.Function == Port.Functions.MOTOR || p.Function == Port.Functions.TRAIN_MOTOR)
						{
							TrackBar tb = (TrackBar)(p.label.Tag);
							if (tb != null && p.Speed != tb.Value)
							{
								tb.Value = (p.Speed == 127) ? -1 : (p.Speed < -100) ? -100 : (p.Speed > 100) ? 100 : p.Speed;

								//modified by Tom Cook for 'inverted' function
								if (p.Inverted) tb.Value = -tb.Value;

								p.label.Text = $"- Port {p.Id} -" + Environment.NewLine + $"Speed: {tb.Value}";
							}
						}
						else if (p.Function == Port.Functions.SWITCH_TRIXBRIX || 
							p.Function == Port.Functions.SWITCH_STANDARD || p.Function == Port.Functions.SWITCH_DOUBLECROSS)
						{
							//modified by Tom Cook for 'inverted' function. Mod not necessary, since invert check/changed in SetMotorSpeed()
							p.label.Text = $"- Port {p.Id} -" + Environment.NewLine + "Pos: " + ((p.TargetSpeed == 0) ? "Unknown" : (p.TargetSpeed < 0) ? "Left" : "Right");
							//if (p.Inverted)	p.label.Text = $"- Port {p.Id} -" + Environment.NewLine + "Pos: " + ((p.TargetSpeed == 0) ? "Unknown" : (p.TargetSpeed > 0) ? "Left" : "Right");
							//else p.label.Text = $"- Port {p.Id} -" + Environment.NewLine + "Pos: " + ((p.TargetSpeed == 0) ? "Unknown" : (p.TargetSpeed < 0) ? "Left" : "Right");
						}
						else if (p.Function == Port.Functions.LIGHT)
						{
							//modified by Tom Cook to show mulitiple light settings
							//p.label.Text = $"- Port {p.Id} - " + Environment.NewLine + "Light is " + ((p.TargetSpeed == 0) ? "Off" : "On");
							int lightSetting = Hub.GetMotorSpeed(p.Id);
							string lightSettingStr;
							switch (lightSetting)
							{
								case 0:
									lightSettingStr = "Off";
									break;
								case 20:
									lightSettingStr = "Dim";
									break;
								case 50:
									lightSettingStr = "Normal";
									break;
								case 80:
									lightSettingStr = "Bright";
									break;
								case 100:
									lightSettingStr = "Full";
									break;
								default:
									lightSettingStr = lightSetting.ToString();
									break;
							}
							p.label.Text = $"- Port {p.Id} - " + Environment.NewLine + "Light is " + Hub.GetMotorSpeed(p.Id).ToString();
						}
						else if (p.Function == Port.Functions.SENSOR)
						{
							p.label.Text = $"- Port {p.Id} - " + Environment.NewLine;

							//modified by Tom Cook to NOT show Color & Distance (Boost) sensor with WeDo Hub
							//if (p.Device == Port.Devices.BOOST_DISTANCE || p.Device == Port.Devices.EV3_COLOR_SENSOR)
							if ((p.Device == Port.Devices.BOOST_DISTANCE && Hub.Type != Hub.Types.WEDO_2_SMART_HUB) || p.Device == Port.Devices.EV3_COLOR_SENSOR)
								//p.label.Text += "Color:" + Enum.GetName(typeof(Port.Colors), p.LatestColor) + Environment.NewLine;
								p.label.Text += "Color: " + p.LatestColor + Environment.NewLine;

							if (p.Device == Port.Devices.EV3_SENSOR || p.Device == Port.Devices.NXT_SENSOR)
								p.label.Text += "Raw: " + p.LatestDistance;
							else if (p.Device != Port.Devices.EV3_COLOR_SENSOR)
							{
								//modified by Tom Cook to show distance in absolute (0-10) and approximate cm
								//p.label.Text += Environment.NewLine + "Distance: " + p.LatestDistance;
								if (p.Device == Port.Devices.WEDO2_DISTANCE || (p.Device == Port.Devices.BOOST_DISTANCE && Hub.Type != Hub.Types.WEDO_2_SMART_HUB))
									p.label.Text += "Distance: " + p.LatestDistance + Environment.NewLine + " ( ~" + p.distance.ToString("F1") + " cm )";
								//added by Tom Cook to NOT show distance with Color & Distance (Boost) sensor connected to WeDo Hub
								if (p.Device == Port.Devices.BOOST_DISTANCE && Hub.Type == Hub.Types.WEDO_2_SMART_HUB)
								{
									if (p.LatestDistance == 255) p.label.Text += "Distance: Far";
									else p.label.Text += "Distance: Near";
								}
							}
						}
					}

				//added by Tom Cook to show battery Volts and mAmps and to force initialization if necessary since System Hubs do not update
				this.Visible = true;
				if (Hub.Type != Hub.Types.WEDO_2_SMART_HUB)
					lblVolts.Text = $"{train.BatteryVoltage:F2}" + " V";
				else
					lblVolts.Text = "";
				//Remotes to not have current sensor, so do not display mAmps
				if (Hub.Type != Hub.Types.POWERED_UP_REMOTE && Hub.Type != Hub.Types.WEDO_2_SMART_HUB)
					lblAmps.Text = $"{train.BatteryCurrent:F0}" + " mA";
				else
					lblAmps.Text = "";
				if (Hub.updateBatteryLevel == true && Hub.Type == Hub.Types.POWERED_UP_HUB)
						Hub.InitializeNotifications();  //to force update/refesh to get Battery Level

				// Update the Label!
				//modified by Tom Cook to NOT display battery level for WeDo Hub
				//richTextBoxLabelHub.Text = train.Name + " - " + train.BatteryLevel + "%";
				if (Hub.Type == Hub.Types.WEDO_2_SMART_HUB)
					richTextBoxLabelHub.Text = train.Name;
				else
					richTextBoxLabelHub.Text = train.Name + " - " + train.BatteryLevel + "%";

				//added by Tom Cook to only turn-off the initialization flag if conditions are met
				if (Hub.BatteryLevel > 0 && Hub.BatteryVoltage > 1.0) Hub.updateBatteryLevel = false;

				using (Graphics g = CreateGraphics())
				{
					SizeF size = g.MeasureString(richTextBoxLabelHub.Text, richTextBoxLabelHub.Font, 495);
					richTextBoxLabelHub.Width = (int)Math.Ceiling(size.Width);

					if (Width < richTextBoxLabelHub.Width + 25)
						MainBoard.SetControlPropertyThreadSafe(this, "Width", richTextBoxLabelHub.Width + 25);
				}

				pictureBoxStatus.Image = (train.IsConnected) ? Port.colorBitmaps[(int)train.LEDColor] : Properties.Resources.disconnected;
				
				//modified by Tom Cook to comment-out visibility to keep button visible and use it for hiding whole hub panel
				//buttonDisconnect.Visible = train.IsConnected;

			}
		}

		//added by Tom Cook to change sensor color based on which button is pressed
		private void ButtonColor_Click(object sender, EventArgs e)
		{
			Hub hub = (Hub)((object[])((Button)sender).Tag)[0];
			string port = (String)((object[])((Button)sender).Tag)[1];
			Color color = (Color)((object[])((Button)sender).Tag)[2];
			//00=Black 03=Blue 05=Green 09=Red 0A=White FF=No object
			hub.SetColor(port, color);
		}

		private void ButtonOnOff_Click(object sender, EventArgs e)
		{
			Hub hub = (Hub)((object[])((Button)sender).Tag)[0];
			string port = (String)((object[])((Button)sender).Tag)[1];
			int brightness = (int)((object[])((Button)sender).Tag)[2];

			hub.SetLightBrightness(port, brightness);

			//added by Tom Cook to use motor speed to set light brightness value
			hub.SetMotorSpeed(port, brightness);
		}

		private void ButtonActivateSwitch_Click(object sender, EventArgs e)
		{
			Hub hub = (Hub)((object[])((Button)sender).Tag)[0];
			string port = (String)((object[])((Button)sender).Tag)[1];
			int speed = (int)((object[])((Button)sender).Tag)[2];

			//modified by Tom Cook for 'inverted' function. Mod not necessary here, since it is checked/changed in SetMotorSpeed()
			hub.ActivateSwitch(port, speed < 0);
			//if (hub.GetPortFromPortId(port).Inverted) hub.ActivateSwitch(port, speed > 0);
			//else hub.ActivateSwitch(port, speed < 0);
			
			//added by Tom Cook for MU function
			Port p = hub.GetPortFromPortId(port);
			if (p.MUcolor > 0)
				foreach (Hub hMU in Project.RegisteredTrains)
					foreach (Port pMU in hMU.RegistredPorts)
						if (pMU.MUcolor == p.MUcolor)
							hMU.ActivateSwitch(pMU.Id, speed < 0);
			
		}

		/// <summary>
		/// Changing the speed value
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Tb_Scrolled(object sender, EventArgs e)
		{
			Hub hub = (Hub)((object[])((TrackBar)sender).Tag)[0];
			Label label = (Label)((object[])((TrackBar)sender).Tag)[1];
			string port = (String)((object[])((TrackBar)sender).Tag)[2];

			hub.SetMotorSpeed(port, ((TrackBar)sender).Value);

			//added by Tom Cook to MU to locos when the same MUcolor is selected
			Port p = hub.GetPortFromPortId(port);
			if (p.MUcolor > 0)
				foreach (Hub hMU in Project.RegisteredTrains)
					foreach (Port pMU in hMU.RegistredPorts)
						if (pMU.MUcolor == p.MUcolor)
							hMU.SetMotorSpeed(pMU.Id, ((TrackBar)sender).Value);
			
			label.Text = $"- Port {port} -" + Environment.NewLine + $"Speed: {((TrackBar)sender).Value}";
			//added by Tom Cook to play sound when at zero
			if (((TrackBar)sender).Value == 0)
            {
				SoundPlayer simpleSound = new SoundPlayer(@"c:\Windows\Media\chimes.wav");
				simpleSound.Play();
			}
		}

		/// <summary>
		/// Click on Stop a Train
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonStopTrain_Click(object sender, EventArgs e)
		{
			Hub train = (Hub)((object[])((Button)sender).Tag)[0];
			string port = (String)((object[])((Button)sender).Tag)[1];
			//modified by Tom Cook to NOT brake
			//train.Stop(port, true);
			train.Stop(port, false);

			//added by Tom Cook to MU to locos when the same MUcolor is selected
			Port p = train.GetPortFromPortId(port);
			if (p.MUcolor > 0)
				foreach (Hub hMU in Project.RegisteredTrains)
					foreach (Port pMU in hMU.RegistredPorts)
						if (pMU.MUcolor == p.MUcolor)
							hMU.Stop(pMU.Id, false);
		
		}

		private void ButtonConfigure_Click(object sender, EventArgs e)
		{
			// Wait for the Dialog
			DialogResult r = Editor.ShowDialog();

			// Refresh all Ports
			RefreshUI();

			// Refresh Combo Boxes
			PortTypeRefreshed?.Invoke();

			// Update the label of the Hub
			UpdateLabels();
		}

		internal void ClearAllEvents()
		{
			Hub.PortTypeUpdated -= RefreshUI;
			Hub.DataUpdated -= UpdateLabels;
		}

		private void buttonDisconnect_Click(object sender, EventArgs e)
		{
			//added by Tom Cook  to use button to disconnect, and then if disconnected use it again to hide whole hub panel
			if (!Hub.IsConnected) this.Visible = false;
			else
			{

				Hub.Dispose();
				UpdateHubLabel(Hub);

			//added by Tom Cook, see above
			}
		}
	}
}
