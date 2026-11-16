using System;
using System.Drawing;
using System.Windows.Forms;
using Buttons = LegoTrainProject.GamepadController.Buttons;

namespace LegoTrainProject
{
	/// <summary>
	/// Xbox controller guide: what every button does, plus live controller status.
	/// The row of a button lights up while that button is held, so the mapping can be tried out.
	/// Built in code (no designer file).
	/// </summary>
	public class GamepadForm : Form
	{
		/// <summary>One row of the guide. Stick is +1 / -1 for the left stick row, 0 otherwise.</summary>
		private class Mapping
		{
			public string Control;
			public string Action;
			public string Description;
			public Buttons Buttons;
			public bool LeftStick;
		}

		// Keep in sync with GamepadController.Poll()
		private static readonly Mapping[] Mappings =
		{
			new Mapping { Control = "LB", Buttons = Buttons.LeftShoulder, Action = "Previous train",
				Description = "Select the previous connected train. The controller rumbles briefly." },
			new Mapping { Control = "RB", Buttons = Buttons.RightShoulder, Action = "Next train",
				Description = "Select the next connected train. The controller rumbles briefly." },
			new Mapping { Control = "D-pad Up", Buttons = Buttons.DPadUp, Action = "Speed +10",
				Description = "Speed up the selected train by one step, up to 100." },
			new Mapping { Control = "D-pad Down", Buttons = Buttons.DPadDown, Action = "Speed -10",
				Description = "Slow the selected train by one step. Below 0 it runs in reverse, down to -100." },
			new Mapping { Control = "Left stick up / down", LeftStick = true, Action = "Throttle",
				Description = "Changes speed while held; push further to change faster. Speed stays when released." },
			new Mapping { Control = "B", Buttons = Buttons.B, Action = "Stop",
				Description = "Stop the selected train." },
			new Mapping { Control = "Y", Buttons = Buttons.Y, Action = "Reverse",
				Description = "Run the selected train at the same speed in the opposite direction." },
			new Mapping { Control = "X", Buttons = Buttons.X, Action = "Stop and next",
				Description = "Stop the selected train, then select the next one." },
			new Mapping { Control = "Back / View", Buttons = Buttons.Back, Action = "EMERGENCY STOP",
				Description = "Stop all trains at once, same as the space bar. Long rumble." },
			new Mapping { Control = "Start / Menu", Buttons = Buttons.Start, Action = "Help",
				Description = "Print the button list in the console." },
			new Mapping { Control = "A, D-pad Left / Right, stick clicks", Action = "Not used",
				Buttons = Buttons.A | Buttons.DPadLeft | Buttons.DPadRight | Buttons.LeftThumb | Buttons.RightThumb,
				Description = "No action assigned. Triggers and the right stick are not used either." },
		};

		private readonly GamepadController Gamepad;
		private readonly ListView list = new ListView();
		private readonly Label status = new Label();
		private readonly Timer refreshTimer = new Timer();

		public GamepadForm(GamepadController gamepad)
		{
			Gamepad = gamepad;

			AutoScaleDimensions = new SizeF(96F, 96F);
			AutoScaleMode = AutoScaleMode.Dpi;
			Text = "Xbox Controller Buttons";
			ClientSize = new Size(820, 420);
			MinimumSize = new Size(500, 300);
			FormBorderStyle = FormBorderStyle.SizableToolWindow;
			StartPosition = FormStartPosition.CenterParent;
			ShowInTaskbar = false;

			status.Dock = DockStyle.Top;
			status.Height = 36;
			status.Padding = new Padding(8, 0, 8, 0);
			status.TextAlign = ContentAlignment.MiddleLeft;
			status.Font = new Font(Font, FontStyle.Bold);

			list.Dock = DockStyle.Fill;
			list.View = View.Details;
			list.FullRowSelect = true;
			list.ShowItemToolTips = true;
			list.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			list.Columns.Add("Button", 200);
			list.Columns.Add("Action", 120);
			list.Columns.Add("Description", 480);
			foreach (Mapping m in Mappings)
			{
				ListViewItem item = new ListViewItem(m.Control);
				item.SubItems.Add(m.Action);
				item.SubItems.Add(m.Description);
				item.ToolTipText = m.Description;
				item.Tag = m;
				list.Items.Add(item);
			}

			Label note = new Label();
			note.Dock = DockStyle.Bottom;
			note.Height = 58;
			note.Padding = new Padding(8, 6, 110, 6);
			note.Text = "Any Xbox 360, One or Series controller works, wired or wireless. Plug it in or pair it at any time and it is picked up automatically. " +
				"Only connected hubs with a train motor port can be driven. Press a button to see its row light up.";

			Button close = new Button();
			close.Text = "Close";
			close.Size = new Size(90, 28);
			close.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			close.Location = new Point(note.Width - close.Width - 10, note.Height - close.Height - 10);
			close.Click += (s, e) => Close();
			note.Controls.Add(close);

			Controls.Add(list);
			Controls.Add(status);
			Controls.Add(note);
			CancelButton = close;

			refreshTimer.Interval = 100;
			refreshTimer.Tick += (s, e) => RefreshStatus();
			refreshTimer.Start();
			RefreshStatus();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				refreshTimer.Dispose();
			base.Dispose(disposing);
		}

		private void RefreshStatus()
		{
			string text;
			Color color;

			if (Gamepad == null)
			{
				text = "Controller support could not be started - see the console.";
				color = Color.Firebrick;
			}
			else if (!Gamepad.IsConnected)
			{
				text = "No controller detected.";
				color = Color.Firebrick;
			}
			else
			{
				string train = Gamepad.SelectedTrainName;
				text = $"Controller {Gamepad.ControllerNumber} connected - " +
					(train == null ? "no train selected (no connected hub has a train motor port)" : $"driving {train}, speed {Gamepad.SelectedTrainSpeed}");
				color = Color.DarkGreen;
			}

			if (status.Text != text)
				status.Text = text;
			status.ForeColor = color;

			Buttons held = (Gamepad == null) ? 0 : Gamepad.HeldButtons;
			bool stick = Gamepad != null && Gamepad.LeftStickDirection != 0;

			foreach (ListViewItem item in list.Items)
			{
				Mapping m = (Mapping)item.Tag;
				bool active = m.LeftStick ? stick : (held & m.Buttons) != 0;
				Color back = active ? Color.Gold : list.BackColor;
				if (item.BackColor != back)
					item.BackColor = back;
			}
		}
	}
}
