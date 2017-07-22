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
		// parameters used to prepare a sheet image for processing
		public int DefaultSheetWidth;       // every sheet will be resized accordingly (preserving aspect ratio) before the tables are extracted
		public int BlackWhiteThreshold;     // how bright a shade of grey can be to be recognized as black (the rest will be white)

		// parameters of the table extraction process
		public int TableCount;                          // the number of table in the answer sheet
		public int TableLineLength;                     // the length of lines to be detected
		public float TableLineEccentricityLimit;        // how slanted a line can be to be recognized as either horizontal or vertical (in radians)
		public int TableLineCurvatureLimit;             // how curved a line can be to still be recognized as a straight line (1 is minimum)

		// parameters of the cell extraction process
		public int TableRows;           // the number of rows in a table
		public int TableColumns;        // the number of columns in a table

		// parameters of the cell evaluation process
		public int DefaultCellWidth;            // every cell will be resized to have this width
		public int DefaultCellHeight;           // every cell will be resized to have this height
		public int CrossLineLength;             // the length of lines to be detected
		public int CrossLineCurvatureLimit;     // how curved a line can be to still be recognized as a straight line (1 is minimum)
		public int RubbishLinesLimit;           // amount of lines that don't form a cross that will be ignored and not considered as a correction

		public void SetDefaultValues()
		{
			DefaultSheetWidth = 1700;
			BlackWhiteThreshold = 230;

			TableCount = 3;
			TableLineLength = 350;
			TableLineEccentricityLimit = (float)(Math.PI / 8);
			TableLineCurvatureLimit = 1;

			TableRows = 9;
			TableColumns = 6;

			DefaultCellWidth = 80;
			DefaultCellHeight = 40;
			CrossLineLength = 35;
			CrossLineCurvatureLimit = 5;
			RubbishLinesLimit = 10;
		}
	}
}
