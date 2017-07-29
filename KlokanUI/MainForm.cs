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
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Hides this form and creates and shows an evaluation form.
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
	}
}
