using System;
using System.Windows.Forms;

using System.Threading;
using System.Globalization;
using System.ComponentModel;

namespace KlokanUI
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

			if (Thread.CurrentThread.CurrentUICulture.Name == "cs-CZ")
			{
				czechRadioButton.Checked = true;
			}
			else
			{
				englishRadioButton.Checked = true;
			}
		}

		/// <summary>
		/// Hides this form and creates and shows the evaluation form.
		/// Also registers a callback at the evaluation form closing event which makes this form show up again.
		/// </summary>
		private void evaluateButton_Click(object sender, EventArgs e)
		{
			EvaluationForm evaluationForm = new EvaluationForm();
			evaluationForm.StartPosition = FormStartPosition.CenterScreen;
			evaluationForm.FormClosed += delegate { this.Show(); };
			evaluationForm.Show();
			this.Hide();
		}

		/// <summary>
		/// Hides this form and creates and shows the database form.
		/// Also registers a callback at the database form closing event which makes this form show up again.
		/// </summary>
		private void databaseButton_Click(object sender, EventArgs e)
		{
			DatabaseForm databaseForm = new DatabaseForm();
			databaseForm.StartPosition = FormStartPosition.CenterScreen;
			databaseForm.FormClosed += delegate { this.Show(); };
			databaseForm.Show();
			this.Hide();
		}

		/// <summary>
		/// Hides this form and creates and shows the test form.
		/// Also registers a callback at the test form closing event which makes this form show up again.
		/// </summary>
		private void testButton_Click(object sender, EventArgs e)
		{
			TestForm testForm = new TestForm();
			testForm.StartPosition = FormStartPosition.CenterScreen;
			testForm.FormClosed += delegate { this.Show(); };
			testForm.Show();
			this.Hide();
		}

		private void englishRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (englishRadioButton.Checked == true)
			{
				// there is no localization for en-GB, so this will fall back to default (which is in English...)
				Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
				UpdateUIControls();
			}
		}

		private void czechRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (czechRadioButton.Checked == true)
			{
				Thread.CurrentThread.CurrentUICulture = new CultureInfo("cs-CZ");
				UpdateUIControls();
			}
		}

		/// <summary>
		/// Re-applies resources to all controls ensuring proper localization according to the current UI culture.
		/// </summary>
		private void UpdateUIControls()
		{
			ComponentResourceManager resourceManager = new ComponentResourceManager(typeof(MainForm));

			foreach (Control control in this.Controls)
			{
				resourceManager.ApplyResources(control, control.Name);
			}
		}
	}
}
