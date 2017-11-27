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
			resources.ApplyResources(this.sheetLabel, "sheetLabel");
			this.sheetLabel.Name = "sheetLabel";
			// 
			// answerSheetsListBox
			// 
			resources.ApplyResources(this.answerSheetsListBox, "answerSheetsListBox");
			this.answerSheetsListBox.FormattingEnabled = true;
			this.answerSheetsListBox.Name = "answerSheetsListBox";
			this.answerSheetsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			// 
			// addButton
			// 
			resources.ApplyResources(this.addButton, "addButton");
			this.addButton.Name = "addButton";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// removeButton
			// 
			resources.ApplyResources(this.removeButton, "removeButton");
			this.removeButton.Name = "removeButton";
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// openFilesDialog
			// 
			resources.ApplyResources(this.openFilesDialog, "openFilesDialog");
			this.openFilesDialog.Multiselect = true;
			// 
			// saveButton
			// 
			resources.ApplyResources(this.saveButton, "saveButton");
			this.saveButton.Name = "saveButton";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// table3PictureBox
			// 
			this.table3PictureBox.Image = global::KlokanUI.Properties.Resources.answerTable3Image;
			resources.ApplyResources(this.table3PictureBox, "table3PictureBox");
			this.table3PictureBox.Name = "table3PictureBox";
			this.table3PictureBox.TabStop = false;
			this.table3PictureBox.Click += new System.EventHandler(this.table3PictureBox_Click);
			// 
			// table2PictureBox
			// 
			this.table2PictureBox.Image = global::KlokanUI.Properties.Resources.answerTable2Image;
			resources.ApplyResources(this.table2PictureBox, "table2PictureBox");
			this.table2PictureBox.Name = "table2PictureBox";
			this.table2PictureBox.TabStop = false;
			this.table2PictureBox.Click += new System.EventHandler(this.table2PictureBox_Click);
			// 
			// table1PictureBox
			// 
			this.table1PictureBox.Image = global::KlokanUI.Properties.Resources.answerTable1Image;
			resources.ApplyResources(this.table1PictureBox, "table1PictureBox");
			this.table1PictureBox.Name = "table1PictureBox";
			this.table1PictureBox.TabStop = false;
			this.table1PictureBox.Click += new System.EventHandler(this.table1PictureBox_Click);
			// 
			// CategoryEditForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.table3PictureBox);
			this.Controls.Add(this.table2PictureBox);
			this.Controls.Add(this.table1PictureBox);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.answerSheetsListBox);
			this.Controls.Add(this.sheetLabel);
			this.Name = "CategoryEditForm";
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