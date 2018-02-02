using System;
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
			toolTip.SetToolTip(defaultSheetWidthLabel, Properties.Resources.ToolTipDefaultSheetWidth);
			toolTip.SetToolTip(blackWhiteThresholdLabel, Properties.Resources.ToolTipBlackWhiteThreshold);

			// toolTip.SetToolTip(tableCountLabel, Properties.Resources.ToolTipTableCount);
			toolTip.SetToolTip(tableLineEccetricityLimitLabel, Properties.Resources.ToolTipTableLineEccentricityLimit);
			toolTip.SetToolTip(tableLineCurvatureLimitLabel, Properties.Resources.ToolTipTableLineCurvatureLimit);
			// toolTip.SetToolTip(studentTableRowsLabel, Properties.Resources.ToolTipStudentTableRows);
			// toolTip.SetToolTip(studentTableColumnsLabel, Properties.Resources.ToolTipStudentTableColumns);
			// toolTip.SetToolTip(answerTableRowsLabel, Properties.Resources.ToolTipAnswerTableRows);
			// toolTip.SetToolTip(answerTableColumnsLabel, Properties.Resources.ToolTipAnswerTableColumns);
			toolTip.SetToolTip(resizedCellWidthLabel, Properties.Resources.ToolTipResizedCellWidth);
			toolTip.SetToolTip(resizedCellHeightLabel, Properties.Resources.ToolTipResizedCellHeight);

			toolTip.SetToolTip(defaultCellWidthLabel, Properties.Resources.ToolTipDefaultCellWidth);
			toolTip.SetToolTip(defaultCellHeightLabel, Properties.Resources.ToolTipDefaultCellHeight);
			toolTip.SetToolTip(cellEvaluationTypeLabel, Properties.Resources.ToolTipCellEvaluationType);

			toolTip.SetToolTip(crossLineLengthLabel, Properties.Resources.ToolTipCrossLineLength);
			toolTip.SetToolTip(crossLineCurvatureLimitLabel, Properties.Resources.ToolTipCrossLineCurvatureLimit);
			toolTip.SetToolTip(rubbishLinesLimitLabel, Properties.Resources.ToolTipRubbishLinesLimit);

			toolTip.SetToolTip(lowerThresholdLabel, Properties.Resources.ToolTipLowerThreshold);
			toolTip.SetToolTip(upperThresholdLabel, Properties.Resources.ToolTipUpperThreshold);
		}

		private void FillTextBoxes()
		{
			defaultSheetWidthTextBox.Text = Parameters.DefaultSheetWidth.ToString();
			blackWhiteThresholdTextBox.Text = Parameters.BlackWhiteThreshold.ToString();

			// tableCountTextBox.Text = Parameters.TableCount.ToString();
			tableLineEccentricityLimitTextBox.Text = Parameters.TableLineEccentricityLimit.ToString();
			tableLineCurvatureLimitTextBox.Text = Parameters.TableLineCurvatureLimit.ToString();
			// studentTableRowsTextBox.Text = Parameters.StudentTableRows.ToString();
			// studentTableColumnsTextBox.Text = Parameters.StudentTableColumns.ToString();
			// answerTableRowsTextBox.Text = Parameters.AnswerTableRows.ToString();
			// answerTableColumnsTextBox.Text = Parameters.AnswerTableColumns.ToString();
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

			// int tableCount;
			float tableLineEccentricityLimit;
			int tableLineCurvatureLimit;
			// int studentTableRows;
			// int studentTableColumns;
			// int answerTableRows;
			// int answerTableColumns;
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

			// default sheet width
			if (!Int32.TryParse(defaultSheetWidthTextBox.Text, out defaultSheetWidth))
			{
				MessageBox.Show(Properties.Resources.ErrorTextDefaultSheetWidthFormat, Properties.Resources.ErrorCaptionGeneral, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (defaultSheetWidth != 1700)
			{
				MessageBox.Show(Properties.Resources.ErrorTextDefaultSheetWidthValue, Properties.Resources.ErrorCaptionGeneral,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// black white threshold
			if (!Int32.TryParse(blackWhiteThresholdTextBox.Text, out blackWhiteThreshold))
			{
				MessageBox.Show(Properties.Resources.ErrorTextBlackWhiteThresholdFormat, Properties.Resources.ErrorCaptionGeneral, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (blackWhiteThreshold < 0 || blackWhiteThreshold > 255)
			{
				MessageBox.Show(Properties.Resources.ErrorTextBlackWhiteThresholdValue, Properties.Resources.ErrorCaptionGeneral,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			/* DISABLED
			if (!Int32.TryParse(tableCountTextBox.Text, out tableCount))
			{
				MessageBox.Show(Properties.Resources.ErrorTextTableCountFormat, Properties.Resources.ErrorCaptionGeneral, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			*/

			// table line eccentricity limit
			if (!float.TryParse(tableLineEccentricityLimitTextBox.Text, out tableLineEccentricityLimit))
			{
				MessageBox.Show(Properties.Resources.ErrorTextTableLineEccentricityLimitFormat, Properties.Resources.ErrorCaptionGeneral, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (tableLineEccentricityLimit < 0 || tableLineEccentricityLimit > 0.7)
			{
				MessageBox.Show(Properties.Resources.ErrorTextTableLineEccentricityLimitValue, Properties.Resources.ErrorCaptionGeneral,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// table line curvature limit
			if (!Int32.TryParse(tableLineCurvatureLimitTextBox.Text, out tableLineCurvatureLimit))
			{
				MessageBox.Show(Properties.Resources.ErrorTextTableLineCurvatureLimitFormat, Properties.Resources.ErrorCaptionGeneral, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (tableLineCurvatureLimit < 1 || tableLineCurvatureLimit > 10)
			{
				MessageBox.Show(Properties.Resources.ErrorTextTableLineCurvatureLimitValue, Properties.Resources.ErrorCaptionGeneral,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			/* DISABLED
			if (!Int32.TryParse(studentTableRowsTextBox.Text, out studentTableRows))
			{
				MessageBox.Show(Properties.Resources.ErrorTextStudentTableRowsFormat, Properties.Resources.ErrorCaptionGeneral, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			*/

			/* DISABLED
			if (!Int32.TryParse(studentTableColumnsTextBox.Text, out studentTableColumns))
			{
				MessageBox.Show(Properties.Resources.ErrorTextStudentTableColumnsFormat, Properties.Resources.ErrorCaptionGeneral, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			*/

			/* DISABLED
			if (!Int32.TryParse(answerTableRowsTextBox.Text, out answerTableRows))
			{
				MessageBox.Show(Properties.Resources.ErrorTextAnswerTableRowsFormat, Properties.Resources.ErrorCaptionGeneral, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			*/

			/* DISABLED
			if (!Int32.TryParse(answerTableColumnsTextBox.Text, out answerTableColumns))
			{
				MessageBox.Show(Properties.Resources.ErrorTextAnswerTableColumnsFormat, Properties.Resources.ErrorCaptionGeneral, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			*/

			// resized cell width
			if (!Int32.TryParse(resizedCellWidthTextBox.Text, out resizedCellWidth))
			{
				MessageBox.Show(Properties.Resources.ErrorTextResizedCellWidthFormat, Properties.Resources.ErrorCaptionGeneral, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (resizedCellWidth < 10 || resizedCellWidth > 100)
			{
				MessageBox.Show(Properties.Resources.ErrorTextResizedCellWidthValue, Properties.Resources.ErrorCaptionGeneral,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// resized cell height
			if (!Int32.TryParse(resizedCellHeightTextBox.Text, out resizedCellHeight))
			{
				MessageBox.Show(Properties.Resources.ErrorTextResizedCellHeightFormat, Properties.Resources.ErrorCaptionGeneral, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (resizedCellHeight < 10 || resizedCellHeight > 100)
			{
				MessageBox.Show(Properties.Resources.ErrorTextResizedCellHeightValue, Properties.Resources.ErrorCaptionGeneral,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// default cell width
			if (!Int32.TryParse(defaultCellWidthTextBox.Text, out defaultCellWidth))
			{
				MessageBox.Show(Properties.Resources.ErrotTextDefaultCellWidthFormat, Properties.Resources.ErrorCaptionGeneral, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (defaultCellWidth < 10 || defaultCellWidth > 100)
			{
				MessageBox.Show(Properties.Resources.ErrotTextDefaultCellWidthValue, Properties.Resources.ErrorCaptionGeneral,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// default cell height
			if (!Int32.TryParse(defaultCellHeightTextBox.Text, out defaultCellHeight))
			{
				MessageBox.Show(Properties.Resources.ErrorTextDefaultCellHeightFormat, Properties.Resources.ErrorCaptionGeneral, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (defaultCellHeight < 10 || defaultCellHeight > 100)
			{
				MessageBox.Show(Properties.Resources.ErrorTextDefaultCellHeightValue, Properties.Resources.ErrorCaptionGeneral,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// cell evaluation type
			if (!bool.TryParse(cellEvaluationTypeTextBox.Text, out cellEvaluationType))
			{
				MessageBox.Show(Properties.Resources.ErrorTextCellEvaluationTypeFormat, Properties.Resources.ErrorCaptionGeneral, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// cross line length
			if (!Int32.TryParse(crossLineLengthTextBox.Text, out crossLineLength))
			{
				MessageBox.Show(Properties.Resources.ErrorTextCrossLineLengthFormat, Properties.Resources.ErrorCaptionGeneral,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (crossLineLength < 1 || crossLineLength > 200)
			{
				MessageBox.Show(Properties.Resources.ErrorTextCrossLineLengthValue, Properties.Resources.ErrorCaptionGeneral,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// cross line curvature limit
			if (!Int32.TryParse(crossLineCurvatureLimitTextBox.Text, out crossLineCurvatureLimit))
			{
				MessageBox.Show(Properties.Resources.ErrorTextCrossLineCurvatureLimitFormat, Properties.Resources.ErrorCaptionGeneral,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (crossLineCurvatureLimit < 1 || crossLineCurvatureLimit > 15)
			{
				MessageBox.Show(Properties.Resources.ErrorTextCrossLineCurvatureLimitValue, Properties.Resources.ErrorCaptionGeneral,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// rubbish lines limit
			if (!Int32.TryParse(rubbishLinesLimitTextBox.Text, out rubbishLinesLimit))
			{
				MessageBox.Show(Properties.Resources.ErrorTextRubbishLinesLimitFormat, Properties.Resources.ErrorCaptionGeneral,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (rubbishLinesLimit < 0 || rubbishLinesLimit > 100)
			{
				MessageBox.Show(Properties.Resources.ErrorTextRubbishLinesLimitValue, Properties.Resources.ErrorCaptionGeneral,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// lower threshold
			if (!float.TryParse(lowerThresholdTextBox.Text, out lowerThreshold))
			{
				MessageBox.Show(Properties.Resources.ErrorTextLowerThresholdFormat, Properties.Resources.ErrorCaptionGeneral,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (lowerThreshold < 0.01 || lowerThreshold > 0.9)
			{
				MessageBox.Show(Properties.Resources.ErrorTextLowerThresholdValue, Properties.Resources.ErrorCaptionGeneral,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// upper threshold
			if (!float.TryParse(upperThresholdTextBox.Text, out upperThreshold))
			{
				MessageBox.Show(Properties.Resources.ErrorTextUpperThresholdFormat, Properties.Resources.ErrorCaptionGeneral,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (upperThreshold < lowerThreshold || upperThreshold > 0.99)
			{
				MessageBox.Show(Properties.Resources.ErrorTextUpperThresholdValue, Properties.Resources.ErrorCaptionGeneral,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			Parameters = new Parameters(defaultSheetWidth, blackWhiteThreshold,
								Parameters.TableCount, tableLineEccentricityLimit, tableLineCurvatureLimit,
								Parameters.StudentTableRows, Parameters.StudentTableColumns, 
								Parameters.AnswerTableRows, Parameters.AnswerTableColumns,
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
