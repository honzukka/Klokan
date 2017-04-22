#ifndef PARAMETERS_
#define PARAMATERS_

// parameters used to prepare a sheet image for processing
const int DEFAULT_SHEET_WIDTH = 1700;	// every sheet will be resized accordingly (preserving aspect ratio) before the tables are extracted
const int BLACK_WHITE_THRESHOLD = 230;	// how bright a shade of grey can be to be recognized as black (the rest will be white)

// parameters of the table extraction process
const int TABLE_LINE_LENGTH = 350;			// the length of lines to be detected
const float TABLE_LINE_ECCENTRICITY_LIMIT = CV_PI / 8;		// how slanted a line can be to be recognized as either horizontal or vertical (in radians)
const int TABLE_LINE_CURVATURE_LIMIT = 1;	// how curved a line can be to still be recognized as a straight line (1 is minimum)

// parameters of the cell evaluation process
const int DEFAULT_CELL_WIDTH = 80;			// every cell will be resized to have this width
const int DEFAULT_CELL_HEIGHT = 40;			// every cell will be resized to have this height
const int CROSS_LINE_LENGTH = 35;			// the length of lines to be detected
const int CROSS_LINE_CURVATURE_LIMIT = 5;	// how curved a line can be to still be recognized as a straight line (1 is minimum)
const int RUBBISH_LINES_LIMIT = 10;			// amount of lines that don't form a cross that will be ignored and not considered as a correction

#endif // !PARAMETERS_
