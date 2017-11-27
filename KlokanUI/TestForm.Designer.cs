namespace KlokanUI
{
	partial class TestForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestForm));
			this.dataView = new System.Windows.Forms.DataGridView();
			this.addItemButton = new System.Windows.Forms.Button();
			this.removeItemButton = new System.Windows.Forms.Button();
			this.editParamsButton = new System.Windows.Forms.Button();
			this.evaluateButton = new System.Windows.Forms.Button();
			this.viewItemButton = new System.Windows.Forms.Button();
			this.averageCorrectnessLabel = new System.Windows.Forms.Label();
			this.averageCorrectnessValueLabel = new System.Windows.Forms.Label();
			this.idColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.correctnessColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.idColumn,
            this.correctnessColumn});
			this.dataView.MultiSelect = false;
			this.dataView.Name = "dataView";
			this.dataView.ReadOnly = true;
			this.dataView.Click += new System.EventHandler(this.dataView_Click);
			// 
			// addItemButton
			// 
			resources.ApplyResources(this.addItemButton, "addItemButton");
			this.addItemButton.Name = "addItemButton";
			this.addItemButton.UseVisualStyleBackColor = true;
			this.addItemButton.Click += new System.EventHandler(this.addItemButton_Click);
			// 
			// removeItemButton
			// 
			resources.ApplyResources(this.removeItemButton, "removeItemButton");
			this.removeItemButton.Name = "removeItemButton";
			this.removeItemButton.UseVisualStyleBackColor = true;
			this.removeItemButton.Click += new System.EventHandler(this.removeItemButton_Click);
			// 
			// editParamsButton
			// 
			resources.ApplyResources(this.editParamsButton, "editParamsButton");
			this.editParamsButton.Name = "editParamsButton";
			this.editParamsButton.UseVisualStyleBackColor = true;
			this.editParamsButton.Click += new System.EventHandler(this.editParamsButton_Click);
			// 
			// evaluateButton
			// 
			resources.ApplyResources(this.evaluateButton, "evaluateButton");
			this.evaluateButton.Name = "evaluateButton";
			this.evaluateButton.UseVisualStyleBackColor = true;
			this.evaluateButton.Click += new System.EventHandler(this.evaluateButton_Click);
			// 
			// viewItemButton
			// 
			resources.ApplyResources(this.viewItemButton, "viewItemButton");
			this.viewItemButton.Name = "viewItemButton";
			this.viewItemButton.UseVisualStyleBackColor = true;
			this.viewItemButton.Click += new System.EventHandler(this.viewItemButton_Click);
			// 
			// averageCorrectnessLabel
			// 
			resources.ApplyResources(this.averageCorrectnessLabel, "averageCorrectnessLabel");
			this.averageCorrectnessLabel.Name = "averageCorrectnessLabel";
			// 
			// averageCorrectnessValueLabel
			// 
			resources.ApplyResources(this.averageCorrectnessValueLabel, "averageCorrectnessValueLabel");
			this.averageCorrectnessValueLabel.Name = "averageCorrectnessValueLabel";
			// 
			// idColumn
			// 
			resources.ApplyResources(this.idColumn, "idColumn");
			this.idColumn.Name = "idColumn";
			this.idColumn.ReadOnly = true;
			// 
			// correctnessColumn
			// 
			resources.ApplyResources(this.correctnessColumn, "correctnessColumn");
			this.correctnessColumn.Name = "correctnessColumn";
			this.correctnessColumn.ReadOnly = true;
			// 
			// TestForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.averageCorrectnessValueLabel);
			this.Controls.Add(this.averageCorrectnessLabel);
			this.Controls.Add(this.viewItemButton);
			this.Controls.Add(this.evaluateButton);
			this.Controls.Add(this.editParamsButton);
			this.Controls.Add(this.removeItemButton);
			this.Controls.Add(this.addItemButton);
			this.Controls.Add(this.dataView);
			this.Name = "TestForm";
			((System.ComponentModel.ISupportInitialize)(this.dataView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView dataView;
		private System.Windows.Forms.Button addItemButton;
		private System.Windows.Forms.Button removeItemButton;
		private System.Windows.Forms.Button editParamsButton;
		private System.Windows.Forms.Button evaluateButton;
		private System.Windows.Forms.Button viewItemButton;
		private System.Windows.Forms.Label averageCorrectnessLabel;
		private System.Windows.Forms.Label averageCorrectnessValueLabel;
		private System.Windows.Forms.DataGridViewTextBoxColumn idColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn correctnessColumn;
	}
}