using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace LegoTrainProject
{
	/// <summary>
	/// Live track map for Self-Driving: each section is a block, drawn red when a train is
	/// in it, amber while reserved, green when free. Arrows show what follows each section
	/// (solid = left/straight, dashed = right); for switches the active branch is bold.
	/// Drag blocks to match the real layout; positions are saved with the project.
	/// </summary>
	public class SectionMapForm : Form
	{
		private readonly TrainProject project;
		private readonly MapCanvas canvas;
		private readonly ToolStripComboBox pathCombo;
		private readonly Timer refreshTimer;

		public SectionMapForm(TrainProject project)
		{
			this.project = project;

			Text = "Track Map";
			try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch { }
			StartPosition = FormStartPosition.CenterParent;
			Size = new Size(1000, 720);
			MinimumSize = new Size(500, 400);

			var toolStrip = new ToolStrip { GripStyle = ToolStripGripStyle.Hidden, ImageScalingSize = new Size(24, 24) };

			var autoLayout = new ToolStripButton("Auto layout") { ToolTipText = "Re-arrange all sections automatically" };
			autoLayout.Click += (s, e) =>
			{
				if (MessageBox.Show(this, "Re-arrange all sections? Your manual positions will be replaced.", "Auto layout",
					MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
				{
					MapLayout.AutoLayout(project.Sections.GetAll(), onlyUnplaced: false);
					canvas.Invalidate();
				}
			};

			pathCombo = new ToolStripComboBox { DropDownStyle = ComboBoxStyle.DropDownList, AutoSize = false, Width = 220 };
			pathCombo.SelectedIndexChanged += (s, e) => canvas.HighlightTrain = (pathCombo.SelectedItem as TrainItem)?.Hub;
			pathCombo.DropDown += (s, e) => FillTrains();

			toolStrip.Items.Add(autoLayout);
			toolStrip.Items.Add(new ToolStripSeparator());
			toolStrip.Items.Add(new ToolStripLabel("Show path of:"));
			toolStrip.Items.Add(pathCombo);

			canvas = new MapCanvas(project) { Dock = DockStyle.Fill };

			Controls.Add(canvas);
			Controls.Add(toolStrip);

			FillTrains();
			MapLayout.AutoLayout(project.Sections.GetAll(), onlyUnplaced: true);

			// Sections.DataUpdated fires from background threads; marshal to the UI thread
			project.Sections.DataUpdated += OnSectionsChanged;

			// Also repaint regularly, since not every state change raises DataUpdated
			refreshTimer = new Timer { Interval = 500 };
			refreshTimer.Tick += (s, e) => canvas.Invalidate();
			refreshTimer.Start();
		}

		private void OnSectionsChanged()
		{
			if (IsHandleCreated && !IsDisposed)
				BeginInvoke((Action)(() => canvas.Invalidate()));
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			project.Sections.DataUpdated -= OnSectionsChanged;
			refreshTimer.Stop();
			refreshTimer.Dispose();
			base.OnFormClosed(e);
		}

		/// <summary>
		/// Call after sections are added or removed elsewhere
		/// </summary>
		public void SectionsChanged()
		{
			MapLayout.AutoLayout(project.Sections.GetAll(), onlyUnplaced: true);
			canvas.Invalidate();
		}

		private void FillTrains()
		{
			Hub selected = (pathCombo.SelectedItem as TrainItem)?.Hub;
			pathCombo.Items.Clear();
			pathCombo.Items.Add(new TrainItem(null));
			foreach (Hub h in project.RegisteredTrains.Where(h => h.CurrentPath != null))
				pathCombo.Items.Add(new TrainItem(h));

			pathCombo.SelectedItem = pathCombo.Items.Cast<TrainItem>().FirstOrDefault(i => i.Hub == selected) ?? pathCombo.Items[0];
		}

		private sealed class TrainItem
		{
			public readonly Hub Hub;
			public TrainItem(Hub hub) { Hub = hub; }
			public override string ToString() => Hub == null ? "(none)" : $"{Hub.Name} - {Hub.CurrentPath.Name}";
		}
	}

	/// <summary>
	/// Automatic placement: the main loop (following left/straight links from the first
	/// section) goes on an ellipse; branches are placed outside the section they leave from.
	/// </summary>
	internal static class MapLayout
	{
		public const float Width = 1000, Height = 700;

		public static void AutoLayout(List<Section> sections, bool onlyUnplaced)
		{
			if (sections.Count == 0)
				return;
			if (onlyUnplaced && sections.All(s => s.MapPlaced))
				return;

			bool anyPlaced = onlyUnplaced && sections.Any(s => s.MapPlaced);

			if (!anyPlaced)
			{
				// Main line: follow left links from the first section until we loop back or stop
				var main = new List<Section>();
				for (Section s = sections[0]; s != null && !main.Contains(s); s = s.LeftSection)
					main.Add(s);

				float cx = Width / 2, cy = Height / 2, rx = Width * 0.36f, ry = Height * 0.34f;
				for (int i = 0; i < main.Count; i++)
				{
					double a = -Math.PI / 2 + 2 * Math.PI * i / main.Count;
					Place(main[i], cx + rx * (float)Math.Cos(a), cy + ry * (float)Math.Sin(a));
				}

				// Everything else: walk outwards from placed sections
				PlaceBranches(sections, s => !main.Contains(s));
			}
			else
			{
				PlaceBranches(sections, s => !s.MapPlaced);
			}

			// Anything still unreachable goes in a row along the bottom
			int row = 0;
			foreach (Section s in sections.Where(s => !s.MapPlaced))
				Place(s, 80 + (row++ % 7) * 140, Height - 40);
		}

		private static void PlaceBranches(List<Section> sections, Func<Section, bool> needsPlacing)
		{
			var todo = new HashSet<Section>(sections.Where(needsPlacing));
			foreach (Section s in todo)
				s.MapPlaced = false;

			bool progress = true;
			while (todo.Count > 0 && progress)
			{
				progress = false;
				foreach (Section from in sections.Where(s => s.MapPlaced).ToList())
				{
					int k = 0;
					foreach (Section next in new[] { from.LeftSection, from.RightSection })
					{
						k++;
						if (next == null || !todo.Contains(next))
							continue;

						// Push outward from the centre, and to the side for the right branch
						float dx = from.MapX - Width / 2, dy = from.MapY - Height / 2;
						float len = Math.Max(1, (float)Math.Sqrt(dx * dx + dy * dy));
						float ox = dx / len * 130, oy = dy / len * 110;
						float side = (k == 2) ? 70 : 0;
						Place(next, from.MapX + ox - oy / len * side, from.MapY + oy + ox / len * side);

						todo.Remove(next);
						progress = true;
					}
				}
			}
		}

		private static void Place(Section s, float x, float y)
		{
			s.MapX = x;
			s.MapY = y;
			s.MapPlaced = true;
		}
	}

	internal class MapCanvas : Panel
	{
		private const float BlockW = 130, BlockH = 50;

		private static readonly Color FreeColor = Color.FromArgb(46, 160, 67);
		private static readonly Color ReservedColor = Color.FromArgb(218, 150, 0);
		private static readonly Color OccupiedColor = Color.FromArgb(214, 57, 57);
		private static readonly Color IdleColor = Color.FromArgb(120, 128, 140);
		private static readonly Color PathColor = Color.FromArgb(45, 120, 230);

		private readonly TrainProject project;
		private Section dragging;
		private PointF dragOffset;
		private Hub highlightTrain;

		// map <-> screen transform, recomputed on every paint
		private float scale = 1, offsetX, offsetY;

		public MapCanvas(TrainProject project)
		{
			this.project = project;
			DoubleBuffered = true;
			ResizeRedraw = true;
			BackColor = Color.White;
		}

		public Hub HighlightTrain
		{
			get => highlightTrain;
			set { highlightTrain = value; Invalidate(); }
		}

		private List<Section> Sections => project.Sections.GetAll();

		private void ComputeTransform()
		{
			var all = Sections.Where(s => s.MapPlaced).ToList();
			float minX = 0, minY = 0, maxX = MapLayout.Width, maxY = MapLayout.Height;
			if (all.Count > 0)
			{
				minX = Math.Min(minX, all.Min(s => s.MapX) - BlockW);
				minY = Math.Min(minY, all.Min(s => s.MapY) - BlockH);
				maxX = Math.Max(maxX, all.Max(s => s.MapX) + BlockW);
				maxY = Math.Max(maxY, all.Max(s => s.MapY) + BlockH);
			}

			const int margin = 20, legend = 34;
			float w = Math.Max(1, ClientSize.Width - 2 * margin);
			float h = Math.Max(1, ClientSize.Height - 2 * margin - legend);
			scale = Math.Min(w / (maxX - minX), h / (maxY - minY));
			offsetX = margin + (w - (maxX - minX) * scale) / 2 - minX * scale;
			offsetY = margin + (h - (maxY - minY) * scale) / 2 - minY * scale;
		}

		private PointF ToScreen(float x, float y) => new PointF(x * scale + offsetX, y * scale + offsetY);
		private PointF ToMap(Point p) => new PointF((p.X - offsetX) / scale, (p.Y - offsetY) / scale);

		private RectangleF BlockRect(Section s)
		{
			PointF c = ToScreen(s.MapX, s.MapY);
			float w = BlockW * scale, h = BlockH * scale;
			return new RectangleF(c.X - w / 2, c.Y - h / 2, w, h);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			var sections = Sections;
			if (sections.Count == 0)
			{
				TextRenderer.DrawText(g, "No sections yet. Add sections in the Self-Driving tab.", Font, ClientRectangle,
					IdleColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
				return;
			}

			// Keep the transform fixed while dragging so the block follows the mouse
			if (dragging == null)
				ComputeTransform();
			bool running = project.Sections.IsRunning;

			// Sections on the highlighted train's path, with their step numbers
			var pathSteps = new Dictionary<Section, List<int>>();
			if (highlightTrain?.CurrentPath?.Sections != null)
			{
				int[] ids = highlightTrain.CurrentPath.Sections;
				for (int i = 0; i < ids.Length; i++)
				{
					if (ids[i] < 0 || ids[i] >= sections.Count) continue;
					if (!pathSteps.TryGetValue(sections[ids[i]], out var steps))
						pathSteps[sections[ids[i]]] = steps = new List<int>();
					steps.Add(i + 1);
				}
			}

			// Links first, so blocks are drawn on top
			foreach (Section s in sections.Where(x => x.MapPlaced))
			{
				bool? switchLeft = SwitchIsLeft(s);
				DrawLink(g, s, s.LeftSection, dashed: false, active: switchLeft != false, onPath: OnPath(s, s.LeftSection));
				DrawLink(g, s, s.RightSection, dashed: true, active: switchLeft != true, onPath: OnPath(s, s.RightSection));
			}

			using (var nameFont = new Font(Font.FontFamily, Math.Max(6f, 10f * scale), FontStyle.Bold))
			using (var smallFont = new Font(Font.FontFamily, Math.Max(5.5f, 8.5f * scale)))
			{
				foreach (Section s in sections.Where(x => x.MapPlaced))
				{
					RectangleF r = BlockRect(s);

					Color state;
					string detail;
					if (s.CurrentHub != null)
					{
						state = OccupiedColor;
						detail = s.CurrentHub.Name;
					}
					else if (s.ReservedBy != null || s.IsBeingCleared)
					{
						state = ReservedColor;
						detail = s.ReservedBy != null ? "reserved: " + s.ReservedBy.Name : "clearing";
					}
					else
					{
						state = running ? FreeColor : IdleColor;
						detail = running ? "free" : (s.Detector == null ? "no sensor" : "");
					}

					using (GraphicsPath gp = RoundedRect(r, 8 * scale))
					using (var fill = new SolidBrush(Color.FromArgb(40, state)))
					using (var border = new Pen(state, Math.Max(1.5f, 2.5f * scale)))
					{
						g.FillPath(fill, gp);
						g.DrawPath(border, gp);
					}

					if (pathSteps.TryGetValue(s, out var steps))
					{
						RectangleF hr = RectangleF.Inflate(r, 4 * scale, 4 * scale);
						using (GraphicsPath gp = RoundedRect(hr, 11 * scale))
						using (var pen = new Pen(PathColor, Math.Max(1.5f, 3 * scale)) { DashStyle = DashStyle.Dot })
							g.DrawPath(pen, gp);

						string label = string.Join(",", steps);
						SizeF ls = g.MeasureString(label, smallFont);
						var badge = new RectangleF(r.Right - ls.Width - 4, r.Top - ls.Height / 2 - 2, ls.Width + 6, ls.Height + 2);
						using (var b = new SolidBrush(PathColor)) g.FillEllipse(b, badge);
						g.DrawString(label, smallFont, Brushes.White, badge.X + 3, badge.Y + 1);
					}

					var fmt = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter };
					var top = new RectangleF(r.X + 2, r.Y + 2, r.Width - 4, r.Height * 0.55f);
					var bottom = new RectangleF(r.X + 2, r.Y + r.Height * 0.5f, r.Width - 4, r.Height * 0.45f);
					g.DrawString(s.Name ?? "", nameFont, Brushes.Black, top, fmt);
					using (var b = new SolidBrush(Color.FromArgb(70, 70, 70)))
						g.DrawString(detail, smallFont, b, bottom, fmt);
				}
			}

			DrawLegend(g, running);
		}

		private bool OnPath(Section from, Section to)
		{
			int[] ids = highlightTrain?.CurrentPath?.Sections;
			if (ids == null || to == null) return false;
			var all = Sections;
			for (int i = 0; i + 1 < ids.Length; i++)
				if (ids[i] >= 0 && ids[i] < all.Count && ids[i + 1] >= 0 && ids[i + 1] < all.Count &&
					all[ids[i]] == from && all[ids[i + 1]] == to)
					return true;
			// Looping path: last -> first
			return highlightTrain.LoopCurrentPath && ids.Length > 1 &&
				ids[^1] < all.Count && ids[0] < all.Count && all[ids[^1]] == from && all[ids[0]] == to;
		}

		/// <summary>
		/// Which way the section's switch is thrown, or null if it has none / it isn't connected
		/// </summary>
		private bool? SwitchIsLeft(Section s)
		{
			if (s.RightSection == null || s.Switch == null)
				return null;

			Hub hub = s.Switch1Hub ?? project.GetHubIndexByDeviceId(s.GetSwitchDeviceId());
			string port = s.Switch1Port ?? s.GetSwitchPort();
			if (hub == null || !hub.IsConnected || port == null || hub.GetPortFromPortId(port) == null)
				return null;

			return hub.IsSwitchOnTheLeft(port);
		}

		private void DrawLink(Graphics g, Section from, Section to, bool dashed, bool active, bool onPath)
		{
			if (to == null || !to.MapPlaced)
				return;

			RectangleF a = BlockRect(from), b = BlockRect(to);
			PointF p1 = EdgePoint(a, Center(b)), p2 = EdgePoint(b, Center(a));

			Color c = onPath ? PathColor : (active ? Color.FromArgb(60, 60, 60) : Color.FromArgb(190, 190, 190));
			float width = (onPath ? 3.5f : active ? 2.2f : 1.2f) * Math.Max(0.6f, scale);

			using (var pen = new Pen(c, width))
			{
				if (dashed) pen.DashStyle = DashStyle.Dash;
				pen.CustomEndCap = new AdjustableArrowCap(4, 5);
				g.DrawLine(pen, p1, p2);
			}
		}

		private static PointF Center(RectangleF r) => new PointF(r.X + r.Width / 2, r.Y + r.Height / 2);

		/// <summary>
		/// Point where the line from the rectangle's centre towards target leaves the rectangle
		/// </summary>
		private static PointF EdgePoint(RectangleF r, PointF target)
		{
			PointF c = Center(r);
			float dx = target.X - c.X, dy = target.Y - c.Y;
			if (dx == 0 && dy == 0) return c;
			float sx = dx == 0 ? float.MaxValue : (r.Width / 2) / Math.Abs(dx);
			float sy = dy == 0 ? float.MaxValue : (r.Height / 2) / Math.Abs(dy);
			float t = Math.Min(sx, sy);
			return new PointF(c.X + dx * t, c.Y + dy * t);
		}

		private void DrawLegend(Graphics g, bool running)
		{
			float x = 12, y = ClientSize.Height - 26;
			void Swatch(Color c, string label)
			{
				using (var b = new SolidBrush(c)) g.FillRectangle(b, x, y + 3, 14, 14);
				g.DrawString(label, Font, Brushes.DimGray, x + 18, y + 2);
				x += 26 + g.MeasureString(label, Font).Width;
			}

			Swatch(OccupiedColor, "Train in section");
			Swatch(ReservedColor, "Reserved / clearing");
			Swatch(running ? FreeColor : IdleColor, running ? "Free" : "Free (not running)");
			g.DrawString("Solid arrow = left/straight, dashed = right. Drag blocks to arrange.", Font, Brushes.DimGray, x + 10, y + 2);
		}

		private static GraphicsPath RoundedRect(RectangleF r, float radius)
		{
			float d = Math.Max(1, Math.Min(radius * 2, Math.Min(r.Width, r.Height)));
			var gp = new GraphicsPath();
			gp.AddArc(r.X, r.Y, d, d, 180, 90);
			gp.AddArc(r.Right - d, r.Y, d, d, 270, 90);
			gp.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
			gp.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
			gp.CloseFigure();
			return gp;
		}

		//////////////////////////////////////////
		//// Dragging
		//////////////////////////////////////////

		private Section HitTest(Point p)
		{
			// Topmost (last drawn) first
			return Sections.Where(s => s.MapPlaced).LastOrDefault(s => BlockRect(s).Contains(p));
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button != MouseButtons.Left) return;

			ComputeTransform();
			dragging = HitTest(e.Location);
			if (dragging != null)
			{
				PointF m = ToMap(e.Location);
				dragOffset = new PointF(dragging.MapX - m.X, dragging.MapY - m.Y);
				Capture = true;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (dragging == null)
			{
				Cursor = HitTest(e.Location) != null ? Cursors.SizeAll : Cursors.Default;
				return;
			}

			PointF m = ToMap(e.Location);
			dragging.MapX = m.X + dragOffset.X;
			dragging.MapY = m.Y + dragOffset.Y;
			dragging.MapPlaced = true;
			Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (dragging != null)
			{
				dragging = null;
				Capture = false;
				Invalidate(); // refit now that the drag is over
			}
		}
	}
}
