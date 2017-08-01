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
			this.evaluateButton = new System.Windows.Forms.Button();
			this.categoryLabel = new System.Windows.Forms.Label();
			this.benjaminCheckBox = new System.Windows.Forms.CheckBox();
			this.benjaminEditButton = new System.Windows.Forms.Button();
			this.kadetEditButton = new System.Windows.Forms.Button();
			this.kadetCheckBox = new System.Windows.Forms.CheckBox();
			this.juniorEditButton = new System.Windows.Forms.Button();
			this.juniorCheckBox = new System.Windows.Forms.CheckBox();
			this.studentEditButton = new System.Windows.Forms.Button();
			this.studentCheckBox = new System.Windows.Forms.CheckBox();
			this.parameterLabel = new System.Windows.Forms.Label();
			this.defaultParamsRadioButton = new System.Windows.Forms.RadioButton();
			this.customParamsRadioButton = new System.Windows.Forms.RadioButton();
			this.editParamsButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// evaluateButton
			// 
			this.evaluateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.evaluateButton.Location = new System.Drawing.Point(39, 245);
			this.evaluateButton.Name = "evaluateButton";
			this.evaluateButton.Size = new System.Drawing.Size(75, 23);
			this.evaluateButton.TabIndex = 9;
			this.evaluateButton.Text = "Evaluate";
			this.evaluateButton.UseVisualStyleBackColor = true;
			this.evaluateButton.Click += new System.EventHandler(this.evaluateButton_Click);
			// 
			// categoryLabel
			// 
			this.categoryLabel.AutoSize = true;
			this.categoryLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.categoryLabel.Location = new System.Drawing.Point(35, 30);
			this.categoryLabel.Name = "categoryLabel";
			this.categoryLabel.Size = new System.Drawing.Size(108, 13);
			this.categoryLabel.TabIndex = 11;
			this.categoryLabel.Text = "Configure Categories:";
			// 
			// benjaminCheckBox
			// 
			this.benjaminCheckBox.AutoSize = true;
			this.benjaminCheckBox.Location = new System.Drawing.Point(39, 71);
			this.benjaminCheckBox.Name = "benjaminCheckBox";
			this.benjaminCheckBox.Size = new System.Drawing.Size(69, 17);
			this.benjaminCheckBox.TabIndex = 12;
			this.benjaminCheckBox.Text = "Benjamin";
			this.benjaminCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.benjaminCheckBox.UseVisualStyleBackColor = true;
			this.benjaminCheckBox.CheckedChanged += new System.EventHandler(this.benjaminCheckBox_CheckedChanged);
			// 
			// benjaminEditButton
			// 
			this.benjaminEditButton.Location = new System.Drawing.Point(39, 94);
			this.benjaminEditButton.Name = "benjaminEditButton";
			this.benjaminEditButton.Size = new System.Drawing.Size(75, 23);
			this.benjaminEditButton.TabIndex = 13;
			this.benjaminEditButton.Text = "Edit";
			this.benjaminEditButton.UseVisualStyleBackColor = true;
			this.benjaminEditButton.Click += new System.EventHandler(this.benjaminEditButton_Click);
			// 
			// kadetEditButton
			// 
			this.kadetEditButton.Location = new System.Drawing.Point(120, 94);
			this.kadetEditButton.Name = "kadetEditButton";
			this.kadetEditButton.Size = new System.Drawing.Size(75, 23);
			this.kadetEditButton.TabIndex = 15;
			this.kadetEditButton.Text = "Edit";
			this.kadetEditButton.UseVisualStyleBackColor = true;
			this.kadetEditButton.Click += new System.EventHandler(this.kadetEditButton_Click);
			// 
			// kadetCheckBox
			// 
			this.kadetCheckBox.AutoSize = true;
			this.kadetCheckBox.Location = new System.Drawing.Point(120, 71);
			this.kadetCheckBox.Name = "kadetCheckBox";
			this.kadetCheckBox.Size = new System.Drawing.Size(54, 17);
			this.kadetCheckBox.TabIndex = 14;
			this.kadetCheckBox.Text = "Kadet";
			this.kadetCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.kadetCheckBox.UseVisualStyleBackColor = true;
			this.kadetCheckBox.CheckedChanged += new System.EventHandler(this.kadetCheckBox_CheckedChanged);
			// 
			// juniorEditButton
			// 
			this.juniorEditButton.Location = new System.Drawing.Point(201, 94);
			this.juniorEditButton.Name = "juniorEditButton";
			this.juniorEditButton.Size = new System.Drawing.Size(75, 23);
			this.juniorEditButton.TabIndex = 17;
			this.juniorEditButton.Text = "Edit";
			this.juniorEditButton.UseVisualStyleBackColor = true;
			this.juniorEditButton.Click += new System.EventHandler(this.juniorEditButton_Click);
			// 
			// juniorCheckBox
			// 
			this.juniorCheckBox.AutoSize = true;
			this.juniorCheckBox.Location = new System.Drawing.Point(201, 71);
			this.juniorCheckBox.Name = "juniorCheckBox";
			this.juniorCheckBox.Size = new System.Drawing.Size(54, 17);
			this.juniorCheckBox.TabIndex = 16;
			this.juniorCheckBox.Text = "Junior";
			this.juniorCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.juniorCheckBox.UseVisualStyleBackColor = true;
			this.juniorCheckBox.CheckedChanged += new System.EventHandler(this.juniorCheckBox_CheckedChanged);
			// 
			// studentEditButton
			// 
			this.studentEditButton.Location = new System.Drawing.Point(282, 94);
			this.studentEditButton.Name = "studentEditButton";
			this.studentEditButton.Size = new System.Drawing.Size(75, 23);
			this.studentEditButton.TabIndex = 19;
			this.studentEditButton.Text = "Edit";
			this.studentEditButton.UseVisualStyleBackColor = true;
			this.studentEditButton.Click += new System.EventHandler(this.studentEditButton_Click);
			// 
			// studentCheckBox
			// 
			this.studentCheckBox.AutoSize = true;
			this.studentCheckBox.Location = new System.Drawing.Point(282, 71);
			this.studentCheckBox.Name = "studentCheckBox";
			this.studentCheckBox.Size = new System.Drawing.Size(63, 17);
			this.studentCheckBox.TabIndex = 18;
			this.studentCheckBox.Text = "Student";
			this.studentCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.studentCheckBox.UseVisualStyleBackColor = true;
			this.studentCheckBox.CheckedChanged += new System.EventHandler(this.studentCheckBox_CheckedChanged);
			// 
			// parameterLabel
			// 
			this.parameterLabel.AutoSize = true;
			this.parameterLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.parameterLabel.Location = new System.Drawing.Point(36, 147);
			this.parameterLabel.Name = "parameterLabel";
			this.parameterLabel.Size = new System.Drawing.Size(82, 13);
			this.parameterLabel.TabIndex = 20;
			this.parameterLabel.Text = "Set Parameters:";
			// 
			// defaultParamsRadioButton
			// 
			this.defaultParamsRadioButton.AutoSize = true;
			this.defaultParamsRadioButton.Checked = true;
			this.defaultParamsRadioButton.Location = new System.Drawing.Point(40, 182);
			this.defaultParamsRadioButton.Name = "defaultParamsRadioButton";
			this.defaultParamsRadioButton.Size = new System.Drawing.Size(59, 17);
			this.defaultParamsRadioButton.TabIndex = 22;
			this.defaultParamsRadioButton.TabStop = true;
			this.defaultParamsRadioButton.Text = "Default";
			this.defaultParamsRadioButton.UseVisualStyleBackColor = true;
			this.defaultParamsRadioButton.CheckedChanged += new System.EventHandler(this.defaultParamsRadioButton_CheckedChanged);
			// 
			// customParamsRadioButton
			// 
			this.customParamsRadioButton.AutoSize = true;
			this.customParamsRadioButton.Location = new System.Drawing.Point(282, 182);
			this.customParamsRadioButton.Name = "customParamsRadioButton";
			this.customParamsRadioButton.Size = new System.Drawing.Size(60, 17);
			this.customParamsRadioButton.TabIndex = 23;
			this.customParamsRadioButton.TabStop = true;
			this.customParamsRadioButton.Text = "Custom";
			this.customParamsRadioButton.UseVisualStyleBackColor = true;
			this.customParamsRadioButton.CheckedChanged += new System.EventHandler(this.customParamsRadioButton_CheckedChanged);
			// 
			// editParamsButton
			// 
			this.editParamsButton.Location = new System.Drawing.Point(282, 205);
			this.editParamsButton.Name = "editParamsButton";
			this.editParamsButton.Size = new System.Drawing.Size(75, 23);
			this.editParamsButton.TabIndex = 24;
			this.editParamsButton.Text = "Edit";
			this.editParamsButton.UseVisualStyleBackColor = true;
			this.editParamsButton.Click += new System.EventHandler(this.editParamsButton_Click);
			// 
			// EvaluationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(397, 310);
			this.Controls.Add(this.editParamsButton);
			this.Controls.Add(this.customParamsRadioButton);
			this.Controls.Add(this.defaultParamsRadioButton);
			this.Controls.Add(this.parameterLabel);
			this.Controls.Add(this.studentEditButton);
			this.Controls.Add(this.studentCheckBox);
			this.Controls.Add(this.juniorEditButton);
			this.Controls.Add(this.juniorCheckBox);
			this.Controls.Add(this.kadetEditButton);
			this.Controls.Add(this.kadetCheckBox);
			this.Controls.Add(this.benjaminEditButton);
			this.Controls.Add(this.benjaminCheckBox);
			this.Controls.Add(this.categoryLabel);
			this.Controls.Add(this.evaluateButton);
			this.MaximumSize = new System.Drawing.Size(413, 349);
			this.MinimumSize = new System.Drawing.Size(413, 349);
			this.Name = "EvaluationForm";
			this.Text = "Klokan - Evaluation";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button evaluateButton;
		private System.Windows.Forms.Label categoryLabel;
		private System.Windows.Forms.CheckBox benjaminCheckBox;
		private System.Windows.Forms.Button benjaminEditButton;
		private System.Windows.Forms.Button kadetEditButton;
		private System.Windows.Forms.CheckBox kadetCheckBox;
		private System.Windows.Forms.Button juniorEditButton;
		private System.Windows.Forms.CheckBox juniorCheckBox;
		private System.Windows.Forms.Button studentEditButton;
		private System.Windows.Forms.CheckBox studentCheckBox;
		private System.Windows.Forms.Label parameterLabel;
		private System.Windows.Forms.RadioButton defaultParamsRadioButton;
		private System.Windows.Forms.RadioButton customParamsRadioButton;
		private System.Windows.Forms.Button editParamsButton;
	}
}

