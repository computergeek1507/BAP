using MQTTnet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using Windows.Devices.Bluetooth;
using Windows.Media.Protection.PlayReady;
using static LegoTrainProject.Hub;

namespace LegoTrainProject
{
	[Serializable]
	class JRMIHub : Hub
	{

		[NonSerialized]
		MQTTClient _mqttClient;

		//Added by Scott
		//https://www.jmri.org/help/en/html/hardware/mqtt/index.shtml
		public JRMIHub(BluetoothLEDevice device, Types type, TrainProject project, string trainID, MQTTClient mqttClient) : base(device, type, project)
		{
			Name = "JRMIMQTT";
			DeviceId = trainID;
			_mqttClient = mqttClient;
			Type = type;
			IsConnected = false;

			InitPorts();
		}

		public override void InitPorts()
		{
			// Clear any previous port
			RegistredPorts.Clear();

			Port portA = new Port("A", 0, true);
			Port portB = new Port("B", 1, true);
			Port portC = new Port("C", 2, true);
			Port portD = new Port("D", 3, true);

			//Port port1 = new Port("One", 4, true);
			//Port port2 = new Port("Two", 5, true);
			//Port port3 = new Port("Three", 6, true);
			//Port port4 = new Port("Four", 7, true);

			RegistredPorts.Add(portA);
			RegistredPorts.Add(portB);
			RegistredPorts.Add(portC);
			RegistredPorts.Add(portD);


			//RegistredPorts.Add(port1);
			//RegistredPorts.Add(port2);
			//RegistredPorts.Add(port3);
			//RegistredPorts.Add(port4);
		}

		public override void TryToConnect()
		{
			MainBoard.WriteLine("Connecting to MQTT on port " + DeviceId);

			//var conType = CreateConnection();
			IsConnected = _mqttClient.IsConnected();
		}

		protected override void ActivatePortDevice(byte port, byte type, byte mode, byte format)
		{
			// Do nothing
		}

		public override void SetLEDColor(Port.Colors color)
		{
			//LedPattern ledPattern = (color == Port.Colors.GREEN) ? LedPattern.Green : (color == Port.Colors.RED) ? LedPattern.Red : (color == Port.Colors.ORANGE) ? LedPattern.Orange : LedPattern.Black;
			//brick.DirectCommand.SetLedPatternAsync(ledPattern);
		}

		public override void SetMotorSpeed(string port, int speed)
		{
			Port portObj = GetPortFromPortId(port);

			// If we can't find the port, we can't do anything!
			if (portObj == null)
			{
				MainBoard.WriteLine("Could not set Motor Speed to " + speed + " for " + Name, Color.Red);
				return;
			}

			portObj.Speed = speed;
			IsBusy = (speed != 0);

			OnDataUpdated();
			if (portObj.Inverted)
			{
				speed = -speed;
			}

			//OutputPort outputPort = (port == "A") ? OutputPort.A : (port == "B") ? OutputPort.B : (port == "C") ? OutputPort.C : OutputPort.D;
			//brick.DirectCommand.TurnMotorAtPowerAsync(outputPort, speed);
			int absSpeed = Math.Abs(speed);

			var dircommand = (speed > 0) ? "FORWARD" : "REVERSE";
			if (speed == 0) dircommand = "STOP";
			_mqttClient.SendMessage("cab/" + IncrementID(DeviceId, portObj) + "/direction", dircommand);

			_mqttClient.SendMessage("cab/"+ IncrementID(DeviceId, portObj) + "/throttle", absSpeed.ToString());
		}

		public override void SetLightBrightness(string port, int brightness)
		{
			Port portObj = GetPortFromPortId(port);

			// If we can't find the port, we can't do anything!
			if (portObj == null)
			{
				MainBoard.WriteLine("Could not set light brightness for " + Name, Color.Red);
				return;
			}

			portObj.Speed = brightness;
			string command = "INTENSITY " + (brightness/100).ToString();
			if (brightness == 100)
			{
				command = "ON";
			}
			else if (brightness == 0)
			{
				command = "OFF";
			}

			OnDataUpdated();
			//brick.DirectCommand.(OutputPort.A, speed);
			_mqttClient.SendMessage("track/light/" + IncrementID(DeviceId, portObj), command);

		}

		public override void ActivateSwitch(string port, bool left)
		{
			Port targetPort = GetPortFromPortId(port);

			switch (targetPort.Function)
			{
				case Port.Functions.SWITCH_DOUBLECROSS:
				case Port.Functions.SWITCH_TRIXBRIX:
				case Port.Functions.SWITCH_STANDARD:
					{
						targetPort.TargetSpeed = (left) ? -100 : 100;
						//SetMotorSpeed(port, targetPort.TargetSpeed);
						if (targetPort.Inverted)
						{
							left = !left;
						}
						var command = (left) ? "CLOSED" : "THROWN";

						//IsBusy = (speed != 0);

						OnDataUpdated();

						_mqttClient.SendMessage("track/turnout/" + IncrementID(DeviceId, targetPort), command);
						break;
					}
			}
		}

		public override void Disconnect()
		{
			//if (IsConnected)
			//	brick.Disconnect();
		}

		private string IncrementID(string devID, Port port)
		{
			var tmpID = devID;
			try
			{
				int iid = int.Parse(tmpID);
				iid += port.Value;
				tmpID = iid.ToString();
			}
			catch (FormatException)
			{
			}
			return tmpID;
		}
	}
}
