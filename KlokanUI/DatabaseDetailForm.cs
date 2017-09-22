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
using System.Globalization;
using System.Drawing.Drawing2D;

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
				var sheetQuery = from sheet in db.AnswerSheets
								 where sheet.AnswerSheetId == answerSheetId
								 select sheet;

				AnswerSheet answerSheet = sheetQuery.FirstOrDefault();

				var answersQuery = from answer in db.Answers
								   where answer.AnswerSheetId == answerSheetId
								   select answer;

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
			}
		}
	}
}
