using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LegoTrainProject
{
	/// <summary>
	/// "About this hub": live controller information and button status for one hub.
	/// Built in code (no designer file) and refreshed on a timer while open.
	/// </summary>
	public class HubInfoForm : Form
	{
		private const string GroupController = "Controller";
		private const string GroupBattery = "Battery";
		private const string GroupPorts = "Ports";
		private const string GroupButtons = "Buttons";

		private readonly Hub Hub;
		private readonly ListView list = new ListView();
		private readonly Timer refreshTimer = new Timer();

		// Written from the hub's Bluetooth thread, read by the UI timer
		private volatile string lastButtonEvent = "None yet";

		public HubInfoForm(Hub hub)
		{
			Hub = hub;

			AutoScaleDimensions = new SizeF(96F, 96F);
			AutoScaleMode = AutoScaleMode.Dpi;
			ClientSize = new Size(440, 480);
			MinimumSize = new Size(340, 300);
			FormBorderStyle = FormBorderStyle.SizableToolWindow;
			StartPosition = FormStartPosition.CenterParent;
			ShowInTaskbar = false;

			list.Dock = DockStyle.Fill;
			list.View = View.Details;
			list.FullRowSelect = true;
			list.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			list.Columns.Add("Property", 160);
			list.Columns.Add("Value", 250);
			foreach (string group in new[] { GroupController, GroupBattery, GroupPorts, GroupButtons })
				list.Groups.Add(new ListViewGroup(group, group));

			Button close = new Button();
			close.Text = "Close";
			close.Size = new Size(90, 28);
			close.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			close.Click += (s, e) => Close();

			Panel bottom = new Panel();
			bottom.Dock = DockStyle.Bottom;
			bottom.Height = 44;
			close.Location = new Point(bottom.Width - close.Width - 10, 8);
			bottom.Controls.Add(close);

			Controls.Add(list);
			Controls.Add(bottom);
			CancelButton = close;

			Hub.RemoteTriggered += Hub_RemoteTriggered;

			refreshTimer.Interval = 200;
			refreshTimer.Tick += (s, e) => RefreshRows();
			refreshTimer.Start();
			RefreshRows();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Hub.RemoteTriggered -= Hub_RemoteTriggered;
				refreshTimer.Dispose();
			}
			base.Dispose(disposing);
		}

		private void Hub_RemoteTriggered(Hub hub, Port p, Port.RemoteButtons button)
		{
			string source = (p == null) ? "Hub button" : "Port " + p.Id;
			lastButtonEvent = $"{source}: {ButtonName(button)} at {DateTime.Now:HH:mm:ss}";
		}

		private static string ButtonName(Port.RemoteButtons button)
		{
			switch (button)
			{
				case Port.RemoteButtons.BUTTON_PLUS: return "+ pressed";
				case Port.RemoteButtons.BUTTON_MINUS: return "- pressed";
				case Port.RemoteButtons.BUTTON_STOP: return "Stop (red) pressed";
				case Port.RemoteButtons.BUTTON_POWER: return "Pressed";
				default: return "Released";
			}
		}

		/// <summary>
		/// The rows to show right now, as (group, property, value)
		/// </summary>
		private List<string[]> BuildRows()
		{
			var rows = new List<string[]>();
			void Add(string group, string name, string value) => rows.Add(new[] { group, name, value });

			Add(GroupController, "Name", Hub.Name);
			Add(GroupController, "Type", Hub.Type.ToString());
			Add(GroupController, "Status", Hub.IsConnected ? "Connected" : "Disconnected");
			if (!string.IsNullOrEmpty(Hub.DeviceId))
				Add(GroupController, "Device ID", Hub.DeviceId);
			if (Hub.BluetoothAddress != 0)
				Add(GroupController, "Bluetooth address", FormatAddress(Hub.BluetoothAddress));
			Add(GroupController, "Firmware", string.IsNullOrEmpty(Hub.FirmwareVersion) ? "Not reported" : Hub.FirmwareVersion);
			Add(GroupController, "LED color", Hub.LEDColor.ToString());

			if (Hub.BatteryLevel > 0)
				Add(GroupBattery, "Level", $"{Hub.BatteryLevel:F0} %");
			if (Hub.BatteryVoltage > 0)
				Add(GroupBattery, "Voltage", $"{Hub.BatteryVoltage:F2} V");
			if (Hub.BatteryCurrent > 0)
				Add(GroupBattery, "Current", $"{Hub.BatteryCurrent:F0} mA");

			List<Port> ports = Hub.RegistredPorts.ToList();
			foreach (Port p in ports)
				Add(GroupPorts, "Port " + p.Id, DescribePort(p));

			bool hasButtons = false;
			if (Hub.PowerButtonPressed.HasValue)
			{
				Add(GroupButtons, "Hub button", Hub.PowerButtonPressed.Value ? "Pressed" : "Released");
				hasButtons = true;
			}
			foreach (Port p in ports.Where(p => p.Function == Port.Functions.BUTTON))
			{
				Add(GroupButtons, $"Port {p.Id} buttons", p.LastButton.HasValue ? ButtonName(p.LastButton.Value) : "Released");
				hasButtons = true;
			}
			if (hasButtons || HubReportsButtons())
				Add(GroupButtons, "Last button event", lastButtonEvent);
			else
				Add(GroupButtons, "Buttons", "Not reported by this controller");

			return rows;
		}

		private bool HubReportsButtons()
		{
			switch (Hub.Type)
			{
				case Hub.Types.BOOST_MOVE_HUB:
				case Hub.Types.POWERED_UP_HUB:
				case Hub.Types.POWERED_UP_REMOTE:
				case Hub.Types.TECHNIC_HUB:
				case Hub.Types.WEDO_2_SMART_HUB:
					return true;
				default:
					return false;
			}
		}

		private static string DescribePort(Port p)
		{
			string text = p.Function.ToString();
			if (p.Device != Port.Devices.UNKNOWN)
				text += " (" + p.Device + ")";

			switch (p.Function)
			{
				case Port.Functions.MOTOR:
				case Port.Functions.TRAIN_MOTOR:
				case Port.Functions.LIGHT:
					text += ", speed " + p.Speed;
					break;
				case Port.Functions.SENSOR:
					text += $", color {p.LatestColor}, distance {p.LatestDistance}";
					break;
			}
			return text;
		}

		private static string FormatAddress(ulong address)
		{
			byte[] bytes = BitConverter.GetBytes(address);
			return string.Join(":", bytes.Take(6).Reverse().Select(b => b.ToString("X2")));
		}

		private void RefreshRows()
		{
			List<string[]> rows;
			try
			{
				rows = BuildRows();
			}
			catch (InvalidOperationException)
			{
				// Ports changed on the hub's thread while we were reading them; next tick picks it up
				return;
			}

			Text = "Hub Info - " + Hub.Name;

			// Same rows as last time? Only touch the values that changed, so the list does not flicker
			bool sameLayout = rows.Count == list.Items.Count;
			for (int i = 0; sameLayout && i < rows.Count; i++)
				sameLayout = list.Items[i].Text == rows[i][1] && list.Items[i].Group.Name == rows[i][0];

			if (sameLayout)
			{
				for (int i = 0; i < rows.Count; i++)
					if (list.Items[i].SubItems[1].Text != rows[i][2])
						list.Items[i].SubItems[1].Text = rows[i][2];
				return;
			}

			list.BeginUpdate();
			list.Items.Clear();
			foreach (string[] row in rows)
			{
				ListViewItem item = new ListViewItem(row[1], list.Groups[row[0]]);
				item.SubItems.Add(row[2]);
				list.Items.Add(item);
			}
			list.EndUpdate();
		}
	}
}
