namespace KlokanUI
{
	partial class MainForm
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
			this.evaluateButton = new System.Windows.Forms.Button();
			this.databaseButton = new System.Windows.Forms.Button();
			this.testButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// evaluateButton
			// 
			this.evaluateButton.Location = new System.Drawing.Point(118, 110);
			this.evaluateButton.Name = "evaluateButton";
			this.evaluateButton.Size = new System.Drawing.Size(118, 23);
			this.evaluateButton.TabIndex = 0;
			this.evaluateButton.Text = "Evaluation Module";
			this.evaluateButton.UseVisualStyleBackColor = true;
			this.evaluateButton.Click += new System.EventHandler(this.evaluateButton_Click);
			// 
			// databaseButton
			// 
			this.databaseButton.Location = new System.Drawing.Point(118, 139);
			this.databaseButton.Name = "databaseButton";
			this.databaseButton.Size = new System.Drawing.Size(118, 23);
			this.databaseButton.TabIndex = 1;
			this.databaseButton.Text = "Database Module";
			this.databaseButton.UseVisualStyleBackColor = true;
			this.databaseButton.Click += new System.EventHandler(this.databaseButton_Click);
			// 
			// testButton
			// 
			this.testButton.Location = new System.Drawing.Point(118, 168);
			this.testButton.Name = "testButton";
			this.testButton.Size = new System.Drawing.Size(118, 23);
			this.testButton.TabIndex = 2;
			this.testButton.Text = "Test Module";
			this.testButton.UseVisualStyleBackColor = true;
			this.testButton.Click += new System.EventHandler(this.testButton_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(361, 318);
			this.Controls.Add(this.testButton);
			this.Controls.Add(this.databaseButton);
			this.Controls.Add(this.evaluateButton);
			this.MaximumSize = new System.Drawing.Size(377, 357);
			this.MinimumSize = new System.Drawing.Size(377, 357);
			this.Name = "MainForm";
			this.Text = "Klokan";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button evaluateButton;
		private System.Windows.Forms.Button databaseButton;
		private System.Windows.Forms.Button testButton;
	}
}