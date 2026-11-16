using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LegoTrainProject.Sections_UI;
using System.Windows.Threading;

namespace LegoTrainProject
{
	public partial class SectionsEditor : UserControl
	{
		private TrainProject Project;
		private SectionMapForm mapForm;

		public SectionsEditor(TrainProject project)
		{
			Project = project;
			InitializeComponent();
			AddMapButton();

			RecreateTrainsUI();
			RecreateSectionsUI();

			// Hook up event if any part of the sections change
			Project.Sections.DataUpdated += RefreshTrainProperties;
		}

		public void RefreshUI()
		{
			if (flowLayoutPanelSections.InvokeRequired)
			{
				flowLayoutPanelSections.Invoke(new Sections.RefreshUIThreadSafeDelegate(RefreshUI),
					new object[] { });
			}
			else
			{
				RecreateTrainsUI();
				RefreshSectionProperties();
				UpdateStartButtonStatus();
			}
		}

		private void AddMapButton()
		{
			var mapButton = new ToolStripButton("Track Map", Properties.Resources.icons8_map_pinpoint_96)
			{
				ImageTransparentColor = Color.Magenta,
				ToolTipText = "Show a live map of sections and trains"
			};
			mapButton.Click += (s, e) =>
			{
				if (mapForm == null || mapForm.IsDisposed)
				{
					mapForm = new SectionMapForm(Project);
					mapForm.Show(FindForm());
				}
				else
					mapForm.Activate();
			};
			toolStrip.Items.Add(mapButton);

			var importButton = new ToolStripButton("Import BlueBrick Layout")
			{
				ToolTipText = "Create sections from a BlueBrick (.bbm) track layout"
			};
			importButton.Click += (s, e) => ImportBlueBrick();
			toolStrip.Items.Add(importButton);

			// The map belongs to this project; close it when this editor goes away (e.g. project reloaded)
			Disposed += (s, e) => mapForm?.Close();
		}

		private void ImportBlueBrick()
		{
			if (Project.Sections.IsRunning)
			{
				MessageBox.Show("Stop Self-Driving before importing a layout.", "Import BlueBrick Layout");
				return;
			}

			string file;
			using (var dialog = new OpenFileDialog
			{
				Title = "Import BlueBrick Layout",
				Filter = "BlueBrick layouts (*.bbm)|*.bbm|All files (*.*)|*.*",
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
			})
			{
				if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
					return;
				file = dialog.FileName;
			}

			BlueBrickImporter.Result result;
			try
			{
				result = BlueBrickImporter.Import(file);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Could not read this layout:\n" + ex.Message, "Import BlueBrick Layout", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (result.Sections.Count == 0)
			{
				MessageBox.Show(string.Join("\n", result.Warnings), "Import BlueBrick Layout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (Project.Sections.GetAll().Count > 0 &&
				MessageBox.Show($"This replaces your {Project.Sections.GetAll().Count} existing section(s) and clears all paths. Continue?",
					"Import BlueBrick Layout", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
				return;

			// Paths and train positions refer to the old sections
			Project.Sections.GetAll().Clear();
			Project.Sections.GetAll().AddRange(result.Sections);
			Project.Sections.Paths.Clear();
			foreach (Hub h in Project.RegisteredTrains)
				h.CurrentPath = null;

			RecreateSectionsUI();
			RecreateTrainsUI();
			RefreshTrainProperties();

			string summary = $"Imported {result.Sections.Count} sections from {result.Pieces} track pieces " +
				$"({result.Switches} switches, {result.Crossings} crossings).\n\n" +
				"Next: pick the sensor at the end of each section, set the switch for sections that split, then create paths.";
			if (result.Warnings.Count > 0)
				summary += "\n\nNotes:\n- " + string.Join("\n- ", result.Warnings);
			MessageBox.Show(summary, "Import BlueBrick Layout", MessageBoxButtons.OK, MessageBoxIcon.Information);

			// Show the result
			if (mapForm == null || mapForm.IsDisposed)
			{
				mapForm = new SectionMapForm(Project);
				mapForm.Show(FindForm());
			}
			else
				mapForm.Activate();
		}

		public void RecreateSectionsUI()
		{
			if (mapForm != null && !mapForm.IsDisposed)
				mapForm.SectionsChanged();

			flowLayoutPanelSections.Controls.Clear();
			int totalSections = Project.Sections.GetAll().Count;

			for (int i = 0; i < totalSections; i++)
			{
				Section s = Project.Sections[i];
				SectionControl sc = new SectionControl(Project, s, this, (i == totalSections - 1));
				sc.Width = 147;

				//modified by Tom Cook
				//sc.Height = flowLayoutPanelSections.Height;
				//sc.Height = flowLayoutPanelSections.Height - 21;
				sc.Height = 239;  //Train Driving Module to shrinks the Section control
				flowLayoutPanelSections.Controls.Add(sc);
				flowLayoutPanelSections.SetFlowBreak(sc, true);
			}
		}

		private void RecreateTrainsUI()
		{
			// If we have any previous visual, they need to detach from their hub's event
			foreach (TrainSectionControl tsc in flowLayoutPanelTrains.Controls)
				tsc.ClearTrainEvent();

			// Then we clear them all
			flowLayoutPanelTrains.Controls.Clear();
			int addedTrains = 0;

			// And rebuild them!
			foreach (Hub h in Project.RegisteredTrains)
			{
				h.ClearingTimeInMs = (h.ClearingTimeInMs == 0) ? 2000 : h.ClearingTimeInMs;
				h.SpeedCoefficient = (h.SpeedCoefficient == 0) ? 1.0f : h.SpeedCoefficient;
				h.SpeedWhenAboutToStop = (h.SpeedWhenAboutToStop == 0) ? 40 : h.SpeedWhenAboutToStop;

				foreach (Port p in h.RegistredPorts)
				{
					if (p.Function != Port.Functions.TRAIN_MOTOR)
						continue;

					addedTrains++;

					TrainSectionControl tsc = new TrainSectionControl(Project, this, h, p);

					flowLayoutPanelTrains.Controls.Add(tsc);
					//flowLayoutPanelTrains.SetFlowBreak(tsc, true);
				}
			}

			if (addedTrains == 0)
				labelNoTrains.Show();
			else
				labelNoTrains.Hide();


		}

		internal void RefreshSectionProperties()
		{
			if (flowLayoutPanelSections.InvokeRequired)
			{
				flowLayoutPanelSections.Invoke(new Sections.RefreshUIThreadSafeDelegate(RefreshSectionProperties),
					new object[] { });
			}
			else
			{
				foreach (SectionControl sc in flowLayoutPanelSections.Controls)
				{
					sc.RefreshUI();
				}
			}
		}

		internal void RefreshTrainProperties()
		{
			if (flowLayoutPanelTrains.InvokeRequired)
			{
				flowLayoutPanelTrains.Invoke(new Sections.RefreshUIThreadSafeDelegate(RefreshTrainProperties),
					new object[] { });
			}
			else
			{
				foreach (TrainSectionControl tsc in flowLayoutPanelTrains.Controls)
				{
					tsc.RefreshUI();
				}
			}
		}

		private void UpdateStartButtonStatus()
		{
			if (!Project.Sections.IsRunning)
			{
				buttonStart.Image = Properties.Resources.icons8_start_48;
				buttonStart.Text = "Start Program";
			}
			else
			{
				buttonStart.Image = Properties.Resources.icons8_stop_48;
				buttonStart.Text = "Stop Program";
			}

			foreach (TrainSectionControl tsc in flowLayoutPanelTrains.Controls)
			{
				tsc.comboBoxCurrentSection.Enabled = !Project.Sections.IsRunning;
			}
		}

		private void toolStripButtonAddSection_Click(object sender, EventArgs e)
		{
			Section newSection = new Section()
			{
				Name = "Section " + Project.Sections.GetAll().Count
			};

			Project.Sections.GetAll().Add(newSection);
			RecreateSectionsUI();
			RefreshTrainProperties();
		}

		private async void toolStripButtonStart_Click(object sender, EventArgs e)
		{
			if (Project.Sections.IsRunning)
			{
				Project.Sections.Stop();

				foreach (Hub h in Project.RegisteredTrains)
				{
					h.IsPathProgramRunning = false;

						//modified by Tom Cook to ignore trains with no path selected
						if (h.IsTrain() && h.CurrentPath != null)

							//if (h.IsTrain())
							await h.ClearAutomatedPathAndStop(false);
					h.RestoreLEDColor();
				}
			}
			else
			{
				foreach (Hub h in Project.RegisteredTrains)
				{
					h.IsWaitingSection = false;
					h.AbortReserve = false;
					h.IsPathProgramRunning = false;
					
					if (h.IsConnected && h.IsTrain() && !Project.Sections.IsTrainInNetwork(h))
					{
						MessageBox.Show($"{h.Name} does not have a Section assigned.", "Start Is Aborted");
						return;
					}
					
				}

				foreach (Section s in Project.Sections)
				{
					s.IsBeingCleared = false;
				}

				Project.Sections.Start(Project);
			}

			UpdateStartButtonStatus();
		}

		private void toolStripButtonEditPaths_Click(object sender, EventArgs e)
		{
			PathEditor pathEditor = new PathEditor(Project);
			pathEditor.ShowDialog();

			RefreshTrainProperties();
		}
	}

	public class DetectorItem
	{
		public Hub hub;
		public Port port;

		public DetectorItem(Hub h, Port p)
		{
			hub = h;
			port = p;
		}

		public string GetFullID()
		{
			return port.Id + "*" + hub.DeviceId;
		}

		public override string ToString()
		{
			return hub.Name + " on port " + port.Id;
		}
	}

}
