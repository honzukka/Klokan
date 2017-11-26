using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using System.Threading;

namespace KlokanUI
{
	public partial class TestForm : Form, IEvaluationForm
	{
		/// <summary>
		/// Parameters to be used in the evaluation process.
		/// </summary>
		Parameters chosenParameters;

		public TestForm()
		{
			InitializeComponent();

			viewItemButton.Enabled = false;
			removeItemButton.Enabled = false;

			PopulateDataView();

			chosenParameters = Parameters.CreateDefaultParameters();

			ShowAverageCorrectness();
		}

		#region UI Functions

		private void evaluateButton_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Do you want to start evaluation?", "Evaluation Start", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}

			evaluateButton.Enabled = false;

			List<TestKlokanInstance> testInstances = new List<TestKlokanInstance>();

			// get all available test instances
			using (var testDB = new KlokanTestDBContext())
			{
				var allScansQuery = from scan in testDB.Scans
									select scan;

				foreach (var scan in allScansQuery)
				{
					bool[,,] studentExpectedValues;
					bool[,,] answerExpectedValues;
					TableArrayHandling.DbSetToAnswers(new List<KlokanTestDBExpectedAnswer>(scan.ExpectedValues), out studentExpectedValues, out answerExpectedValues);
					
					TestKlokanInstance testInstance = new TestKlokanInstance {
						ScanId = scan.ScanId,
						Image = scan.Image,
						StudentExpectedValues = studentExpectedValues,
						AnswerExpectedValues = answerExpectedValues
					};

					testInstances.Add(testInstance);
				}
			}

			if (testInstances.Count == 0)
			{
				MessageBox.Show("There are no test items to evaluate.", "No test items", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			TestKlokanBatch testBatch = new TestKlokanBatch {
				Parameters = chosenParameters,
				TestInstances = testInstances
			};

			var jobScheduler = new JobScheduler(testBatch, this);

			// new thread created, so that all tasks in it are planned in the threadpool and not in the WinForms synchronization context
			Thread thread = new Thread(jobScheduler.Run);
			thread.IsBackground = true;
			thread.Start();
		}

		private void addItemButton_Click(object sender, EventArgs e)
		{
			KlokanTestDBScan newScanItem = new KlokanTestDBScan();
			TestItemForm testAddItemForm = new TestItemForm(newScanItem, false);
			testAddItemForm.StartPosition = FormStartPosition.CenterScreen;

			// the new scan item will either be set up and added into the database or not
			testAddItemForm.ShowDialog();

			averageCorrectnessLabel.Hide();
			averageCorrectnessValueLabel.Hide();
			PopulateDataView();
		}

		private void removeItemButton_Click(object sender, EventArgs e)
		{
			if (dataView.SelectedRows.Count == 0)
			{
				MessageBox.Show("No row has been selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// multiselect is set to false for this data view
			var rowToRemove = dataView.SelectedRows[0];
			int rowToRemoveID = (int)(rowToRemove.Cells[0].Value);
			dataView.Rows.Remove(rowToRemove);

			using (var testDB = new KlokanTestDBContext())
			{
				var scanToRemoveQuery = from scan in testDB.Scans
										where scan.ScanId == rowToRemoveID
										select scan;

				var scanToRemove = scanToRemoveQuery.FirstOrDefault();
				if (scanToRemove != default(KlokanTestDBScan))
				{
					// lazy loading is used, so we need to load all the relations of scan if we want Remove() to remove those as well
					var expectedAnswers = scanToRemove.ExpectedValues;
					var computedValues = scanToRemove.ComputedValues;

					testDB.Scans.Remove(scanToRemove);
					testDB.SaveChanges();
				}
			}

			ShowAverageCorrectness();
		}

		private void viewItemButton_Click(object sender, EventArgs e)
		{
			if (dataView.SelectedRows.Count == 0)
			{
				MessageBox.Show("No row has been selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// multiselect is set to false for this data view
			int scanItemId = (int)dataView.SelectedRows[0].Cells[0].Value;

			KlokanTestDBScan scanItemToView = null;

			using (var testDB = new KlokanTestDBContext())
			{
				var scanItemQuery = from scan in testDB.Scans
									where scan.ScanId == scanItemId
									select scan;

				var scanItem = scanItemQuery.FirstOrDefault();

				scanItemToView = new KlokanTestDBScan {
					ScanId = scanItem.ScanId,
					ComputedValues = scanItem.ComputedValues,
					ExpectedValues = scanItem.ExpectedValues,
					Image = scanItem.Image,
					Correctness = scanItem.Correctness
				};
			}

			TestItemForm form = new TestItemForm(scanItemToView, true);
			form.StartPosition = FormStartPosition.CenterScreen;

			// all potential changes to the scan item will be saved into the database if the user chooses to do so
			form.ShowDialog();

			PopulateDataView();
			ShowAverageCorrectness();
		}

		private void editParamsButton_Click(object sender, EventArgs e)
		{
			ParameterEditForm form = new ParameterEditForm(chosenParameters, "Klokan - Test - Parameters");
			form.StartPosition = FormStartPosition.CenterScreen;

			if (form.ShowDialog() == DialogResult.OK)
			{
				chosenParameters = form.Parameters;
			}
		}

		private void dataView_Click(object sender, EventArgs e)
		{
			if (dataView.SelectedRows.Count == 1)
			{
				viewItemButton.Enabled = true;
				removeItemButton.Enabled = true;
			}
			else
			{
				viewItemButton.Enabled = false;
				removeItemButton.Enabled = false;
			}
		}

		#endregion

		#region Helper Functions

		/// <summary>
		/// Loads test items from the database and shows them in the data view.
		/// </summary>
		private void PopulateDataView()
		{
			dataView.Rows.Clear();

			using (var testDB = new KlokanTestDBContext())
			{
				var scanQuery = from scan in testDB.Scans
								select new { scan.ScanId, scan.Correctness };

				foreach (var scanInfo in scanQuery)
				{
					dataView.Rows.Add(scanInfo.ScanId, scanInfo.Correctness);
				}
			}
		}

		/// <summary>
		/// Computes the average correctness of test items based on the data available in the data view 
		/// (so that one database query is saved).
		/// </summary>
		/// <returns>Returns -1 if average correctness cannot be determined.</returns>
		private float GetAverageCorrectness()
		{
			// no test items available
			if (dataView.Rows.Count == 0)
			{
				return -1;
			}

			float correctnessSum = 0;

			foreach (DataGridViewRow row in dataView.Rows)
			{
				float itemCorrectness = (float)(row.Cells[1].Value);

				// correctness data not complete, evaluation needed
				if (itemCorrectness == -1)
				{
					return -1;
				}

				correctnessSum += itemCorrectness;
			}

			float averageCorrectness = correctnessSum / dataView.Rows.Count;

			return averageCorrectness;
		}

		/// <summary>
		/// Uses the GetAverageCorrectness() function to get the value and 
		/// the shows it properly in the appropriate label.
		/// </summary>
		private void ShowAverageCorrectness()
		{
			float averageCorrectness = GetAverageCorrectness();

			// if all test items have been evaluated
			if (averageCorrectness != -1)
			{
				averageCorrectnessValueLabel.Text = GetAverageCorrectness().ToString();

				averageCorrectnessLabel.Show();
				averageCorrectnessValueLabel.Show();
			}
			else
			{
				averageCorrectnessLabel.Hide();
				averageCorrectnessValueLabel.Hide();
			}
		}

		/// <summary>
		/// Enables the evaluation button again.
		/// Can be called from any thread.
		/// </summary>
		public void EnableGoButton()
		{
			if (evaluateButton.InvokeRequired)
			{
				evaluateButton.BeginInvoke(new Action(
					() => {
						PopulateDataView();
						ShowAverageCorrectness();
						evaluateButton.Enabled = true;
					}
				));
			}
			else
			{
				PopulateDataView();
				ShowAverageCorrectness();
				evaluateButton.Enabled = true;
			}
		}

		/// <summary>
		/// Shows a custom informative message.
		/// Can be called from any thread.
		/// </summary>
		public void ShowMessageBoxInfo(string message, string caption)
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new Action(
					() => {
						MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				));
			}
			else
			{
				MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		#endregion
	}
}
