using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace LegoTrainProject
{
	/// <summary>
	/// Turns a BlueBrick layout (.bbm) into BAP sections.
	///
	/// A .bbm lists every track piece with its position, rotation and which of its connectors
	/// are linked to which other pieces. From that track network:
	///   - a piece linked to 3 others is a switch; its trunk is the leg pointing away from the
	///     other two, and left/right is judged from the direction of travel
	///   - a piece linked to 4 others is a crossing; trains go straight through it
	///   - each stretch of track between switches (or dead ends) becomes a section
	///   - one direction of travel is chosen and spread across the network, so a train arriving
	///     at a switch's trunk (facing) gets a left and a right next section, and one arriving
	///     on a branch (trailing) continues onto the trunk
	///
	/// Sensors and switch hubs can't be known from the drawing; the user assigns them after import.
	/// </summary>
	public static class BlueBrickImporter
	{
		public sealed class Result
		{
			public List<Section> Sections = new List<Section>();
			public List<string> Warnings = new List<string>();
			public int Pieces;
			public int Switches;
			public int Crossings;
		}

		/// <param name="maxPiecesPerSection">Split long stretches into sections of about this many
		/// track pieces (0 = only split switchless loops in two)</param>
		public static Result Import(string path, int maxPiecesPerSection = 0)
		{
			using (var reader = File.OpenText(path))
				return Import(reader, maxPiecesPerSection);
		}

		public static Result Import(TextReader bbm, int maxPiecesPerSection = 0)
		{
			var result = new Result();
			List<Piece> pieces = Parse(XDocument.Load(bbm));

			// Only pieces that are part of a track network of a reasonable size
			var components = Components(pieces).Where(c => c.Count >= 3).ToList();
			int dropped = pieces.Count(p => p.Neighbors.Count > 0) - components.Sum(c => c.Count);
			if (components.Count == 0)
			{
				result.Warnings.Add("No connected track was found in this layout.");
				return result;
			}
			if (components.Count > 1)
				result.Warnings.Add($"The layout has {components.Count} separate track networks; all were imported. Delete any you don't need (roads, for example).");
			if (dropped > 0)
				result.Warnings.Add($"{dropped} loose piece(s) not connected to a larger track network were skipped.");

			var used = components.SelectMany(c => c).ToList();
			result.Pieces = used.Count;
			foreach (Piece p in used)
				Classify(p, result);

			List<Segment> segments = BuildSegments(components);
			Orient(segments, result);

			var sectionsOf = new Dictionary<Segment, List<Section>>();
			var rawTrack = new Dictionary<Section, List<PointF>>();
			int n = 0;
			foreach (Segment seg in segments)
			{
				var parts = SplitSegment(seg, maxPiecesPerSection);
				var secs = parts.Select(_ => new Section { Name = "Section " + (++n) }).ToList();
				for (int i = 0; i + 1 < secs.Count; i++)
					secs[i].LeftSection = secs[i + 1];
				if (seg.IsClosedLoop)
					secs[secs.Count - 1].LeftSection = secs[0];

				for (int i = 0; i < secs.Count; i++)
					rawTrack[secs[i]] = parts[i];

				sectionsOf[seg] = secs;
				result.Sections.AddRange(secs);
			}

			LinkAtSwitches(segments, sectionsOf, result);
			FitToMap(rawTrack);

			return result;
		}

		//////////////////////////////////////////
		//// Parsing
		//////////////////////////////////////////

		private sealed class Piece
		{
			public string Part;
			public PointF Center;
			public List<Piece> Neighbors = new List<Piece>();

			public Kind Kind;
			public Piece Trunk, Left, Right;                        // switches
			public Dictionary<Piece, Piece> Through;                // crossings: in -> out
			public override string ToString() => $"{Part} @ {Center}";
		}

		private enum Kind { Plain, End, Switch, Crossing, Junction }

		private static float F(XElement e, string name) =>
			float.Parse((string)e.Element(name) ?? "0", CultureInfo.InvariantCulture);

		private static List<Piece> Parse(XDocument doc)
		{
			var pieces = new List<Piece>();
			var ownerOfConnection = new Dictionary<string, Piece>();
			var links = new List<(Piece piece, string linkedTo)>();

			var bricks = doc.Descendants("Layer")
				.Where(l => (string)l.Attribute("type") == "brick")
				.SelectMany(l => l.Elements("Bricks").Elements("Brick"));

			foreach (XElement brick in bricks)
			{
				XElement area = brick.Element("DisplayArea");
				if (area == null)
					continue;

				var piece = new Piece
				{
					Part = (string)brick.Element("PartNumber") ?? "",
					Center = new PointF(F(area, "X") + F(area, "Width") / 2, F(area, "Y") + F(area, "Height") / 2)
				};

				foreach (XElement conn in brick.Elements("Connexions").Elements("Connexion"))
				{
					string id = (string)conn.Attribute("id");
					if (!string.IsNullOrEmpty(id))
						ownerOfConnection[id] = piece;

					string linked = ((string)conn.Element("LinkedTo"))?.Trim();
					if (!string.IsNullOrEmpty(linked))
						links.Add((piece, linked));
				}

				pieces.Add(piece);
			}

			foreach (var (piece, linkedTo) in links)
			{
				if (ownerOfConnection.TryGetValue(linkedTo, out Piece other) && other != piece)
				{
					if (!piece.Neighbors.Contains(other)) piece.Neighbors.Add(other);
					if (!other.Neighbors.Contains(piece)) other.Neighbors.Add(piece);
				}
			}

			return pieces;
		}

		private static List<List<Piece>> Components(List<Piece> pieces)
		{
			var seen = new HashSet<Piece>();
			var result = new List<List<Piece>>();
			foreach (Piece start in pieces.Where(p => p.Neighbors.Count > 0))
			{
				if (!seen.Add(start)) continue;
				var comp = new List<Piece>();
				var queue = new Queue<Piece>(new[] { start });
				while (queue.Count > 0)
				{
					Piece p = queue.Dequeue();
					comp.Add(p);
					foreach (Piece q in p.Neighbors)
						if (seen.Add(q)) queue.Enqueue(q);
				}
				result.Add(comp);
			}
			return result;
		}

		//////////////////////////////////////////
		//// Switches and crossings
		//////////////////////////////////////////

		private static PointF Dir(Piece from, Piece to)
		{
			float dx = to.Center.X - from.Center.X, dy = to.Center.Y - from.Center.Y;
			float len = (float)Math.Sqrt(dx * dx + dy * dy);
			return len < 1e-6 ? new PointF(0, 0) : new PointF(dx / len, dy / len);
		}

		private static float Dot(PointF a, PointF b) => a.X * b.X + a.Y * b.Y;

		private static void Classify(Piece p, Result result)
		{
			switch (p.Neighbors.Count)
			{
				case 1:
					p.Kind = Kind.End;
					break;
				case 2:
					p.Kind = Kind.Plain;
					break;
				case 3:
				{
					p.Kind = Kind.Switch;
					result.Switches++;

					// Trunk: the leg pointing most away from the other two
					var dirs = p.Neighbors.Select(n => Dir(p, n)).ToList();
					int trunk = Enumerable.Range(0, 3)
						.OrderBy(i => Enumerable.Range(0, 3).Where(j => j != i).Sum(j => Dot(dirs[i], dirs[j])))
						.First();
					p.Trunk = p.Neighbors[trunk];

					// Left/right as seen by a train coming in from the trunk (screen coords, y down)
					PointF travel = Dir(p.Trunk, p);
					var branches = p.Neighbors.Where(n => n != p.Trunk).ToList();
					float Cross(Piece b) { PointF v = Dir(p, b); return travel.X * v.Y - travel.Y * v.X; }
					var ordered = branches.OrderBy(Cross).ToList(); // most negative = most to the left
					p.Left = ordered[0];
					p.Right = ordered[1];
					break;
				}
				case 4:
				{
					p.Kind = Kind.Crossing;
					result.Crossings++;

					// Pair each leg with the one most opposite to it
					p.Through = new Dictionary<Piece, Piece>();
					var remaining = p.Neighbors.ToList();
					while (remaining.Count >= 2)
					{
						Piece a = remaining[0];
						Piece b = remaining.Skip(1).OrderBy(x => Dot(Dir(p, a), Dir(p, x))).First();
						p.Through[a] = b;
						p.Through[b] = a;
						remaining.Remove(a);
						remaining.Remove(b);
					}
					break;
				}
				default:
					p.Kind = Kind.Junction;
					result.Warnings.Add($"Piece {p.Part} connects to {p.Neighbors.Count} others; treated as the end of a section.");
					break;
			}
		}

		//////////////////////////////////////////
		//// Segments: track between switches
		//////////////////////////////////////////

		private enum Port { None, Trunk, Left, Right }

		private sealed class Segment
		{
			// Pieces in order, including the switch pieces at either end (if any)
			public List<Piece> Path = new List<Piece>();
			public Piece StartSwitch, EndSwitch;
			public Port StartPort, EndPort;
			public bool IsClosedLoop;

			// Orientation: +1 = trains run Path[0] -> Path[^1], -1 = reverse, 0 = unknown
			public int Direction;

			public void Reverse()
			{
				Path.Reverse();
				(StartSwitch, EndSwitch) = (EndSwitch, StartSwitch);
				(StartPort, EndPort) = (EndPort, StartPort);
			}
		}

		private static bool IsStop(Piece p) => p.Kind == Kind.Switch || p.Kind == Kind.Junction;

		private static Port PortOf(Piece sw, Piece neighbor)
		{
			if (sw.Kind != Kind.Switch) return Port.None;
			if (neighbor == sw.Trunk) return Port.Trunk;
			if (neighbor == sw.Left) return Port.Left;
			if (neighbor == sw.Right) return Port.Right;
			return Port.None;
		}

		private static List<Segment> BuildSegments(List<List<Piece>> components)
		{
			var segments = new List<Segment>();
			var walked = new HashSet<(Piece, Piece)>(); // first step out of a stop piece

			foreach (var comp in components)
			{
				// Every leg of every switch starts a segment
				foreach (Piece stop in comp.Where(IsStop))
				{
					foreach (Piece next in stop.Neighbors)
					{
						if (walked.Contains((stop, next)))
							continue;

						Segment seg = Walk(stop, next, fromStop: true);
						walked.Add((stop, next));
						Piece endPiece = seg.Path[seg.Path.Count - 1];
						if (IsStop(endPiece) && seg.Path.Count >= 2)
							walked.Add((endPiece, seg.Path[seg.Path.Count - 2]));
						segments.Add(seg);
					}
				}

				// Track not reachable from a switch: plain lines and loops, including ones that
				// only touch the rest through a crossing. Keep walking until every piece is covered.
				var covered = new HashSet<Piece>(segments.SelectMany(sg => sg.Path));
				while (true)
				{
					Piece start = comp.FirstOrDefault(p => !covered.Contains(p) && p.Kind == Kind.End)
						?? comp.FirstOrDefault(p => !covered.Contains(p) && p.Kind == Kind.Plain);
					if (start == null)
						break;

					Segment seg = Walk(start, start.Neighbors[0], fromStop: false);
					segments.Add(seg);
					covered.UnionWith(seg.Path);
				}
			}

			return segments;
		}

		private static Segment Walk(Piece start, Piece first, bool fromStop)
		{
			var seg = new Segment();
			seg.Path.Add(start);
			if (fromStop)
			{
				seg.StartSwitch = start.Kind == Kind.Switch ? start : null;
				seg.StartPort = PortOf(start, first);
			}

			Piece prev = start, cur = first;
			int steps = 0;
			while (steps++ < 100000)
			{
				seg.Path.Add(cur);

				if (IsStop(cur))
				{
					seg.EndSwitch = cur.Kind == Kind.Switch ? cur : null;
					seg.EndPort = PortOf(cur, prev);
					break;
				}
				if (cur == start)
				{
					// Back where we started without meeting a switch: a plain loop
					seg.IsClosedLoop = true;
					if (seg.Path[seg.Path.Count - 1] == seg.Path[0] && seg.Path.Count > 1)
						seg.Path.RemoveAt(seg.Path.Count - 1);
					break;
				}

				Piece next;
				if (cur.Kind == Kind.Plain)
					next = cur.Neighbors[0] == prev ? cur.Neighbors[1] : cur.Neighbors[0];
				else if (cur.Kind == Kind.Crossing && cur.Through.TryGetValue(prev, out Piece through))
					next = through;
				else
					break; // dead end

				prev = cur;
				cur = next;
			}

			return seg;
		}

		//////////////////////////////////////////
		//// Direction of travel
		//////////////////////////////////////////

		private enum Mode { Unknown, Facing, Trailing }

		private static void Orient(List<Segment> segments, Result result)
		{
			var mode = new Dictionary<Piece, Mode>();
			var bySwitch = new Dictionary<Piece, List<Segment>>();
			foreach (Segment s in segments)
			{
				foreach (Piece sw in new[] { s.StartSwitch, s.EndSwitch })
					if (sw != null)
					{
						if (!bySwitch.TryGetValue(sw, out var list)) bySwitch[sw] = list = new List<Segment>();
						if (!list.Contains(s)) list.Add(s);
					}
			}

			bool conflict = false;
			var queue = new Queue<Piece>();

			// A segment arriving at a switch's trunk makes it facing; arriving on a branch, trailing
			void SetMode(Piece sw, Mode m)
			{
				if (sw == null) return;
				mode.TryGetValue(sw, out Mode current);
				if (current == Mode.Unknown) { mode[sw] = m; queue.Enqueue(sw); }
				else if (current != m) conflict = true;
			}

			void OrientSegment(Segment s, bool arrivesAtEnd)
			{
				if (s.Direction != 0)
				{
					bool already = s.Direction == 1;
					if (already != arrivesAtEnd) conflict = true;
					return;
				}
				if (!arrivesAtEnd)
					s.Reverse();
				s.Direction = 1;

				// After normalising, trains run from Start to End
				SetMode(s.EndSwitch, s.EndPort == Port.Trunk ? Mode.Facing : Mode.Trailing);
				SetMode(s.StartSwitch, s.StartPort == Port.Trunk ? Mode.Trailing : Mode.Facing);
			}

			foreach (Segment seed in segments)
			{
				if (seed.Direction != 0) continue;
				OrientSegment(seed, arrivesAtEnd: true);

				while (queue.Count > 0)
				{
					Piece sw = queue.Dequeue();
					Mode m = mode[sw];
					foreach (Segment s in bySwitch[sw])
					{
						// Which end of s touches this switch, and on which port
						bool atEnd = s.EndSwitch == sw;
						Port port = atEnd ? s.EndPort : s.StartPort;
						if (s.StartSwitch == sw && s.EndSwitch == sw)
						{
							conflict = true; // reversing loop back into the same switch
							continue;
						}

						bool shouldArriveHere = (m == Mode.Facing) == (port == Port.Trunk);
						// arrive here == this end is where trains go
						OrientSegment(s, arrivesAtEnd: shouldArriveHere == atEnd);
					}
				}
			}

			if (conflict)
				result.Warnings.Add("The layout has a reversing loop or a wye, so not every switch could be given a consistent direction. Check the left/right links of the sections around it.");
		}

		//////////////////////////////////////////
		//// Sections
		//////////////////////////////////////////

		private static List<List<PointF>> SplitSegment(Segment seg, int maxPieces)
		{
			var pts = seg.Path.Select(p => p.Center).ToList();
			if (seg.IsClosedLoop)
				pts.Add(pts[0]);

			// Pieces that belong to this section (switch pieces at the ends are shared)
			int inner = Math.Max(1, seg.Path.Count(p => !IsStop(p)));
			int parts = 1;
			if (maxPieces > 0)
				parts = Math.Max(1, (int)Math.Ceiling(inner / (double)maxPieces));
			if (seg.IsClosedLoop)
				parts = Math.Max(2, parts); // anti-collision needs at least two blocks
			parts = Math.Min(parts, Math.Max(1, pts.Count - 1));

			var result = new List<List<PointF>>();
			for (int i = 0; i < parts; i++)
			{
				int from = (int)Math.Round(i * (pts.Count - 1) / (double)parts);
				int to = (int)Math.Round((i + 1) * (pts.Count - 1) / (double)parts);
				result.Add(pts.GetRange(from, to - from + 1));
			}
			return result;
		}

		private static void LinkAtSwitches(List<Segment> segments, Dictionary<Segment, List<Section>> sectionsOf, Result result)
		{
			Section FirstOf(Segment s) => sectionsOf[s][0];
			Section LastOf(Segment s) => sectionsOf[s][sectionsOf[s].Count - 1];

			Segment Departing(Piece sw, Port port) =>
				segments.FirstOrDefault(s => s.StartSwitch == sw && s.StartPort == port && s.EndSwitch != sw);

			foreach (Segment seg in segments.Where(s => s.EndSwitch != null))
			{
				Piece sw = seg.EndSwitch;
				Section last = LastOf(seg);

				if (seg.EndPort == Port.Trunk)
				{
					// Facing switch: choose left or right
					Segment left = Departing(sw, Port.Left), right = Departing(sw, Port.Right);
					if (left != null) last.LeftSection = FirstOf(left);
					if (right != null) last.RightSection = FirstOf(right);
					if (left == null && right != null) { last.LeftSection = last.RightSection; last.RightSection = null; }
				}
				else
				{
					// Trailing switch: merge onto the trunk
					Segment trunk = Departing(sw, Port.Trunk);
					if (trunk != null) last.LeftSection = FirstOf(trunk);
				}
			}
		}

		/// <summary>
		/// Scale BlueBrick stud coordinates into the track map's coordinate space
		/// </summary>
		private static void FitToMap(Dictionary<Section, List<PointF>> rawTrack)
		{
			var all = rawTrack.Values.SelectMany(v => v).ToList();
			if (all.Count == 0)
				return;

			float minX = all.Min(p => p.X), maxX = all.Max(p => p.X);
			float minY = all.Min(p => p.Y), maxY = all.Max(p => p.Y);
			const float margin = 70;
			float scale = Math.Min((MapLayoutSize.Width - 2 * margin) / Math.Max(1, maxX - minX),
			                       (MapLayoutSize.Height - 2 * margin) / Math.Max(1, maxY - minY));
			float ox = margin + ((MapLayoutSize.Width - 2 * margin) - (maxX - minX) * scale) / 2;
			float oy = margin + ((MapLayoutSize.Height - 2 * margin) - (maxY - minY) * scale) / 2;

			foreach (var (s, raw) in rawTrack)
			{
				var pts = raw.Select(p => new PointF(ox + (p.X - minX) * scale, oy + (p.Y - minY) * scale)).ToList();

				s.MapTrack = pts.SelectMany(p => new[] { p.X, p.Y }).ToArray();
				PointF mid = PointAlong(pts, 0.5f);
				s.MapX = mid.X;
				s.MapY = mid.Y;
				s.MapPlaced = true;
			}
		}

		private static PointF PointAlong(List<PointF> pts, float fraction)
		{
			if (pts.Count == 1) return pts[0];
			var lengths = new List<float>();
			float total = 0;
			for (int i = 1; i < pts.Count; i++)
			{
				float l = (float)Math.Sqrt(Math.Pow(pts[i].X - pts[i - 1].X, 2) + Math.Pow(pts[i].Y - pts[i - 1].Y, 2));
				lengths.Add(l);
				total += l;
			}
			float target = total * fraction;
			for (int i = 0; i < lengths.Count; i++)
			{
				if (target <= lengths[i] || i == lengths.Count - 1)
				{
					float t = lengths[i] < 1e-6 ? 0 : Math.Min(1, target / lengths[i]);
					return new PointF(pts[i].X + (pts[i + 1].X - pts[i].X) * t, pts[i].Y + (pts[i + 1].Y - pts[i].Y) * t);
				}
				target -= lengths[i];
			}
			return pts[pts.Count - 1];
		}
	}

	/// <summary>
	/// Coordinate space of the track map (shared by the importer and the map window)
	/// </summary>
	public static class MapLayoutSize
	{
		public const float Width = 1000, Height = 700;
	}
}
