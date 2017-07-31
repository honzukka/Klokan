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

			// show the content of the batch if there is something there already
			if (categoryBatch.CorrectSheetFilename != null)
			{
				correctSheetTextBox.Text = categoryBatch.CorrectSheetFilename;
			}

			if (categoryBatch.SheetFilenames != null)
			{
				answerSheetsListBox.DataSource = categoryBatch.SheetFilenames;
			}

			this.categoryBatch = categoryBatch;
			answerSheetFilenames = new List<string>();
		}

		// file dialog (no multiselect) for finding the path of the correct answer sheet
		private void searchButton_Click(object sender, EventArgs e)
		{
			var dialogResult = openCorrectFileDialog.ShowDialog();

			if (dialogResult == DialogResult.OK)
			{
				correctSheetTextBox.Text = openCorrectFileDialog.FileName;
			}
		}

		// file dialog (multiselect) for adding answer sheets to be evaluated
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
			if (correctSheetTextBox.Text == "")
			{
				MessageBox.Show("Correct answer sheet not selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (answerSheetFilenames.Count == 0)
			{
				MessageBox.Show("No answer sheets selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			categoryBatch.CorrectSheetFilename = correctSheetTextBox.Text;
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
	}
}
