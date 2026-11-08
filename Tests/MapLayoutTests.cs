using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace LegoTrainProject.Tests
{
	public class MapLayoutTests
	{
		/// <summary>
		/// Four-section loop with a siding off section 1, plus one section nothing links to
		/// </summary>
		private static List<Section> BuildTrack()
		{
			var s = Enumerable.Range(0, 6).Select(i => new Section { Name = "S" + i }).ToList();
			s[0].LeftSection = s[1];
			s[1].LeftSection = s[2];
			s[1].RightSection = s[4]; // siding
			s[2].LeftSection = s[3];
			s[3].LeftSection = s[0];
			// s[5] is not connected to anything
			return s;
		}

		[Fact]
		public void Auto_layout_places_every_section_at_a_distinct_spot()
		{
			var sections = BuildTrack();
			MapLayout.AutoLayout(sections, onlyUnplaced: true);

			Assert.All(sections, s => Assert.True(s.MapPlaced));
			var spots = sections.Select(s => (System.Math.Round(s.MapX), System.Math.Round(s.MapY))).ToList();
			Assert.Equal(spots.Count, spots.Distinct().Count());
		}

		[Fact]
		public void Siding_is_placed_outside_the_main_loop()
		{
			var sections = BuildTrack();
			MapLayout.AutoLayout(sections, onlyUnplaced: true);

			float Dist(Section s) => (float)System.Math.Sqrt(
				System.Math.Pow(s.MapX - MapLayout.Width / 2, 2) + System.Math.Pow(s.MapY - MapLayout.Height / 2, 2));

			Assert.True(Dist(sections[4]) > Dist(sections[1]));
		}

		[Fact]
		public void Only_unplaced_keeps_manual_positions()
		{
			var sections = BuildTrack();
			sections[2].MapPlaced = true;
			sections[2].MapX = 123;
			sections[2].MapY = 456;

			MapLayout.AutoLayout(sections, onlyUnplaced: true);

			Assert.Equal(123, sections[2].MapX);
			Assert.Equal(456, sections[2].MapY);
			Assert.All(sections, s => Assert.True(s.MapPlaced));
		}

		[Fact]
		public void Map_positions_are_saved_with_the_project()
		{
			var project = new TrainProject();
			project.Sections.sections.AddRange(BuildTrack());
			MapLayout.AutoLayout(project.Sections.sections, onlyUnplaced: false);

			string file = System.IO.Path.Combine(Directory.CreateTempSubdirectory("bap-map").FullName, "map.ltp");
			ProjectSerializer.Save(project, file);
			var loaded = ProjectSerializer.Load<TrainProject>(file, out _);

			for (int i = 0; i < project.Sections.sections.Count; i++)
			{
				Assert.True(loaded.Sections.sections[i].MapPlaced);
				Assert.Equal(project.Sections.sections[i].MapX, loaded.Sections.sections[i].MapX);
				Assert.Equal(project.Sections.sections[i].MapY, loaded.Sections.sections[i].MapY);
			}
		}
	}
}
