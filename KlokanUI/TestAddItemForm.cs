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
	public partial class TestAddItemForm : Form
	{
		KlokanTestDBScan newScanItem;
		string scanFilePath;

		public TestAddItemForm(KlokanTestDBScan newScanItem)
		{
			InitializeComponent();

			filePathLabel.Text = "";

			this.newScanItem = newScanItem;
			scanFilePath = null;
		}

		private void chooseFileButton_Click(object sender, EventArgs e)
		{
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				scanFilePath = openFileDialog.FileName;

				filePathLabel.Text = scanFilePath;

				Image scanImage = new Bitmap(scanFilePath);
				scanPictureBox.Image = scanImage;
			}
		}
	}
}
