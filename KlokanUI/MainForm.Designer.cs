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
			this.welcomeLabel = new System.Windows.Forms.Label();
			this.instructionLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// evaluateButton
			// 
			this.evaluateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.evaluateButton.Location = new System.Drawing.Point(62, 183);
			this.evaluateButton.Name = "evaluateButton";
			this.evaluateButton.Size = new System.Drawing.Size(154, 42);
			this.evaluateButton.TabIndex = 0;
			this.evaluateButton.Text = "Evaluation Module";
			this.evaluateButton.UseVisualStyleBackColor = true;
			this.evaluateButton.Click += new System.EventHandler(this.evaluateButton_Click);
			// 
			// databaseButton
			// 
			this.databaseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.databaseButton.Location = new System.Drawing.Point(222, 183);
			this.databaseButton.Name = "databaseButton";
			this.databaseButton.Size = new System.Drawing.Size(154, 42);
			this.databaseButton.TabIndex = 1;
			this.databaseButton.Text = "Database Module";
			this.databaseButton.UseVisualStyleBackColor = true;
			this.databaseButton.Click += new System.EventHandler(this.databaseButton_Click);
			// 
			// testButton
			// 
			this.testButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.testButton.Location = new System.Drawing.Point(382, 183);
			this.testButton.Name = "testButton";
			this.testButton.Size = new System.Drawing.Size(154, 42);
			this.testButton.TabIndex = 2;
			this.testButton.Text = "Test Module";
			this.testButton.UseVisualStyleBackColor = true;
			this.testButton.Click += new System.EventHandler(this.testButton_Click);
			// 
			// welcomeLabel
			// 
			this.welcomeLabel.AutoSize = true;
			this.welcomeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.welcomeLabel.Location = new System.Drawing.Point(117, 26);
			this.welcomeLabel.Name = "welcomeLabel";
			this.welcomeLabel.Size = new System.Drawing.Size(351, 31);
			this.welcomeLabel.TabIndex = 3;
			this.welcomeLabel.Text = "Hello, welcome to Klokan!";
			// 
			// instructionLabel
			// 
			this.instructionLabel.AutoSize = true;
			this.instructionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.instructionLabel.Location = new System.Drawing.Point(58, 132);
			this.instructionLabel.Name = "instructionLabel";
			this.instructionLabel.Size = new System.Drawing.Size(285, 20);
			this.instructionLabel.TabIndex = 4;
			this.instructionLabel.Text = "Choose one of our 3 amazing modules:";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(597, 277);
			this.Controls.Add(this.instructionLabel);
			this.Controls.Add(this.welcomeLabel);
			this.Controls.Add(this.testButton);
			this.Controls.Add(this.databaseButton);
			this.Controls.Add(this.evaluateButton);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(613, 316);
			this.MinimumSize = new System.Drawing.Size(613, 316);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Klokan";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button evaluateButton;
		private System.Windows.Forms.Button databaseButton;
		private System.Windows.Forms.Button testButton;
		private System.Windows.Forms.Label welcomeLabel;
		private System.Windows.Forms.Label instructionLabel;
	}
}