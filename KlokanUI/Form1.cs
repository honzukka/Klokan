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
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		public void AddTextToTextbox(string text)
		{
			if (outputTextBox.InvokeRequired)
			{
				outputTextBox.BeginInvoke(new Action(
					() => { outputTextBox.AppendText(text); }	
				));
			}
			else
			{
				outputTextBox.AppendText(text);
			}
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
			outputTextBox.Text = "";
			goButton.Enabled = false;

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
				CategoryBatches = new List<KlokanCategoryBatch> {
										new KlokanCategoryBatch {
													Category = Category.Kadet,
													CorrectSheetFilename = correctSheetTextBox.Text,
													SheetFilenames = sheetFilenames
										}
				}
			};

			var jobScheduler = new JobScheduler(klokanBatch, this);

			// new thread created, so that all tasks in it are planned in the threadpool and not in the WinForms synchronization context
			Thread thread = new Thread(jobScheduler.Run);
			thread.IsBackground = true;
			thread.Start();
		}
	}
}
