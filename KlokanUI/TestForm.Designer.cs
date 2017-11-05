﻿namespace KlokanUI
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
			this.dataView = new System.Windows.Forms.DataGridView();
			this.idColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.correctnessColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.addItemButton = new System.Windows.Forms.Button();
			this.removeItemButton = new System.Windows.Forms.Button();
			this.editParamsButton = new System.Windows.Forms.Button();
			this.evaluateButton = new System.Windows.Forms.Button();
			this.viewItemButton = new System.Windows.Forms.Button();
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
            this.idColumn,
            this.correctnessColumn});
			this.dataView.Location = new System.Drawing.Point(43, 92);
			this.dataView.Name = "dataView";
			this.dataView.Size = new System.Drawing.Size(289, 385);
			this.dataView.TabIndex = 0;
			// 
			// idColumn
			// 
			this.idColumn.HeaderText = "ID";
			this.idColumn.Name = "idColumn";
			this.idColumn.ReadOnly = true;
			// 
			// correctnessColumn
			// 
			this.correctnessColumn.HeaderText = "Correctness";
			this.correctnessColumn.Name = "correctnessColumn";
			this.correctnessColumn.ReadOnly = true;
			// 
			// addItemButton
			// 
			this.addItemButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.addItemButton.Location = new System.Drawing.Point(360, 92);
			this.addItemButton.Name = "addItemButton";
			this.addItemButton.Size = new System.Drawing.Size(86, 23);
			this.addItemButton.TabIndex = 1;
			this.addItemButton.Text = "Add Item";
			this.addItemButton.UseVisualStyleBackColor = true;
			this.addItemButton.Click += new System.EventHandler(this.addItemButton_Click);
			// 
			// removeItemButton
			// 
			this.removeItemButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.removeItemButton.Location = new System.Drawing.Point(360, 121);
			this.removeItemButton.Name = "removeItemButton";
			this.removeItemButton.Size = new System.Drawing.Size(86, 23);
			this.removeItemButton.TabIndex = 2;
			this.removeItemButton.Text = "Remove Item";
			this.removeItemButton.UseVisualStyleBackColor = true;
			// 
			// editParamsButton
			// 
			this.editParamsButton.Location = new System.Drawing.Point(43, 34);
			this.editParamsButton.Name = "editParamsButton";
			this.editParamsButton.Size = new System.Drawing.Size(102, 23);
			this.editParamsButton.TabIndex = 3;
			this.editParamsButton.Text = "Edit Parameters";
			this.editParamsButton.UseVisualStyleBackColor = true;
			// 
			// evaluateButton
			// 
			this.evaluateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.evaluateButton.Location = new System.Drawing.Point(360, 34);
			this.evaluateButton.Name = "evaluateButton";
			this.evaluateButton.Size = new System.Drawing.Size(86, 23);
			this.evaluateButton.TabIndex = 4;
			this.evaluateButton.Text = "Evaluate";
			this.evaluateButton.UseVisualStyleBackColor = true;
			// 
			// viewItemButton
			// 
			this.viewItemButton.Location = new System.Drawing.Point(360, 150);
			this.viewItemButton.Name = "viewItemButton";
			this.viewItemButton.Size = new System.Drawing.Size(86, 23);
			this.viewItemButton.TabIndex = 5;
			this.viewItemButton.Text = "View Item";
			this.viewItemButton.UseVisualStyleBackColor = true;
			// 
			// TestForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(492, 532);
			this.Controls.Add(this.viewItemButton);
			this.Controls.Add(this.evaluateButton);
			this.Controls.Add(this.editParamsButton);
			this.Controls.Add(this.removeItemButton);
			this.Controls.Add(this.addItemButton);
			this.Controls.Add(this.dataView);
			this.MinimumSize = new System.Drawing.Size(479, 372);
			this.Name = "TestForm";
			this.Text = "Klokan - Test";
			((System.ComponentModel.ISupportInitialize)(this.dataView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dataView;
		private System.Windows.Forms.DataGridViewTextBoxColumn idColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn correctnessColumn;
		private System.Windows.Forms.Button addItemButton;
		private System.Windows.Forms.Button removeItemButton;
		private System.Windows.Forms.Button editParamsButton;
		private System.Windows.Forms.Button evaluateButton;
		private System.Windows.Forms.Button viewItemButton;
	}
}