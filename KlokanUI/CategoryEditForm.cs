using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KlokanUI
{
	internal partial class CategoryEditForm : Form
	{
		#region Fields

		/// <summary>
		/// A reference to a category batch being edited in this form
		/// </summary>
		KlokanCategoryBatch categoryBatch;

		/// <summary>
		/// A temporary container for editing category batch answers
		/// </summary>
		bool[,,] correctAnswers;

		/// <summary>
		/// A temporary container for editing category batch filenames
		/// </summary>
		List<string> answerSheetFilenames;

		/// <summary>
		/// Base text displayed as a caption of this form.
		/// Further information is later appended to this base.
		/// </summary>
		string formCaptionBase;

		/// <summary>
		/// Base text displayed as label of the answers sheet list.
		/// Further information is later appended to this base.
		/// </summary>
		string sheetLabelTextBase;

		#endregion

		/// <param name="categoryBatch">
		/// The batch needs to have a name set, so that it can be displayed in this form.
		/// The batch is then going to be edited in this form and changes will be saved if the DialogResult is OK.
		/// </param>
		public CategoryEditForm(KlokanCategoryBatch categoryBatch)
		{
			InitializeComponent();

			formCaptionBase = this.Text;
			sheetLabelTextBase = sheetLabel.Text;

			this.Text = formCaptionBase + " (" + categoryBatch.CategoryName + ")";
			sheetLabel.Text = sheetLabelTextBase + " (0):";

			this.categoryBatch = categoryBatch;

			// show the content of the batch if there is something there already,
			if (categoryBatch.CorrectAnswers != null)
			{
				correctAnswers = categoryBatch.CorrectAnswers;

				FormTableHandling.DrawAnswers(table1PictureBox, correctAnswers, 0, FormTableHandling.DrawCross, Color.Black);
				FormTableHandling.DrawAnswers(table2PictureBox, correctAnswers, 1, FormTableHandling.DrawCross, Color.Black);
				FormTableHandling.DrawAnswers(table3PictureBox, correctAnswers, 2, FormTableHandling.DrawCross, Color.Black);
			}
			// otherwise initialize default values
			else
			{
				correctAnswers = new bool[3, 8, 5];
			}

			if (categoryBatch.SheetFilenames != null && categoryBatch.SheetFilenames.Count > 0)
			{
				answerSheetFilenames = categoryBatch.SheetFilenames;
				answerSheetsListBox.DataSource = answerSheetFilenames;
			}
			else
			{
				answerSheetFilenames = new List<string>();
			}			
		}

		#region UI Functions

		// a file dialog (multiselect) for adding answer sheets to be evaluated
		private void addButton_Click(object sender, EventArgs e)
		{
			bool duplicateFilenames = false;
			
			var dialogResult = openFilesDialog.ShowDialog();
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
					MessageBox.Show(Properties.Resources.InfoTextDuplicateFilenames, Properties.Resources.InfoCaptionDuplicateFilenames, 
						MessageBoxButtons.OK, MessageBoxIcon.Information);
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
			for (int i = 0; i < 3; i++)
			{
				if (!TableArrayHandling.CheckAnswers(correctAnswers, i))
				{
					MessageBox.Show(Properties.Resources.ErrorTextCorrectAnswersNotSelected, Properties.Resources.ErrorCaptionGeneral, 
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			if (answerSheetFilenames.Count == 0)
			{
				MessageBox.Show(Properties.Resources.ErrorTextNoAnswerSheetsSelected, Properties.Resources.ErrorCaptionGeneral, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			categoryBatch.CorrectAnswers = correctAnswers;
			categoryBatch.SheetFilenames = answerSheetFilenames;

			DialogResult = DialogResult.OK;
			this.Close();
		}

		private void table1PictureBox_Click(object sender, EventArgs e)
		{
			FormTableHandling.HandleTableImageClicks(e as MouseEventArgs, table1PictureBox, 0, correctAnswers);
		}

		private void table2PictureBox_Click(object sender, EventArgs e)
		{
			FormTableHandling.HandleTableImageClicks(e as MouseEventArgs, table2PictureBox, 1, correctAnswers);
		}

		private void table3PictureBox_Click(object sender, EventArgs e)
		{
			FormTableHandling.HandleTableImageClicks(e as MouseEventArgs, table3PictureBox, 2, correctAnswers);
		}

		#endregion

		private void UpdateListBox()
		{
			answerSheetsListBox.DataSource = null;
			answerSheetsListBox.DataSource = answerSheetFilenames;

			sheetLabel.Text = sheetLabelTextBase + " (" + answerSheetFilenames.Count + "):";
		}
	}
}
