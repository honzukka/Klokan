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
	// TODO: add comments
	public partial class DatabaseDetailForm : Form
	{
		int answerSheetId;
		bool[,,] chosenAnswers;
		bool[,,] chosenAnswersTemp;
		bool[,,] correctAnswers;
		int points;

		public DatabaseDetailForm(int answerSheetId)
		{
			InitializeComponent();

			this.answerSheetId = answerSheetId;
			chosenAnswers = null;
			chosenAnswersTemp = null;
			correctAnswers = null;
			points = -1;

			PopulateForm();
		}

		private void editButton_Click(object sender, EventArgs e)
		{
			applyButton.Enabled = true;
			discardButton.Enabled = true;
			editButton.Enabled = false;

			ResetTableImages();
			AnswerDrawing.DrawAnswers(table1PictureBox, table2PictureBox, table3PictureBox, chosenAnswers, AnswerDrawing.DrawCross, Color.Black);

			// create a copy of currently chosen answers for editing
			chosenAnswersTemp = new bool[3, 8, 5];
			Array.Copy(chosenAnswers, chosenAnswersTemp, 3 * 8 * 5);
		}

		private void applyButton_Click(object sender, EventArgs e)
		{
			applyButton.Enabled = false;
			discardButton.Enabled = false;
			editButton.Enabled = true;
			reevaluateButton.Enabled = true;

			chosenAnswers = chosenAnswersTemp;
		}

		private void discardButton_Click(object sender, EventArgs e)
		{
			applyButton.Enabled = false;
			discardButton.Enabled = false;
			editButton.Enabled = true;

			// draw the original answers
			ResetTableImages();
			AnswerDrawing.DrawAnswers(table1PictureBox, table2PictureBox, table3PictureBox, chosenAnswers, AnswerDrawing.DrawCross, Color.Black);
		}

		private void reevaluateButton_Click(object sender, EventArgs e)
		{
			reevaluateButton.Enabled = false;

			points = ReevaluateAnswers();
			pointsValueLabel.Text = points.ToString();

			// redraw answers
			ResetTableImages();
			AnswerDrawing.DrawAnswers(table1PictureBox, table2PictureBox, table3PictureBox, chosenAnswers, AnswerDrawing.DrawCross, Color.Black);
			AnswerDrawing.DrawAnswers(table1PictureBox, table2PictureBox, table3PictureBox, correctAnswers, AnswerDrawing.DrawCircle, Color.Red);
		}

		private void updateDatabaseButton_Click(object sender, EventArgs e)
		{

		}

		private void table1PictureBox_Click(object sender, EventArgs e)
		{
			// if in edit mode
			if (editButton.Enabled == false)
			{
				AnswerSelection.HandleTableImageClicks(e as MouseEventArgs, table1PictureBox, 0, chosenAnswersTemp);
			}
		}

		private void table2PictureBox_Click(object sender, EventArgs e)
		{
			// if in edit mode
			if (editButton.Enabled == false)
			{
				AnswerSelection.HandleTableImageClicks(e as MouseEventArgs, table2PictureBox, 1, chosenAnswersTemp);
			}		
		}

		private void table3PictureBox_Click(object sender, EventArgs e)
		{
			// if in edit mode
			if (editButton.Enabled == false)
			{
				AnswerSelection.HandleTableImageClicks(e as MouseEventArgs, table3PictureBox, 2, chosenAnswersTemp);
			}
				
		}

		private void PopulateForm()
		{
			using (var db = new KlokanDBContext())
			{
				// show sheet info
				var sheetQuery = from sheet in db.AnswerSheets
								 where sheet.AnswerSheetId == answerSheetId
								 select sheet;

				AnswerSheet answerSheet = sheetQuery.FirstOrDefault();

				var instanceQuery = from instance in db.Instances
									where instance.InstanceId == answerSheet.InstanceId
									select instance;

				Instance currentInstance = instanceQuery.FirstOrDefault();

				idValueLabel.Text = answerSheet.AnswerSheetId.ToString();
				yearValueLabel.Text = currentInstance.Year.ToString();
				categoryValueLabel.Text = currentInstance.Category.ToString();
				pointsValueLabel.Text = answerSheet.Points.ToString();

				// load scan
				var imageConverter = new ImageConverter();
				Bitmap bmp = (Bitmap)imageConverter.ConvertFrom(answerSheet.Scan);
				scanPictureBox.Image = bmp;

				// draw answers
				var chosenAnswersQuery = from chosenAnswer in db.ChosenAnswers
								   where chosenAnswer.AnswerSheetId == answerSheetId
								   select chosenAnswer;

				chosenAnswers = GetAnswersTableArray(chosenAnswersQuery);
				AnswerDrawing.DrawAnswers(table1PictureBox, table2PictureBox, table3PictureBox, chosenAnswers, AnswerDrawing.DrawCross, Color.Black);

				var correctAnswersQuery = from correctAnswer in db.CorrectAnswers
										  where correctAnswer.InstanceId == answerSheet.InstanceId
										  select correctAnswer;

				correctAnswers = GetAnswersTableArray(correctAnswersQuery);
				AnswerDrawing.DrawAnswers(table1PictureBox, table2PictureBox, table3PictureBox, correctAnswers, AnswerDrawing.DrawCircle, Color.Red);
			}
		}

		private bool[,,] GetAnswersTableArray(IQueryable<Answer> answerQuery)
		{
			bool[,,] answers = new bool[3, 8, 5];

			int i = 0;
			foreach (var answer in answerQuery)
			{
				if (answer.Value[0] >= 'a' && answer.Value[0] <= 'e')
				{
					int table = i / 8;
					int row = i % 8;
					int column = answer.Value[0] - 'a';

					answers[table, row, column] = true;
				}

				i++;
			}

			return answers;
		}

		private void ResetTableImages()
		{
			List<Image> oldImages = new List<Image>();
			oldImages.Add(table1PictureBox.Image);
			oldImages.Add(table2PictureBox.Image);
			oldImages.Add(table3PictureBox.Image);
			foreach (var oldImage in oldImages)
			{
				if (oldImage != null) oldImage.Dispose();
			}

			table1PictureBox.Image = Properties.Resources.table1Image;
			table2PictureBox.Image = Properties.Resources.table2Image;
			table3PictureBox.Image = Properties.Resources.table3Image;

			table1PictureBox.Refresh();
			table2PictureBox.Refresh();
			table3PictureBox.Refresh();
		}

		// TODO: duplicate code?
		private int ReevaluateAnswers()
		{
			int newPoints = 24;

			for (int table = 0; table < 3; table++)
			{
				for (int row = 0; row < 8; row++)
				{
					int correctAnswerCount = 0;
					int incorrectAnswerCount = 0;

					for (int col = 0; col < 5; col++)
					{
						if (chosenAnswers[table, row, col] == true && correctAnswers[table, row, col] == true)
						{
							correctAnswerCount++;
						}
						else if (chosenAnswers[table, row, col] == true && correctAnswers[table, row, col] == false)
						{
							incorrectAnswerCount++;
						}
					}

					// assign points for the question (row)
					// if it's correct
					// NOTE: here we test if only one question has been selected!!!
					if (correctAnswerCount == 1 && incorrectAnswerCount == 0)
					{
						switch (table)
						{
							case 0: newPoints += 3; break;
							case 1: newPoints += 4; break;
							case 2: newPoints += 5; break;
						}
					}
					// if it's incorrect
					else if (incorrectAnswerCount > 0)
					{
						newPoints--;
					}
					// otherwise the score doesn't change
				}
			}

			return newPoints;
		}
	}
}
