namespace KlokanUI
{
	partial class CategoryEditForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CategoryEditForm));
			this.openCorrectFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.correctSheetTextBox = new System.Windows.Forms.TextBox();
			this.searchButton = new System.Windows.Forms.Button();
			this.sheetLabel = new System.Windows.Forms.Label();
			this.answerSheetsListBox = new System.Windows.Forms.ListBox();
			this.addButton = new System.Windows.Forms.Button();
			this.removeButton = new System.Windows.Forms.Button();
			this.openFilesDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveButton = new System.Windows.Forms.Button();
			this.correctAnswerSheetRadioButton = new System.Windows.Forms.RadioButton();
			this.correctAnswerPickerRadioButton = new System.Windows.Forms.RadioButton();
			this.table1PictureBox = new System.Windows.Forms.PictureBox();
			this.table2PictureBox = new System.Windows.Forms.PictureBox();
			this.table3PictureBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.table1PictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.table2PictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.table3PictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// openCorrectFileDialog
			// 
			this.openCorrectFileDialog.Filter = "JPEG Files|(*.jpg;*.jpeg;*JPG;*.JPEG)|All Files|*.*";
			// 
			// correctSheetTextBox
			// 
			this.correctSheetTextBox.Enabled = false;
			this.correctSheetTextBox.Location = new System.Drawing.Point(39, 37);
			this.correctSheetTextBox.Name = "correctSheetTextBox";
			this.correctSheetTextBox.Size = new System.Drawing.Size(395, 20);
			this.correctSheetTextBox.TabIndex = 3;
			// 
			// searchButton
			// 
			this.searchButton.Enabled = false;
			this.searchButton.Location = new System.Drawing.Point(440, 34);
			this.searchButton.Name = "searchButton";
			this.searchButton.Size = new System.Drawing.Size(97, 23);
			this.searchButton.TabIndex = 4;
			this.searchButton.Text = "Search";
			this.searchButton.UseVisualStyleBackColor = true;
			this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
			// 
			// sheetLabel
			// 
			this.sheetLabel.AutoSize = true;
			this.sheetLabel.Location = new System.Drawing.Point(553, 17);
			this.sheetLabel.Name = "sheetLabel";
			this.sheetLabel.Size = new System.Drawing.Size(96, 13);
			this.sheetLabel.TabIndex = 5;
			this.sheetLabel.Text = "Answer Sheets (0):";
			// 
			// answerSheetsListBox
			// 
			this.answerSheetsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.answerSheetsListBox.FormattingEnabled = true;
			this.answerSheetsListBox.HorizontalScrollbar = true;
			this.answerSheetsListBox.Location = new System.Drawing.Point(554, 37);
			this.answerSheetsListBox.Name = "answerSheetsListBox";
			this.answerSheetsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.answerSheetsListBox.Size = new System.Drawing.Size(452, 485);
			this.answerSheetsListBox.TabIndex = 6;
			// 
			// addButton
			// 
			this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.addButton.Location = new System.Drawing.Point(1012, 35);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(87, 23);
			this.addButton.TabIndex = 7;
			this.addButton.Text = "Add";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// removeButton
			// 
			this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.removeButton.Location = new System.Drawing.Point(1012, 64);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(87, 23);
			this.removeButton.TabIndex = 8;
			this.removeButton.Text = "Remove";
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// openFilesDialog
			// 
			this.openFilesDialog.Filter = "JPEG Files|(*.jpg;*.jpeg;*JPG;*.JPEG)|All Files|*.*";
			this.openFilesDialog.Multiselect = true;
			// 
			// saveButton
			// 
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveButton.Location = new System.Drawing.Point(902, 549);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(104, 23);
			this.saveButton.TabIndex = 9;
			this.saveButton.Text = "Save Category";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// correctAnswerSheetRadioButton
			// 
			this.correctAnswerSheetRadioButton.AutoSize = true;
			this.correctAnswerSheetRadioButton.Location = new System.Drawing.Point(39, 15);
			this.correctAnswerSheetRadioButton.Name = "correctAnswerSheetRadioButton";
			this.correctAnswerSheetRadioButton.Size = new System.Drawing.Size(131, 17);
			this.correctAnswerSheetRadioButton.TabIndex = 10;
			this.correctAnswerSheetRadioButton.Text = "Correct Answer Sheet:";
			this.correctAnswerSheetRadioButton.UseVisualStyleBackColor = true;
			this.correctAnswerSheetRadioButton.CheckedChanged += new System.EventHandler(this.correctAnswerSheetRadioButton_CheckedChanged);
			// 
			// correctAnswerPickerRadioButton
			// 
			this.correctAnswerPickerRadioButton.AutoSize = true;
			this.correctAnswerPickerRadioButton.Location = new System.Drawing.Point(39, 85);
			this.correctAnswerPickerRadioButton.Name = "correctAnswerPickerRadioButton";
			this.correctAnswerPickerRadioButton.Size = new System.Drawing.Size(144, 17);
			this.correctAnswerPickerRadioButton.TabIndex = 11;
			this.correctAnswerPickerRadioButton.Text = "Choose Correct Answers:";
			this.correctAnswerPickerRadioButton.UseVisualStyleBackColor = true;
			this.correctAnswerPickerRadioButton.CheckedChanged += new System.EventHandler(this.correctAnswerPickerRadioButton_CheckedChanged);
			// 
			// table1PictureBox
			// 
			this.table1PictureBox.Enabled = false;
			this.table1PictureBox.Image = ((System.Drawing.Image)(resources.GetObject("table1PictureBox.Image")));
			this.table1PictureBox.Location = new System.Drawing.Point(39, 108);
			this.table1PictureBox.MaximumSize = new System.Drawing.Size(241, 225);
			this.table1PictureBox.MinimumSize = new System.Drawing.Size(241, 225);
			this.table1PictureBox.Name = "table1PictureBox";
			this.table1PictureBox.Size = new System.Drawing.Size(241, 225);
			this.table1PictureBox.TabIndex = 12;
			this.table1PictureBox.TabStop = false;
			this.table1PictureBox.Visible = false;
			this.table1PictureBox.Click += new System.EventHandler(this.table1PictureBox_Click);
			// 
			// table2PictureBox
			// 
			this.table2PictureBox.Enabled = false;
			this.table2PictureBox.Image = ((System.Drawing.Image)(resources.GetObject("table2PictureBox.Image")));
			this.table2PictureBox.Location = new System.Drawing.Point(296, 108);
			this.table2PictureBox.MaximumSize = new System.Drawing.Size(241, 225);
			this.table2PictureBox.MinimumSize = new System.Drawing.Size(241, 225);
			this.table2PictureBox.Name = "table2PictureBox";
			this.table2PictureBox.Size = new System.Drawing.Size(241, 225);
			this.table2PictureBox.TabIndex = 13;
			this.table2PictureBox.TabStop = false;
			this.table2PictureBox.Visible = false;
			this.table2PictureBox.Click += new System.EventHandler(this.table2PictureBox_Click);
			// 
			// table3PictureBox
			// 
			this.table3PictureBox.Enabled = false;
			this.table3PictureBox.Image = ((System.Drawing.Image)(resources.GetObject("table3PictureBox.Image")));
			this.table3PictureBox.Location = new System.Drawing.Point(39, 347);
			this.table3PictureBox.MaximumSize = new System.Drawing.Size(241, 225);
			this.table3PictureBox.MinimumSize = new System.Drawing.Size(241, 225);
			this.table3PictureBox.Name = "table3PictureBox";
			this.table3PictureBox.Size = new System.Drawing.Size(241, 225);
			this.table3PictureBox.TabIndex = 14;
			this.table3PictureBox.TabStop = false;
			this.table3PictureBox.Visible = false;
			this.table3PictureBox.Click += new System.EventHandler(this.table3PictureBox_Click);
			// 
			// CategoryEditForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1111, 597);
			this.Controls.Add(this.table3PictureBox);
			this.Controls.Add(this.table2PictureBox);
			this.Controls.Add(this.table1PictureBox);
			this.Controls.Add(this.correctAnswerPickerRadioButton);
			this.Controls.Add(this.correctAnswerSheetRadioButton);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.answerSheetsListBox);
			this.Controls.Add(this.sheetLabel);
			this.Controls.Add(this.searchButton);
			this.Controls.Add(this.correctSheetTextBox);
			this.MinimumSize = new System.Drawing.Size(827, 636);
			this.Name = "CategoryEditForm";
			this.Text = "Klokan - Category Edit";
			((System.ComponentModel.ISupportInitialize)(this.table1PictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.table2PictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.table3PictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.OpenFileDialog openCorrectFileDialog;
		private System.Windows.Forms.TextBox correctSheetTextBox;
		private System.Windows.Forms.Button searchButton;
		private System.Windows.Forms.Label sheetLabel;
		private System.Windows.Forms.ListBox answerSheetsListBox;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.OpenFileDialog openFilesDialog;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.RadioButton correctAnswerSheetRadioButton;
		private System.Windows.Forms.RadioButton correctAnswerPickerRadioButton;
		private System.Windows.Forms.PictureBox table1PictureBox;
		private System.Windows.Forms.PictureBox table2PictureBox;
		private System.Windows.Forms.PictureBox table3PictureBox;
	}
}