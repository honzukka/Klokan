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
			correctAnswers = new bool[3, 8, 5];
			answerSheetFilenames = new List<string>();

			table3PictureBox.LoadCompleted += delegate { DrawCorrectAnswers(); MessageBox.Show("NOW"); };
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

		private void table1PictureBox_Click(object sender, EventArgs e)
		{
			HandlePictureBoxClicks(table1PictureBox, 0, e as MouseEventArgs);
		}

		private void table2PictureBox_Click(object sender, EventArgs e)
		{
			HandlePictureBoxClicks(table2PictureBox, 1, e as MouseEventArgs);
		}

		private void table3PictureBox_Click(object sender, EventArgs e)
		{
			HandlePictureBoxClicks(table3PictureBox, 2, e as MouseEventArgs);
		}

		private void correctAnswerSheetRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (correctAnswerSheetRadioButton.Checked)
			{
				correctSheetTextBox.Enabled = true;
				searchButton.Enabled = true;
			}
			else
			{
				correctSheetTextBox.Enabled = false;
				searchButton.Enabled = false;
			}
		}

		private void correctAnswerPickerRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (correctAnswerPickerRadioButton.Checked)
			{
				table1PictureBox.Enabled = true;
				table2PictureBox.Enabled = true;
				table3PictureBox.Enabled = true;

				table1PictureBox.Visible = true;
				table2PictureBox.Visible = true;
				table3PictureBox.Visible = true;

				//this.Invoke(new Action(() => { DrawCorrectAnswers(); }));
			}
			else
			{
				table1PictureBox.Enabled = false;
				table2PictureBox.Enabled = false;
				table3PictureBox.Enabled = false;

				table1PictureBox.Visible = false;
				table2PictureBox.Visible = false;
				table3PictureBox.Visible = false;
			}
		}

		/// <summary>
		/// Handles click for all picture boxes in this form.
		/// </summary>
		/// <param name="pictureBox">A reference to the picture box that was clicked.</param>
		/// <param name="tableIndex">An index of the table represented by the picture box in the correctAnswers array.</param>
		/// <param name="mouseEvent">The click event.</param>
		private void HandlePictureBoxClicks(PictureBox pictureBox, int tableIndex, MouseEventArgs mouseEvent)
		{
			int cellHeight = pictureBox.Height / 9;
			int cellWidth = pictureBox.Width / 6;
			int rowClicked = mouseEvent.Y / cellHeight;
			int columnClicked = mouseEvent.X / cellWidth;

			// if the area designated for answers was clicked
			if (rowClicked != 0 && columnClicked != 0)
			{
				// get ready for drawing
				Graphics graphics = pictureBox.CreateGraphics();
				Pen blackPen = new Pen(Color.Black, 2);

				// remove any crosses that are already in the row
				for (int i = 0; i < 5; i++)
				{
					if (correctAnswers[tableIndex, rowClicked - 1, i] == true)
					{
						correctAnswers[tableIndex, rowClicked - 1, i] = false;
						RemoveCross(rowClicked, i + 1, cellWidth, cellHeight, graphics);
					}
				}

				correctAnswers[tableIndex, rowClicked - 1, columnClicked - 1] = true;
				DrawCross(rowClicked, columnClicked, cellWidth, cellHeight, graphics, blackPen);
			}
		}

		private void DrawCorrectAnswers()
		{
			PictureBox[] pictureBoxes = new PictureBox[] { table1PictureBox, table2PictureBox, table3PictureBox };

			for (int i = 0; i < pictureBoxes.Length; i++)
			{
				int cellHeight = pictureBoxes[i].Height / 9;
				int cellWidth = pictureBoxes[i].Width / 6;
				Graphics graphics = pictureBoxes[i].CreateGraphics();
				Pen blackPen = new Pen(Color.Black, 2);

				for (int row = 0; row < 8; row++)
				{
					for (int col = 0; col < 5; col++)
					{
						if (correctAnswers[i, row, col] == true)
						{
							DrawCross(row + 1, col + 1, cellWidth, cellHeight, graphics, blackPen);
						}
					}
				}
			}
		}

		private void DrawCross(int row, int column, int cellWidth, int cellHeight, Graphics graphics, Pen pen)
		{
			Point upperLeft = new Point(column * cellWidth, row * cellHeight);
			Point upperRight = new Point(upperLeft.X + cellWidth, upperLeft.Y);
			Point lowerLeft = new Point(upperLeft.X, upperLeft.Y + cellHeight);
			Point lowerRight = new Point(upperLeft.X + cellWidth, upperLeft.Y + cellHeight);

			graphics.DrawLine(pen, upperLeft, lowerRight);
			graphics.DrawLine(pen, upperRight, lowerLeft);
		}

		private void RemoveCross(int row, int column, int cellWidth, int cellHeight, Graphics graphics)
		{
			Rectangle cell = new Rectangle((column * cellWidth) + 1, (row * cellHeight) + 1, cellWidth - 2, cellHeight - 2);
			graphics.FillRectangle(Brushes.White, cell);
		}
	}
}
