#include "api.h"
#include "table_extract.h"
#include "cell_extract.h"
#include "cell_eval.h"

// extracts answers from an answer sheet using image recognition
// relies on the caller to provide answerArray of size (params.table_count * (params.table_rows - 1) * (params.table_columns - 1)) and the success variable
void extract_answers_api(char* filename, Parameters params, bool* answerArray, bool* success)
{
	sheetAnswers answers;

	// load the answer sheet
	cv::Mat sheetImage = cv::imread(filename, CV_LOAD_IMAGE_GRAYSCALE);
	if (sheetImage.empty())
	{
		*success = false;
		return;
	}

	// extract tables ordered by x-coordinate
	std::vector<Table> tables = extract_tables(sheetImage, params);

	// evaluate tables one by one
	for (int table = 0; table < params.table_count; table++)
	{
		// start a new answer table
		answers.push_back(tableAnswers());
		
		// extract the cells of the table
		auto tableCells = extract_cells(tables[table].image, params);

		// for each cell output if it's crossed of not and save the answer
		// the first row and the first column do not contain answers
		for (int row = 1; row < params.table_rows; row++)
		{
			// start a new answer table row
			answers[table].push_back(std::vector<bool>());
			
			for (int col = 1; col < params.table_columns; col++)
			{
				auto&& cell = tableCells[row][col];

				if (is_cell_crossed(cell, params))
				{
					answers[table][row - 1].push_back(true);
				}
				else
				{
					answers[table][row - 1].push_back(false);
				}
			}
		}
	}

	// save the answers into the answer array
	for (int table = 0; table < params.table_count; table++)
	{
		// again, the first row and the first column of the original table were removed as they do not contain any answers
		for (int row = 0; row < params.table_rows - 1; row++)
		{
			for (int col = 0; col < params.table_columns - 1; col++)
			{
				int arrayIndex = table * (params.table_rows - 1) * (params.table_columns - 1) + row * (params.table_columns - 1) + col;
				answerArray[arrayIndex] = answers[table][row][col];
			}
		}
	}

	*success = true;
}
