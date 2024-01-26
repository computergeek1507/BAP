using System;
using System.Collections.Generic;
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
	}
}


