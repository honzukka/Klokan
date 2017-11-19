﻿using System;
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
		// id of the answer sheet that's being viewed in this form
		int answerSheetId;

		// answer sheet data this form needs to work with
		bool[,,] chosenAnswers;
		bool[,,] chosenAnswersTemp;
		bool[,,] correctAnswers;

		int studentNumber;
		int studentNumberTemp;

		int points;

		public DatabaseDetailForm(int answerSheetId)
		{
			InitializeComponent();

			this.answerSheetId = answerSheetId;

			chosenAnswers = null;
			chosenAnswersTemp = null;
			correctAnswers = null;

			studentNumber = 0;
			studentNumberTemp = 0;

			points = -1;

			studentNumberTextBox.Hide();

			PopulateForm();
		}

		// toggle edit mode
		private void editButton_Click(object sender, EventArgs e)
		{
			applyButton.Enabled = true;
			discardButton.Enabled = true;
			editButton.Enabled = false;
			reevaluateButton.Enabled = false;
			updateDatabaseButton.Enabled = false;

			// make the student number field editable
			studentNumberTextBox.Text = studentNumber.ToString();
			studentNumberValueLabel.Hide();
			studentNumberTextBox.Show();

			// draw only chosen answers as only those can be edited
			ResetTableImages();
			
			HelperFunctions.DrawAnswers(table1PictureBox, chosenAnswers, 0, HelperFunctions.DrawCross, Color.Black);
			HelperFunctions.DrawAnswers(table2PictureBox, chosenAnswers, 1, HelperFunctions.DrawCross, Color.Black);
			HelperFunctions.DrawAnswers(table3PictureBox, chosenAnswers, 2, HelperFunctions.DrawCross, Color.Black);

			// create a copy of currently chosen answers for editing
			chosenAnswersTemp = new bool[3, 8, 5];
			Array.Copy(chosenAnswers, chosenAnswersTemp, 3 * 8 * 5);
		}

		private void applyButton_Click(object sender, EventArgs e)
		{
			if (int.TryParse(studentNumberTextBox.Text, out studentNumberTemp) == false)
			{
				MessageBox.Show("Invalid student number format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (studentNumberTemp < 0 || studentNumberTemp > 99999)
			{
				MessageBox.Show("Student number out of range.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			applyButton.Enabled = false;
			discardButton.Enabled = false;
			editButton.Enabled = true;
			reevaluateButton.Enabled = true;

			studentNumberTextBox.Hide();
			studentNumberValueLabel.Text = studentNumberTemp.ToString();
			studentNumberValueLabel.Show();

			studentNumber = studentNumberTemp;
			chosenAnswers = chosenAnswersTemp;
		}

		private void discardButton_Click(object sender, EventArgs e)
		{
			applyButton.Enabled = false;
			discardButton.Enabled = false;
			editButton.Enabled = true;

			studentNumberTextBox.Hide();
			studentNumberValueLabel.Show();

			// draw the original answers
			ResetTableImages();

			HelperFunctions.DrawAnswers(table1PictureBox, chosenAnswers, 0, HelperFunctions.DrawCross, Color.Black);
			HelperFunctions.DrawAnswers(table2PictureBox, chosenAnswers, 1, HelperFunctions.DrawCross, Color.Black);
			HelperFunctions.DrawAnswers(table3PictureBox, chosenAnswers, 2, HelperFunctions.DrawCross, Color.Black);

			HelperFunctions.DrawAnswers(table1PictureBox, correctAnswers, 0, HelperFunctions.DrawCircle, Color.Red);
			HelperFunctions.DrawAnswers(table2PictureBox, correctAnswers, 1, HelperFunctions.DrawCircle, Color.Red);
			HelperFunctions.DrawAnswers(table3PictureBox, correctAnswers, 2, HelperFunctions.DrawCircle, Color.Red);
		}

		private void reevaluateButton_Click(object sender, EventArgs e)
		{
			reevaluateButton.Enabled = false;
			updateDatabaseButton.Enabled = true;

			points = ReevaluateAnswers();
			pointsValueLabel.Text = points.ToString();

			// draw both chosen and correct answers again
			ResetTableImages();

			HelperFunctions.DrawAnswers(table1PictureBox, chosenAnswers, 0, HelperFunctions.DrawCross, Color.Black);
			HelperFunctions.DrawAnswers(table2PictureBox, chosenAnswers, 1, HelperFunctions.DrawCross, Color.Black);
			HelperFunctions.DrawAnswers(table3PictureBox, chosenAnswers, 2, HelperFunctions.DrawCross, Color.Black);

			HelperFunctions.DrawAnswers(table1PictureBox, correctAnswers, 0, HelperFunctions.DrawCircle, Color.Red);
			HelperFunctions.DrawAnswers(table2PictureBox, correctAnswers, 1, HelperFunctions.DrawCircle, Color.Red);
			HelperFunctions.DrawAnswers(table3PictureBox, correctAnswers, 2, HelperFunctions.DrawCircle, Color.Red);
		}

		private void updateDatabaseButton_Click(object sender, EventArgs e)
		{
			updateDatabaseButton.Enabled = false;

			var dialogResult = MessageBox.Show("Are you sure you want to update the database?", "Database Change", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (dialogResult == DialogResult.No)
			{
				return;
			}

			List<KlokanDBChosenAnswer> newChosenAnswers = new List<KlokanDBChosenAnswer>();
			for (int i = 0; i < 3; i++)
			{
				newChosenAnswers.AddRange(HelperFunctions.AnswersToDbSet<KlokanDBChosenAnswer>(chosenAnswers, i, false));
			}

			int newPoints = points;

			using (var db = new KlokanDBContext())
			{
				var query = from sheet in db.AnswerSheets
							where sheet.AnswerSheetId == answerSheetId
							select sheet;

				KlokanDBAnswerSheet answerSheet = query.FirstOrDefault();
				
				// load the old chosen answers so that EF knows it should delete them when the old answer sheet is deleted
				List<KlokanDBChosenAnswer> blah = new List<KlokanDBChosenAnswer>(answerSheet.ChosenAnswers);

				KlokanDBAnswerSheet updatedAnswerSheet = new KlokanDBAnswerSheet {
					StudentNumber = studentNumber,
					Points = newPoints,
					Scan = answerSheet.Scan,
					InstanceId = answerSheet.InstanceId,
					Instance = answerSheet.Instance,
					ChosenAnswers = newChosenAnswers
				};

				db.AnswerSheets.Remove(answerSheet);
				db.AnswerSheets.Add(updatedAnswerSheet);

				db.SaveChanges();
			}
		}

		private void table1PictureBox_Click(object sender, EventArgs e)
		{
			// if in edit mode
			if (editButton.Enabled == false)
			{
				HelperFunctions.HandleTableImageClicks(e as MouseEventArgs, table1PictureBox, 0, chosenAnswersTemp);
			}
		}

		private void table2PictureBox_Click(object sender, EventArgs e)
		{
			// if in edit mode
			if (editButton.Enabled == false)
			{
				HelperFunctions.HandleTableImageClicks(e as MouseEventArgs, table2PictureBox, 1, chosenAnswersTemp);
			}		
		}

		private void table3PictureBox_Click(object sender, EventArgs e)
		{
			// if in edit mode
			if (editButton.Enabled == false)
			{
				HelperFunctions.HandleTableImageClicks(e as MouseEventArgs, table3PictureBox, 2, chosenAnswersTemp);
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

				KlokanDBAnswerSheet answerSheet = sheetQuery.FirstOrDefault();

				var instanceQuery = from instance in db.Instances
									where instance.InstanceId == answerSheet.InstanceId
									select instance;

				KlokanDBInstance currentInstance = instanceQuery.FirstOrDefault();

				studentNumber = answerSheet.StudentNumber;

				studentNumberValueLabel.Text = answerSheet.StudentNumber.ToString();
				idValueLabel.Text = answerSheet.AnswerSheetId.ToString();
				yearValueLabel.Text = currentInstance.Year.ToString();
				categoryValueLabel.Text = currentInstance.Category.ToString();
				pointsValueLabel.Text = answerSheet.Points.ToString();

				// load scan
				scanPictureBox.Image = HelperFunctions.GetBitmap(answerSheet.Scan);

				// draw answers
				var chosenAnswersQuery = from chosenAnswer in db.ChosenAnswers
								   where chosenAnswer.AnswerSheetId == answerSheetId
								   select chosenAnswer;

				HelperFunctions.DbSetToAnswers(chosenAnswersQuery.ToList(), out chosenAnswers);
				
				HelperFunctions.DrawAnswers(table1PictureBox, chosenAnswers, 0, HelperFunctions.DrawCross, Color.Black);
				HelperFunctions.DrawAnswers(table2PictureBox, chosenAnswers, 1, HelperFunctions.DrawCross, Color.Black);
				HelperFunctions.DrawAnswers(table3PictureBox, chosenAnswers, 2, HelperFunctions.DrawCross, Color.Black);

				var correctAnswersQuery = from correctAnswer in db.CorrectAnswers
										  where correctAnswer.InstanceId == answerSheet.InstanceId
										  select correctAnswer;

				HelperFunctions.DbSetToAnswers(correctAnswersQuery.ToList(), out correctAnswers);
				
				HelperFunctions.DrawAnswers(table1PictureBox, correctAnswers, 0, HelperFunctions.DrawCircle, Color.Red);
				HelperFunctions.DrawAnswers(table2PictureBox, correctAnswers, 1, HelperFunctions.DrawCircle, Color.Red);
				HelperFunctions.DrawAnswers(table3PictureBox, correctAnswers, 2, HelperFunctions.DrawCircle, Color.Red);
			}
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

			table1PictureBox.Image = Properties.Resources.answerTable1Image;
			table2PictureBox.Image = Properties.Resources.answerTable2Image;
			table3PictureBox.Image = Properties.Resources.answerTable3Image;

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
