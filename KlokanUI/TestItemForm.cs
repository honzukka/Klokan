using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using System.Drawing.Imaging;
using System.Data.Entity.Infrastructure;

namespace KlokanUI
{
	public partial class TestItemForm : Form
	{
		#region Fields

		/// <summary>
		/// A test item in its DbSet form which is shown and can be edited in this form.
		/// </summary>
		KlokanTestDBScan scanItem;

		/// <summary>
		/// Filename of the answer sheet image which is to be added to a scan item.
		/// </summary>
		string scanFilePath;

		/// <summary>
		/// Is this form in a view mode? (= item data cannot be edited)
		/// </summary>
		bool viewMode;

		/// <summary>
		/// Is this form in an add mode? (= a new answer sheet image can be added to a scan item)
		/// </summary>
		bool addMode;

		/// <summary>
		/// A set of answers (values) which are expected in the student number table in a scan item.
		/// </summary>
		bool[,,] expectedValuesStudentTable;

		/// <summary>
		/// A set of answers (value) which are expected in the answer table in a scan item.
		/// </summary>
		bool[,,] expectedValuesAnswerTable;

		/// <summary>
		/// A temporary set of answers (value) which are expected in the student number table.
		/// Result of changes made in this form.
		/// </summary>
		bool[,,] expectedValuesStudentTableTemp;

		/// <summary>
		/// A temporary set of answers (values) which are expected in the answer table.
		/// Result of changes made in this form.
		/// </summary>
		bool[,,] expectedValuesAnswerTableTemp;

		#endregion

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
			expectedValuesStudentTable = new bool[1, 5, 10];
			expectedValuesAnswerTable = new bool[3, 8, 5];

			expectedValuesStudentTableTemp = new bool[1, 5, 10];
			expectedValuesAnswerTableTemp = new bool[3, 8, 5];

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

		#region UI Functions

		private void chooseFileButton_Click(object sender, EventArgs e)
		{
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				scanFilePath = openFileDialog.FileName;

				filePathLabel.Text = scanFilePath;

				Image scanImage = new Bitmap(scanFilePath);

				Image oldImage = scanPictureBox.Image;
				if (oldImage != null)
				{
					oldImage.Dispose();
				}

				scanPictureBox.Image = scanImage;
			}
		}

		private void studentTablePictureBox_Click(object sender, EventArgs e)
		{
			if (!viewMode)
			{
				FormTableHandling.HandleTableImageClicks(e as MouseEventArgs, studentTablePictureBox, 0, expectedValuesStudentTableTemp);
			}
		}

		private void answerTable1PictureBox_Click(object sender, EventArgs e)
		{
			if (!viewMode)
			{
				FormTableHandling.HandleTableImageClicks(e as MouseEventArgs, answerTable1PictureBox, 0, expectedValuesAnswerTableTemp);
			}
		}

		private void answerTable2PictureBox_Click(object sender, EventArgs e)
		{
			if (!viewMode)
			{
				FormTableHandling.HandleTableImageClicks(e as MouseEventArgs, answerTable2PictureBox, 1, expectedValuesAnswerTableTemp);
			}
		}

		private void answerTable3PictureBox_Click(object sender, EventArgs e)
		{
			if (!viewMode)
			{
				FormTableHandling.HandleTableImageClicks(e as MouseEventArgs, answerTable3PictureBox, 2, expectedValuesAnswerTableTemp);
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
			FormTableHandling.DrawAnswers(studentTablePictureBox, expectedValuesStudentTable, 0, FormTableHandling.DrawCross, Color.Black);
			FormTableHandling.DrawAnswers(answerTable1PictureBox, expectedValuesAnswerTable, 0, FormTableHandling.DrawCross, Color.Black);
			FormTableHandling.DrawAnswers(answerTable2PictureBox, expectedValuesAnswerTable, 1, FormTableHandling.DrawCross, Color.Black);
			FormTableHandling.DrawAnswers(answerTable3PictureBox, expectedValuesAnswerTable, 2, FormTableHandling.DrawCross, Color.Black);

			// create a copy of currently chosen answers for editing
			expectedValuesStudentTableTemp = new bool[1, 5, 10];
			Array.Copy(expectedValuesStudentTable, expectedValuesStudentTableTemp, 1 * 5 * 10);

			expectedValuesAnswerTableTemp = new bool[3, 8, 5];
			Array.Copy(expectedValuesAnswerTable, expectedValuesAnswerTableTemp, 3 * 8 * 5);
		}

		private void applyButton_Click(object sender, EventArgs e)
		{
			if (addMode && (scanFilePath == null || scanFilePath == ""))
			{
				MessageBox.Show(Properties.Resources.ErrorTextNoFileSelected, Properties.Resources.ErrorCaptionGeneral,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!TableArrayHandling.CheckAnswers(expectedValuesStudentTableTemp, 0))
			{
				MessageBox.Show(Properties.Resources.ErrorTextStudentNumberNotSelected, Properties.Resources.ErrorCaptionGeneral,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			for (int i = 0; i < 3; i++)
			{
				if (!TableArrayHandling.CheckAnswers(expectedValuesAnswerTableTemp, i))
				{
					MessageBox.Show(Properties.Resources.ErrorTextExpectedAnswersNotSelected, Properties.Resources.ErrorCaptionGeneral,
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			// apply the changes
			expectedValuesStudentTable = expectedValuesStudentTableTemp;
			expectedValuesAnswerTable = expectedValuesAnswerTableTemp;

			// draw the computed answers for comparison (if there are any)
			bool[,,] computedValuesStudentTable;
			bool[,,] computedValuesAnswerTable;

			List<KlokanTestDBComputedAnswer> computedValues = new List<KlokanTestDBComputedAnswer>(scanItem.ComputedValues);
			TableArrayHandling.DbSetToAnswers(computedValues, out computedValuesStudentTable, out computedValuesAnswerTable);

			FormTableHandling.DrawAnswers(studentTablePictureBox, computedValuesStudentTable, 0, FormTableHandling.DrawCircle, Color.Red);
			FormTableHandling.DrawAnswers(answerTable1PictureBox, computedValuesAnswerTable, 0, FormTableHandling.DrawCircle, Color.Red);
			FormTableHandling.DrawAnswers(answerTable2PictureBox, computedValuesAnswerTable, 1, FormTableHandling.DrawCircle, Color.Red);
			FormTableHandling.DrawAnswers(answerTable3PictureBox, computedValuesAnswerTable, 2, FormTableHandling.DrawCircle, Color.Red);

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
			FormTableHandling.DrawAnswers(studentTablePictureBox, expectedValuesStudentTable, 0, FormTableHandling.DrawCross, Color.Black);
			FormTableHandling.DrawAnswers(answerTable1PictureBox, expectedValuesAnswerTable, 0, FormTableHandling.DrawCross, Color.Black);
			FormTableHandling.DrawAnswers(answerTable2PictureBox, expectedValuesAnswerTable, 1, FormTableHandling.DrawCross, Color.Black);
			FormTableHandling.DrawAnswers(answerTable3PictureBox, expectedValuesAnswerTable, 2, FormTableHandling.DrawCross, Color.Black);

			bool[,,] computedValuesStudentTable;
			bool[,,] computedValuesAnswerTable;

			List<KlokanTestDBComputedAnswer> computedValues = new List<KlokanTestDBComputedAnswer>(scanItem.ComputedValues);
			TableArrayHandling.DbSetToAnswers(computedValues, out computedValuesStudentTable, out computedValuesAnswerTable);

			FormTableHandling.DrawAnswers(studentTablePictureBox, computedValuesStudentTable, 0, FormTableHandling.DrawCircle, Color.Red);
			FormTableHandling.DrawAnswers(answerTable1PictureBox, computedValuesAnswerTable, 0, FormTableHandling.DrawCircle, Color.Red);
			FormTableHandling.DrawAnswers(answerTable2PictureBox, computedValuesAnswerTable, 1, FormTableHandling.DrawCircle, Color.Red);
			FormTableHandling.DrawAnswers(answerTable3PictureBox, computedValuesAnswerTable, 2, FormTableHandling.DrawCircle, Color.Red);

			editButton.Enabled = true;
			applyButton.Enabled = false;
			discardButton.Enabled = false;
			viewMode = true;
		}

		private void updateButton_Click(object sender, EventArgs e)
		{
			var dialogResult = MessageBox.Show(Properties.Resources.PromptTextDatabaseUpdate, Properties.Resources.PromptCaptionDatabaseUpdate,
				MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (dialogResult == DialogResult.No)
			{
				return;
			}

			updateButton.Enabled = false;

			// prepare the DbSet of chosen expected answers
			List<KlokanTestDBExpectedAnswer> expectedAnswers = new List<KlokanTestDBExpectedAnswer>();
			expectedAnswers.AddRange(TableArrayHandling.AnswersToDbSet<KlokanTestDBExpectedAnswer>(expectedValuesStudentTable, 0, true));
			for (int i = 0; i < 3; i++)
			{
				expectedAnswers.AddRange(TableArrayHandling.AnswersToDbSet<KlokanTestDBExpectedAnswer>(expectedValuesAnswerTable, i, false));
			}

			if (addMode)
			{
				scanItem.Image = ImageHandling.GetImageBytes(scanFilePath, ImageFormat.Png);
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

				try
				{
					testDB.SaveChanges();

					MessageBox.Show(Properties.Resources.InfoTextDatabaseUpdated, Properties.Resources.InfoCaptionDatabaseUpdated, 
						MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch (Exception ex) when (ex is DbUpdateException || ex is DbUpdateConcurrencyException)
				{
					MessageBox.Show(Properties.Resources.ErrorTextDatabase, Properties.Resources.ErrorCaptionGeneral, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}

			PopulateForm();
		}

		#endregion

		#region Helper Functions

		/// <summary>
		/// Gets data from the scan item field and shows it.
		/// </summary>
		private void PopulateForm()
		{
			// load scan
			scanPictureBox.Image = ImageHandling.GetBitmap(scanItem.Image);

			// load the answers
			List<KlokanTestDBExpectedAnswer> expectedValues = new List<KlokanTestDBExpectedAnswer>(scanItem.ExpectedValues);
			TableArrayHandling.DbSetToAnswers(expectedValues, out expectedValuesStudentTable, out expectedValuesAnswerTable);

			FormTableHandling.DrawAnswers(studentTablePictureBox, expectedValuesStudentTable, 0, FormTableHandling.DrawCross, Color.Black);
			FormTableHandling.DrawAnswers(answerTable1PictureBox, expectedValuesAnswerTable, 0, FormTableHandling.DrawCross, Color.Black);
			FormTableHandling.DrawAnswers(answerTable2PictureBox, expectedValuesAnswerTable, 1, FormTableHandling.DrawCross, Color.Black);
			FormTableHandling.DrawAnswers(answerTable3PictureBox, expectedValuesAnswerTable, 2, FormTableHandling.DrawCross, Color.Black);

			bool[,,] computedValuesStudentTable;
			bool[,,] computedValuesAnswerTable;

			List<KlokanTestDBComputedAnswer> computedValues = new List<KlokanTestDBComputedAnswer>(scanItem.ComputedValues);
			TableArrayHandling.DbSetToAnswers(computedValues, out computedValuesStudentTable, out computedValuesAnswerTable);

			FormTableHandling.DrawAnswers(studentTablePictureBox, computedValuesStudentTable, 0, FormTableHandling.DrawCircle, Color.Red);
			FormTableHandling.DrawAnswers(answerTable1PictureBox, computedValuesAnswerTable, 0, FormTableHandling.DrawCircle, Color.Red);
			FormTableHandling.DrawAnswers(answerTable2PictureBox, computedValuesAnswerTable, 1, FormTableHandling.DrawCircle, Color.Red);
			FormTableHandling.DrawAnswers(answerTable3PictureBox, computedValuesAnswerTable, 2, FormTableHandling.DrawCircle, Color.Red);
		}

		/// <summary>
		/// Resets all picture boxes in this form which get rid of all answer drawings that had been made.
		/// </summary>
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

		#endregion
	}
}
