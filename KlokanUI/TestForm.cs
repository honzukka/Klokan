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
		}

		private void addItemButton_Click(object sender, EventArgs e)
		{
			KlokanTestDBScan newScanItem = new KlokanTestDBScan();
			TestAddItemForm testAddItemForm = new TestAddItemForm(newScanItem);
			testAddItemForm.StartPosition = FormStartPosition.CenterScreen;

			if (testAddItemForm.ShowDialog() == DialogResult.OK)
			{
				// save the successfully created item
				using (var testDB = new KlokanTestDBContext())
				{
					testDB.Scans.Add(newScanItem);

					// TODO: async???
					testDB.SaveChanges();

					// repopulate the data view
					dataView.Rows.Clear();

					var scanQuery = from scan in testDB.Scans
									select new { scan.ScanId, scan.Correctness };

					foreach (var item in scanQuery)
					{
						dataView.Rows.Add(item.ScanId, item.Correctness);
					}
				}
			}
		}
	}
}
