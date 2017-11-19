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
			this.dataView.AllowUserToAddRows = false;
			this.dataView.AllowUserToDeleteRows = false;
			this.dataView.AllowUserToResizeRows = false;
			this.dataView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.studentNumber,
            this.year,
            this.category,
            this.points});
			this.dataView.Location = new System.Drawing.Point(48, 86);
			this.dataView.MultiSelect = false;
			this.dataView.Name = "dataView";
			this.dataView.ReadOnly = true;
			this.dataView.Size = new System.Drawing.Size(546, 483);
			this.dataView.TabIndex = 0;
			this.dataView.Click += new System.EventHandler(this.dataView_Click);
			// 
			// id
			// 
			this.id.HeaderText = "ID";
			this.id.Name = "id";
			this.id.ReadOnly = true;
			// 
			// studentNumber
			// 
			this.studentNumber.HeaderText = "Student Number";
			this.studentNumber.Name = "studentNumber";
			this.studentNumber.ReadOnly = true;
			// 
			// year
			// 
			this.year.HeaderText = "Year";
			this.year.Name = "year";
			this.year.ReadOnly = true;
			// 
			// category
			// 
			this.category.HeaderText = "Category";
			this.category.Name = "category";
			this.category.ReadOnly = true;
			// 
			// points
			// 
			this.points.HeaderText = "Points";
			this.points.Name = "points";
			this.points.ReadOnly = true;
			// 
			// yearLabel
			// 
			this.yearLabel.AutoSize = true;
			this.yearLabel.Location = new System.Drawing.Point(48, 13);
			this.yearLabel.Name = "yearLabel";
			this.yearLabel.Size = new System.Drawing.Size(63, 13);
			this.yearLabel.TabIndex = 1;
			this.yearLabel.Text = "Select year:";
			// 
			// yearComboBox
			// 
			this.yearComboBox.FormattingEnabled = true;
			this.yearComboBox.Location = new System.Drawing.Point(51, 29);
			this.yearComboBox.Name = "yearComboBox";
			this.yearComboBox.Size = new System.Drawing.Size(90, 21);
			this.yearComboBox.TabIndex = 2;
			// 
			// categoryLabel
			// 
			this.categoryLabel.AutoSize = true;
			this.categoryLabel.Location = new System.Drawing.Point(190, 13);
			this.categoryLabel.Name = "categoryLabel";
			this.categoryLabel.Size = new System.Drawing.Size(84, 13);
			this.categoryLabel.TabIndex = 3;
			this.categoryLabel.Text = "Select category:";
			// 
			// categoryComboBox
			// 
			this.categoryComboBox.FormattingEnabled = true;
			this.categoryComboBox.Location = new System.Drawing.Point(193, 28);
			this.categoryComboBox.Name = "categoryComboBox";
			this.categoryComboBox.Size = new System.Drawing.Size(127, 21);
			this.categoryComboBox.TabIndex = 4;
			// 
			// viewButton
			// 
			this.viewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.viewButton.Location = new System.Drawing.Point(631, 26);
			this.viewButton.Name = "viewButton";
			this.viewButton.Size = new System.Drawing.Size(108, 23);
			this.viewButton.TabIndex = 5;
			this.viewButton.Text = "View Selection";
			this.viewButton.UseVisualStyleBackColor = true;
			this.viewButton.Click += new System.EventHandler(this.viewButton_Click);
			// 
			// detailButton
			// 
			this.detailButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.detailButton.Enabled = false;
			this.detailButton.Location = new System.Drawing.Point(631, 86);
			this.detailButton.Name = "detailButton";
			this.detailButton.Size = new System.Drawing.Size(108, 23);
			this.detailButton.TabIndex = 6;
			this.detailButton.Text = "View Detail";
			this.detailButton.UseVisualStyleBackColor = true;
			this.detailButton.Click += new System.EventHandler(this.detailButton_Click);
			// 
			// exportSelectionButton
			// 
			this.exportSelectionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.exportSelectionButton.Location = new System.Drawing.Point(631, 546);
			this.exportSelectionButton.Name = "exportSelectionButton";
			this.exportSelectionButton.Size = new System.Drawing.Size(108, 23);
			this.exportSelectionButton.TabIndex = 8;
			this.exportSelectionButton.Text = "Export Selection";
			this.exportSelectionButton.UseVisualStyleBackColor = true;
			this.exportSelectionButton.Click += new System.EventHandler(this.exportSelectionButton_Click);
			// 
			// saveFileDialogExport
			// 
			this.saveFileDialogExport.Filter = "CSV File|*.csv";
			// 
			// DatabaseForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(769, 601);
			this.Controls.Add(this.exportSelectionButton);
			this.Controls.Add(this.detailButton);
			this.Controls.Add(this.viewButton);
			this.Controls.Add(this.categoryComboBox);
			this.Controls.Add(this.categoryLabel);
			this.Controls.Add(this.yearComboBox);
			this.Controls.Add(this.yearLabel);
			this.Controls.Add(this.dataView);
			this.MinimumSize = new System.Drawing.Size(785, 640);
			this.Name = "DatabaseForm";
			this.Text = "Klokan - Database";
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