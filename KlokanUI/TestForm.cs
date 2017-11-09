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
	public partial class TestForm : Form
	{
		public TestForm()
		{
			InitializeComponent();

			PopulateDataView();
		}

		private void PopulateDataView()
		{
			dataView.Rows.Clear();

			using (var testDB = new KlokanTestDBContext())
			{
				var scanQuery = from scan in testDB.Scans
								select new { scan.ScanId, scan.Correctness };

				foreach (var scanInfo in scanQuery)
				{
					dataView.Rows.Add(scanInfo.ScanId, scanInfo.Correctness);
				}
			}
		}

		private void addItemButton_Click(object sender, EventArgs e)
		{
			KlokanTestDBScan newScanItem = new KlokanTestDBScan();
			TestItemForm testAddItemForm = new TestItemForm(newScanItem, false);
			testAddItemForm.StartPosition = FormStartPosition.CenterScreen;
			testAddItemForm.ShowDialog();

			PopulateDataView();
		}

		private void removeItemButton_Click(object sender, EventArgs e)
		{
			if (dataView.SelectedRows.Count == 0)
			{
				MessageBox.Show("No row has been selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// multiselect is set to false for this data view
			var rowToRemove = dataView.SelectedRows[0];
			int rowToRemoveID = (int)(rowToRemove.Cells[0].Value);
			dataView.Rows.Remove(rowToRemove);

			using (var testDB = new KlokanTestDBContext())
			{
				var scanToRemoveQuery = from scan in testDB.Scans
										where scan.ScanId == rowToRemoveID
										select scan;

				var scanToRemove = scanToRemoveQuery.FirstOrDefault();
				if (scanToRemove != default(KlokanTestDBScan))
				{
					// lazy loading is used, so we need to load all the relations of scan if we want Remove() to remove those as well
					var expectedAnswers = scanToRemove.ExpectedValues;
					var computedValues = scanToRemove.ComputedValues;

					testDB.Scans.Remove(scanToRemove);
					testDB.SaveChanges();
				}
			}
		}

		private void viewItemButton_Click(object sender, EventArgs e)
		{
			if (dataView.SelectedRows.Count == 0)
			{
				MessageBox.Show("No row has been selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// multiselect is set to false for this data view
			int scanItemId = (int)dataView.SelectedRows[0].Cells[0].Value;

			KlokanTestDBScan scanItemToView = null;

			using (var testDB = new KlokanTestDBContext())
			{
				var scanItemQuery = from scan in testDB.Scans
									where scan.ScanId == scanItemId
									select scan;

				var scanItem = scanItemQuery.FirstOrDefault();

				scanItemToView = new KlokanTestDBScan {
					ScanId = scanItem.ScanId,
					ComputedValues = scanItem.ComputedValues,
					ExpectedValues = scanItem.ExpectedValues,
					Image = scanItem.Image,
					Correctness = scanItem.Correctness
				};
			}

			TestItemForm form = new TestItemForm(scanItemToView, true);
			form.StartPosition = FormStartPosition.CenterScreen;
			form.ShowDialog();

			PopulateDataView();
		}
	}
}
