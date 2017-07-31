using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace KlokanUI
{
	class NativeAPIWrapper
	{
		// unsafe!!!
		[DllImport("klokan.dll")]
		public unsafe static extern void extract_answers_api(string filename, Parameters parameters, bool* answers, bool* success);
	}

	// unsafe!!!
	unsafe struct AnswerWrapper
	{
		public fixed bool answers[3 * 8 * 5];
	}

	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	struct Parameters
	{
		/// <param name="defaultSheetWidth">Every sheet will be resized accordingly (preserving aspect ratio) before the tables are extracted.</param>
		/// <param name="blackWhiteThreshold">How bright a shade of grey can be to be recognized as black (the rest will be white).</param>
		/// <param name="tableCount">The number of table in the answer sheet.</param>
		/// <param name="tableLineLength">The length of lines to be detected.</param>
		/// <param name="tableLineEccentricityLimit">How slanted a line can be to be recognized as either horizontal or vertical (in radians).</param>
		/// <param name="tableLineCurvatureLimit">How curved a line can be to still be recognized as a straight line (1 is minimum).</param>
		/// <param name="tableRows">The number of rows in a table.</param>
		/// <param name="tableColumns">The number of columns in a table.</param>
		/// <param name="defaultCellWidth">Every cell will be resized to have this width.</param>
		/// <param name="defaultCellHeight">Every cell will be resized to have this height.</param>
		/// <param name="crossLineLength">The length of lines to be detected.</param>
		/// <param name="crossLineCurvatureLimit">How curved a line can be to still be recognized as a straight line (1 is minimum).</param>
		/// <param name="rubbishLinesLimit">Amount of lines that don't form a cross that will be ignored and not considered as a correction.</param>
		public Parameters(int defaultSheetWidth, int blackWhiteThreshold,
			int tableCount, int tableLineLength, float tableLineEccentricityLimit, int tableLineCurvatureLimit,
			int tableRows, int tableColumns,
			int defaultCellWidth, int defaultCellHeight, int crossLineLength, int crossLineCurvatureLimit, int rubbishLinesLimit)
		{
			DefaultSheetWidth = defaultSheetWidth;
			BlackWhiteThreshold = blackWhiteThreshold;

			TableCount = tableCount;
			TableLineLength = tableLineLength;
			TableLineEccentricityLimit = tableLineEccentricityLimit;
			TableLineCurvatureLimit = tableLineCurvatureLimit;

			TableRows = tableRows;
			TableColumns = tableColumns;

			DefaultCellWidth = defaultCellWidth;
			DefaultCellHeight = defaultCellHeight;
			CrossLineLength = crossLineLength;
			CrossLineCurvatureLimit = crossLineCurvatureLimit;
			RubbishLinesLimit = rubbishLinesLimit;
		}
		
		// parameters used to prepare a sheet image for processing
		public int DefaultSheetWidth { get; }
		public int BlackWhiteThreshold { get; }

		// parameters of the table extraction process
		public int TableCount { get; }
		public int TableLineLength { get; }
		public float TableLineEccentricityLimit { get; }
		public int TableLineCurvatureLimit { get; } 

		// parameters of the cell extraction process
		public int TableRows { get; }
		public int TableColumns { get; }

		// parameters of the cell evaluation process
		public int DefaultCellWidth { get; }
		public int DefaultCellHeight { get; }
		public int CrossLineLength { get; } 
		public int CrossLineCurvatureLimit { get; }
		public int RubbishLinesLimit { get; }

		public static Parameters CreateDefaultParameters()
		{
			return new Parameters (
				1700, 230,
				3, 350, (float)(Math.PI / 8), 1,
				9, 6,
				80, 40, 35, 5, 10
			);	
		}
	}
}
