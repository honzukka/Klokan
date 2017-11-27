namespace KlokanUI
{
	partial class DatabaseForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseForm));
			this.dataView = new System.Windows.Forms.DataGridView();
			this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.studentNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.year = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.category = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.points = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.yearLabel = new System.Windows.Forms.Label();
			this.yearComboBox = new System.Windows.Forms.ComboBox();
			this.categoryLabel = new System.Windows.Forms.Label();
			this.categoryComboBox = new System.Windows.Forms.ComboBox();
			this.viewButton = new System.Windows.Forms.Button();
			this.detailButton = new System.Windows.Forms.Button();
			this.exportSelectionButton = new System.Windows.Forms.Button();
			this.saveFileDialogExport = new System.Windows.Forms.SaveFileDialog();
			this.openDBDialog = new System.Windows.Forms.OpenFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.dataView)).BeginInit();
			this.SuspendLayout();
			// 
			// dataView
			// 
			resources.ApplyResources(this.dataView, "dataView");
			this.dataView.AllowUserToAddRows = false;
			this.dataView.AllowUserToDeleteRows = false;
			this.dataView.AllowUserToResizeRows = false;
			this.dataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.studentNumber,
            this.year,
            this.category,
            this.points});
			this.dataView.MultiSelect = false;
			this.dataView.Name = "dataView";
			this.dataView.ReadOnly = true;
			this.dataView.Click += new System.EventHandler(this.dataView_Click);
			// 
			// id
			// 
			resources.ApplyResources(this.id, "id");
			this.id.Name = "id";
			this.id.ReadOnly = true;
			// 
			// studentNumber
			// 
			resources.ApplyResources(this.studentNumber, "studentNumber");
			this.studentNumber.Name = "studentNumber";
			this.studentNumber.ReadOnly = true;
			// 
			// year
			// 
			resources.ApplyResources(this.year, "year");
			this.year.Name = "year";
			this.year.ReadOnly = true;
			// 
			// category
			// 
			resources.ApplyResources(this.category, "category");
			this.category.Name = "category";
			this.category.ReadOnly = true;
			// 
			// points
			// 
			resources.ApplyResources(this.points, "points");
			this.points.Name = "points";
			this.points.ReadOnly = true;
			// 
			// yearLabel
			// 
			resources.ApplyResources(this.yearLabel, "yearLabel");
			this.yearLabel.Name = "yearLabel";
			// 
			// yearComboBox
			// 
			resources.ApplyResources(this.yearComboBox, "yearComboBox");
			this.yearComboBox.FormattingEnabled = true;
			this.yearComboBox.Name = "yearComboBox";
			// 
			// categoryLabel
			// 
			resources.ApplyResources(this.categoryLabel, "categoryLabel");
			this.categoryLabel.Name = "categoryLabel";
			// 
			// categoryComboBox
			// 
			resources.ApplyResources(this.categoryComboBox, "categoryComboBox");
			this.categoryComboBox.FormattingEnabled = true;
			this.categoryComboBox.Name = "categoryComboBox";
			// 
			// viewButton
			// 
			resources.ApplyResources(this.viewButton, "viewButton");
			this.viewButton.Name = "viewButton";
			this.viewButton.UseVisualStyleBackColor = true;
			this.viewButton.Click += new System.EventHandler(this.viewButton_Click);
			// 
			// detailButton
			// 
			resources.ApplyResources(this.detailButton, "detailButton");
			this.detailButton.Name = "detailButton";
			this.detailButton.UseVisualStyleBackColor = true;
			this.detailButton.Click += new System.EventHandler(this.detailButton_Click);
			// 
			// exportSelectionButton
			// 
			resources.ApplyResources(this.exportSelectionButton, "exportSelectionButton");
			this.exportSelectionButton.Name = "exportSelectionButton";
			this.exportSelectionButton.UseVisualStyleBackColor = true;
			this.exportSelectionButton.Click += new System.EventHandler(this.exportSelectionButton_Click);
			// 
			// saveFileDialogExport
			// 
			resources.ApplyResources(this.saveFileDialogExport, "saveFileDialogExport");
			// 
			// openDBDialog
			// 
			resources.ApplyResources(this.openDBDialog, "openDBDialog");
			// 
			// DatabaseForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.exportSelectionButton);
			this.Controls.Add(this.detailButton);
			this.Controls.Add(this.viewButton);
			this.Controls.Add(this.categoryComboBox);
			this.Controls.Add(this.categoryLabel);
			this.Controls.Add(this.yearComboBox);
			this.Controls.Add(this.yearLabel);
			this.Controls.Add(this.dataView);
			this.Name = "DatabaseForm";
			((System.ComponentModel.ISupportInitialize)(this.dataView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView dataView;
		private System.Windows.Forms.Label yearLabel;
		private System.Windows.Forms.ComboBox yearComboBox;
		private System.Windows.Forms.Label categoryLabel;
		private System.Windows.Forms.ComboBox categoryComboBox;
		private System.Windows.Forms.Button viewButton;
		private System.Windows.Forms.Button detailButton;
		private System.Windows.Forms.Button exportSelectionButton;
		private System.Windows.Forms.SaveFileDialog saveFileDialogExport;
		private System.Windows.Forms.OpenFileDialog openDBDialog;
		private System.Windows.Forms.DataGridViewTextBoxColumn id;
		private System.Windows.Forms.DataGridViewTextBoxColumn studentNumber;
		private System.Windows.Forms.DataGridViewTextBoxColumn year;
		private System.Windows.Forms.DataGridViewTextBoxColumn category;
		private System.Windows.Forms.DataGridViewTextBoxColumn points;
	}
}