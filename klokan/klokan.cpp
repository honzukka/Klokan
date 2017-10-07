#include "klokan.h"
#include "table_extract.h"
#include "cell_extract.h"
#include "cell_eval.h"
#include "debug.h"

// loads and stores correct answers and evaluates all answer sheets outputting their score and corrected answers into a file
void Klokan::run(const std::string& correctAnswerSheetName, const std::vector<std::string>& answerSheetNames)
{
	// get and store correct answers
	cv::Mat correctSheetImage = cv::imread(correctAnswerSheetName, CV_LOAD_IMAGE_GRAYSCALE);
	if (correctSheetImage.empty())
	{
		std::cerr << "Correct sheet could not be loaded!" << std::endl;
		return;
	}
	correctAnswers_ = extract_answers(correctSheetImage);

	// evaluate other sheets one by one
	int i = 1;		// sheet number
	for (auto&& answerSheetName : answerSheetNames)
	{
		// load the sheet image
		cv::Mat sheetImage = cv::imread(answerSheetName, CV_LOAD_IMAGE_GRAYSCALE);
		if (sheetImage.empty())
		{
			std::cerr << "Answer sheet number " << i << " could not be loaded!" << std::endl;
			continue;
		}

		// get the answers
		auto answers = extract_answers(sheetImage);

		// evaluate them
		auto correctedAnswers = correct_answers(answers);
		size_t score = count_score(correctedAnswers);

		// output the results into a file
		if (output_results(score, correctedAnswers, answerSheetName))
		{
			std::cerr << "Results of answer sheet number " << i << " were outputted successfully." << std::endl;
		}
		else
		{
			std::cerr << "There was an error outputting the results of answer sheet number " << i << "." << std::endl;
		}

		i++;
	}
}

// extracts answers from an answer sheet using image recognition
sheetAnswers Klokan::extract_answers(cv::Mat& sheetImage)
{
	sheetAnswers answers;
	
	// extract tables ordered by x-coordinate
	std::vector<Table> tables = extract_tables(sheetImage, parameters_);

	// evaluate tables one by one
	size_t tableNumber = 0;
	for (auto&& table : tables)
	{
		// start a new answer table
		answers.push_back(tableAnswers());
		
		// extracts the cells of the table
		auto tableCells = extract_cells(table.image, parameters_);

		// for each cell output if it's crossed of not and save the answer
		// the first row and the first column do not contain answers
		for (size_t row = 1; row < parameters_.table_rows; row++)
		{
			// start a new answer table row
			answers[tableNumber].push_back(std::vector<bool>());
			
			for (size_t col = 1; col < parameters_.table_columns; col++)
			{
				auto&& cell = tableCells[row][col];

				if (is_cell_crossed_shape(cell, parameters_))
				{
					answers[tableNumber][row - 1].push_back(true);
				}
				else
				{
					answers[tableNumber][row - 1].push_back(false);
				}
			}
		}

		tableNumber++;
	}

	return std::move(answers);
}

// for each cell of the answer sheet it saves whether it is void, correct, corrected or incorrect
correctedSheetAnswers Klokan::correct_answers(const sheetAnswers& answers)
{
	correctedSheetAnswers correctedAnswers;

	// correct all answers and save them
	for (size_t table = 0; table < parameters_.table_count; table++)
	{
		// start a new corrected answer table
		correctedAnswers.push_back(correctedTableAnswers());
		
		// again, the first row and the first column of the original table were removed as they do not contain any answers
		for (size_t row = 0; row < parameters_.table_rows - 1; row++)
		{
			// start a new corrected answer table row
			correctedAnswers[table].push_back(std::vector<AnswerType>());
			
			for (size_t col = 0; col < parameters_.table_columns - 1; col++)
			{
				// if the answer is no and correct
				if (answers[table][row][col] == false && correctAnswers_[table][row][col] == false)
					correctedAnswers[table][row].push_back(VOID);
				
				// if the answer is yes and correct
				else if (answers[table][row][col] == true && correctAnswers_[table][row][col] == true)
					correctedAnswers[table][row].push_back(CORRECT);
				
				// if the answer is yes and incorrect
				else if (answers[table][row][col] == true && correctAnswers_[table][row][col] == false)
					correctedAnswers[table][row].push_back(INCORRECT);

				// if the answer is no and incorrect
				else if (answers[table][row][col] == false && correctAnswers_[table][row][col] == true)
					correctedAnswers[table][row].push_back(CORRECTED);
			}
		}
	}

	return std::move(correctedAnswers);
}

// counts the score based on corrected answers acording to the standard rules of the Mathematical Kangaroo
size_t Klokan::count_score(const correctedSheetAnswers& correctedAnswers)
{
	size_t score = 24;
	
	for (size_t table = 0; table < parameters_.table_count; table++)
	{
		// again, the first row and the first column of the original table were removed as they do not contain any answers
		for (size_t row = 0; row < parameters_.table_rows - 1; row++)
		{
			size_t correctAnswersCount = 0;
			size_t incorrectAnswersCount = 0;
			
			// count the types of answers necessary to assign points
			for (size_t col = 0; col < parameters_.table_columns - 1; col++)
			{
				switch (correctedAnswers[table][row][col])
				{
					case CORRECT: correctAnswersCount++; break;
					case INCORRECT: incorrectAnswersCount++; break;
					default: break;
				}
			}

			// assign points for the question (row)
			// if it's correct
			if (correctAnswersCount == 1 && incorrectAnswersCount == 0)
			{
				switch (table)
				{
					case 0: score += 3; break;
					case 1: score += 4; break;
					case 2: score += 5; break;
				}
			}
			// if it's incorrect
			else if (incorrectAnswersCount > 0)
			{
				score--;
			}
			// otherwise the score doesn't change
		}
	}

	return score;
}

// saves all the information about a sheet into a file
bool Klokan::output_results(size_t score, const correctedSheetAnswers& correctedAnswers, const std::string& originalFilename)
{
	// prepare the output file name
	size_t dotPosition = originalFilename.find('.');
	std::string prefix = originalFilename.substr(0, dotPosition);
	std::string outputFilename = prefix + "RESULT.txt";
	
	// open/create the output file
	std::ofstream outputFile;
	outputFile.open(outputFilename, std::fstream::out);

	// check for errors
	if (!outputFile.is_open()) return false;

	// output results
	for (size_t table = 0; table < parameters_.table_count; table++)
	{
		outputFile << "Table " << table + 1 << ":" << std::endl;
		outputFile << "-----------------------" << std::endl;
		outputFile << "\tA B C D E" << std::endl;

		// again, the first row and the first column of the original table were removed as they do not contain any answers
		for (size_t row = 0; row < parameters_.table_rows - 1; row++)
		{
			outputFile << table * 8 + row + 1 << "\t";

			for (size_t col = 0; col < parameters_.table_columns - 1; col++)
			{
				switch (correctedAnswers[table][row][col])
				{
					case VOID:		outputFile << "  ";	break;
					case CORRECT:	outputFile << "X ";	break;
					case CORRECTED:	outputFile << "O ";	break;
					case INCORRECT:	outputFile << "! ";	break;
				}
			}

			outputFile << std::endl;
		}

		outputFile << std::endl << std::endl;
	}

	outputFile << "Total score: " << score << std::endl;

	// close the file
	outputFile.close();
	if (outputFile.fail())
		return false;
	else 
		return true;
}