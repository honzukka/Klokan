#include "parameters.h"

Parameters::Parameters()
{
	default_sheet_width = 1700;
	black_white_threshold = 230;

	table_count = 4;
	table_line_eccentricity_limit = CV_PI / 8;
	table_line_curvature_limit = 1;
	student_table_rows = 6;
	student_table_columns = 11;
	answer_table_rows = 9;
	answer_table_columns = 6;
	resized_cell_width = 55;
	resized_cell_height = 35;

	default_cell_width = 80;
	default_cell_height = 40;
	cell_evaluation_type = false;

	cross_line_length = 35;
	cross_line_curvature_limit = 5;
	rubbish_lines_limit = 10;

	lower_threshold = 0.20f;
	upper_threshold = 0.70f;
}