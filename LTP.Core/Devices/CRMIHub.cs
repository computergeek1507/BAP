using MQTTnet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;
using Windows.Devices.Bluetooth;
using System.IO;
using System.IO.Ports;

namespace LegoTrainProject
{
	[Serializable]
	class CRMIHub : Hub
	{

		/*
		9600 baud
		no flow control
		no parity
		no modem control lines
		one stop bit
		8 bit data
		 */
		[NonSerialized]
		SerialPort _serialPort;

		[NonSerialized]
		byte[] _bufferarray;

		[NonSerialized]
		private Timer timer1;

		//modified by Tom Cook for MU function to add TrainProject project to class where need to loop thru all hubs and ports
		//public MTC4PUHub(BluetoothLEDevice device, Types type, string comAddress) : base(device, type)
		public CRMIHub(BluetoothLEDevice device, Types type, TrainProject project, string comAddress) : base(device, type, project)
		{
			Name = "CRMI";
			DeviceId = comAddress;

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
			_bufferarray = GenerateByteArray();
		}

		public override void TryToConnect()
		{
			MainBoard.WriteLine("Connecting to DACTA on port " + DeviceId);
			//(9600, SERIAL_8N2);
			_serialPort = new SerialPort(DeviceId, 9600, Parity.None, 8, StopBits.Two);
			//_serialPort.Handshake = Handshake.None;
			//_serialPort.RtsEnable = true;
			ConnectAsync();
		}

		/// <summary>
		/// Connect to the Serial Port.
		/// </summary>
		/// <returns></returns>
		public Task ConnectAsync()
		{
			_serialPort.DataReceived += SerialPortDataReceived;
			try
			{
				_serialPort.Open();
			}
			catch
			{
				return Task.FromResult(false);
			}
			byte[] smallArray = new byte[] { 0xFF, 0xFF, 0x02, 0x41, 0x50, 0x03 };//poll

			 //_reader = new BinaryReader(_serialPort.BaseStream);
			 //_serialPort.WriteLine("p\0###Do you byte, when I knock?$$$");
			 _serialPort.Write(smallArray, 0, smallArray.Length);
			 IsConnected = true;
			timer1 = new Timer();
			timer1.Elapsed += (sender, e) =>
			{
				SendKeepAlive();
			};

			timer1.Interval = 1000;//miliseconds
			timer1.Start();

			return Task.FromResult(true);
		}

		private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			IsConnected = true;
		}

		/// <summary>
		/// Disconnect from the Serial Port.
		/// </summary>
		public override void Disconnect()
		{
			if (timer1 != null)
			{
				timer1.Dispose();
			}
			if (_serialPort != null)
			{
				_serialPort.DataReceived -= SerialPortDataReceived;
				_serialPort.Close();
				_serialPort = null;
			}
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

			//OutputPort outputPort = (port == "A") ? OutputPort.A : (port == "B") ? OutputPort.B : (port == "C") ? OutputPort.C : OutputPort.D;
			//brick.DirectCommand.TurnMotorAtPowerAsync(outputPort, speed);
			bool direction = (speed > 0);
			int absSpeed = Math.Abs(speed);
			SetByteInArray(portObj.Value, absSpeed);
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

			SetByteInArray(portObj.Value, brightness);
		}

		private void SendKeepAlive()
		{
			_serialPort.Write(_bufferarray, 0, _bufferarray.Length);
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

						int command = (left) ? 0 : 100;

						//IsBusy = (speed != 0);

						OnDataUpdated();

						SetByteInArray(targetPort.Value, command);
						break;
					}
			}
		}

		private void SetByteInArray(int portidx, int value)
		{
			value *= 255;
			value /= 100;

			_bufferarray[portidx + 5] =(byte)value;

		}
		private byte[] GenerateByteArray()
		{
			byte[] bufarray =
				new byte[] {
					0xFF,
					0xFF,
					0x02,
					0x41,
					0x54,
					0x00,
					0x00,
					0x00,
					0x00,
					0x03 };

			return bufarray;

		}

	}
}
