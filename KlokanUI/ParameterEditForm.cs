using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KlokanUI
{
	public partial class ParameterEditForm : Form
	{
		ToolTip toolTip;
		internal Parameters Parameters { get; set; }

		internal ParameterEditForm(Parameters currentParameters, string formText)
		{
			InitializeComponent();

			this.Text = formText;

			Parameters = currentParameters;
			FillTextBoxes();

			toolTip = new ToolTip();
			SetToolTips();
		}

		private void SetToolTips()
		{
			// labels
			toolTip.SetToolTip(defaultSheetWidthLabel, "Every sheet will be resized accordingly (preserving aspect ratio) before the tables are extracted.");
			toolTip.SetToolTip(blackWhiteThresholdLabel, "How bright a shade of grey can be to be recognized as black (the rest will be white).");

			toolTip.SetToolTip(tableCountLabel, "The number of table in the answer sheet.");
			toolTip.SetToolTip(tableLineEccetricityLimitLabel, "How slanted a line can be to be recognized as either horizontal or vertical (in radians).");
			toolTip.SetToolTip(tableLineCurvatureLimitLabel, "How curved a line can be to still be recognized as a straight line (1 is minimum).");
			toolTip.SetToolTip(studentTableRowsLabel, "The number of rows in the student number table.");
			toolTip.SetToolTip(studentTableColumnsLabel, "The number of columns in the student number table.");
			toolTip.SetToolTip(answerTableRowsLabel, "The number of rows in an answer table.");
			toolTip.SetToolTip(answerTableColumnsLabel, "The number of columns in an answer table.");
			toolTip.SetToolTip(resizedCellWidthLabel, "This is the width of a cell after the sheet has been resized (see default_sheet_width).");
			toolTip.SetToolTip(resizedCellHeightLabel, "This is the height of a cell after the sheet has been resized (see default_sheet_height).");

			toolTip.SetToolTip(defaultCellWidthLabel, "Every cell will be resized to have this width.");
			toolTip.SetToolTip(defaultCellHeightLabel, "Every cell will be resized to have this height.");
			toolTip.SetToolTip(cellEvaluationTypeLabel, "TRUE - shape recognition; FALSE - pixel ratio");

			toolTip.SetToolTip(crossLineLengthLabel, "The length of lines to be detected.");
			toolTip.SetToolTip(crossLineCurvatureLimitLabel, "How curved a line can be to still be recognized as a straight line (1 is minimum).");
			toolTip.SetToolTip(rubbishLinesLimitLabel, "Amount of lines that don't form a cross that will be ignored and not considered as a correction.");

			toolTip.SetToolTip(lowerThresholdLabel, "If the ratio of pixels representing student input in the whole cell is lower than this, answer was not chosen.");
			toolTip.SetToolTip(upperThresholdLabel, "If the ratio of pixels representing student input in the whole cell is higher than this, answer was invalidated.");
		}

		private void FillTextBoxes()
		{
			defaultSheetWidthTextBox.Text = Parameters.DefaultSheetWidth.ToString();
			blackWhiteThresholdTextBox.Text = Parameters.BlackWhiteThreshold.ToString();

			tableCountTextBox.Text = Parameters.TableCount.ToString();
			tableLineEccentricityLimitTextBox.Text = Parameters.TableLineEccentricityLimit.ToString();
			tableLineCurvatureLimitTextBox.Text = Parameters.TableLineCurvatureLimit.ToString();
			studentTableRowsTextBox.Text = Parameters.StudentTableRows.ToString();
			studentTableColumnsTextBox.Text = Parameters.StudentTableColumns.ToString();
			answerTableRowsTextBox.Text = Parameters.AnswerTableRows.ToString();
			answerTableColumnsTextBox.Text = Parameters.AnswerTableColumns.ToString();
			resizedCellWidthTextBox.Text = Parameters.ResizedCellWidth.ToString();
			resizedCellHeightTextBox.Text = Parameters.ResizedCellHeight.ToString();

			defaultCellWidthTextBox.Text = Parameters.DefaultCellWidth.ToString();
			defaultCellHeightTextBox.Text = Parameters.DefaultCellHeight.ToString();
			cellEvaluationTypeTextBox.Text = Parameters.CellEvaluationType.ToString();

			crossLineLengthTextBox.Text = Parameters.CrossLineLength.ToString();
			crossLineCurvatureLimitTextBox.Text = Parameters.CrossLineCurvatureLimit.ToString();
			rubbishLinesLimitTextBox.Text = Parameters.RubbishLinesLimit.ToString();

			lowerThresholdTextBox.Text = Parameters.LowerThreshold.ToString();
			upperThresholdTextBox.Text = Parameters.UpperThreshold.ToString();
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			int defaultSheetWidth;
			int blackWhiteThreshold;

			int tableCount;
			float tableLineEccentricityLimit;
			int tableLineCurvatureLimit;
			int studentTableRows;
			int studentTableColumns;
			int answerTableRows;
			int answerTableColumns;
			int resizedCellWidth;
			int resizedCellHeight;

			int defaultCellWidth;
			int defaultCellHeight;
			bool cellEvaluationType;

			int crossLineLength;
			int crossLineCurvatureLimit;
			int rubbishLinesLimit;
			float lowerThreshold;
			float upperThreshold;

			// TODO: add value control too!

			if (!Int32.TryParse(defaultSheetWidthTextBox.Text, out defaultSheetWidth))
			{
				MessageBox.Show("Default Sheet Width has incorrect format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!Int32.TryParse(blackWhiteThresholdTextBox.Text, out blackWhiteThreshold))
			{
				MessageBox.Show("Black and White Threshold has incorrect format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!Int32.TryParse(tableCountTextBox.Text, out tableCount))
			{
				MessageBox.Show("Table Count has incorrect format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!float.TryParse(tableLineEccentricityLimitTextBox.Text, out tableLineEccentricityLimit))
			{
				MessageBox.Show("Table Line Eccentricity Limit has incorrect format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!Int32.TryParse(tableLineCurvatureLimitTextBox.Text, out tableLineCurvatureLimit))
			{
				MessageBox.Show("Table Line Curvature Limit has incorrect format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!Int32.TryParse(studentTableRowsTextBox.Text, out studentTableRows))
			{
				MessageBox.Show("Student Table Rows has incorrect format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!Int32.TryParse(studentTableColumnsTextBox.Text, out studentTableColumns))
			{
				MessageBox.Show("Student Table Columns has incorrect format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!Int32.TryParse(answerTableRowsTextBox.Text, out answerTableRows))
			{
				MessageBox.Show("Answer Table Rows has incorrect format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!Int32.TryParse(answerTableColumnsTextBox.Text, out answerTableColumns))
			{
				MessageBox.Show("Answer Table Columns has incorrect format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!Int32.TryParse(resizedCellWidthTextBox.Text, out resizedCellWidth))
			{
				MessageBox.Show("Resized Cell Width has incorrect format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!Int32.TryParse(resizedCellHeightTextBox.Text, out resizedCellHeight))
			{
				MessageBox.Show("Resized Cell Height has incorrect format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!Int32.TryParse(defaultCellWidthTextBox.Text, out defaultCellWidth))
			{
				MessageBox.Show("Default Cell Width has incorrect format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!Int32.TryParse(defaultCellHeightTextBox.Text, out defaultCellHeight))
			{
				MessageBox.Show("Default Cell Height has incorrect format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!bool.TryParse(cellEvaluationTypeTextBox.Text, out cellEvaluationType))
			{
				MessageBox.Show("Cell Evaluation Type has incorrect format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!Int32.TryParse(crossLineLengthTextBox.Text, out crossLineLength))
			{
				MessageBox.Show("Cross Line Length has incorrect format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!Int32.TryParse(crossLineCurvatureLimitTextBox.Text, out crossLineCurvatureLimit))
			{
				MessageBox.Show("Cross Line Curvature Limit has incorrect format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!Int32.TryParse(rubbishLinesLimitTextBox.Text, out rubbishLinesLimit))
			{
				MessageBox.Show("Rubbish Lines Limit has incorrect format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!float.TryParse(lowerThresholdTextBox.Text, out lowerThreshold))
			{
				MessageBox.Show("Lower Threshold has incorrect format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!float.TryParse(upperThresholdTextBox.Text, out upperThreshold))
			{
				MessageBox.Show("Upper Threshold has incorrect format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			Parameters = new Parameters(defaultSheetWidth, blackWhiteThreshold,
								tableCount, tableLineEccentricityLimit, tableLineCurvatureLimit,
								studentTableRows, studentTableColumns, answerTableRows, answerTableColumns,
								resizedCellWidth, resizedCellHeight,
								defaultCellWidth, defaultCellHeight, cellEvaluationType,
								crossLineLength, crossLineCurvatureLimit, rubbishLinesLimit,
								lowerThreshold, upperThreshold
			);

			DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
