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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.evaluateButton = new System.Windows.Forms.Button();
			this.databaseButton = new System.Windows.Forms.Button();
			this.testButton = new System.Windows.Forms.Button();
			this.welcomeLabel = new System.Windows.Forms.Label();
			this.instructionLabel = new System.Windows.Forms.Label();
			this.czechRadioButton = new System.Windows.Forms.RadioButton();
			this.englishRadioButton = new System.Windows.Forms.RadioButton();
			this.SuspendLayout();
			// 
			// evaluateButton
			// 
			resources.ApplyResources(this.evaluateButton, "evaluateButton");
			this.evaluateButton.Name = "evaluateButton";
			this.evaluateButton.UseVisualStyleBackColor = true;
			this.evaluateButton.Click += new System.EventHandler(this.evaluateButton_Click);
			// 
			// databaseButton
			// 
			resources.ApplyResources(this.databaseButton, "databaseButton");
			this.databaseButton.Name = "databaseButton";
			this.databaseButton.UseVisualStyleBackColor = true;
			this.databaseButton.Click += new System.EventHandler(this.databaseButton_Click);
			// 
			// testButton
			// 
			resources.ApplyResources(this.testButton, "testButton");
			this.testButton.Name = "testButton";
			this.testButton.UseVisualStyleBackColor = true;
			this.testButton.Click += new System.EventHandler(this.testButton_Click);
			// 
			// welcomeLabel
			// 
			resources.ApplyResources(this.welcomeLabel, "welcomeLabel");
			this.welcomeLabel.Name = "welcomeLabel";
			// 
			// instructionLabel
			// 
			resources.ApplyResources(this.instructionLabel, "instructionLabel");
			this.instructionLabel.Name = "instructionLabel";
			// 
			// czechRadioButton
			// 
			resources.ApplyResources(this.czechRadioButton, "czechRadioButton");
			this.czechRadioButton.Name = "czechRadioButton";
			this.czechRadioButton.TabStop = true;
			this.czechRadioButton.UseVisualStyleBackColor = true;
			this.czechRadioButton.CheckedChanged += new System.EventHandler(this.czechRadioButton_CheckedChanged);
			// 
			// englishRadioButton
			// 
			resources.ApplyResources(this.englishRadioButton, "englishRadioButton");
			this.englishRadioButton.Name = "englishRadioButton";
			this.englishRadioButton.TabStop = true;
			this.englishRadioButton.UseVisualStyleBackColor = true;
			this.englishRadioButton.CheckedChanged += new System.EventHandler(this.englishRadioButton_CheckedChanged);
			// 
			// MainForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.englishRadioButton);
			this.Controls.Add(this.czechRadioButton);
			this.Controls.Add(this.instructionLabel);
			this.Controls.Add(this.welcomeLabel);
			this.Controls.Add(this.testButton);
			this.Controls.Add(this.databaseButton);
			this.Controls.Add(this.evaluateButton);
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button evaluateButton;
		private System.Windows.Forms.Button databaseButton;
		private System.Windows.Forms.Button testButton;
		private System.Windows.Forms.Label welcomeLabel;
		private System.Windows.Forms.Label instructionLabel;
		private System.Windows.Forms.RadioButton czechRadioButton;
		private System.Windows.Forms.RadioButton englishRadioButton;
	}
}