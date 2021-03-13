using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RJ_Editor
{
	public partial class Form1 : Form
	{
		private string filePath = "";
		GraphForm graphForm;
		public Form1()
		{

			InitializeComponent();
			timetableView.AutoGenerateColumns = false;
			this.trainset.SelectedIndex = 0;
			this.postFrom.SelectedIndex = 0;
			this.postTo.SelectedIndex = 0;
			this.priority.SelectedIndex = 6;
			this.trainTypeSWDR.SelectedIndex = 0;
			this.timetableTypeSWDR.SelectedIndex = 0;
			this.tbInfoStation.SelectedIndex = 1;
			graphForm = new GraphForm();


		}
		

		private void colorPicker_Click(object sender, EventArgs e)
		{
			ColorDialog MyDialog = new ColorDialog();
			MyDialog.Color = extractColor.BackColor;
			if (MyDialog.ShowDialog() == DialogResult.OK)
				extractColor.BackColor = MyDialog.Color;
		}

		private void debug_TextChanged(object sender, EventArgs e)
		{

		}

		private void otwórzToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				using (OpenFileDialog openFileDialog = new OpenFileDialog())
				{
					openFileDialog.InitialDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location;
					openFileDialog.Filter = "Plik rozkładu ISDR (*.roz)|*.roz";
					openFileDialog.FilterIndex = 1;
					openFileDialog.RestoreDirectory = true;

					if (openFileDialog.ShowDialog() == DialogResult.OK)
					{
						this.Text = "Edytor Rozkładu Jazdy dla ISDR :: Pracuję";
						filePath = openFileDialog.FileName;
						ParserFromFile.Parse(openFileDialog.FileName);
					}
					this.Text = "Edytor Rozkładu Jazdy dla ISDR";
					timetableView.DataSource = null;
					timetableView.DataSource = Logic_Parser.Trains;
					timetableView.Refresh();
					tbInfoStation.Text = Logic_Parser.Timetable.StationName;
					tbInfoStartDate.Text = Logic_Parser.Timetable.StartDate;
					tbInfoAuthor.Text = Logic_Parser.Timetable.Author;
					tbInfoDescription.Text = Logic_Parser.Timetable.FileDescription;
				}
			}
			catch (ArgumentException ex)
			{
				MessageBox.Show(ex.Message, "Syntax error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			catch(IOException ex)
            {
				MessageBox.Show(ex.Message, "Open file error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

		}

		private void zapiszJakoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				SetTimetableProperties();
			}
			catch (ArgumentException ex)
			{
				MessageBox.Show(ex.Message, "Syntax error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			try
			{
				ValidationBeforeSave();
			}
			catch (ArgumentNullException ex)
			{
				MessageBox.Show(ex.Message, "Syntax error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			SaveFileDialog saveFileDialog = new SaveFileDialog();

			//saveFileDialog.InitialDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location;
			saveFileDialog.Filter = "Plik rozkładu ISDR (*.roz)|*.roz";
			saveFileDialog.Title = "Zapis do pliku...";
			saveFileDialog.FilterIndex = 1;
			saveFileDialog.RestoreDirectory = true;
			saveFileDialog.ShowDialog();
			if (saveFileDialog.FileName != "")
			{
				try
                {
					WriterToFile.Write(saveFileDialog.FileName);
				}
				catch(IOException ex)
                {
					MessageBox.Show(ex.Message, "Write to file error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}

		}
		private void ValidationBeforeSave()
		{
			Logic_Parser.FindPreviousInCycle();
		}

		private void zapiszToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (filePath != "")
				WriterToFile.Write(filePath);
			else
				zapiszJakoToolStripMenuItem_Click(sender, e);
		}

		private void bDeleteTrain_Click(object sender, EventArgs e)
		{
			if (timetableView.SelectedRows.Count > 0)
			{
				Logic_Parser.Trains.RemoveAt(timetableView.SelectedRows[0].Index);
				timetableView.DataSource = null;
				timetableView.DataSource = Logic_Parser.Trains;
				timetableView.Refresh();
			}

		}

		private void bSortByTime_Click(object sender, EventArgs e)
		{
			// sort List by thisArrival nullable field, if there object has no value, get thisDeparture nullable field to compare
			// but Parser must check that thisArrival or thisDeparture has value
			try
			{
				Logic_Parser.SortByTime();
				timetableView.Refresh();
			}
			catch (InvalidOperationException ex)
			{
				MessageBox.Show(ex.Message, "Sorting problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

			}


		}

		private void timetableView_CellClick(object sender, DataGridViewCellEventArgs e)
		{

		}

		private void timetableView_SelectionChanged(object sender, EventArgs e)
		{
			GetTrainProperties(timetableView.CurrentCell.RowIndex);

		}
		private void SetTimetableProperties()
		{
			Logic_Parser.Timetable.Author = tbInfoAuthor.Text;
			Logic_Parser.Timetable.FileDescription = tbInfoDescription.Text;
			Logic_Parser.Timetable.StartDate = tbInfoStartDate.Text;
			Logic_Parser.Timetable.StationName = tbInfoStation.SelectedItem.ToString();
		}
		private void GetTrainProperties(int index)
		{
			if (index >= Logic_Parser.Trains.Count || index < 0)
				return;
			relationFrom.Text = Logic_Parser.Trains.ElementAt(index).RelationFrom;
			relationTo.Text = Logic_Parser.Trains.ElementAt(index).RelationTo;
			if (Logic_Parser.Trains.ElementAt(index).FromPost != null)
			{
				postFrom.Text = Logic_Parser.Trains.ElementAt(index).FromPost;
			}
			else
				postFrom.SelectedIndex = 0;
			if (Logic_Parser.Trains.ElementAt(index).ToPost != null)
				postTo.Text = Logic_Parser.Trains.ElementAt(index).ToPost;
			else
				postFrom.SelectedIndex = 0;
			type3L.Text = Logic_Parser.Trains.ElementAt(index).Type3L;
			carrier.Text = Logic_Parser.Trains.ElementAt(index).Carieer;
			number.Text = Logic_Parser.Trains.ElementAt(index).TrainNumber.ToString();
			trainName.Text = Logic_Parser.Trains.ElementAt(index).TrainName;
			IsExtractView.Checked = Logic_Parser.Trains.ElementAt(index).IsExtractView;
			IsSWDRQuality.Checked = Logic_Parser.Trains.ElementAt(index).IsSWDRQuality;
			IsSWDRView.Checked = Logic_Parser.Trains.ElementAt(index).IsSWDRView;

			extractColor.BackColor = ColorTranslator.FromWin32((int)Logic_Parser.Trains.ElementAt(index).ColorExtract);
			timetableView.CurrentRow.Cells[0].Style.SelectionBackColor = ColorTranslator.FromWin32((int)Logic_Parser.Trains.ElementAt(index).ColorExtract);
			timetableView.CurrentRow.Cells[0].Style.SelectionForeColor = Color.White;

			trackExit.Text = Logic_Parser.Trains.ElementAt(index).ExitTracks;
			timeExit.Text = Logic_Parser.Trains.ElementAt(index).ExitTime.HasValue ? Logic_Parser.Trains.ElementAt(index).ExitTime.Value.ToString() : "";
			departureNb.Text = Logic_Parser.Trains.ElementAt(index).PreviousDeparture.HasValue ? Logic_Parser.Trains.ElementAt(index).PreviousDeparture.Value.ToString() : "";
			departureSt.Text = Logic_Parser.Trains.ElementAt(index).ThisDeparture.HasValue ? Logic_Parser.Trains.ElementAt(index).ThisDeparture.Value.ToString() : "";
			arrivalSt.Text = Logic_Parser.Trains.ElementAt(index).ThisArrival.HasValue ? Logic_Parser.Trains.ElementAt(index).ThisArrival.Value.ToString() : "";
			arrivalNb.Text = Logic_Parser.Trains.ElementAt(index).NextArrival.HasValue ? Logic_Parser.Trains.ElementAt(index).NextArrival.Value.ToString() : "";
			exitSpeed.Text = Logic_Parser.Trains.ElementAt(index).ExitVelocity.ToString();
			stopOrdered.Text = Logic_Parser.Trains.ElementAt(index).OrderedStop.HasValue ? Logic_Parser.Trains.ElementAt(index).OrderedStop.Value.TotalMinutes.ToString().Replace('.', ',') : "";
			stopType.Text = Logic_Parser.Trains.ElementAt(index).StopTypeSWDR;
			stopTrack.Text = Logic_Parser.Trains.ElementAt(index).StationTrackNumber;
			runningExit.Text = Logic_Parser.Trains.ElementAt(index).ExitDays;
			runningExtract.Text = Logic_Parser.Trains.ElementAt(index).ExtractDays;
			priority.SelectedIndex = Logic_Parser.Trains.ElementAt(index).Priority;
			switch (Logic_Parser.Trains.ElementAt(index).IdentW4)
			{
				case "1":
					stopLocation.SelectedIndex = 1;
					break;
				case "2":
					stopLocation.SelectedIndex = 2;
					break;
				case "1,2":
					stopLocation.SelectedIndex = 3;
					break;
				default:
					stopLocation.SelectedIndex = 0;
					break;
			}
			departureSignal.Text = Logic_Parser.Trains.ElementAt(index).DepartureSignal;
			nextInputSignal.Text = Logic_Parser.Trains.ElementAt(index).NextInputSignal;
			nextInCycle.Text = Logic_Parser.Trains.ElementAt(index).NextInCycle.HasValue ? Logic_Parser.Trains.ElementAt(index).NextInCycle.Value.ToString() : "";
			changeNumberAuto.Checked = Logic_Parser.Trains.ElementAt(index).ChangeNumberAuto;
			previousInCycle.Text = Logic_Parser.Trains.ElementAt(index).PreviousInCycle.HasValue ? Logic_Parser.Trains.ElementAt(index).PreviousInCycle.Value.ToString() : "";
			waitForPreviousInCycle.Text = Logic_Parser.Trains.ElementAt(index).WaitForPreviousInCycle.HasValue ? Logic_Parser.Trains.ElementAt(index).WaitForPreviousInCycle.Value.ToString() : "";
			trainset.Text = Logic_Parser.Trains.ElementAt(index).TrainSetString;
			exitProbability.Text = Logic_Parser.Trains.ElementAt(index).ExitProbability.ToString();
			beforeProbability.Text = Logic_Parser.Trains.ElementAt(index).BeforeProbability.HasValue ? Logic_Parser.Trains.ElementAt(index).BeforeProbability.Value.ToString() : "";
			delayProbability.Text = Logic_Parser.Trains.ElementAt(index).DelayProbability.HasValue ? Logic_Parser.Trains.ElementAt(index).DelayProbability.Value.ToString() : "";
			maxBefore.Text = Logic_Parser.Trains.ElementAt(index).MaxBefore.HasValue ? Logic_Parser.Trains.ElementAt(index).MaxBefore.Value.ToString() : "";
			maxDelay.Text = Logic_Parser.Trains.ElementAt(index).MaxDelay.HasValue ? Logic_Parser.Trains.ElementAt(index).MaxDelay.Value.ToString() : "";
			additionalInfoSWDR.Text = Logic_Parser.Trains.ElementAt(index).AdditionalInfoSWDR;
			operatingNotesSWDR.Text = Logic_Parser.Trains.ElementAt(index).OperatingNotesSWDR;
			loadSWDR.Text = Logic_Parser.Trains.ElementAt(index).LoadSWDR;
			phoneDescription.Text = Logic_Parser.Trains.ElementAt(index).PhoneDescription;
			trainTypeSWDR.Text = Logic_Parser.Trains.ElementAt(index).TrainTypeSWDR;
			timetableTypeSWDR.Text = Logic_Parser.Trains.ElementAt(index).TimetableTypeSWDR;


		}

		private void SetTrainProperties(int index)
		{
			Logic_Parser.Trains.ElementAt(index).RelationFrom = relationFrom.Text;
			Logic_Parser.Trains.ElementAt(index).RelationTo = relationTo.Text;
			Logic_Parser.Trains.ElementAt(index).FromPost = postFrom.SelectedItem.ToString();
			Logic_Parser.Trains.ElementAt(index).ToPost = postTo.SelectedItem.ToString();
			Logic_Parser.Trains.ElementAt(index).Type3L = type3L.Text;
			Logic_Parser.Trains.ElementAt(index).Carieer = carrier.Text;
			Logic_Parser.Trains.ElementAt(index).TrainNumber = Convert.ToUInt32(number.Text);
			Logic_Parser.Trains.ElementAt(index).TrainName = trainName.Text;
			Logic_Parser.Trains.ElementAt(index).IsExtractView = IsExtractView.Checked;
			Logic_Parser.Trains.ElementAt(index).IsSWDRQuality = IsSWDRQuality.Checked;
			Logic_Parser.Trains.ElementAt(index).IsSWDRView = IsSWDRView.Checked;

			Logic_Parser.Trains.ElementAt(index).ColorExtract = (uint)ColorTranslator.ToWin32(extractColor.BackColor);

			Logic_Parser.Trains.ElementAt(index).ExitTracks = trackExit.Text;

			_ = timeExit.MaskFull == false ? Logic_Parser.Trains.ElementAt(index).ExitTime = null : Logic_Parser.Trains.ElementAt(index).ExitTime = TimeSpan.ParseExact(timeExit.Text, "g", CultureInfo.CurrentCulture, TimeSpanStyles.None);
			_ = departureNb.MaskFull == false ? Logic_Parser.Trains.ElementAt(index).PreviousDeparture = null : Logic_Parser.Trains.ElementAt(index).PreviousDeparture = TimeSpan.ParseExact(departureNb.Text, "g", CultureInfo.CurrentCulture, TimeSpanStyles.None);
			_ = departureSt.MaskFull == false ? Logic_Parser.Trains.ElementAt(index).ThisDeparture = null : Logic_Parser.Trains.ElementAt(index).ThisDeparture = TimeSpan.ParseExact(departureSt.Text, "g", CultureInfo.CurrentCulture, TimeSpanStyles.None);
			_ = arrivalSt.MaskFull == false ? Logic_Parser.Trains.ElementAt(index).ThisArrival = null : Logic_Parser.Trains.ElementAt(index).ThisArrival = TimeSpan.ParseExact(arrivalSt.Text, "g", CultureInfo.CurrentCulture, TimeSpanStyles.None);
			_ = arrivalNb.MaskFull == false ? Logic_Parser.Trains.ElementAt(index).NextArrival = null : Logic_Parser.Trains.ElementAt(index).NextArrival = TimeSpan.ParseExact(arrivalNb.Text, "g", CultureInfo.CurrentCulture, TimeSpanStyles.None);
			_ = exitSpeed.Text == "" ? Logic_Parser.Trains.ElementAt(index).ExitVelocity = null : Logic_Parser.Trains.ElementAt(index).ExitVelocity = Convert.ToUInt16(exitSpeed.Text);

			NumberFormatInfo provider = new NumberFormatInfo();
			provider.NumberDecimalSeparator = ".";
			_ = stopOrdered.Text == "" ? Logic_Parser.Trains.ElementAt(index).OrderedStop = null : Logic_Parser.Trains.ElementAt(index).OrderedStop = TimeSpan.FromMinutes(Convert.ToDouble(stopOrdered.Text.Replace(',', '.'), provider));
			Logic_Parser.Trains.ElementAt(index).StopTypeSWDR = stopType.Text;
			Logic_Parser.Trains.ElementAt(index).StationTrackNumber = stopTrack.Text;
			Logic_Parser.Trains.ElementAt(index).ExitDays = runningExit.Text;
			Logic_Parser.Trains.ElementAt(index).ExtractDays = runningExtract.Text;
			Logic_Parser.Trains.ElementAt(index).Priority = (ushort)priority.SelectedIndex;

			switch (stopLocation.SelectedIndex)
			{
				case 0:
					Logic_Parser.Trains.ElementAt(index).IdentW4 = "";
					break;
				case 1:
					Logic_Parser.Trains.ElementAt(index).IdentW4 = "1";
					break;
				case 2:
					Logic_Parser.Trains.ElementAt(index).IdentW4 = "2";
					break;
				case 3:
					Logic_Parser.Trains.ElementAt(index).IdentW4 = "1,2";
					break;
				default:
					Logic_Parser.Trains.ElementAt(index).IdentW4 = "";
					break;
			}

			Logic_Parser.Trains.ElementAt(index).DepartureSignal = departureSignal.Text;
			Logic_Parser.Trains.ElementAt(index).NextInputSignal = nextInputSignal.Text;
			_ = nextInCycle.Text == "" ? Logic_Parser.Trains.ElementAt(index).NextInCycle = null : Logic_Parser.Trains.ElementAt(index).NextInCycle = Convert.ToUInt32(nextInCycle.Text);
			Logic_Parser.Trains.ElementAt(index).ChangeNumberAuto = changeNumberAuto.Checked;
			_ = previousInCycle.Text == "" ? Logic_Parser.Trains.ElementAt(index).PreviousInCycle = null : Logic_Parser.Trains.ElementAt(index).PreviousInCycle = Convert.ToUInt32(previousInCycle.Text);
			_ = waitForPreviousInCycle.Text == "" ? Logic_Parser.Trains.ElementAt(index).WaitForPreviousInCycle = null : Logic_Parser.Trains.ElementAt(index).WaitForPreviousInCycle = Convert.ToUInt16(waitForPreviousInCycle.Text);
			Logic_Parser.Trains.ElementAt(index).TrainSetString = trainset.Text;
			Logic_Parser.Trains.ElementAt(index).ExitProbability = Convert.ToInt16(exitProbability.Text);
			_ = beforeProbability.Text == "" ? Logic_Parser.Trains.ElementAt(index).BeforeProbability = null : Logic_Parser.Trains.ElementAt(index).BeforeProbability = Convert.ToUInt16(beforeProbability.Text);
			_ = delayProbability.Text == "" ? Logic_Parser.Trains.ElementAt(index).DelayProbability = null : Logic_Parser.Trains.ElementAt(index).DelayProbability = Convert.ToUInt16(delayProbability.Text);
			_ = maxBefore.Text == "" ? Logic_Parser.Trains.ElementAt(index).MaxBefore = null : Logic_Parser.Trains.ElementAt(index).MaxBefore = Convert.ToUInt16(maxBefore.Text);
			_ = maxDelay.Text == "" ? Logic_Parser.Trains.ElementAt(index).MaxDelay = null : Logic_Parser.Trains.ElementAt(index).MaxDelay = Convert.ToUInt16(maxDelay.Text);
			Logic_Parser.Trains.ElementAt(index).AdditionalInfoSWDR = additionalInfoSWDR.Text;
			Logic_Parser.Trains.ElementAt(index).OperatingNotesSWDR = operatingNotesSWDR.Text;
			Logic_Parser.Trains.ElementAt(index).LoadSWDR = loadSWDR.Text;
			Logic_Parser.Trains.ElementAt(index).PhoneDescription = phoneDescription.Text;
			Logic_Parser.Trains.ElementAt(index).TimetableTypeSWDR = timetableTypeSWDR.SelectedItem.ToString();
			Logic_Parser.Trains.ElementAt(index).TrainTypeSWDR = trainTypeSWDR.SelectedItem.ToString();
		}

		private void label34_Click(object sender, EventArgs e)
		{

		}

		private void bUpdateTrain_Click(object sender, EventArgs e)
		{
			if (timetableView.SelectedRows.Count > 0)
			{
				SetTrainProperties(timetableView.CurrentCell.RowIndex);
				timetableView.Refresh();
			}


		}

		private void bAddTrain_Click(object sender, EventArgs e)
		{
			if (timetableView.SelectedRows.Count > 0)
			{
				AddTrain(timetableView.CurrentCell.RowIndex + 1);
				timetableView.DataSource = null;
				timetableView.DataSource = Logic_Parser.Trains;
				timetableView.Refresh();
			}
			else
			{
				AddTrain(Logic_Parser.Trains.Count);
				timetableView.DataSource = null;
				timetableView.DataSource = Logic_Parser.Trains;
				timetableView.Refresh();
			}
		}
		private void AddTrain(int index)
		{
			Logic_Parser.Trains.Insert(index, new Train());
			SetTrainProperties(index);
		}

		private void bPlusTime_Click(object sender, EventArgs e)
		{
			try
			{
				Logic_Parser.Trains.ElementAt(timetableView.CurrentCell.RowIndex).TimeInterval(Convert.ToDouble(intervalMinutes.Text));
				GetTrainProperties(timetableView.CurrentCell.RowIndex);
				timetableView.Refresh();
			}
			catch (Exception)
			{
				intervalMinutes.Text = "1";
			}

		}

		private void bMinusMinutes_Click(object sender, EventArgs e)
		{
			try
			{
				Logic_Parser.Trains.ElementAt(timetableView.CurrentCell.RowIndex).TimeInterval(-1 * Convert.ToDouble(intervalMinutes.Text));
				GetTrainProperties(timetableView.CurrentCell.RowIndex);
				timetableView.Refresh();
			}
			catch (Exception)
			{
				intervalMinutes.Text = "1";
			}
		}

		private void label35_Click(object sender, EventArgs e)
		{

		}

		private void departureSignal_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void bFind_Click(object sender, EventArgs e)
		{
			try
			{
				timetableView.Rows[Logic_Parser.Trains.FindIndex(x => x.TrainNumber == Convert.ToInt32(tbFindNumber.Text))].Selected = true;
			}
			catch (Exception)
			{
				tbFindNumber.Text = "brak";
			}
		}

		private void tbDuplicateCount_TextChanged(object sender, EventArgs e)
		{

		}

		private void btDuplicate_Click(object sender, EventArgs e)
		{
			int count = Convert.ToInt32(tbDuplicateCount.Text);
			int firstIndex = timetableView.CurrentCell.RowIndex;
			for (int i = 0; i < count; i++)
			{
				Logic_Parser.Trains.Insert(firstIndex + i + 1, Logic_Parser.DuplicateAndInterval(Convert.ToDouble(tbInterval.Text), Logic_Parser.Trains.ElementAt(firstIndex + i)));
			}

			timetableView.DataSource = null;
			timetableView.DataSource = Logic_Parser.Trains;
			timetableView.Refresh();
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{

		}

		private void number_Validating(object sender, CancelEventArgs e)
		{
			string int32 = "Invalid int32 value";
			string stringEmpty = "Empty";
			// genius
			_ = int.TryParse(number.Text, out int t1) ? Good(number) : Bad(e, int32, number);
			if (previousInCycle.Text != "")
			{
				_ = int.TryParse(previousInCycle.Text, out int t2) ? Good(previousInCycle) : Bad(e, int32, previousInCycle);
				_ = int.TryParse(waitForPreviousInCycle.Text, out int t3) ? Good(waitForPreviousInCycle) : Bad(e, int32, waitForPreviousInCycle);
			}
			else
			{
				Good(previousInCycle);
				Good(waitForPreviousInCycle);
			}

			if (nextInCycle.Text != "")
				_ = int.TryParse(nextInCycle.Text, out int t2) ? Good(nextInCycle) : Bad(e, int32, nextInCycle);
			else
				Good(nextInCycle);
			_ = runningExit.Text != "" ? Good(runningExit) : Bad(e, stringEmpty, runningExit);


			bool Bad(CancelEventArgs ex, string info, Control name)
			{
				e.Cancel = true;
				number.Focus();
				epNumber.SetError(name, info);
				bAddTrain.Enabled = false;
				bUpdateTrain.Enabled = false;
				inputError.Visible = true;
				return false;
			}
			bool Good(Control name)
			{

				bAddTrain.Enabled = true;
				bUpdateTrain.Enabled = true;
				e.Cancel = false;
				epNumber.SetError(name, "");
				inputError.Visible = false;
				return true;
			}

		}

		private void carrier_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void bGraph_Click(object sender, EventArgs e)
		{

			if (graphForm.Visible)
				graphForm.BringToFront();
			else
			{
				graphForm = new GraphForm();
				graphForm.Show();
			}



		}
	}
}
