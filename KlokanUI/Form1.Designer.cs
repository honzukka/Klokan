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
			this.goButton = new System.Windows.Forms.Button();
			this.outputTextBox = new System.Windows.Forms.TextBox();
			this.correctSheetLabel = new System.Windows.Forms.Label();
			this.correctSheetTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// goButton
			// 
			this.goButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.goButton.Location = new System.Drawing.Point(39, 57);
			this.goButton.Name = "goButton";
			this.goButton.Size = new System.Drawing.Size(177, 22);
			this.goButton.TabIndex = 9;
			this.goButton.Text = "Go!";
			this.goButton.UseVisualStyleBackColor = true;
			this.goButton.Click += new System.EventHandler(this.goButton_Click);
			// 
			// outputTextBox
			// 
			this.outputTextBox.Location = new System.Drawing.Point(240, 12);
			this.outputTextBox.Multiline = true;
			this.outputTextBox.Name = "outputTextBox";
			this.outputTextBox.ReadOnly = true;
			this.outputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.outputTextBox.Size = new System.Drawing.Size(459, 579);
			this.outputTextBox.TabIndex = 8;
			// 
			// correctSheetLabel
			// 
			this.correctSheetLabel.AutoSize = true;
			this.correctSheetLabel.Location = new System.Drawing.Point(36, 15);
			this.correctSheetLabel.Name = "correctSheetLabel";
			this.correctSheetLabel.Size = new System.Drawing.Size(113, 13);
			this.correctSheetLabel.TabIndex = 0;
			this.correctSheetLabel.Text = "Correct Answer Sheet:";
			// 
			// correctSheetTextBox
			// 
			this.correctSheetTextBox.Location = new System.Drawing.Point(39, 31);
			this.correctSheetTextBox.Name = "correctSheetTextBox";
			this.correctSheetTextBox.Size = new System.Drawing.Size(177, 20);
			this.correctSheetTextBox.TabIndex = 1;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(731, 603);
			this.Controls.Add(this.correctSheetTextBox);
			this.Controls.Add(this.correctSheetLabel);
			this.Controls.Add(this.outputTextBox);
			this.Controls.Add(this.goButton);
			this.Name = "Form1";
			this.Text = "Klokan";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button goButton;
		private System.Windows.Forms.TextBox outputTextBox;
		private System.Windows.Forms.Label correctSheetLabel;
		private System.Windows.Forms.TextBox correctSheetTextBox;
	}
}

