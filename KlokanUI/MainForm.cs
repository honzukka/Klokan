using System;
using System.Windows.Forms;

namespace KlokanUI
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
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
	}
}
