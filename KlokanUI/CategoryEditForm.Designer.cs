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
			this.sheetLabel = new System.Windows.Forms.Label();
			this.answerSheetsListBox = new System.Windows.Forms.ListBox();
			this.addButton = new System.Windows.Forms.Button();
			this.removeButton = new System.Windows.Forms.Button();
			this.openFilesDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.table3PictureBox = new System.Windows.Forms.PictureBox();
			this.table2PictureBox = new System.Windows.Forms.PictureBox();
			this.table1PictureBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.table3PictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.table2PictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.table1PictureBox)).BeginInit();
			this.SuspendLayout();
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
			this.answerSheetsListBox.Size = new System.Drawing.Size(459, 459);
			this.answerSheetsListBox.TabIndex = 6;
			// 
			// addButton
			// 
			this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.addButton.Location = new System.Drawing.Point(1019, 35);
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
			this.removeButton.Location = new System.Drawing.Point(1019, 64);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(87, 23);
			this.removeButton.TabIndex = 8;
			this.removeButton.Text = "Remove";
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// openFilesDialog
			// 
			this.openFilesDialog.Filter = "JPEG Files|*.jpg;*.jpeg;*JPG;*.JPEG|PNG Files|*.png;*.PNG";
			this.openFilesDialog.Multiselect = true;
			// 
			// saveButton
			// 
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveButton.Location = new System.Drawing.Point(909, 518);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(104, 23);
			this.saveButton.TabIndex = 9;
			this.saveButton.Text = "Save Category";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(36, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(124, 13);
			this.label1.TabIndex = 15;
			this.label1.Text = "Choose correct answers:";
			// 
			// table3PictureBox
			// 
			this.table3PictureBox.Image = global::KlokanUI.Properties.Resources.answerTable3Image;
			this.table3PictureBox.Location = new System.Drawing.Point(39, 276);
			this.table3PictureBox.MaximumSize = new System.Drawing.Size(241, 225);
			this.table3PictureBox.MinimumSize = new System.Drawing.Size(241, 225);
			this.table3PictureBox.Name = "table3PictureBox";
			this.table3PictureBox.Size = new System.Drawing.Size(241, 225);
			this.table3PictureBox.TabIndex = 14;
			this.table3PictureBox.TabStop = false;
			this.table3PictureBox.Click += new System.EventHandler(this.table3PictureBox_Click);
			// 
			// table2PictureBox
			// 
			this.table2PictureBox.Image = global::KlokanUI.Properties.Resources.answerTable2Image;
			this.table2PictureBox.Location = new System.Drawing.Point(296, 37);
			this.table2PictureBox.MaximumSize = new System.Drawing.Size(241, 225);
			this.table2PictureBox.MinimumSize = new System.Drawing.Size(241, 225);
			this.table2PictureBox.Name = "table2PictureBox";
			this.table2PictureBox.Size = new System.Drawing.Size(241, 225);
			this.table2PictureBox.TabIndex = 13;
			this.table2PictureBox.TabStop = false;
			this.table2PictureBox.Click += new System.EventHandler(this.table2PictureBox_Click);
			// 
			// table1PictureBox
			// 
			this.table1PictureBox.Image = global::KlokanUI.Properties.Resources.answerTable1Image;
			this.table1PictureBox.Location = new System.Drawing.Point(39, 37);
			this.table1PictureBox.MaximumSize = new System.Drawing.Size(241, 225);
			this.table1PictureBox.MinimumSize = new System.Drawing.Size(241, 225);
			this.table1PictureBox.Name = "table1PictureBox";
			this.table1PictureBox.Size = new System.Drawing.Size(241, 225);
			this.table1PictureBox.TabIndex = 12;
			this.table1PictureBox.TabStop = false;
			this.table1PictureBox.Click += new System.EventHandler(this.table1PictureBox_Click);
			// 
			// CategoryEditForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1118, 567);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.table3PictureBox);
			this.Controls.Add(this.table2PictureBox);
			this.Controls.Add(this.table1PictureBox);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.answerSheetsListBox);
			this.Controls.Add(this.sheetLabel);
			this.MinimumSize = new System.Drawing.Size(830, 560);
			this.Name = "CategoryEditForm";
			this.Text = "Klokan - Evaluation - Category Edit";
			((System.ComponentModel.ISupportInitialize)(this.table3PictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.table2PictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.table1PictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label sheetLabel;
		private System.Windows.Forms.ListBox answerSheetsListBox;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.OpenFileDialog openFilesDialog;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.PictureBox table1PictureBox;
		private System.Windows.Forms.PictureBox table2PictureBox;
		private System.Windows.Forms.PictureBox table3PictureBox;
		private System.Windows.Forms.Label label1;
	}
}