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

				var answersQuery = from answer in db.Answers
								   where answer.AnswerSheetId == answerSheetId
								   select answer;

				// it's fine to do this because sheetQuery always finds exactly one record and we're not going to query it further
				List<AnswerSheet> answerSheetsReturned = sheetQuery.ToList();

				idValueLabel.Text = answerSheetsReturned[0].AnswerSheetId.ToString();
				yearValueLabel.Text = answerSheetsReturned[0].Year.ToString();
				categoryValueLabel.Text = answerSheetsReturned[0].Category.ToString();
				pointsValueLabel.Text = answerSheetsReturned[0].Points.ToString();

				// load scan
				var imageConverter = new ImageConverter();
				Bitmap bmp = (Bitmap)imageConverter.ConvertFrom(answerSheetsReturned[0].Scan);
				scanPictureBox.Image = bmp;
			}
		}
	}
}
