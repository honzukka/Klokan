using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

namespace KlokanUI
{
	public partial class EvaluationForm : Form
	{
		KlokanBatch klokanBatch;

		public EvaluationForm()
		{
			InitializeComponent();

			// assign default parameters to the batch
			Parameters parameters = new Parameters();
			parameters.SetDefaultValues();

			klokanBatch = new KlokanBatch();
			klokanBatch.Parameters = parameters;
			klokanBatch.CategoryBatches = new Dictionary<string, KlokanCategoryBatch>();
		}

		public void EnableGoButton()
		{
			if (goButton.InvokeRequired)
			{
				goButton.BeginInvoke(new Action(
					() => { goButton.Enabled = true; }
				));
			}
			else
			{
				goButton.Enabled = true;
			}
		}

		private void goButton_Click(object sender, EventArgs e)
		{
			goButton.Enabled = false;

			/*
			Parameters parameters = new Parameters();
			parameters.SetDefaultValues();

			var sheetFilenames = new List<string>();
			for (int i = 0; i < 100; i++)
			{
				sheetFilenames.Add("01-varying_size.jpeg");
			}

			var klokanBatch = new KlokanBatch
			{
				Parameters = parameters,
				CategoryBatches = new Dictionary<string, KlokanCategoryBatch> { {
						"TestBatch",	new KlokanCategoryBatch {
													CategoryName = Category.Kadet.ToString(),
													CorrectSheetFilename = "01-varying_size.jpeg",
													SheetFilenames = sheetFilenames
										}
						}
				}
			};
			*/

			var jobScheduler = new JobScheduler(klokanBatch, this);

			// new thread created, so that all tasks in it are planned in the threadpool and not in the WinForms synchronization context
			Thread thread = new Thread(jobScheduler.Run);
			thread.IsBackground = true;
			thread.Start();
		}

		private void listBoxAddButton_Click(object sender, EventArgs e)
		{
			CategoryEditForm categoryEditForm = new CategoryEditForm(klokanBatch);
			categoryEditForm.StartPosition = FormStartPosition.CenterScreen;
			var dialogResult = categoryEditForm.ShowDialog();

			if (dialogResult == DialogResult.OK)
			{
				categoryBatchListBox.DataSource = null;
				categoryBatchListBox.DataSource = new List<string>(klokanBatch.CategoryBatches.Keys);
			}
		}
	}
}
