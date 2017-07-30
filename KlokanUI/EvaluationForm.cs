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
		class CategoryBatchConfig
		{
			public KlokanCategoryBatch batch;
			public bool isIncluded;
		}

		KlokanBatch klokanBatch;
		Dictionary<string, CategoryBatchConfig> categoryConfigurations;

		public EvaluationForm()
		{
			InitializeComponent();

			benjaminEditButton.Enabled = false;
			kadetEditButton.Enabled = false;
			juniorEditButton.Enabled = false;
			studentEditButton.Enabled = false;

			// assign default parameters to the batch
			Parameters parameters = new Parameters();
			parameters.SetDefaultValues();

			klokanBatch = new KlokanBatch();
			klokanBatch.Parameters = parameters;
			klokanBatch.CategoryBatches = new Dictionary<string, KlokanCategoryBatch>();

			categoryConfigurations = new Dictionary<string, CategoryBatchConfig>();
		}

		public void EnableGoButton()
		{
			if (evaluateButton.InvokeRequired)
			{
				evaluateButton.BeginInvoke(new Action(
					() => { evaluateButton.Enabled = true; }
				));
			}
			else
			{
				evaluateButton.Enabled = true;
			}
		}

		public void ShowMessageBoxInfo(string message, string caption)
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new Action(
					() => { MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information); }	
				));
			}
			else
			{
				MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void evaluateButton_Click(object sender, EventArgs e)
		{
			if (categoryConfigurations.Count == 0)
			{
				MessageBox.Show("No categories configured!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// save all included batches into the final klokan batch
			foreach (var pair in categoryConfigurations)
			{
				if (pair.Value.isIncluded)
				{
					klokanBatch.CategoryBatches[pair.Key] = pair.Value.batch;
				}
			}

			if (klokanBatch.CategoryBatches.Count == 0)
			{
				MessageBox.Show("No categories selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			evaluateButton.Enabled = false;

			var jobScheduler = new JobScheduler(klokanBatch, this);

			// new thread created, so that all tasks in it are planned in the threadpool and not in the WinForms synchronization context
			Thread thread = new Thread(jobScheduler.Run);
			thread.IsBackground = true;
			thread.Start();
		}

		private void benjaminCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (benjaminCheckBox.Checked)
			{
				if (categoryConfigurations.ContainsKey("Benjamin"))
				{
					categoryConfigurations["Benjamin"].isIncluded = true;
				}
				else
				{
					KlokanCategoryBatch benjaminBatch = new KlokanCategoryBatch { CategoryName = "Benjamin" };
					CategoryEditForm form = new CategoryEditForm(benjaminBatch);
					form.StartPosition = FormStartPosition.CenterScreen;

					if (form.ShowDialog() == DialogResult.OK)
					{
						categoryConfigurations["Benjamin"] = new CategoryBatchConfig { batch = benjaminBatch, isIncluded = true };
					}
				}

				benjaminEditButton.Enabled = true;
			}
			else
			{
				if (categoryConfigurations.ContainsKey("Benjamin"))
				{
					categoryConfigurations["Benjamin"].isIncluded = false;
				}

				benjaminEditButton.Enabled = false;
			}
		}

		private void benjaminEditButton_Click(object sender, EventArgs e)
		{
			KlokanCategoryBatch benjaminBatch;
			bool isAdd = false;

			if (categoryConfigurations.ContainsKey("Benjamin"))
			{
				benjaminBatch = categoryConfigurations["Benjamin"].batch;
			}
			else
			{
				benjaminBatch = new KlokanCategoryBatch { CategoryName = "Benjamin" };
				isAdd = true;
			}

			CategoryEditForm form = new CategoryEditForm(benjaminBatch);
			form.StartPosition = FormStartPosition.CenterScreen;
			var dialogResult = form.ShowDialog();

			if (dialogResult == DialogResult.OK && isAdd)
			{
				categoryConfigurations["Benjamin"] = new CategoryBatchConfig { batch = benjaminBatch, isIncluded = true };
			}
		}

		private void menuButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
