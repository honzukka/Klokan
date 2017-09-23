using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KlokanUI
{
	internal partial class CategoryEditForm : Form
	{
		// temporary containers for configuration data
		bool[,,] correctAnswers;
		List<string> answerSheetFilenames;

		// a reference to the batch that's being edited in this form
		KlokanCategoryBatch categoryBatch;

		/// <param name="categoryBatch">
		/// The batch needs to have a name set, so that it can be displayed in this form.
		/// The batch is then going to be edited in this form and changes will be saved if the DialogResult is OK.
		/// </param>
		public CategoryEditForm(KlokanCategoryBatch categoryBatch)
		{
			InitializeComponent();

			this.Text = "Klokan - Category Edit (" + categoryBatch.CategoryName + ")";
			this.categoryBatch = categoryBatch;

			// show the content of the batch if there is something there already,
			// otherwise initialize default values
			if (categoryBatch.CorrectAnswers != null)
			{
				correctAnswers = categoryBatch.CorrectAnswers;
				AnswerDrawing.DrawAnswers(table1PictureBox, table2PictureBox, table3PictureBox, correctAnswers, AnswerDrawing.DrawCross, Color.Black);
			}
			else
			{
				correctAnswers = new bool[3, 8, 5];
			}

			if (categoryBatch.SheetFilenames != null)
			{
				answerSheetFilenames = categoryBatch.SheetFilenames;
				answerSheetsListBox.DataSource = answerSheetFilenames;
			}
			else
			{
				answerSheetFilenames = new List<string>();
			}			
		}

		// a file dialog (multiselect) for adding answer sheets to be evaluated
		private void addButton_Click(object sender, EventArgs e)
		{
			var dialogResult = openFilesDialog.ShowDialog();
			bool duplicateFilenames = false;

			if (dialogResult == DialogResult.OK)
			{
				// for each filename that was chosen in the dialog
				foreach (var filename in openFilesDialog.FileNames)
				{
					// add the filename only if it's not there already 
					// (it doesn't generally make sense to process a sheet twice, so we can assume that such an action would be an error of the user)
					if (!answerSheetFilenames.Contains(filename))
					{
						answerSheetFilenames.Add(filename);
					}
					else
					{
						duplicateFilenames = true;
					}
				}

				UpdateListBox();

				// inform the user that duplicate filenames were not added
				if (duplicateFilenames)
				{
					MessageBox.Show("One or more files were already present in the list, so they were not added.", "Duplicate Filenames", MessageBoxButtons.OK, MessageBoxIcon.Information);
					duplicateFilenames = false;
				}
			}
		}

		// remove selected items from the list box
		private void removeButton_Click(object sender, EventArgs e)
		{
			// if something is selected
			if (answerSheetsListBox.SelectedIndices.Count > 0)
			{
				foreach (string item in answerSheetsListBox.SelectedItems)
				{
					answerSheetFilenames.Remove(item);
				}

				UpdateListBox();
			}
		}

		// save the batch and close the form
		private void saveButton_Click(object sender, EventArgs e)
		{
			if (!CheckCorrectAnswers())
			{
				MessageBox.Show("Correct answers have not been properly selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (answerSheetFilenames.Count == 0)
			{
				MessageBox.Show("No answer sheets selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			categoryBatch.CorrectAnswers = correctAnswers;
			categoryBatch.SheetFilenames = answerSheetFilenames;

			DialogResult = DialogResult.OK;
			this.Close();
		}

		private void UpdateListBox()
		{
			answerSheetsListBox.DataSource = null;
			answerSheetsListBox.DataSource = answerSheetFilenames;

			sheetLabel.Text = "Answers Sheets (" + answerSheetFilenames.Count + "):";
		}

		private void table1PictureBox_Click(object sender, EventArgs e)
		{
			AnswerSelection.HandleTableImageClicks(e as MouseEventArgs, table1PictureBox, 0, correctAnswers);
		}

		private void table2PictureBox_Click(object sender, EventArgs e)
		{
			AnswerSelection.HandleTableImageClicks(e as MouseEventArgs, table2PictureBox, 1, correctAnswers);
		}

		private void table3PictureBox_Click(object sender, EventArgs e)
		{
			AnswerSelection.HandleTableImageClicks(e as MouseEventArgs, table3PictureBox, 2, correctAnswers);
		}

		// checks whether an answer is selected in each row 
		// (there can't be two or more answers selected thanks to the implementation of HandlePictureBoxClicks() )
		private bool CheckCorrectAnswers()
		{
			for (int table = 0; table < 3; table++)
			{
				for (int row = 0; row < 8; row++)
				{
					bool rowContainsAnswer = false;

					for (int col = 0; col < 5; col++)
					{
						if (correctAnswers[table, row, col] == true)
						{
							rowContainsAnswer = true;
							break;
						}
					}

					if (!rowContainsAnswer)
					{
						return false;
					}
				}
			}

			return true;
		}
	}
}
