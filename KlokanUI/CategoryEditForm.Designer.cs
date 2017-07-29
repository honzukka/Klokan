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
			this.nameLabel = new System.Windows.Forms.Label();
			this.correctSheetLabel = new System.Windows.Forms.Label();
			this.openCorrectFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.correctSheetTextBox = new System.Windows.Forms.TextBox();
			this.searchButton = new System.Windows.Forms.Button();
			this.sheetLabel = new System.Windows.Forms.Label();
			this.answerSheetsListBox = new System.Windows.Forms.ListBox();
			this.addButton = new System.Windows.Forms.Button();
			this.removeButton = new System.Windows.Forms.Button();
			this.openFilesDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveButton = new System.Windows.Forms.Button();
			this.categoryComboBox = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Location = new System.Drawing.Point(27, 33);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(83, 13);
			this.nameLabel.TabIndex = 1;
			this.nameLabel.Text = "Category Name:";
			// 
			// correctSheetLabel
			// 
			this.correctSheetLabel.AutoSize = true;
			this.correctSheetLabel.Location = new System.Drawing.Point(27, 72);
			this.correctSheetLabel.Name = "correctSheetLabel";
			this.correctSheetLabel.Size = new System.Drawing.Size(113, 13);
			this.correctSheetLabel.TabIndex = 2;
			this.correctSheetLabel.Text = "Correct Answer Sheet:";
			// 
			// openCorrectFileDialog
			// 
			this.openCorrectFileDialog.Filter = "JPEG Files|(*.jpg;*.jpeg;*JPG;*.JPEG)|All Files|*.*";
			// 
			// correctSheetTextBox
			// 
			this.correctSheetTextBox.Location = new System.Drawing.Point(30, 88);
			this.correctSheetTextBox.Name = "correctSheetTextBox";
			this.correctSheetTextBox.Size = new System.Drawing.Size(285, 20);
			this.correctSheetTextBox.TabIndex = 3;
			// 
			// searchButton
			// 
			this.searchButton.Location = new System.Drawing.Point(321, 85);
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
			this.sheetLabel.Location = new System.Drawing.Point(27, 111);
			this.sheetLabel.Name = "sheetLabel";
			this.sheetLabel.Size = new System.Drawing.Size(96, 13);
			this.sheetLabel.TabIndex = 5;
			this.sheetLabel.Text = "Answer Sheets (0):";
			// 
			// answerSheetsListBox
			// 
			this.answerSheetsListBox.FormattingEnabled = true;
			this.answerSheetsListBox.HorizontalScrollbar = true;
			this.answerSheetsListBox.Location = new System.Drawing.Point(30, 128);
			this.answerSheetsListBox.Name = "answerSheetsListBox";
			this.answerSheetsListBox.Size = new System.Drawing.Size(285, 225);
			this.answerSheetsListBox.TabIndex = 6;
			// 
			// addButton
			// 
			this.addButton.Location = new System.Drawing.Point(322, 128);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(96, 23);
			this.addButton.TabIndex = 7;
			this.addButton.Text = "Add";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// removeButton
			// 
			this.removeButton.Location = new System.Drawing.Point(321, 157);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(97, 23);
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
			this.saveButton.Location = new System.Drawing.Point(169, 382);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(117, 30);
			this.saveButton.TabIndex = 9;
			this.saveButton.Text = "Save Category";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// categoryComboBox
			// 
			this.categoryComboBox.FormattingEnabled = true;
			this.categoryComboBox.Location = new System.Drawing.Point(30, 50);
			this.categoryComboBox.Name = "categoryComboBox";
			this.categoryComboBox.Size = new System.Drawing.Size(285, 21);
			this.categoryComboBox.TabIndex = 10;
			// 
			// CategoryEditForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(454, 424);
			this.Controls.Add(this.categoryComboBox);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.answerSheetsListBox);
			this.Controls.Add(this.sheetLabel);
			this.Controls.Add(this.searchButton);
			this.Controls.Add(this.correctSheetTextBox);
			this.Controls.Add(this.correctSheetLabel);
			this.Controls.Add(this.nameLabel);
			this.Name = "CategoryEditForm";
			this.Text = "Klokan - Category Edit";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.Label correctSheetLabel;
		private System.Windows.Forms.OpenFileDialog openCorrectFileDialog;
		private System.Windows.Forms.TextBox correctSheetTextBox;
		private System.Windows.Forms.Button searchButton;
		private System.Windows.Forms.Label sheetLabel;
		private System.Windows.Forms.ListBox answerSheetsListBox;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.OpenFileDialog openFilesDialog;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.ComboBox categoryComboBox;
	}
}