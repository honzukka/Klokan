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
	public partial class DatabaseForm : Form
	{
		List<int> yearList;
		List<string> categoryList;

		public DatabaseForm()
		{
			InitializeComponent();

			// initialize the year combo box with a list of years
			yearList = new List<int>();
			for (int year = DateTime.Now.Year; year >= 2000; year--)
			{
				yearList.Add(year);
			}
			yearComboBox.DataSource = yearList;

			// intialize the catgory combo box with a list of categories
			categoryList = new List<string>();
			foreach (var category in Enum.GetValues(typeof(Category)))
			{
				categoryList.Add(category.ToString());
			}
			categoryComboBox.DataSource = categoryList;
		}

		private void PopulateDataView(int year, string category)
		{
			dataView.Rows.Clear();

			using (var db = new KlokanDBContext())
			{
				var instanceQuery = from instance in db.Instances
									where instance.Year == year && instance.Category == category
									select instance;

				Instance currentInstance = instanceQuery.FirstOrDefault();

				// if this instance exits
				if (currentInstance != default(Instance))
				{
					var answerSheetQuery = from sheet in db.AnswerSheets
										   where sheet.Instance.InstanceId == currentInstance.InstanceId
										   orderby sheet.Points descending
										   select new { sheet.AnswerSheetId, sheet.Points };

					foreach (var item in answerSheetQuery)
					{
						dataView.Rows.Add(item.AnswerSheetId, item.Points);
					}
				}
			}
		}

		private void viewButton_Click(object sender, EventArgs e)
		{
			// TODO: make this asynchronous!
			PopulateDataView((int)yearComboBox.SelectedItem, (string)categoryComboBox.SelectedItem);
		}

		private void detailButton_Click(object sender, EventArgs e)
		{
			if (dataView.SelectedRows.Count == 0)
			{
				MessageBox.Show("No row has been selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			DatabaseDetailForm form = new DatabaseDetailForm((int)(dataView.SelectedRows[0].Cells[0].Value));
			form.StartPosition = FormStartPosition.CenterScreen;
			form.ShowDialog();
		}
	}
}
