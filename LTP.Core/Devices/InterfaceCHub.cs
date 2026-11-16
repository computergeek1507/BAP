using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using System.Drawing;
using System.Timers;

namespace LegoTrainProject
{
	[Serializable]
	class InterfaceCHub : Hub
	{
		[NonSerialized]
		SerialPort _serialPort;

		[NonSerialized]
		string _reply = "";

		//[NonSerialized]
		//private Timer timer1;

		public InterfaceCHub(BluetoothLEDevice device, Types type, TrainProject project, string comAddress) : base(device, type, project)
		{
			Name = "InterfaceC";
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

			RegistredPorts.Add(portA);
			portA.Function = Port.Functions.MOTOR;
		}

		public override void TryToConnect()
		{
			MainBoard.WriteLine("Connecting to InterfaceCHub on port " + DeviceId);

			_serialPort = new SerialPort(DeviceId, 115200, Parity.None, 8, StopBits.One);
			_serialPort.Handshake = Handshake.None;
			_serialPort.RtsEnable = false;
			//_serialPort.
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

			//_reader = new BinaryReader(_serialPort.BaseStream);
			_serialPort.WriteLine("Version");
			//_serialPort.WriteLine("p\0###Do you byte, when I knock?$$$");
			IsConnected = true;
			//timer1 = new Timer();
			//timer1.Elapsed += (sender, e) =>
			//{
			//	SendKeepAlive();
			//};

			//timer1.Interval = 2000;//miliseconds
			//timer1.Start();

			return Task.FromResult(true);
		}

		private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			IsConnected = true;

			try
			{
				// Keep the first full line the controller sends (its answer to "Version") for the Hub Info dialog
				_reply += _serialPort.ReadExisting();
				int end = _reply.LastIndexOf('\n');
				if (end < 0)
					return;

				string[] lines = _reply.Substring(0, end).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
				if (lines.Length > 0 && string.IsNullOrEmpty(FirmwareVersion))
					FirmwareVersion = lines[0].Trim();
				_reply = _reply.Substring(end + 1);
			}
			catch (Exception)
			{
				// Port closed while reading
			}
		}

		/// <summary>
		/// Disconnect from the Serial Port.
		/// </summary>
		public override void Disconnect()
		{
			//if (timer1 != null)
			//{
			//	timer1.Dispose();
			//}
			if (_serialPort != null)
			{
				//_serialPort.WriteLine("p\0###Do you byte, when I knock?$$$");
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

			if (portObj.Inverted)
			{
				speed = -speed;
			}

			OnDataUpdated();

			int outputPort = char.ToUpper(port[0]) - 'A';
			//int outputPort = (port == "A") ? 0 : (port == "B") ? 1 : (port == "C") ? 2 : 3;
			if (speed == 0)
			{
				SendOnOff(false, outputPort);
				return;
			}
			//SendOnOff(true, outputPort);
			//SendDirection(speed > 0, outputPort);
			SendLevel(speed, outputPort);
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
		}

		/// <summary>
		/// Write data to the Serial Port.
		/// </summary>
		/// <param name="data">Byte array of data to send to the Serial Port.</param>
		/// <returns></returns>
		public Task WriteAsync(byte[] data)
		{
			return Task.Run(() =>
			{
				if (!_serialPort.IsOpen)
					return;

				lock (_serialPort)
					_serialPort.Write(data, 0, data.Length);
			});
		}

		//Send Direction commands
		public Task SendDirection(bool right, int port)
		{
			byte buf;
			byte buf2;
			//set byte for Right
			if (right == true) buf = 147; /* 10010011 */
			//set byte for Left
			else buf = 148; /* 10010100 */
			//send byte for each output
			buf2 = (byte)(0x01 << port);
			byte[] dcommand = new byte[2] { buf, buf2 };
			return WriteAsync(dcommand);
		}

		public Task SendLevel(int level, int port)
		{
			string command = $"Set M{port + 1} {SpeedToLevel(level)}\n";

			return WriteAsync(Encoding.ASCII.GetBytes(command));
		}

		public Task SendOnOff(bool on, int port)
		{
			string command = $"Set M{port + 1} 0\n";
			
			return WriteAsync(Encoding.ASCII.GetBytes(command));
		}

		private void readData()
		{
			byte checksum = 0;
			byte inBuf1, inBuf2;
			byte[] inframe = new byte[20];
			do
			{
				inBuf1 = (byte)_serialPort.ReadByte();
				inBuf2 = (byte)_serialPort.ReadByte();
			} while (inBuf1 != 0 || inBuf2 != 0);
			inframe[0] = 0;
			inframe[1] = 0;
			for (int i = 2; i < 19; i++)
			{
				inframe[i] = (byte)_serialPort.ReadByte();
			}
			for (int i = 0; i < 19; i++) checksum += inframe[i];
			if (checksum != 255)
			{
				//for (int i = 0; i < 19; i++) inframe[i] = pinframe[i];
				//errorcounter++;
				//toolStripStatusLblStatus.Text = "Checksum error No. " + errorcounter.ToString() + " in message No. " + counter.ToString() + ".  Previous message used.";
			}
			float[] TempF = new float[4] { 0, 0, 0, 0 };
			float[] TempC = new float[4] { 0, 0, 0, 0 };
			bool[] Touch = new bool[4] { false, false, false, false };
			double[] Rot = new double[4];
			//bool[] Pressed = new bool[4] { false, false, false, false };
			//bool[] Released = new bool[4] { false, false, false, false };
			int[] Light = new int[4] { 0, 0, 0, 0 };
			//update temperature and touch sensor readings
			for (int i = 0; i < 4; i++)
			{
				int Raw = (int)(((inframe[14 - 4 * i] << 8) | inframe[15 - 4 * i]) >> 6);
				TempF[i] = (float)((760 - (float)Raw) / 4.4 + 32);
				TempC[i] = (float)((760 - (float)Raw) / 4.4 * 5 / 9);
				if (inframe[14 - 4 * i] != 255) Touch[i] = true; else Touch[i] = false;
				//if (inframe[14 - 4 * i] != 255 && pinframe[14 - 4 * i] == 255) Pressed[i] = true; else Pressed[i] = false;
				//if (inframe[14 - 4 * i] == 255 && pinframe[14 - 4 * i] != 255) Released[i] = true; else Released[i] = false;
			}
			//update light and rotation sensor readings
			for (int i = 0; i < 4; i++)
			{
				int Raw = (int)(((inframe[16 - 4 * i] << 8) | inframe[17 - 4 * i]) >> 6);
				Light[i] = (int)(146 - (float)Raw / 7);
				if ((int)(0x04 & inframe[17 - 4 * i]) > 0) Rot[i] = Rot[i] + 22.5 * (float)(0x03 & inframe[17 - 4 * i]);
				else Rot[i] = Rot[i] - 22.5 * (float)(0x03 & inframe[17 - 4 * i]);
			}

		} //end timer send and receive

		private void SendKeepAlive()
		{
			//readData();
			byte[] kcommand = new byte[1] { 2 };
			WriteAsync(kcommand);
		}

		private int SpeedToLevel(int speed)
		{
			//int level = 0;
			int level = (int)Math.Ceiling((double)speed / 16);
			if (level > 6) level = 6;
			return (int)level;
		}
	}
}
