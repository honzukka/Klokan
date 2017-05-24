#ifndef PARAMETERS_
#define PARAMETERS_

#include "opencv2\core\core.hpp"

#include <string>
#include <fstream>
#include <exception>
#include <iostream>

// parameters describing the tables in the answer sheet
const int TABLE_ROWS = 9;
const int TABLE_COLUMNS = 6;
const int TABLE_COUNT = 3;

struct Parameters
{
	Parameters();

	bool update_from_file(const std::string& filename);
	
	// parameters used to prepare a sheet image for processing
	int default_sheet_width;		// every sheet will be resized accordingly (preserving aspect ratio) before the tables are extracted
	int black_white_threshold;		// how bright a shade of grey can be to be recognized as black (the rest will be white)

	// parameters of the table extraction process
	int table_line_length;					// the length of lines to be detected
	float table_line_eccentricity_limit;	// how slanted a line can be to be recognized as either horizontal or vertical (in radians)
	int table_line_curvature_limit;			// how curved a line can be to still be recognized as a straight line (1 is minimum)

	// parameters of the cell evaluation process
	int default_cell_width;			// every cell will be resized to have this width
	int default_cell_height;		// every cell will be resized to have this height
	int cross_line_length;			// the length of lines to be detected
	int cross_line_curvature_limit;	// how curved a line can be to still be recognized as a straight line (1 is minimum)
	int rubbish_lines_limit;		// amount of lines that don't form a cross that will be ignored and not considered as a correction
};

#endif // !PARAMETERS_
