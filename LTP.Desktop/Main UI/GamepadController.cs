using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LegoTrainProject
{
	/// <summary>
	/// Drives trains from an Xbox (or any XInput) controller, wired or wireless.
	///
	/// Mapping:
	///   LB / RB          select previous / next train
	///   D-pad Up / Down  speed +/- one step
	///   Left stick       hold up/down for direct throttle, release to keep speed
	///   B                stop selected train
	///   Y                reverse selected train
	///   X                stop and select next train
	///   Back / View      emergency stop all trains
	///   Start / Menu     print this mapping to the console
	/// </summary>
	public class GamepadController : IDisposable
	{
		private const int SpeedStep = 10;
		private const int MaxSpeed = 100;
		private const int PollMs = 50;
		private const short StickDeadzone = 8000;

		private readonly Func<IEnumerable<Hub>> getTrains;
		private readonly Action emergencyStopAll;
		private readonly Timer timer;

		private int userIndex = -1;
		private ushort lastButtons;
		private Hub selected;
		private int stickRepeat;
		private int rescanCountdown;

		public GamepadController(Func<IEnumerable<Hub>> getTrains, Action emergencyStopAll)
		{
			this.getTrains = getTrains;
			this.emergencyStopAll = emergencyStopAll;

			// WinForms timer: ticks on the UI thread, so hub/UI calls are safe
			timer = new Timer { Interval = PollMs };
			timer.Tick += (s, e) => Poll();
		}

		public void Start() => timer.Start();

		public void Dispose()
		{
			timer.Stop();
			timer.Dispose();
		}

		private void Poll()
		{
			XInputState state;

			if (userIndex < 0)
			{
				// Look for a controller about once a second (probing empty slots is slow)
				if (rescanCountdown-- > 0)
					return;
				rescanCountdown = 1000 / PollMs;

				for (int i = 0; i < 4; i++)
				{
					if (XInputGetState(i, out state) == 0)
					{
						userIndex = i;
						lastButtons = state.Gamepad.wButtons;
						MainBoard.WriteLine($"Controller {i + 1} connected - press Start for button help", Color.Green);
						SelectNext(1);
						return;
					}
				}
				return;
			}

			if (XInputGetState(userIndex, out state) != 0)
			{
				MainBoard.WriteLine($"Controller {userIndex + 1} disconnected", Color.Orange);
				userIndex = -1;
				rescanCountdown = 0;
				return;
			}

			var pad = state.Gamepad;
			ushort pressed = (ushort)(pad.wButtons & ~lastButtons);
			lastButtons = pad.wButtons;

			if (Has(pressed, Buttons.Back))
			{
				emergencyStopAll();
				Rumble(40000, 300);
				MainBoard.WriteLine("Controller: EMERGENCY STOP - all trains", Color.Red);
			}
			if (Has(pressed, Buttons.Start)) PrintHelp();
			if (Has(pressed, Buttons.RightShoulder)) SelectNext(1);
			if (Has(pressed, Buttons.LeftShoulder)) SelectNext(-1);

			if (!IsUsable(selected))
			{
				if (pressed != 0) SelectNext(1);
				return;
			}

			if (Has(pressed, Buttons.DPadUp)) ChangeSpeed(+SpeedStep);
			if (Has(pressed, Buttons.DPadDown)) ChangeSpeed(-SpeedStep);
			if (Has(pressed, Buttons.B)) SetSpeed(0);
			if (Has(pressed, Buttons.Y)) SetSpeed(-CurrentSpeed());
			if (Has(pressed, Buttons.X)) { SetSpeed(0); SelectNext(1); }

			// Left stick: while held, step the throttle; how far it's pushed sets how fast
			short y = pad.sThumbLY;
			if (Math.Abs((int)y) > StickDeadzone)
			{
				int interval = Math.Abs((int)y) > 28000 ? 2 : 5; // ticks between steps
				if (stickRepeat-- <= 0)
				{
					ChangeSpeed(y > 0 ? SpeedStep : -SpeedStep);
					stickRepeat = interval;
				}
			}
			else
				stickRepeat = 0;
		}

		private static bool Has(ushort buttons, Buttons b) => (buttons & (ushort)b) != 0;

		private static bool IsUsable(Hub h) => h != null && h.IsConnected && h.TrainMotorPort != null;

		private void SelectNext(int direction)
		{
			var trains = getTrains().Where(IsUsable).ToList();
			if (trains.Count == 0)
			{
				if (selected != null)
					MainBoard.WriteLine("Controller: no connected trains with a TRAIN_MOTOR port", Color.Orange);
				selected = null;
				return;
			}

			int idx = trains.IndexOf(selected);
			idx = (idx < 0) ? 0 : (idx + direction + trains.Count) % trains.Count;
			selected = trains[idx];
			Rumble(15000, 80);
			MainBoard.WriteLine($"Controller: selected {TrainName} (speed {CurrentSpeed()})", Color.Cyan);
		}

		private string TrainName => string.IsNullOrEmpty(selected.Name) ? selected.DeviceId : selected.Name;

		private int CurrentSpeed()
		{
			int s = selected.GetMotorSpeed(selected.TrainMotorPort);
			if (s == 127) return 0; // 127 = PU brake command

			// SetMotorSpeed stores the already-inverted value; undo it so stepping works
			Port p = selected.GetPortFromPortId(selected.TrainMotorPort);
			return (p != null && p.Inverted) ? -s : s;
		}

		private void ChangeSpeed(int delta) => SetSpeed(CurrentSpeed() + delta);

		private void SetSpeed(int speed)
		{
			speed = Math.Max(-MaxSpeed, Math.Min(MaxSpeed, speed));
			if (speed == CurrentSpeed())
				return;

			if (speed == 0)
				selected.Stop(selected.TrainMotorPort);
			else
				selected.SetMotorSpeed(selected.TrainMotorPort, speed);

			MainBoard.WriteLine($"Controller: {TrainName} speed {speed}");
		}

		private void PrintHelp()
		{
			MainBoard.WriteLine("Controller buttons:", Color.Cyan);
			MainBoard.WriteLine("  LB / RB = previous / next train    D-pad Up/Down = speed +/-");
			MainBoard.WriteLine("  Left stick up/down = throttle      B = stop    Y = reverse");
			MainBoard.WriteLine("  X = stop and select next train     Back/View = EMERGENCY STOP ALL");
		}

		private void Rumble(ushort strength, int ms)
		{
			if (userIndex < 0) return;
			var v = new XInputVibration { wLeftMotorSpeed = strength, wRightMotorSpeed = strength };
			XInputSetState(userIndex, ref v);
			var t = new Timer { Interval = ms };
			int idx = userIndex;
			t.Tick += (s, e) =>
			{
				var off = new XInputVibration();
				XInputSetState(idx, ref off);
				t.Stop();
				t.Dispose();
			};
			t.Start();
		}

		//////////////////////////////////////////
		//// XInput (xinput1_4.dll ships with Windows 8 and later)
		//////////////////////////////////////////

		[Flags]
		private enum Buttons : ushort
		{
			DPadUp = 0x0001, DPadDown = 0x0002, DPadLeft = 0x0004, DPadRight = 0x0008,
			Start = 0x0010, Back = 0x0020, LeftThumb = 0x0040, RightThumb = 0x0080,
			LeftShoulder = 0x0100, RightShoulder = 0x0200,
			A = 0x1000, B = 0x2000, X = 0x4000, Y = 0x8000
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct XInputGamepad
		{
			public ushort wButtons;
			public byte bLeftTrigger;
			public byte bRightTrigger;
			public short sThumbLX;
			public short sThumbLY;
			public short sThumbRX;
			public short sThumbRY;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct XInputState
		{
			public uint dwPacketNumber;
			public XInputGamepad Gamepad;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct XInputVibration
		{
			public ushort wLeftMotorSpeed;
			public ushort wRightMotorSpeed;
		}

		[DllImport("xinput1_4.dll")]
		private static extern int XInputGetState(int dwUserIndex, out XInputState pState);

		[DllImport("xinput1_4.dll")]
		private static extern int XInputSetState(int dwUserIndex, ref XInputVibration pVibration);
	}
}
