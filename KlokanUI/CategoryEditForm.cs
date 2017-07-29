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
		KlokanBatch klokanBatch;

		public CategoryEditForm(KlokanBatch klokanBatch)
		{
			InitializeComponent();

			this.klokanBatch = klokanBatch;
			answerSheetFilenames = new List<string>();

			categoryComboBox.DataSource = Enum.GetValues(typeof(Category));
		}

		private void searchButton_Click(object sender, EventArgs e)
		{
			var dialogResult = openCorrectFileDialog.ShowDialog();

			if (dialogResult == DialogResult.OK)
			{
				correctSheetTextBox.Text = openCorrectFileDialog.FileName;
			}
		}

		private void addButton_Click(object sender, EventArgs e)
		{
			var dialogResult = openFilesDialog.ShowDialog();
			bool duplicateFilenames = false;

			if (dialogResult == DialogResult.OK)
			{
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
					MessageBox.Show("One or more files were already present in the list, so they were not added.", "Duplicate Filenames");
					duplicateFilenames = false;
				}
			}
		}

		private void removeButton_Click(object sender, EventArgs e)
		{
			int selectedItemIndex = answerSheetsListBox.SelectedIndex;

			// if something is selected
			if (selectedItemIndex > -1)
			{
				answerSheetFilenames.RemoveAt(selectedItemIndex);

				UpdateListBox();
			}
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			string categoryName = categoryComboBox.Text;

			klokanBatch.CategoryBatches[categoryName] = new KlokanCategoryBatch {
				CategoryName = categoryName,
				CorrectSheetFilename = correctSheetTextBox.Text,
				SheetFilenames = answerSheetFilenames
			};

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
