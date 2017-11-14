using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Imaging;
using System.Runtime.InteropServices;

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

		private void button1_Click(object sender, EventArgs e)
		{
			Image testImage = new Bitmap("C:/Users/Honza/source/repos/Klokan/scans/sheet1.jpeg");
			int testImageWidth = testImage.Width;
			int testImageHeight = testImage.Height;

			byte[] imageBytes = HelperFunctions.GetImageBytes("C:/Users/Honza/source/repos/Klokan/scans/sheet1.jpeg", ImageFormat.Bmp);

			unsafe
			{
				fixed (byte* testPtr = imageBytes)
				{
					TestWrapper.test_image_transfer(testPtr, testImageHeight, testImageWidth);
				}
			}
		}

		class TestWrapper
		{
			// unsafe!!!
			[DllImport("klokan.dll")]
			public unsafe static extern void test_image_transfer(byte* imagePtr, int rows, int cols);
		}
	}
}
