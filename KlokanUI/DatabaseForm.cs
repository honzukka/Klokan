using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using System.IO;
using System.Linq.Expressions;

namespace KlokanUI
{
	public partial class DatabaseForm : Form
	{
		/// <summary>
		/// Helper class for data export.
		/// </summary>
		class AnswerSheetSelection
		{
			public int AnswerSheetId { get; set; }
			public int StudentNumber { get; set; }
			public int Points { get; set; }
			public KlokanDBInstance Instance { get; set; }
			public ICollection<KlokanDBChosenAnswer> ChosenAnswers { get; set; }
		}

		/// <summary>
		/// Dropdown data.
		/// </summary>
		List<string> yearList;

		/// <summary>
		/// Dropdown data.
		/// </summary>
		List<string> categoryList;

		public DatabaseForm()
		{
			InitializeComponent();

			// initialize the year combo box with a list of years
			yearList = new List<string>();
			yearList.Add(Properties.Resources.DropdownTextAll);
			for (int year = DateTime.Now.Year; year >= 2000; year--)
			{
				yearList.Add(year.ToString());
			}
			yearComboBox.DataSource = yearList;

			// intialize the catgory combo box with a list of categories
			categoryList = new List<string>();
			categoryList.Add(Properties.Resources.DropdownTextAll);
			foreach (var category in Enum.GetValues(typeof(Category)))
			{
				categoryList.Add(category.ToString());
			}
			categoryComboBox.DataSource = categoryList;
		}

		#region UI Functions

		private void viewButton_Click(object sender, EventArgs e)
		{
			PopulateDataView();
		}

		private void detailButton_Click(object sender, EventArgs e)
		{
			if (dataView.SelectedRows.Count == 0)
			{
				MessageBox.Show(Properties.Resources.ErrorTextNoRowSelected, Properties.Resources.ErrorCaptionGeneral, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// remember the sorting for future reference
			var dataViewSortedColumn = dataView.SortedColumn;
			var dataViewSortOrder = dataView.SortOrder;

			// multiselect is set to false for this data view
			DatabaseDetailForm form = new DatabaseDetailForm((int)(dataView.SelectedRows[0].Cells[0].Value));
			form.StartPosition = FormStartPosition.CenterScreen;

			// show the detail window
			form.ShowDialog();

			// load and show updated data
			PopulateDataView();

			// if the data view was sorted before
			if (dataViewSortedColumn != null)
			{
				ListSortDirection dataViewSortDirection;

				if (dataViewSortOrder == SortOrder.Ascending)
				{
					dataViewSortDirection = ListSortDirection.Ascending;
				}
				// dataViewSortOrder of "None" can't happen because now we know that it was sorted
				else
				{
					dataViewSortDirection = ListSortDirection.Descending;
				}

				// sort the data view as it was before
				dataView.Sort(dataViewSortedColumn, dataViewSortDirection);
			}
		}

		private void dataView_Click(object sender, EventArgs e)
		{
			if (dataView.SelectedRows.Count == 1)
			{
				detailButton.Enabled = true;
			}
			else
			{
				detailButton.Enabled = false;
			}
		}

		private void exportSelectionButton_Click(object sender, EventArgs e)
		{
			var dialogResult = saveFileDialogExport.ShowDialog();

			if (dialogResult == DialogResult.OK)
			{
				string saveFilePath = saveFileDialogExport.FileName;

				using (var file = new StreamWriter(saveFilePath, false))
				{
					var answerSheetSelector = CreateSelector();

					ExportSelection(file, answerSheetSelector);
				}
			}
		}

		#endregion

		#region Helper Functions

		/// <summary>
		/// Load answer sheet data based on the user-chosen filter and show it.
		/// </summary>
		private void PopulateDataView()
		{
			var answerSheetSelector = CreateSelector();

			dataView.Rows.Clear();

			using (var db = new KlokanDBContext())
			{
				var answerSheetQuery = db.AnswerSheets.Where(answerSheetSelector);

				foreach (var answerSheet in answerSheetQuery)
				{
					dataView.Rows.Add(answerSheet.AnswerSheetId, answerSheet.StudentNumber, answerSheet.Instance.Year, answerSheet.Instance.Category, answerSheet.Points);
				}
			}
		}

		/// <summary>
		/// Load answer sheet data based on the selector and output it.
		/// </summary>
		private void ExportSelection(StreamWriter sw, Expression<Func<KlokanDBAnswerSheet, bool>> answerSheetSelector)
		{
			OutputHeader(sw);

			using (var db = new KlokanDBContext())
			{
				// find the suitable answer sheets
				var answerSheetQuery = db.AnswerSheets.Where(answerSheetSelector);

				// select everything from them except for scans
				var answerSheetSelectionQuery = from answerSheet in answerSheetQuery
												select new AnswerSheetSelection
												{
													AnswerSheetId = answerSheet.AnswerSheetId,
													StudentNumber = answerSheet.StudentNumber,
													Points = answerSheet.Points,
													Instance = answerSheet.Instance,
													ChosenAnswers = answerSheet.ChosenAnswers
												};

				OutputAnswerSheetSelection(sw, answerSheetSelectionQuery);
			}
		}

		/// <summary>
		/// Outputs a header for the answer sheet selection in csv format delimited by a semicolon.
		/// </summary>
		private void OutputHeader(StreamWriter sw)
		{
			sw.Write(Properties.Resources.OutputTextAnswerSheetID + ";");
			sw.Write(Properties.Resources.OutputTextStudentNumber + ";");
			sw.Write(Properties.Resources.OutputTextYear + ";");
			sw.Write(Properties.Resources.OutputTextCategory + ";");
			sw.Write(Properties.Resources.OutputTextPoints + ";");

			for (int i = 1; i <= 24; i++)
			{
				sw.Write(i + " (" + Properties.Resources.OutputTextChosen + ");" + 
					i + " (" + Properties.Resources.OutputTextCorrect + ");");
			}

			sw.WriteLine();
		}

		/// <summary>
		/// Outputs answer sheet selection in csv format delimited by a semicolon.
		/// </summary>
		private void OutputAnswerSheetSelection(StreamWriter sw, IQueryable<AnswerSheetSelection> answerSheetSelectionQuery)
		{
			foreach (var answerSheetSelection in answerSheetSelectionQuery)
			{
				KlokanDBInstance currentInstance = answerSheetSelection.Instance;
				List<KlokanDBChosenAnswer> chosenAnswers = new List<KlokanDBChosenAnswer>(answerSheetSelection.ChosenAnswers);
				List<KlokanDBCorrectAnswer> correctAnswers = new List<KlokanDBCorrectAnswer>(currentInstance.CorrectAnswers);

				sw.Write(answerSheetSelection.AnswerSheetId + ";");
				sw.Write(answerSheetSelection.StudentNumber + ";");
				sw.Write(currentInstance.Year + ";");
				sw.Write(currentInstance.Category + ";");
				sw.Write(answerSheetSelection.Points + ";");

				// relies on the order of answers in the database...
				for (int i = 0; i < 24; i++)
				{
					sw.Write(chosenAnswers[i].Value + ";" + correctAnswers[i].Value + ";");
				}

				sw.WriteLine();
			}
		}

		/// <summary>
		/// Creates a selector based on the user-chosen filter.
		/// </summary>
		/// <returns></returns>
		Expression<Func<KlokanDBAnswerSheet, bool>> CreateSelector()
		{
			int selectedYear;
			string selectedCategory = (string)categoryComboBox.SelectedItem;

			// a specific year was selected
			if (int.TryParse((string)yearComboBox.SelectedItem, out selectedYear))
			{
				// all categories were selected
				if (selectedCategory == Properties.Resources.DropdownTextAll)
				{
					return (KlokanDBAnswerSheet answerSheet) => answerSheet.Instance.Year == selectedYear;
				}
				// a specific category was selected
				else
				{
					return (KlokanDBAnswerSheet answerSheet) => answerSheet.Instance.Year == selectedYear && answerSheet.Instance.Category == selectedCategory;
				}
			}
			// all years were selected
			else
			{
				// all categories were selected
				if (selectedCategory == Properties.Resources.DropdownTextAll)
				{
					return (KlokanDBAnswerSheet answerSheet) => true;
				}
				// a specific category was selected
				else
				{
					return (KlokanDBAnswerSheet answerSheet) => answerSheet.Instance.Category == selectedCategory;
				}
			}
		}

		#endregion

		/*
		private void ImportDB(SQLiteConnection externalConnection)
		{
			using (var internalDB = new KlokanDBContext())
			using (var externalDB = new KlokanDBContext(externalConnection))
			{
				// load the whole external database
				// (only one instance of a dbcontext can be tracked at a time and this one won't be modified anyway...)
				var externalInstanceQuery = externalDB.Instances
												.Include("AnswerSheets.ChosenAnswers")
												.Include("CorrectAnswers").AsNoTracking();	

				// import new instances
				foreach (var externalInstance in externalInstanceQuery)
				{
					var internalInstanceQuery = from internalInstance in internalDB.Instances
								where internalInstance.Category == externalInstance.Category &&
										internalInstance.Year == externalInstance.Year
								select internalInstance;

					// if the external instance is new, insert it into the internal table
					if (internalInstanceQuery.Count() == 0)
					{
						internalDB.Instances.Add(externalInstance);
					}

					// (TODO) new answer sheets can also be imported 
					// but only once a set of columns that differentiates them 
					// is determined (student number + school number???)
				}

				// (TODO) asynchronous?
				internalDB.SaveChanges();
			}
		}
		*/
	}
}
