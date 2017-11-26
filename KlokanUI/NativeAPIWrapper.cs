using System;

using System.Runtime.InteropServices;

namespace KlokanUI
{
	// unsafe!!!
	class NativeAPIWrapper
	{
		[DllImport("klokan.dll")]
		public unsafe static extern void extract_answers_path_api(string filename, Parameters parameters, bool* number, bool* answers, bool* success);

		[DllImport("klokan.dll")]
		public unsafe static extern void extract_answers_image_api(byte* imageBytes, int rows, int cols, Parameters parameters, bool* number, bool* answers, bool* success);
	}

	// unsafe!!!
	unsafe struct NumberWrapper
	{
		public fixed bool number[5 * 10];
	}

	// unsafe!!!
	unsafe struct AnswerWrapper
	{
		public fixed bool answers[3 * 8 * 5];
	}

	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	struct Parameters
	{
		#region Parameters used to prepare a sheet image for processing

		/// <summary>
		/// Every sheet will be resized accordingly (preserving aspect ratio) before the tables are extracted.
		/// </summary>
		public int DefaultSheetWidth { get; }

		/// <summary>
		/// How bright a shade of grey can be to be recognized as black (the rest will be white).
		/// </summary>
		public int BlackWhiteThreshold { get; }

		#endregion

		#region Parameters of the table & cell extraction process

		/// <summary>
		/// The number of table in the answer sheet.
		/// </summary>
		public int TableCount { get; }

		/// <summary>
		/// How slanted a line can be to be recognized as either horizontal or vertical (in radians).
		/// </summary>
		public float TableLineEccentricityLimit { get; }

		/// <summary>
		/// How curved a line can be to still be recognized as a straight line (1 is minimum).
		/// </summary>
		public int TableLineCurvatureLimit { get; }

		/// <summary>
		/// The number of rows in the student number table.
		/// </summary>
		public int StudentTableRows { get; }

		/// <summary>
		/// The number of columns in the student number table.
		/// </summary>
		public int StudentTableColumns { get; }

		/// <summary>
		/// The number of rows in the answer table.
		/// </summary>
		public int AnswerTableRows { get; }

		/// <summary>
		/// The number of columns in the answer table.
		/// </summary>
		public int AnswerTableColumns { get; }

		/// <summary>
		/// This is the width of a cell after the sheet has been resized (see default_sheet_width).
		/// </summary>
		public int ResizedCellWidth { get; }

		/// <summary>
		/// This is the height of a cell after the sheet has been resized (see default_sheet_height).
		/// </summary>
		public int ResizedCellHeight { get; }

		#endregion

		#region Parameters of the cell evaluation process

		/// <summary>
		/// Every cell will be resized to have this width.
		/// </summary>
		public int DefaultCellWidth { get; }

		/// <summary>
		/// Every cell will be resized to have this height.
		/// </summary>
		public int DefaultCellHeight { get; }

		/// <summary>
		/// TRUE - shape recognition; FALSE - pixel ratio
		/// </summary>
		public bool CellEvaluationType { get; }

		#endregion

		#region Parameters for shape recognition

		/// <summary>
		/// The length of lines to be detected.
		/// </summary>
		public int CrossLineLength { get; }

		/// <summary>
		/// How curved a line can be to still be recognized as a straight line (1 is minimum).
		/// </summary>
		public int CrossLineCurvatureLimit { get; }

		/// <summary>
		/// Amount of lines that don't form a cross that will be ignored and not considered as a correction.
		/// </summary>
		public int RubbishLinesLimit { get; }

		#endregion

		#region Parameters for pixel ratio

		/// <summary>
		/// If the ratio of pixels representing student input in the whole cell is lower than this, answer was not chosen.
		/// </summary>
		public float LowerThreshold { get; }

		/// <summary>
		/// </summary>
		public float UpperThreshold { get; }

		#endregion

		/// <param name="defaultSheetWidth">Every sheet will be resized accordingly (preserving aspect ratio) before the tables are extracted.</param>
		/// <param name="blackWhiteThreshold">How bright a shade of grey can be to be recognized as black (the rest will be white).</param>
		/// <param name="tableCount">The number of table in the answer sheet.</param>
		/// <param name="tableLineEccentricityLimit">How slanted a line can be to be recognized as either horizontal or vertical (in radians).</param>
		/// <param name="tableLineCurvatureLimit">How curved a line can be to still be recognized as a straight line (1 is minimum).</param>
		/// <param name="studentTableRows">The number of rows in the student number table.</param>
		/// <param name="studentTableColumns">The number of columns in the student number table.</param>
		/// <param name="answerTableRows">The number of rows in the answer table.</param>
		/// <param name="answerTableColumns">The number of columns in the answer table.</param>
		/// <param name="resizedCellWidth">This is the width of a cell after the sheet has been resized (see default_sheet_width).</param>
		/// <param name="resizedCellHeight">This is the height of a cell after the sheet has been resized (see default_sheet_height).</param>
		/// <param name="defaultCellWidth">Every cell will be resized to have this width.</param>
		/// <param name="defaultCellHeight">Every cell will be resized to have this height.</param>
		/// <param name="cellEvaluationType">TRUE - shape recognition; FALSE - pixel ratio</param>
		/// <param name="crossLineLength">The length of lines to be detected.</param>
		/// <param name="crossLineCurvatureLimit">How curved a line can be to still be recognized as a straight line (1 is minimum).</param>
		/// <param name="rubbishLinesLimit">Amount of lines that don't form a cross that will be ignored and not considered as a correction.</param>
		/// <param name="lowerThreshold">If the ratio of pixels representing student input in the whole cell is lower than this, answer was not chosen.</param>
		/// <param name="upperThreshold">If the ratio of pixels representing student input in the whole cell is higher than this, answer was invalidated.</param>
		public Parameters(int defaultSheetWidth, int blackWhiteThreshold,
			int tableCount, float tableLineEccentricityLimit, int tableLineCurvatureLimit,
			int studentTableRows, int studentTableColumns, int answerTableRows, int answerTableColumns,
			int resizedCellWidth, int resizedCellHeight,
			int defaultCellWidth, int defaultCellHeight, bool cellEvaluationType,
			int crossLineLength, int crossLineCurvatureLimit, int rubbishLinesLimit,
			float lowerThreshold, float upperThreshold)
		{
			DefaultSheetWidth = defaultSheetWidth;
			BlackWhiteThreshold = blackWhiteThreshold;

			TableCount = tableCount;
			TableLineEccentricityLimit = tableLineEccentricityLimit;
			TableLineCurvatureLimit = tableLineCurvatureLimit;
			StudentTableRows = studentTableRows;
			StudentTableColumns = studentTableColumns;
			AnswerTableRows = answerTableRows;
			AnswerTableColumns = answerTableColumns;
			ResizedCellWidth = resizedCellWidth;
			ResizedCellHeight = resizedCellHeight;

			DefaultCellWidth = defaultCellWidth;
			DefaultCellHeight = defaultCellHeight;
			CellEvaluationType = cellEvaluationType;

			CrossLineLength = crossLineLength;
			CrossLineCurvatureLimit = crossLineCurvatureLimit;
			RubbishLinesLimit = rubbishLinesLimit;

			LowerThreshold = lowerThreshold;
			UpperThreshold = upperThreshold;
		}

		public static Parameters CreateDefaultParameters()
		{
			return new Parameters (
				1700, 230,
				4, (float)(Math.PI / 8), 1,
				6, 11, 9, 6, 55, 35,
				80, 40, false,
				35, 5, 10,
				0.20f, 0.70f
			);	
		}
	}
}
