using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using LegoTrainProject.LTP.Core.Devices;
using Xunit;

namespace LegoTrainProject.Tests
{
	public class ProjectSerializerTests : IDisposable
	{
		private readonly string dir = Directory.CreateTempSubdirectory("bap-tests").FullName;

		public void Dispose() => Directory.Delete(dir, recursive: true);

		private static readonly System.Reflection.FieldInfo ProjectField =
			typeof(Hub).GetField("Project", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

		private static void SetProject(Hub hub, TrainProject project) => ProjectField.SetValue(hub, project);
		private static TrainProject GetProject(Hub hub) => (TrainProject)ProjectField.GetValue(hub);

		/// <summary>
		/// A small but representative project: two hub types, a looping two-section
		/// track with a train in it, a path, and a program.
		/// </summary>
		private static TrainProject BuildProject()
		{
			var project = new TrainProject();

			// Hubs are normally built from a live Bluetooth device; create them bare like the loader does
			var sbrick = (SbrickHub)RuntimeHelpers.GetUninitializedObject(typeof(SbrickHub));
			SetProject(sbrick, project);
			sbrick.Name = "Freight";
			sbrick.DeviceId = "SB-1";
			sbrick.BluetoothAddress = 0xDEADBEEF0001UL;
			sbrick.Type = Hub.Types.SBRICK;
			sbrick.TrainMotorPort = "A";
			sbrick.SpeedCoefficient = 0.8f;
			sbrick.RegistredPorts = new List<Port>
			{
				new Port("A", 0, true) { Function = Port.Functions.MOTOR, Inverted = true, MUcolor = 2, MUbackcolor = Color.LightGreen, Speed = 42 },
				new Port("B", 1, true) { MUbackcolor = Color.FromArgb(10, 20, 30) }
			};

			var pu = (Hub)RuntimeHelpers.GetUninitializedObject(typeof(Hub));
			SetProject(pu, project);
			pu.Name = "Detector hub";
			pu.DeviceId = "PU-1";
			pu.Type = Hub.Types.POWERED_UP_HUB;
			pu.RegistredPorts = new List<Port> { new Port("A", 0, true) };

			project.RegisteredTrains.Add(sbrick);
			project.RegisteredTrains.Add(pu);

			var s1 = new Section { Name = "Station", Detector = "A*PU-1", MaxSpeed = 40 };
			var s2 = new Section { Name = "Back loop", NeedsTwoSectionReleased = true };
			s1.LeftSection = s2;
			s2.LeftSection = s1;
			s1.CurrentHub = sbrick;
			project.Sections.sections.Add(s1);
			project.Sections.sections.Add(s2);

			var path = new Path { Name = "Loop", Sections = new[] { 0, 1 } };
			project.Sections.Paths.Add(path);
			sbrick.CurrentPath = path;

			var program = new TrainProgram { Name = "Lights at night" };
			program.Events.Add(new TrainProgramEvent(TrainProgramEvent.EventType.User_Triggerd) { Name = "Go" });
			project.Programs.Add(program);

			return project;
		}

		private static void AssertSameProject(TrainProject loaded)
		{
			Assert.Equal(2, loaded.RegisteredTrains.Count);

			// Subclass survives
			var sbrick = Assert.IsType<SbrickHub>(loaded.RegisteredTrains[0]);
			Assert.IsType<Hub>(loaded.RegisteredTrains[1]);

			Assert.Equal("Freight", sbrick.Name);
			Assert.Equal(0xDEADBEEF0001UL, sbrick.BluetoothAddress);
			Assert.Equal(Hub.Types.SBRICK, sbrick.Type);
			Assert.Equal(0.8f, sbrick.SpeedCoefficient);

			Port a = sbrick.RegistredPorts[0];
			Assert.Equal(Port.Functions.MOTOR, a.Function);
			Assert.True(a.Inverted);
			Assert.Equal(2, a.MUcolor);
			Assert.Equal(Color.LightGreen.ToArgb(), a.MUbackcolor.ToArgb());
			Assert.Equal(0, a.Speed); // [NonSerialized] runtime state is not saved
			Assert.Equal(Color.FromArgb(10, 20, 30).ToArgb(), sbrick.RegistredPorts[1].MUbackcolor.ToArgb());

			// Shared references and cycles are the same objects, not copies
			Section s1 = loaded.Sections.sections[0];
			Section s2 = loaded.Sections.sections[1];
			Assert.Equal("Station", s1.Name); // auto-property backing field
			Assert.Equal(40, s1.MaxSpeed);
			Assert.True(s2.NeedsTwoSectionReleased);
			Assert.Same(s2, s1.LeftSection);
			Assert.Same(s1, s2.LeftSection);
			Assert.Same(sbrick, s1.CurrentHub);
			Assert.Same(loaded, GetProject(sbrick)); // private field
			Assert.Same(loaded.Sections.Paths[0], sbrick.CurrentPath);
			Assert.Equal(new[] { 0, 1 }, sbrick.CurrentPath.Sections);

			Assert.Equal("Lights at night", loaded.Programs[0].Name);
			Assert.Equal("Go", loaded.Programs[0].Events[0].Name);
		}

		[Fact]
		public void Project_round_trips_through_json()
		{
			string file = System.IO.Path.Combine(dir, "layout.ltp");
			ProjectSerializer.Save(BuildProject(), file);

			Assert.StartsWith("{", File.ReadAllText(file).TrimStart());

			TrainProject loaded = ProjectSerializer.Load<TrainProject>(file, out bool legacy);
			Assert.False(legacy);
			AssertSameProject(loaded);
		}

		[Fact]
		public void Saved_json_can_be_saved_again_identically()
		{
			string first = System.IO.Path.Combine(dir, "a.ltp");
			string second = System.IO.Path.Combine(dir, "b.ltp");
			ProjectSerializer.Save(BuildProject(), first);
			ProjectSerializer.Save(ProjectSerializer.Load<TrainProject>(first, out _), second);

			Assert.Equal(File.ReadAllText(first), File.ReadAllText(second));
		}

#pragma warning disable SYSLIB0011
		[Fact]
		public void Old_binary_project_still_opens()
		{
			string file = System.IO.Path.Combine(dir, "old.ltp");
			using (var stream = File.Create(file))
				new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Serialize(stream, BuildProject());

			TrainProject loaded = ProjectSerializer.Load<TrainProject>(file, out bool legacy);
			Assert.True(legacy);
			AssertSameProject(loaded);
		}
#pragma warning restore SYSLIB0011

		[Fact]
		public void Connection_settings_round_trip()
		{
			string file = System.IO.Path.Combine(dir, "connectionSettings.bap");
			ProjectSerializer.Save(new ConnectionLimitationSettings(), file);
			Assert.NotNull(ProjectSerializer.Load<ConnectionLimitationSettings>(file, out bool legacy));
			Assert.False(legacy);
		}

		[Fact]
		public void File_from_a_newer_version_is_refused_with_a_clear_message()
		{
			string file = System.IO.Path.Combine(dir, "future.ltp");
			File.WriteAllText(file, "{\"Format\":\"BAP\",\"Version\":99,\"Type\":\"TrainProject\",\"Data\":null}");

			var ex = Assert.Throws<InvalidDataException>(() => ProjectSerializer.Load<TrainProject>(file, out _));
			Assert.Contains("newer version", ex.Message);
		}
	}
}
