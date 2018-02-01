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
	public partial class ProgressDialog : Form
	{
		CancellationTokenSource cts;
		int tasksCompleted;
		int totalTasks;

		object tasksCompletedLock;

		// TODO: cts.Cancel() on form exit too!
		public ProgressDialog(CancellationTokenSource cts)
		{
			InitializeComponent();

			progressLabel.Text = "";
			resultLabel.Text = "";

			this.cts = cts;
			tasksCompleted = 0;
			totalTasks = -1;
			tasksCompletedLock = new object();
		}

		// this method isn't thread-safe!
		public void SetTotalTasks(int value)
		{
			if (value < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(totalTasks) + " has to be a positive integer.");
			}

			totalTasks = value;
		}

		public void IncrementProgressBarValue()
		{
			if (totalTasks == -1)
			{
				throw new InvalidOperationException(nameof(totalTasks) + " variable hasn't been set.");
			}

			lock (tasksCompletedLock)
			{
				if (tasksCompleted == totalTasks)
				{
					throw new InvalidOperationException("Trying to increment value beyond maximum.");
				}

				tasksCompleted++;
			}

			// the following operation will be always be performed in the UI thread
			if (progressBar.InvokeRequired)
			{
				progressBar.BeginInvoke(new Action(
					() => { progressBar.Value++; }
				));
			}
			else
			{
				progressBar.Value++;
			}
		}

		public void SetProgressLabel(string text)
		{
			if (progressLabel.InvokeRequired)
			{
				progressLabel.BeginInvoke(new Action(
					() => { progressLabel.Text = text; }	
				));
			}
			else
			{
				progressLabel.Text = text;
			}
		}

		public void SetResultLabel(int failedSheets, double evaluationTime, double databaseTime)
		{
			if (resultLabel.InvokeRequired)
			{
				resultLabel.BeginInvoke(new Action(
					() => {
						string message = EvaluationHandling.CreateSummaryMessage(failedSheets, evaluationTime, databaseTime);
						resultLabel.Text = message;
					}
				));
			}
			else
			{
				string message = EvaluationHandling.CreateSummaryMessage(failedSheets, evaluationTime, databaseTime);
				resultLabel.Text = message;
			}
		}

		public void EnableOkButton()
		{
			if (okButton.InvokeRequired)
			{
				okButton.BeginInvoke(new Action(
					() => { okButton.Enabled = true; }	
				));
			}
			else
			{
				okButton.Enabled = true;
			}
		}

		public void DisableCancelButton()
		{
			if (cancelButton.InvokeRequired)
			{
				cancelButton.BeginInvoke(new Action(
					() => { cancelButton.Enabled = false; }
				));
			}
			else
			{
				cancelButton.Enabled = false;
			}
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			cts.Cancel();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
