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
	public partial class EvaluationForm : Form, IEvaluationForm
	{
		class CategoryBatchConfig
		{
			public KlokanCategoryBatch batch;
			public bool isIncluded;
		}

		KlokanBatch klokanBatch;
		List<int> yearList;
		Dictionary<string, CategoryBatchConfig> categoryConfigurations;
		Parameters chosenParameters;

		public EvaluationForm()
		{
			InitializeComponent();

			benjaminEditButton.Enabled = false;
			kadetEditButton.Enabled = false;
			juniorEditButton.Enabled = false;
			studentEditButton.Enabled = false;

			editParamsButton.Enabled = false;

			// initialize the year combo box with a list of years
			yearList = new List<int>();
			for (int year = DateTime.Now.Year; year >= 2000; year--)
			{
				yearList.Add(year);
			}
			yearComboBox.DataSource = yearList;

			// assign default parameters to the batch
			chosenParameters = Parameters.CreateDefaultParameters();

			klokanBatch = new KlokanBatch();
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

			// reset the category batches in the final klokan batch
			klokanBatch.CategoryBatches = new Dictionary<string, KlokanCategoryBatch>();

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

			// assign the chosen year
			klokanBatch.Year = (int)(yearComboBox.SelectedItem);

			// assign the chosen parameters
			klokanBatch.Parameters = chosenParameters;

			evaluateButton.Enabled = false;

			var jobScheduler = new JobScheduler(klokanBatch, this);

			// new thread created, so that all tasks in it are planned in the threadpool and not in the WinForms synchronization context
			Thread thread = new Thread(jobScheduler.Run);
			thread.IsBackground = true;
			thread.Start();
		}

		private void benjaminCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			checkBoxHandle("Benjamin", benjaminCheckBox, benjaminEditButton);
		}

		private void benjaminEditButton_Click(object sender, EventArgs e)
		{
			editButtonHandle("Benjamin");
		}

		private void kadetCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			checkBoxHandle("Kadet", kadetCheckBox, kadetEditButton);
		}

		private void kadetEditButton_Click(object sender, EventArgs e)
		{
			editButtonHandle("Kadet");
		}

		private void juniorCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			checkBoxHandle("Junior", juniorCheckBox, juniorEditButton);
		}

		private void juniorEditButton_Click(object sender, EventArgs e)
		{
			editButtonHandle("Junior");
		}

		private void studentCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			checkBoxHandle("Student", studentCheckBox, studentEditButton);
		}

		private void studentEditButton_Click(object sender, EventArgs e)
		{
			editButtonHandle("Student");
		}

		private void defaultParamsRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (defaultParamsRadioButton.Checked)
			{
				chosenParameters = Parameters.CreateDefaultParameters();
			}
		}

		private void customParamsRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (customParamsRadioButton.Checked)
			{
				editParamsButton.Enabled = true;
			}
			else
			{
				editParamsButton.Enabled = false;
			}
		}

		private void editParamsButton_Click(object sender, EventArgs e)
		{
			ParameterEditForm form = new ParameterEditForm(chosenParameters);
			form.StartPosition = FormStartPosition.CenterScreen;

			if (form.ShowDialog() == DialogResult.OK)
			{
				chosenParameters = form.Parameters;
			}
		}

		/// <summary>
		/// Handles the checkBox_CheckedChanged event for all checkboxes in this form.
		/// Makes sure that a category is always configured when added into a selection.
		/// </summary>
		/// <param name="categoryName">Name of the category.</param>
		/// <param name="checkBox">A checkbox belonging to the category.</param>
		/// <param name="editButton">An edit button belonging to the category.</param>
		private void checkBoxHandle(string categoryName, CheckBox checkBox, Button editButton)
		{
			// checkbox was checked
			if (checkBox.Checked)
			{
				// if the category is already configured
				if (categoryConfigurations.ContainsKey(categoryName))
				{
					// just include it in the selection of categories for the final klokan batch
					categoryConfigurations[categoryName].isIncluded = true;
				}
				else
				{
					// open a configuration form in the form of a dialog
					KlokanCategoryBatch categoryBatch = new KlokanCategoryBatch { CategoryName = categoryName };
					CategoryEditForm form = new CategoryEditForm(categoryBatch);
					form.StartPosition = FormStartPosition.CenterScreen;

					if (form.ShowDialog() == DialogResult.OK)
					{
						// save the configuration
						categoryConfigurations[categoryName] = new CategoryBatchConfig { batch = categoryBatch, isIncluded = true };
					}
				}

				editButton.Enabled = true;
			}
			else
			{
				if (categoryConfigurations.ContainsKey(categoryName))
				{
					categoryConfigurations[categoryName].isIncluded = false;
				}

				editButton.Enabled = false;
			}
		}

		/// <summary>
		/// Handles the editButton_Clicked event for all category edit buttons in this form.
		/// Makes sure that correct data is displayed in the configuration form 
		/// and also saves a configuration in case an empty one is being edited.
		/// </summary>
		/// <param name="categoryName"></param>
		private void editButtonHandle(string categoryName)
		{
			KlokanCategoryBatch categoryBatch;
			bool isAdd = false;

			// if the category is already configured
			if (categoryConfigurations.ContainsKey(categoryName))
			{
				categoryBatch = categoryConfigurations[categoryName].batch;
			}
			else
			{
				// create a new one and set the flag saying that it will have to be saved when configured
				categoryBatch = new KlokanCategoryBatch { CategoryName = categoryName };
				isAdd = true;
			}

			// open a configuration form in the form of a dialog
			CategoryEditForm form = new CategoryEditForm(categoryBatch);
			form.StartPosition = FormStartPosition.CenterScreen;
			var dialogResult = form.ShowDialog();

			if (dialogResult == DialogResult.OK && isAdd)
			{
				// save the configuration
				categoryConfigurations[categoryName] = new CategoryBatchConfig { batch = categoryBatch, isIncluded = true };
			}
		}
	}
}
