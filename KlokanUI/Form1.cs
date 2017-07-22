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
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			outputTextBox.Text = "";

			Parameters parameters = new Parameters();
			parameters.SetDefaultValues();

			Evaluator evaluator = new Evaluator(parameters);

			string correctSheetFilename = correctSheetTextBox.Text;
			string sheetFilename = sheetTextBox.Text;

			if (!evaluator.LoadCorrectAnswers(correctSheetFilename))
			{
				outputTextBox.Text = "ERROR";
				return;
			}

			Result result = evaluator.Evaluate(sheetFilename);

			if (result.Error)
			{
				outputTextBox.Text = "ERROR";
				return;
			}

			for (int table = 0; table < parameters.TableCount; table++)
			{
				outputTextBox.Text += "Table " + (table + 1) + ":\r\n";

				for (int row = 0; row < parameters.TableRows - 1; row++)
				{
					for (int col = 0; col < parameters.TableColumns - 1; col++)
					{
						switch (result.CorrectedAnswers[table][row][col])
						{
							case AnswerType.Correct:
								outputTextBox.Text += "X\t";
								break;
							case AnswerType.Incorrect:
								outputTextBox.Text += "!\t";
								break;
							case AnswerType.Void:
								outputTextBox.Text += "\t";
								break;
							case AnswerType.Corrected:
								outputTextBox.Text += "O\t";
								break;
						}
					}

					outputTextBox.Text += "\r\n";
				}

				outputTextBox.Text += "\r\n";
			}

			outputTextBox.Text += "Score: " + result.Score;
		}
	}
}
