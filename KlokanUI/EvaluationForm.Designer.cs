namespace KlokanUI
{
	partial class EvaluationForm
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
			this.goButton = new System.Windows.Forms.Button();
			this.categoryBatchListBox = new System.Windows.Forms.ListBox();
			this.listBoxLabel = new System.Windows.Forms.Label();
			this.listBoxAddButton = new System.Windows.Forms.Button();
			this.listBoxEditButton = new System.Windows.Forms.Button();
			this.listBoxRemoveButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// goButton
			// 
			this.goButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.goButton.Location = new System.Drawing.Point(436, 412);
			this.goButton.Name = "goButton";
			this.goButton.Size = new System.Drawing.Size(177, 116);
			this.goButton.TabIndex = 9;
			this.goButton.Text = "Go!";
			this.goButton.UseVisualStyleBackColor = true;
			this.goButton.Click += new System.EventHandler(this.goButton_Click);
			// 
			// categoryBatchListBox
			// 
			this.categoryBatchListBox.FormattingEnabled = true;
			this.categoryBatchListBox.Location = new System.Drawing.Point(38, 47);
			this.categoryBatchListBox.Name = "categoryBatchListBox";
			this.categoryBatchListBox.Size = new System.Drawing.Size(228, 186);
			this.categoryBatchListBox.TabIndex = 10;
			// 
			// listBoxLabel
			// 
			this.listBoxLabel.AutoSize = true;
			this.listBoxLabel.Location = new System.Drawing.Point(35, 31);
			this.listBoxLabel.Name = "listBoxLabel";
			this.listBoxLabel.Size = new System.Drawing.Size(112, 13);
			this.listBoxLabel.TabIndex = 11;
			this.listBoxLabel.Text = "Categories to process:";
			// 
			// listBoxAddButton
			// 
			this.listBoxAddButton.Location = new System.Drawing.Point(272, 47);
			this.listBoxAddButton.Name = "listBoxAddButton";
			this.listBoxAddButton.Size = new System.Drawing.Size(75, 23);
			this.listBoxAddButton.TabIndex = 12;
			this.listBoxAddButton.Text = "Add";
			this.listBoxAddButton.UseVisualStyleBackColor = true;
			this.listBoxAddButton.Click += new System.EventHandler(this.listBoxAddButton_Click);
			// 
			// listBoxEditButton
			// 
			this.listBoxEditButton.Location = new System.Drawing.Point(272, 76);
			this.listBoxEditButton.Name = "listBoxEditButton";
			this.listBoxEditButton.Size = new System.Drawing.Size(75, 23);
			this.listBoxEditButton.TabIndex = 13;
			this.listBoxEditButton.Text = "Edit";
			this.listBoxEditButton.UseVisualStyleBackColor = true;
			// 
			// listBoxRemoveButton
			// 
			this.listBoxRemoveButton.Location = new System.Drawing.Point(272, 105);
			this.listBoxRemoveButton.Name = "listBoxRemoveButton";
			this.listBoxRemoveButton.Size = new System.Drawing.Size(75, 23);
			this.listBoxRemoveButton.TabIndex = 14;
			this.listBoxRemoveButton.Text = "Remove";
			this.listBoxRemoveButton.UseVisualStyleBackColor = true;
			// 
			// EvaluationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(731, 603);
			this.Controls.Add(this.listBoxRemoveButton);
			this.Controls.Add(this.listBoxEditButton);
			this.Controls.Add(this.listBoxAddButton);
			this.Controls.Add(this.listBoxLabel);
			this.Controls.Add(this.categoryBatchListBox);
			this.Controls.Add(this.goButton);
			this.Name = "EvaluationForm";
			this.Text = "Klokan - Evaluation";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button goButton;
		private System.Windows.Forms.ListBox categoryBatchListBox;
		private System.Windows.Forms.Label listBoxLabel;
		private System.Windows.Forms.Button listBoxAddButton;
		private System.Windows.Forms.Button listBoxEditButton;
		private System.Windows.Forms.Button listBoxRemoveButton;
	}
}

