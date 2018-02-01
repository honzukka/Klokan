using System;
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

		public ProgressDialog(CancellationTokenSource cts)
		{
			InitializeComponent();

			progressLabel.Text = "";
			resultLabel.Text = "";

			this.cts = cts;
			tasksCompleted = 0;
			totalTasks = -1;
			tasksCompletedLock = new object();

			FormClosing += (object sender, FormClosingEventArgs e) => cts.Cancel();
		}

		/// <summary>
		/// Returns a cancellation token which is tied to the form's controls.
		/// </summary>
		public CancellationToken GetCancellationToken()
		{
			return cts.Token;
		}

		/// <summary>
		/// Initialized the progress bar so that its maximum corresponds to the total number of tasks.
		/// This method is NOT thread-safe!
		/// </summary>
		/// <param name="value">The total number of tasks</param>
		public void SetTotalTasks(int value)
		{
			if (value < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(totalTasks) + " has to be a positive integer.");
			}

			totalTasks = value;

			if (progressBar.InvokeRequired)
			{
				progressBar.BeginInvoke(new Action(
					() => { progressBar.Maximum = totalTasks; }
				));
			}
			else
			{
				progressBar.Maximum = totalTasks;
			}
		}

		/// <summary>
		/// A thread-safe method which generates a UI event to increment the progress bar by one.
		/// </summary>
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

		public void SetProgressLabel(ProgressBarState state)
		{
			if (progressLabel.InvokeRequired)
			{
				progressLabel.BeginInvoke(new Action(
					() => {
						string text = "";

						// resources have to be accessed here because they need to suit the UI thread
						switch (state)
						{
							case ProgressBarState.Evaluating:
								text = Properties.Resources.ProgressBarLabelEvaluating;
								break;
							case ProgressBarState.Cancelling:
								text = Properties.Resources.ProgressBarLabelCancelling;
								break;
							case ProgressBarState.Saving:
								text = Properties.Resources.ProgressBarLabelSaving;
								break;
							case ProgressBarState.SavingTest:
								text = Properties.Resources.ProgressBarLabelSavingTest;
								break;
							case ProgressBarState.Done:
								text = Properties.Resources.ProgressBarLabelDone;
								break;
							case ProgressBarState.Cancelled:
								text = Properties.Resources.ProgressBarLabelCancelled;
								break;
						}

						progressLabel.Text = text;
					}	
				));
			}
			else
			{
				string text = "";

				switch (state)
				{
					case ProgressBarState.Evaluating:
						text = Properties.Resources.ProgressBarLabelEvaluating;
						break;
					case ProgressBarState.Cancelling:
						text = Properties.Resources.ProgressBarLabelCancelling;
						break;
					case ProgressBarState.Saving:
						text = Properties.Resources.ProgressBarLabelSaving;
						break;
					case ProgressBarState.SavingTest:
						text = Properties.Resources.ProgressBarLabelSavingTest;
						break;
					case ProgressBarState.Done:
						text = Properties.Resources.ProgressBarLabelDone;
						break;
					case ProgressBarState.Cancelled:
						text = Properties.Resources.ProgressBarLabelCancelled;
						break;
				}

				progressLabel.Text = text;
			}
		}

		/// <summary>
		/// In case the operation was successful, this method sets the result label to contain information about the operation.
		/// </summary>
		/// <param name="failedSheets">The number of sheets which failed to be evaluated.</param>
		/// <param name="evaluationTime">The duration of the evaluation in seconds.</param>
		/// <param name="databaseTime">The duration of the saving of results in seconds.</param>
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

		/// <summary>
		/// In case the operation failed, this method sets the result label to state just that.
		/// </summary>
		public void SetResultLabel()
		{
			if (resultLabel.InvokeRequired)
			{
				resultLabel.BeginInvoke(new Action(
					() => {
						string message = Properties.Resources.SummaryTextEvaluationCancelled;
						resultLabel.Text = message;
					}
				));
			}
			else
			{
				string message = Properties.Resources.SummaryTextEvaluationCancelled;
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
			progressLabel.Text = Properties.Resources.ProgressBarLabelCancelling;
			cts.Cancel();
			cancelButton.Enabled = false;
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
