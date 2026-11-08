using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace LegoTrainProject.Tests
{
	public class BlueBrickImporterTests
	{
		/// <summary>
		/// Builds a .bbm in BlueBrick's save format. Each piece is (name, x, y, linked piece names);
		/// one connector is written per link, paired with the matching connector on the other piece.
		/// </summary>
		private static string Bbm(params (string name, float x, float y, string[] links)[] pieces)
		{
			string Id(string from, string to)
			{
				// Stable GUID per directed connector, like BlueBrick's ids
				var bytes = new byte[16];
				Encoding.UTF8.GetBytes((from + ">" + to).PadRight(16).Substring(0, 16)).CopyTo(bytes, 0);
				return new Guid(bytes).ToString("D");
			}

			var sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?><Map><Version>9</Version><nbItems>").Append(pieces.Length).Append("</nbItems>");
			sb.Append("<Layers><Layer type=\"grid\" id=\"g\"><Name>Grid</Name></Layer>");
			sb.Append("<Layer type=\"brick\" id=\"l\"><Name>Track</Name><Visible>true</Visible><Bricks>");
			foreach (var p in pieces)
			{
				sb.Append($"<Brick id=\"{Guid.NewGuid():D}\"><DisplayArea><X>{p.x - 4}</X><Y>{p.y - 4}</Y><Width>8</Width><Height>8</Height></DisplayArea>");
				sb.Append($"<PartNumber>TRACK.{p.name}</PartNumber><Orientation>0</Orientation><ActiveConnectionPointIndex>0</ActiveConnectionPointIndex><Altitude>0</Altitude>");
				sb.Append($"<Connexions count=\"{p.links.Length}\">");
				foreach (string other in p.links)
					sb.Append($"<Connexion id=\"{Id(p.name, other)}\"><LinkedTo>{Id(other, p.name)}</LinkedTo></Connexion>");
				sb.Append("</Connexions></Brick>");
			}
			sb.Append("</Bricks></Layer></Layers></Map>");
			return sb.ToString();
		}

		private static BlueBrickImporter.Result Import(string bbm) =>
			BlueBrickImporter.Import(new StringReader(bbm));

		/// <summary>
		/// n pieces on a circle, each linked to its neighbours
		/// </summary>
		private static (string, float, float, string[])[] Ring(int n, string prefix = "R")
		{
			return Enumerable.Range(0, n).Select(i =>
			{
				double a = 2 * Math.PI * i / n;
				return ($"{prefix}{i}", (float)(100 * Math.Cos(a)), (float)(100 * Math.Sin(a)),
					new[] { $"{prefix}{(i + n - 1) % n}", $"{prefix}{(i + 1) % n}" });
			}).ToArray();
		}

		[Fact]
		public void Plain_oval_becomes_two_sections_in_a_loop()
		{
			var r = Import(Bbm(Ring(12)));

			Assert.Equal(12, r.Pieces);
			Assert.Equal(0, r.Switches);
			Assert.Equal(2, r.Sections.Count);
			Assert.Same(r.Sections[1], r.Sections[0].LeftSection);
			Assert.Same(r.Sections[0], r.Sections[1].LeftSection);
			Assert.All(r.Sections, s => Assert.Null(s.RightSection));
			Assert.Empty(r.Warnings);
		}

		/// <summary>
		///   E1 - P1 - A - P2 - P3 - B - P4 - E2
		///               \           /
		///                 Q1 --- Q2          (passing siding)
		/// </summary>
		private static string PassingSiding() => Bbm(
			("E1", 0, 0, new[] { "P1" }),
			("P1", 10, 0, new[] { "E1", "A" }),
			("A", 20, 0, new[] { "P1", "P2", "Q1" }),
			("P2", 30, 0, new[] { "A", "P3" }),
			("P3", 40, 0, new[] { "P2", "B" }),
			("B", 50, 0, new[] { "P3", "P4", "Q2" }),
			("P4", 60, 0, new[] { "B", "E2" }),
			("E2", 70, 0, new[] { "P4" }),
			("Q1", 30, 10, new[] { "A", "Q2" }),
			("Q2", 40, 10, new[] { "Q1", "B" }));

		[Fact]
		public void Passing_siding_splits_at_one_switch_and_merges_at_the_other()
		{
			var r = Import(PassingSiding());

			Assert.Equal(2, r.Switches);
			Assert.Equal(4, r.Sections.Count);
			Assert.Empty(r.Warnings);

			// Exactly one section chooses between two ways (the facing switch)
			Section split = Assert.Single(r.Sections, s => s.RightSection != null);
			Assert.NotSame(split.LeftSection, split.RightSection);

			// Both ways lead to the same section after the trailing switch...
			Section merged = split.LeftSection.LeftSection;
			Assert.NotNull(merged);
			Assert.Same(merged, split.RightSection.LeftSection);

			// ...which runs to the dead end, and nothing runs into the first section
			Assert.Null(merged.LeftSection);
			Assert.DoesNotContain(r.Sections, s => s.LeftSection == split || s.RightSection == split);
		}

		[Fact]
		public void Two_lines_crossing_become_two_sections()
		{
			var r = Import(Bbm(
				("W", -20, 0, new[] { "X" }),
				("E", 20, 0, new[] { "X" }),
				("N", 0, -20, new[] { "X" }),
				("S", 0, 20, new[] { "X" }),
				("X", 0, 0, new[] { "W", "E", "N", "S" })));

			Assert.Equal(1, r.Crossings);
			Assert.Equal(0, r.Switches);
			Assert.Equal(2, r.Sections.Count);
			Assert.All(r.Sections, s => Assert.Null(s.RightSection));
		}

		[Fact]
		public void Sections_get_map_positions_and_track_shapes()
		{
			var r = Import(PassingSiding());

			Assert.All(r.Sections, s =>
			{
				Assert.True(s.MapPlaced);
				Assert.NotNull(s.MapTrack);
				Assert.True(s.MapTrack.Length >= 4);
				for (int i = 0; i < s.MapTrack.Length; i += 2)
				{
					Assert.InRange(s.MapTrack[i], 0, MapLayoutSize.Width);
					Assert.InRange(s.MapTrack[i + 1], 0, MapLayoutSize.Height);
				}
			});
		}

		[Fact]
		public void Long_stretches_can_be_split_into_more_sections()
		{
			var r = BlueBrickImporter.Import(new StringReader(Bbm(Ring(24))), maxPiecesPerSection: 6);
			Assert.Equal(4, r.Sections.Count);

			// Still one closed loop
			var s = r.Sections[0];
			for (int i = 0; i < 4; i++) s = s.LeftSection;
			Assert.Same(r.Sections[0], s);
		}

		[Fact]
		public void Separate_networks_are_imported_with_a_warning()
		{
			var r = Import(Bbm(Ring(8, "A").Concat(Ring(8, "B")).ToArray()));
			Assert.Equal(4, r.Sections.Count);
			Assert.Contains(r.Warnings, w => w.Contains("separate track networks"));
		}

		[Fact]
		public void Layout_without_track_gives_a_warning_not_a_crash()
		{
			var r = Import(Bbm(("Lonely", 0, 0, new string[0])));
			Assert.Empty(r.Sections);
			Assert.NotEmpty(r.Warnings);
		}

		[Fact]
		public void Imported_sections_save_and_load()
		{
			var project = new TrainProject();
			project.Sections.sections.AddRange(Import(PassingSiding()).Sections);

			string file = System.IO.Path.Combine(Directory.CreateTempSubdirectory("bap-bb").FullName, "imported.ltp");
			ProjectSerializer.Save(project, file);
			var loaded = ProjectSerializer.Load<TrainProject>(file, out _);

			Assert.Equal(4, loaded.Sections.sections.Count);
			Assert.Equal(project.Sections.sections[0].MapTrack, loaded.Sections.sections[0].MapTrack);
			Section split = Assert.Single(loaded.Sections.sections, s => s.RightSection != null);
			Assert.Same(split.LeftSection.LeftSection, split.RightSection.LeftSection);
		}
	}
}
