using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;
using static LegoTrainProject.Port;

namespace LegoTrainProject
{
    [Serializable]
    public class BuWizz3Hub : BuWizzHub
	{
		//Added by Scott Hanson
		//modified by Tom Cook for MU function to add TrainProject project to class where need to loop thru all hubs and ports
		//public BuWizz3Hub(BluetoothLEDevice device, Types type) : base(device, type)
		public BuWizz3Hub(BluetoothLEDevice device, Types type, TrainProject project) : base(device, type, project)
		{

		}

		internal override async Task RenewCharacteristic()
		{

			//
			//private static readonly Guid SERVICE_UUID = new Guid("500592d1-74fb-4481-88b3-9919b1676e93");
			//private static readonly Guid CHARACTERISTIC_UUID = new Guid("50052901-74fb-4481-88b3-9919b1676e93");
			//
			if (Device != null)
			{
				Device = await BluetoothLEDevice.FromBluetoothAddressAsync(Device.BluetoothAddress);
				Gatt = await Device.GetGattServicesAsync(BluetoothCacheMode.Uncached);
				AllCharacteristic = await Gatt.Services.Single(s => s.Uuid == Guid.Parse("500592D1-74FB-4481-88B3-9919B1676E93")).GetCharacteristicsAsync(BluetoothCacheMode.Uncached);
				Characteristic = AllCharacteristic.Characteristics.Single(s => s.Uuid == Guid.Parse("50052901-74fb-4481-88b3-9919b1676e93"));

				MainBoard.WriteLine("New Hub Found of type " + Enum.GetName(typeof(Hub.Types), Type), Color.Green);
			}
		}

		public override void SetLEDColor(Colors color)
		{
			// Nothing to do
		}


		public override void SetMotorSpeed(string port, int speed)
		{
			Port portObj = GetPortFromPortId(port);

			// If we can't find the port, we can't do anything!
			if (portObj == null)
			{
				MainBoard.WriteLine("Could not set Motor Speed to " + speed + " for " + Name + " because no default port are setup", Color.Red);
				return;
			}

			portObj.Speed = speed;
			IsBusy = (speed != 0);

			OnDataUpdated();

			byte[] data = new byte[8];
			data[0] = 0x30;
			foreach (Port p in RegistredPorts)
				data[RegistredPorts.IndexOf(p) + 1] = (byte)p.Speed;
			data[7] = 0;

			WriteMessage(data, false);
		}

		public override void SetLightBrightness(string port, int brightness)
		{

			Port portObj = GetPortFromPortId(port);

			// If we can't find the port, we can't do anything!
			if (portObj == null)
			{
				MainBoard.WriteLine("Could not set Light Brightness for " + Name + " because no default port are setup", Color.Red);
				return;
			}

			portObj.Speed = brightness;

			OnDataUpdated();

			byte[] data = new byte[8];
			data[0] = 0x30;
			foreach (Port p in RegistredPorts)
				data[RegistredPorts.IndexOf(p) + 1] = (byte)p.Speed;
			data[7] = 0;

			WriteMessage(data, false);
		}

		public override void InitPorts()
		{
			// Clear any previous port
			RegistredPorts.Clear();

			Port port1 = new Port("1", 0, true);
			Port port2 = new Port("2", 2, true);
			Port port3 = new Port("3", 1, true);
			Port port4 = new Port("4", 3, true);
			Port portA = new Port("A", 4, true);
			Port portB = new Port("B", 5, true);

			RegistredPorts.Add(port1);
			RegistredPorts.Add(port2);
			RegistredPorts.Add(port3);
			RegistredPorts.Add(port4);
			RegistredPorts.Add(portA);
			RegistredPorts.Add(portB);

			port1.Function = Port.Functions.MOTOR;
			port2.Function = Port.Functions.MOTOR;
			port3.Function = Port.Functions.MOTOR;
			port4.Function = Port.Functions.MOTOR;
			portA.Function = Port.Functions.MOTOR;
			portB.Function = Port.Functions.MOTOR;
		}

		byte Extract(byte value, byte offset, byte length)
		{
			return (byte)((value >> offset) & ((1 << length) - 1));
		}

		// <summary>
		/// Treat Incoming Data from the train
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		/// 
		internal override void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
		{
			try
			{
				// An Indicate or Notify reported that the value has changed.
				var reader = DataReader.FromBuffer(args.CharacteristicValue);

				byte[] message = new byte[reader.UnconsumedBufferLength];

				reader.ReadBytes(message);

				if (message[0] != 0x01)
				{
					return;
				}


				var status = (byte)(message[1]);
				var batteryStatus = (byte)Extract(status, 3, 2);
				BatteryLevel = (int)batteryStatus * 25;

				var batteryVol = (byte)(message[2]);
				//(9 V + value * 0,05 V) - range 9,00 V – 15,35 V 
				BatteryVoltage = 9 + ((int)batteryVol * 0.05);
				OnDataUpdated();


				//MainBoard.WriteLine(BitConverter.ToString(message));
			}
			catch (Exception ex)
			{
				MainBoard.WriteLine("FATAL BuzHub3: Something went wrong while reading messages!" + ex.Message, Color.DarkRed);
			}
		}
	}
}


