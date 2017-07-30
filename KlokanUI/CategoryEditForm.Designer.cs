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
			this.SuspendLayout();
			// 
			// correctSheetLabel
			// 
			this.correctSheetLabel.AutoSize = true;
			this.correctSheetLabel.Location = new System.Drawing.Point(24, 18);
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
			this.correctSheetTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.correctSheetTextBox.Location = new System.Drawing.Point(27, 34);
			this.correctSheetTextBox.Name = "correctSheetTextBox";
			this.correctSheetTextBox.Size = new System.Drawing.Size(489, 20);
			this.correctSheetTextBox.TabIndex = 3;
			// 
			// searchButton
			// 
			this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.searchButton.Location = new System.Drawing.Point(522, 31);
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
			this.sheetLabel.Location = new System.Drawing.Point(24, 57);
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
			this.answerSheetsListBox.Location = new System.Drawing.Point(27, 74);
			this.answerSheetsListBox.Name = "answerSheetsListBox";
			this.answerSheetsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.answerSheetsListBox.Size = new System.Drawing.Size(489, 277);
			this.answerSheetsListBox.TabIndex = 6;
			// 
			// addButton
			// 
			this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.addButton.Location = new System.Drawing.Point(523, 74);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(96, 23);
			this.addButton.TabIndex = 7;
			this.addButton.Text = "Add";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// removeButton
			// 
			this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.removeButton.Location = new System.Drawing.Point(522, 103);
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
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveButton.Location = new System.Drawing.Point(407, 392);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(109, 23);
			this.saveButton.TabIndex = 9;
			this.saveButton.Text = "Save Category";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// CategoryEditForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(646, 427);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.answerSheetsListBox);
			this.Controls.Add(this.sheetLabel);
			this.Controls.Add(this.searchButton);
			this.Controls.Add(this.correctSheetTextBox);
			this.Controls.Add(this.correctSheetLabel);
			this.MinimumSize = new System.Drawing.Size(308, 313);
			this.Name = "CategoryEditForm";
			this.Text = "Klokan - Category Edit";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
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
	}
}