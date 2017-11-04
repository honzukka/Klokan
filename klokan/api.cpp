#include "api.h"
#include "table_extract.h"
#include "cell_extract.h"
#include "cell_eval.h"

tableAnswers evaluate_table(const cv::Mat& tableImage, const Parameters& params, TableType type, cellEvalFunc isCellCrossed);

// extracts answers from an answer sheet using image recognition
// relies on the caller to provide numberArray of size (params.student_table_rows - 1) * (params.student_table_columns - 1),
// answerArray of size (params.table_count * (params.answer_table_rows - 1) * (params.answer_table_columns - 1)) and the success variable
void extract_answers_api(char* filename, Parameters params, bool* numberArray, bool* answerArray, bool* success)
{
	tableAnswers number;
	sheetAnswers answers;

	// decide which type of cell evaluation will be used
	cellEvalFunc isCellCrossed;

	if (params.cell_evaluation_type == true)
	{
		isCellCrossed = is_cell_crossed_shape;
	}
	else
	{
		isCellCrossed = is_cell_crossed_ratio;
	}

	// load the answer sheet
	cv::Mat sheetImage = cv::imread(filename, CV_LOAD_IMAGE_GRAYSCALE);
	if (sheetImage.empty())
	{
		*success = false;
		return;
	}

	// extract tables ordered by x-coordinate
	Table studentTable;
	std::vector<Table> answerTables;

	std::tie(studentTable, answerTables) = extract_tables(sheetImage, params);

	// evaluate tables
	number = evaluate_table(studentTable.image, params, STUDENTTABLE, isCellCrossed);

	int i = 0;
	for (auto&& answerTable : answerTables)
	{
		answers.push_back(evaluate_table(answerTable.image, params, ANSWERTABLE, isCellCrossed));
		i++;
	}

	// save the number into the number array
	// again, the first row and the first column of the original table were removed as they do not contain any answers
	for (int row = 0; row < params.student_table_rows - 1; row++)
	{
		for (int col = 0; col < params.student_table_columns - 1; col++)
		{
			int arrayIndex = row * (params.student_table_columns - 1) + col;
			numberArray[arrayIndex] = number[row][col];
		}
	}

	// save the answers into the answer array
	for (int table = 0; table < params.table_count - 1; table++)
	{
		// again, the first row and the first column of the original table were removed as they do not contain any answers
		for (int row = 0; row < params.answer_table_rows - 1; row++)
		{
			for (int col = 0; col < params.answer_table_columns - 1; col++)
			{
				int arrayIndex = table * (params.answer_table_rows - 1) * (params.answer_table_columns - 1) + row * (params.answer_table_columns - 1) + col;
				answerArray[arrayIndex] = answers[table][row][col];
			}
		}
	}

	*success = true;
}

tableAnswers evaluate_table(const cv::Mat& tableImage, const Parameters& params, TableType type, cellEvalFunc isCellCrossed)
{
	tableAnswers answers;

	int tableColumns = type == STUDENTTABLE ? params.student_table_columns : params.answer_table_columns;
	int tableRows = type == STUDENTTABLE ? params.student_table_rows : params.answer_table_rows;
	
	// extract the cells of the table
	auto tableCells = extract_cells(tableImage, tableColumns, tableRows);

	// for each cell output if it's crossed of not and save the answer
	// the first row and the first column do not contain answers
	for (int row = 1; row < tableRows; row++)
	{
		// start a new answer table row
		answers.push_back(std::vector<bool>());

		for (int col = 1; col < tableColumns; col++)
		{
			auto&& cell = tableCells[row][col];

			if (isCellCrossed(cell, params))
			{
				answers[row - 1].push_back(true);
			}
			else
			{
				answers[row - 1].push_back(false);
			}
		}
	}

	return std::move(answers);
}