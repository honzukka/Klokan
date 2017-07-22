namespace KlokanUI
{
	partial class Form1
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
			this.sheetTextBox = new System.Windows.Forms.TextBox();
			this.sheetLabel = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.outputTextBox = new System.Windows.Forms.TextBox();
			this.correctSheetLabel = new System.Windows.Forms.Label();
			this.correctSheetTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// sheetTextBox
			// 
			this.sheetTextBox.Location = new System.Drawing.Point(38, 93);
			this.sheetTextBox.Name = "sheetTextBox";
			this.sheetTextBox.Size = new System.Drawing.Size(177, 20);
			this.sheetTextBox.TabIndex = 0;
			// 
			// sheetLabel
			// 
			this.sheetLabel.AutoSize = true;
			this.sheetLabel.Location = new System.Drawing.Point(36, 77);
			this.sheetLabel.Name = "sheetLabel";
			this.sheetLabel.Size = new System.Drawing.Size(76, 13);
			this.sheetLabel.TabIndex = 2;
			this.sheetLabel.Text = "Answer Sheet:";
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.button1.Location = new System.Drawing.Point(361, 36);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(136, 77);
			this.button1.TabIndex = 4;
			this.button1.Text = "Go!";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// outputTextBox
			// 
			this.outputTextBox.Location = new System.Drawing.Point(38, 163);
			this.outputTextBox.Multiline = true;
			this.outputTextBox.Name = "outputTextBox";
			this.outputTextBox.ReadOnly = true;
			this.outputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.outputTextBox.Size = new System.Drawing.Size(459, 417);
			this.outputTextBox.TabIndex = 5;
			// 
			// correctSheetLabel
			// 
			this.correctSheetLabel.AutoSize = true;
			this.correctSheetLabel.Location = new System.Drawing.Point(35, 20);
			this.correctSheetLabel.Name = "correctSheetLabel";
			this.correctSheetLabel.Size = new System.Drawing.Size(113, 13);
			this.correctSheetLabel.TabIndex = 6;
			this.correctSheetLabel.Text = "Correct Answer Sheet:";
			// 
			// correctSheetTextBox
			// 
			this.correctSheetTextBox.Location = new System.Drawing.Point(38, 36);
			this.correctSheetTextBox.Name = "correctSheetTextBox";
			this.correctSheetTextBox.Size = new System.Drawing.Size(177, 20);
			this.correctSheetTextBox.TabIndex = 7;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(541, 610);
			this.Controls.Add(this.correctSheetTextBox);
			this.Controls.Add(this.correctSheetLabel);
			this.Controls.Add(this.outputTextBox);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.sheetLabel);
			this.Controls.Add(this.sheetTextBox);
			this.Name = "Form1";
			this.Text = "Klokan";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox sheetTextBox;
		private System.Windows.Forms.Label sheetLabel;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox outputTextBox;
		private System.Windows.Forms.Label correctSheetLabel;
		private System.Windows.Forms.TextBox correctSheetTextBox;
	}
}

