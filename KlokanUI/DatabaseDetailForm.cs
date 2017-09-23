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
	public partial class DatabaseDetailForm : Form
	{
		public DatabaseDetailForm(int answerSheetId)
		{
			InitializeComponent();

			PopulateForm(answerSheetId);
		}

		private void PopulateForm(int answerSheetId)
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

				var answers = GetAnswersTableArray(chosenAnswersQuery);
				AnswerDrawing.DrawAnswers(table1PictureBox, table2PictureBox, table3PictureBox, answers, AnswerDrawing.DrawCross, Color.Black);

				var correctAnswersQuery = from correctAnswer in db.CorrectAnswers
										  where correctAnswer.InstanceId == answerSheet.InstanceId
										  select correctAnswer;

				var correctAnswers = GetAnswersTableArray(correctAnswersQuery);
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
	}
}
