#ifndef PARAMETERS_
#define PARAMETERS_

#include "opencv2\core\core.hpp"

#include <string>
#include <fstream>
#include <exception>
#include <iostream>

// ensure a specific memory layout - necessary for C# interoperability
#pragma pack(push, 8)
struct Parameters
{
	Parameters();
	
	// parameters used to prepare a sheet image for processing
	int default_sheet_width;		// every sheet will be resized accordingly (preserving aspect ratio) before the tables are extracted
	int black_white_threshold;		// how bright a shade of grey can be to be recognized as black (the rest will be white)

	// parameters of the table & cell extraction process
	int table_count;						// the number of table in the answer sheet
	float table_line_eccentricity_limit;	// how slanted a line can be to be recognized as either horizontal or vertical (in radians)
	int table_line_curvature_limit;			// how curved a line can be to still be recognized as a straight line (1 is minimum)
	int student_table_rows;					// the number of rows in the student number table
	int student_table_columns;				// the number of columns in the student number table
	int answer_table_rows;					// the number of rows in the answer table
	int answer_table_columns;				// the number of columns in the answer table
	int resized_cell_width;					// this is the width of a cell after the sheet has been resized (see default_sheet_width) 
	int resized_cell_height;				// this is the height of a cell after the sheet has been resized (see default_sheet_height) 

	// parameters of the cell evaluation process
	int default_cell_width;			// every cell will be resized to have this width
	int default_cell_height;		// every cell will be resized to have this height
	bool cell_evaluation_type;		// TRUE - shape recognition; FALSE - pixel ratio

	// parameters of the shape recognition process
	int cross_line_length;			// the length of lines to be detected
	int cross_line_curvature_limit;	// how curved a line can be to still be recognized as a straight line (1 is minimum)
	int rubbish_lines_limit;		// amount of lines that don't form a cross that will be ignored and not considered as a correction

	// parameters of the pixel ratio process
	float lower_threshold;		// if the ratio of pixels representing student input in the whole cell is lower than this, answer was not chosen
	float upper_threshold;		// if the ratio of pixels representing student input in the whole cell is higher than this, answer was invalidated
};
#pragma pack(pop)

#endif // !PARAMETERS_
