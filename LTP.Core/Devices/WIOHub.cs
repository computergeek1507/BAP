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
	class WIOHub : Hub
	{

		[NonSerialized]
		MQTTClient _mqttClient;

		//Added by Scott
		//public WIOHub(BluetoothLEDevice device, Types type, string comAddress) : base(device, type)
		public WIOHub(BluetoothLEDevice device, Types type, TrainProject project, string trainID, MQTTClient mqttClient) : base(device, type, project)
		{
			Name = "WIOHub";
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
			bool direction = (speed > 0);
			int absSpeed = Math.Abs(speed);
			////<lc id="123" addr="10194" dir="true" V="51" V_max="100" />

			_mqttClient.SendMessage("rocrail/service/command", $"<lc id=\"123\" addr=\"{DeviceId}\" dir=\"{direction}\" V=\"{absSpeed}\" V_max=\"100\"/>");
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
			OnDataUpdated();
			//brick.DirectCommand.(OutputPort.A, speed);
			//rocrail/service/command
			//<lc id="123" addr="10194" dir="true" V="51" V_max="100" />
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

						var command = (left) ? "straight" : "turnout";

						//IsBusy = (speed != 0);

						OnDataUpdated();//WIO

						_mqttClient.SendMessage("rocrail/service/command", $"<sw> port1=\"{targetPort.Value + 1}\" addr1=\"{DeviceId}\" cmd=\"{command}\"/>");
						break;
					}
			}
		}

		public override void Disconnect()
		{
			//if (IsConnected)
			//	brick.Disconnect();
		}
	}
}
