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
			Parameters parameters = new Parameters();
			parameters.SetDefaultValues();

			string filename = textBox1.Text;
			AnswerWrapper answerWrapper = new AnswerWrapper();
			bool success = false;

			unsafe
			{
				bool* answersPtr = answerWrapper.answers;
				bool* successPtr = &success;

				NativeAPIWrapper.extract_answers_api(filename, parameters, answersPtr, successPtr);

				if (!success)
				{
					textBox3.Text = "ERROR!";
					return;
				}

				for (int table = 0; table < 3; table++)
				{
					textBox3.Text += "Table " + (table + 1) + ":\r\n";

					for (int row = 0; row < 8; row++)
					{
						for (int col = 0; col < 5; col++)
						{
							if (answersPtr[table * 8 * 5 + row * 5 + col] == true)
							{
								textBox3.Text += "X ";
							}
							else
							{
								textBox3.Text += "  ";
							}
						}

						textBox3.Text += "\r\n";
					}
				}
			}
		}
	}
}
