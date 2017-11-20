using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Imaging;

namespace KlokanUI
{
	public partial class TestItemForm : Form
	{
		KlokanTestDBScan scanItem;
		string scanFilePath;
		bool viewMode;
		bool addMode;

		bool[,,] chosenValuesStudentTable;
		bool[,,] chosenValuesAnswerTable;

		bool[,,] chosenValuesStudentTableTemp;
		bool[,,] chosenValuesAnswerTableTemp;

		public TestItemForm(KlokanTestDBScan scanItem, bool viewMode)
		{
			InitializeComponent();

			filePathLabel.Text = "";
			updateButton.Enabled = false;

			this.scanItem = scanItem;
			scanFilePath = null;
			this.viewMode = viewMode;
			addMode = !viewMode;

			// this array is effectively two-dimensional; this is a trick to help make the code cleaner and faster
			// (I want to work with multi-dimensional arrays as opposed to jagged arrays)
			chosenValuesStudentTable = new bool[1, 5, 10];
			chosenValuesAnswerTable = new bool[3, 8, 5];

			chosenValuesStudentTableTemp = new bool[1, 5, 10];
			chosenValuesAnswerTableTemp = new bool[3, 8, 5];

			if (viewMode)
			{
				chooseFileButton.Enabled = false;
				applyButton.Enabled = false;
				discardButton.Enabled = false;
				PopulateForm();
			}
			else
			{
				editButton.Hide();
				discardButton.Hide();
			}
		}

		private void chooseFileButton_Click(object sender, EventArgs e)
		{
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				scanFilePath = openFileDialog.FileName;

				filePathLabel.Text = scanFilePath;

				Image scanImage = new Bitmap(scanFilePath);
				scanPictureBox.Image = scanImage;
			}
		}

		private void studentTablePictureBox_Click(object sender, EventArgs e)
		{
			if (!viewMode)
			{
				HelperFunctions.HandleTableImageClicks(e as MouseEventArgs, studentTablePictureBox, 0, chosenValuesStudentTableTemp);
			}
		}

		private void answerTable1PictureBox_Click(object sender, EventArgs e)
		{
			if (!viewMode)
			{
				HelperFunctions.HandleTableImageClicks(e as MouseEventArgs, answerTable1PictureBox, 0, chosenValuesAnswerTableTemp);
			}
		}

		private void answerTable2PictureBox_Click(object sender, EventArgs e)
		{
			if (!viewMode)
			{
				HelperFunctions.HandleTableImageClicks(e as MouseEventArgs, answerTable2PictureBox, 1, chosenValuesAnswerTableTemp);
			}
		}

		private void answerTable3PictureBox_Click(object sender, EventArgs e)
		{
			if (!viewMode)
			{
				HelperFunctions.HandleTableImageClicks(e as MouseEventArgs, answerTable3PictureBox, 2, chosenValuesAnswerTableTemp);
			}
		}

		private void editButton_Click(object sender, EventArgs e)
		{
			viewMode = false;
			applyButton.Enabled = true;
			discardButton.Enabled = true;
			updateButton.Enabled = false;
			editButton.Enabled = false;

			ResetTableImages();

			// draw only the expected answers because only those can be edited
			HelperFunctions.DrawAnswers(studentTablePictureBox, chosenValuesStudentTable, 0, HelperFunctions.DrawCross, Color.Black);
			HelperFunctions.DrawAnswers(answerTable1PictureBox, chosenValuesAnswerTable, 0, HelperFunctions.DrawCross, Color.Black);
			HelperFunctions.DrawAnswers(answerTable2PictureBox, chosenValuesAnswerTable, 1, HelperFunctions.DrawCross, Color.Black);
			HelperFunctions.DrawAnswers(answerTable3PictureBox, chosenValuesAnswerTable, 2, HelperFunctions.DrawCross, Color.Black);

			// create a copy of currently chosen answers for editing
			chosenValuesStudentTableTemp = new bool[1, 5, 10];
			Array.Copy(chosenValuesStudentTable, chosenValuesStudentTableTemp, 1 * 5 * 10);

			chosenValuesAnswerTableTemp = new bool[3, 8, 5];
			Array.Copy(chosenValuesAnswerTable, chosenValuesAnswerTableTemp, 3 * 8 * 5);
		}

		private void applyButton_Click(object sender, EventArgs e)
		{
			if (addMode && (scanFilePath == null || scanFilePath == ""))
			{
				MessageBox.Show("No file has been selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!HelperFunctions.CheckAnswers(chosenValuesStudentTableTemp, 0))
			{
				MessageBox.Show("Student number has not been properly selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			for (int i = 0; i < 3; i++)
			{
				if (!HelperFunctions.CheckAnswers(chosenValuesAnswerTableTemp, i))
				{
					MessageBox.Show("Expected answers have not been properly selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			chosenValuesStudentTable = chosenValuesStudentTableTemp;
			chosenValuesAnswerTable = chosenValuesAnswerTableTemp;

			// draw the computed answers for comparison
			bool[,,] computedValuesStudentTable;
			bool[,,] computedValuesAnswerTable;

			List<KlokanTestDBComputedAnswer> computedValues = new List<KlokanTestDBComputedAnswer>(scanItem.ComputedValues);
			HelperFunctions.DbSetToAnswers(computedValues, out computedValuesStudentTable, out computedValuesAnswerTable);

			HelperFunctions.DrawAnswers(studentTablePictureBox, computedValuesStudentTable, 0, HelperFunctions.DrawCircle, Color.Red);
			HelperFunctions.DrawAnswers(answerTable1PictureBox, computedValuesAnswerTable, 0, HelperFunctions.DrawCircle, Color.Red);
			HelperFunctions.DrawAnswers(answerTable2PictureBox, computedValuesAnswerTable, 1, HelperFunctions.DrawCircle, Color.Red);
			HelperFunctions.DrawAnswers(answerTable3PictureBox, computedValuesAnswerTable, 2, HelperFunctions.DrawCircle, Color.Red);

			updateButton.Enabled = true;
			editButton.Enabled = true;
			applyButton.Enabled = false;
			discardButton.Enabled = false;
			viewMode = true;
		}

		private void discardButton_Click(object sender, EventArgs e)
		{
			ResetTableImages();
			
			// draw the original answers
			HelperFunctions.DrawAnswers(studentTablePictureBox, chosenValuesStudentTable, 0, HelperFunctions.DrawCross, Color.Black);
			HelperFunctions.DrawAnswers(answerTable1PictureBox, chosenValuesAnswerTable, 0, HelperFunctions.DrawCross, Color.Black);
			HelperFunctions.DrawAnswers(answerTable2PictureBox, chosenValuesAnswerTable, 1, HelperFunctions.DrawCross, Color.Black);
			HelperFunctions.DrawAnswers(answerTable3PictureBox, chosenValuesAnswerTable, 2, HelperFunctions.DrawCross, Color.Black);

			bool[,,] computedValuesStudentTable;
			bool[,,] computedValuesAnswerTable;

			List<KlokanTestDBComputedAnswer> computedValues = new List<KlokanTestDBComputedAnswer>(scanItem.ComputedValues);
			HelperFunctions.DbSetToAnswers(computedValues, out computedValuesStudentTable, out computedValuesAnswerTable);

			HelperFunctions.DrawAnswers(studentTablePictureBox, computedValuesStudentTable, 0, HelperFunctions.DrawCircle, Color.Red);
			HelperFunctions.DrawAnswers(answerTable1PictureBox, computedValuesAnswerTable, 0, HelperFunctions.DrawCircle, Color.Red);
			HelperFunctions.DrawAnswers(answerTable2PictureBox, computedValuesAnswerTable, 1, HelperFunctions.DrawCircle, Color.Red);
			HelperFunctions.DrawAnswers(answerTable3PictureBox, computedValuesAnswerTable, 2, HelperFunctions.DrawCircle, Color.Red);

			editButton.Enabled = true;
			applyButton.Enabled = false;
			discardButton.Enabled = false;
			viewMode = true;
		}

		private void updateButton_Click(object sender, EventArgs e)
		{
			var dialogResult = MessageBox.Show("Are you sure you want to update the database?", "Database Change", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (dialogResult == DialogResult.No)
			{
				return;
			}

			updateButton.Enabled = false;

			// prepare the DbSet of chosen expected answers
			List<KlokanTestDBExpectedAnswer> expectedAnswers = new List<KlokanTestDBExpectedAnswer>();
			expectedAnswers.AddRange(HelperFunctions.AnswersToDbSet<KlokanTestDBExpectedAnswer>(chosenValuesStudentTable, 0, true));
			for (int i = 0; i < 3; i++)
			{
				expectedAnswers.AddRange(HelperFunctions.AnswersToDbSet<KlokanTestDBExpectedAnswer>(chosenValuesAnswerTable, i, false));
			}

			if (addMode)
			{
				scanItem.Image = HelperFunctions.GetImageBytes(scanFilePath, ImageFormat.Png);
			}

			scanItem.ExpectedValues = expectedAnswers;
			scanItem.Correctness = -1;      // correctness will only have a valid value once the evaluation is run

			using (var testDB = new KlokanTestDBContext())
			{
				// when editing an item we first have to delete the old one
				if (!addMode)
				{
					var oldScanItemQuery = from scan in testDB.Scans
										   where scan.ScanId == scanItem.ScanId
										   select scan;

					var oldExpectedAnswers = from answer in testDB.ExpectedValues
											 where answer.ScanId == scanItem.ScanId
											 select answer;

					// delete the old expected answers
					foreach (var answer in oldExpectedAnswers)
					{
						testDB.ExpectedValues.Remove(answer);
					}

					// assign new expected answers
					var oldScanItem = oldScanItemQuery.FirstOrDefault();
					oldScanItem.ExpectedValues = scanItem.ExpectedValues;
					oldScanItem.Correctness = -1;
				}
				else
				{
					KlokanTestDBScan newScanItem = new KlokanTestDBScan
					{
						ExpectedValues = scanItem.ExpectedValues,
						ComputedValues = scanItem.ComputedValues,
						Image = scanItem.Image,
						Correctness = -1
					};

					testDB.Scans.Add(newScanItem);
				}
				
				testDB.SaveChanges();
			}

			if (addMode)
			{
				this.Close();
			}

			PopulateForm();
		}

		private void PopulateForm()
		{
			// load scan
			scanPictureBox.Image = HelperFunctions.GetBitmap(scanItem.Image);

			// load the answers
			List<KlokanTestDBExpectedAnswer> expectedValues = new List<KlokanTestDBExpectedAnswer>(scanItem.ExpectedValues);
			HelperFunctions.DbSetToAnswers(expectedValues, out chosenValuesStudentTable, out chosenValuesAnswerTable);

			HelperFunctions.DrawAnswers(studentTablePictureBox, chosenValuesStudentTable, 0, HelperFunctions.DrawCross, Color.Black);
			HelperFunctions.DrawAnswers(answerTable1PictureBox, chosenValuesAnswerTable, 0, HelperFunctions.DrawCross, Color.Black);
			HelperFunctions.DrawAnswers(answerTable2PictureBox, chosenValuesAnswerTable, 1, HelperFunctions.DrawCross, Color.Black);
			HelperFunctions.DrawAnswers(answerTable3PictureBox, chosenValuesAnswerTable, 2, HelperFunctions.DrawCross, Color.Black);

			bool[,,] computedValuesStudentTable;
			bool[,,] computedValuesAnswerTable;

			List<KlokanTestDBComputedAnswer> computedValues = new List<KlokanTestDBComputedAnswer>(scanItem.ComputedValues);
			HelperFunctions.DbSetToAnswers(computedValues, out computedValuesStudentTable, out computedValuesAnswerTable);

			HelperFunctions.DrawAnswers(studentTablePictureBox, computedValuesStudentTable, 0, HelperFunctions.DrawCircle, Color.Red);
			HelperFunctions.DrawAnswers(answerTable1PictureBox, computedValuesAnswerTable, 0, HelperFunctions.DrawCircle, Color.Red);
			HelperFunctions.DrawAnswers(answerTable2PictureBox, computedValuesAnswerTable, 1, HelperFunctions.DrawCircle, Color.Red);
			HelperFunctions.DrawAnswers(answerTable3PictureBox, computedValuesAnswerTable, 2, HelperFunctions.DrawCircle, Color.Red);
		}

		private void ResetTableImages()
		{
			List<Image> oldImages = new List<Image>();
			oldImages.Add(studentTablePictureBox.Image);
			oldImages.Add(answerTable1PictureBox.Image);
			oldImages.Add(answerTable2PictureBox.Image);
			oldImages.Add(answerTable3PictureBox.Image);

			foreach (var oldImage in oldImages)
			{
				if (oldImage != null) oldImage.Dispose();
			}

			studentTablePictureBox.Image = Properties.Resources.studentTableImage;
			answerTable1PictureBox.Image = Properties.Resources.answerTable1Image;
			answerTable2PictureBox.Image = Properties.Resources.answerTable2Image;
			answerTable3PictureBox.Image = Properties.Resources.answerTable3Image;

			studentTablePictureBox.Refresh();
			answerTable1PictureBox.Refresh();
			answerTable2PictureBox.Refresh();
			answerTable3PictureBox.Refresh();
		}
	}
}
