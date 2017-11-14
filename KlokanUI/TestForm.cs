using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

namespace KlokanUI
{
	public partial class TestForm : Form, IEvaluationForm
	{
		Parameters chosenParameters;

		public TestForm()
		{
			InitializeComponent();

			PopulateDataView();

			chosenParameters = Parameters.CreateDefaultParameters();
		}

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

		public void EnableGoButton()
		{
			if (evaluateButton.InvokeRequired)
			{
				evaluateButton.BeginInvoke(new Action(
					() => { evaluateButton.Enabled = true; }
				));
			}
			else
			{
				evaluateButton.Enabled = true;
			}
		}

		public void ShowMessageBoxInfo(string message, string caption)
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new Action(
					() => {
						MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
						PopulateDataView();
					}
				));
			}
			else
			{
				MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
				PopulateDataView();
			}
		}

		private void evaluateButton_Click(object sender, EventArgs e)
		{
			evaluateButton.Enabled = false;

			List<TestKlokanInstance> testInstances = new List<TestKlokanInstance>();

			using (var testDB = new KlokanTestDBContext())
			{
				var allScansQuery = from scan in testDB.Scans
									select scan;

				foreach (var scan in allScansQuery)
				{
					bool[,,] studentExpectedValues;
					bool[,,] answerExpectedValues;
					HelperFunctions.DbSetToAnswers(new List<KlokanTestDBExpectedAnswer>(scan.ExpectedValues), out studentExpectedValues, out answerExpectedValues);
					
					// TODO: make this variable :D
					TestKlokanInstance testInstance = new TestKlokanInstance {
						ScanId = scan.ScanId,
						SheetFilename = "C:/Users/Honza/source/repos/Klokan/scans/sheet1.jpeg",
						StudentExpectedValues = studentExpectedValues,
						AnswerExpectedValues = answerExpectedValues
					};

					testInstances.Add(testInstance);
				}
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
			testAddItemForm.ShowDialog();

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
			form.ShowDialog();

			PopulateDataView();
		}

		private void editParamsButton_Click(object sender, EventArgs e)
		{
			ParameterEditForm form = new ParameterEditForm(chosenParameters);
			form.StartPosition = FormStartPosition.CenterScreen;

			if (form.ShowDialog() == DialogResult.OK)
			{
				chosenParameters = form.Parameters;
			}
		}
	}
}
