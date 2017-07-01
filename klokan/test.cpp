#include <iostream>
#include <string>
#include <array>

#include "opencv2\core\core.hpp"
#include "opencv2\highgui\highgui.hpp"
#include "opencv2\imgproc\imgproc.hpp"

#include "table_extract.h"
#include "cell_extract.h"
#include "cell_eval.h"
#include "parameters.h"

using namespace std;

using testTableAnswers = array<array<bool, 2>, 3>;
using testSheetAnswers = array<testTableAnswers, 2>;

using tableAnswers = std::array<std::array<bool, TABLE_COLUMNS - 1>, TABLE_ROWS - 1>;
using sheetAnswers = std::array<tableAnswers, TABLE_COUNT>;

#pragma pack(push, 8)
struct MyStructure
{
	int number1;
	float number2;
	int number3;
	int number4;
};
#pragma pack(pop)

extern "C" __declspec(dllexport) void __stdcall hello_world()
{
	cout << "Hello from C++!!!" << endl;
	cout << endl;
}

// conversion happens on the .NET side (during marshalling)
extern "C" __declspec(dllexport) void __stdcall string_test(char* text)
{
	string textString(text);

	cout << "This was sent to C++: " << textString << endl;
	cout << endl;
}

// conversion happens right here
extern "C" __declspec(dllexport) void __stdcall string_test_wide(wchar_t* text)
{
	wstring textWString(text);
	string textString(textWString.begin(), textWString.end());

	cout << "This was sent to C++: " << textString << endl;
	cout << endl;
}

extern "C" __declspec(dllexport) void __stdcall structure_test(MyStructure structure)
{
	cout << "This was sent to C++:" << endl;
	cout << "Number 1: " << structure.number1 << endl;
	cout << "Number 2: " << structure.number2 << endl;
	cout << "Number 3: " << structure.number3 << endl;
	cout << "Number 4: " << structure.number4 << endl;
	cout << endl;
}

// assumming I get a buffer of int[2*3*2]... there's no guarantee!
extern "C" _declspec(dllexport) void __stdcall array_test(int* answers)
{
	const int tables = 2;
	const int rows = 3;
	const int cols = 2;
	
	for (int table = 0; table < tables; table++)
	{
		for (int row = 0; row < rows; row++)
		{
			for (int col = 0; col < cols; col++)
			{
				answers[table * rows * cols + row * cols + col] = row + col + (table * 5);
			}
		}
	}
}

// extracts answers from an answer sheet using image recognition
extern "C" __declspec(dllexport) void __stdcall extract_answers_test(char* filename, Parameters params, bool* answerArray)
{
	sheetAnswers answers;

	cout << "Loading image..." << endl;

	cv::Mat sheetImage = cv::imread(filename, CV_LOAD_IMAGE_GRAYSCALE);
	if (sheetImage.empty())
	{
		std::cerr << "Correct sheet could not be loaded!" << std::endl;
		return;
	}

	cout << "Image loaded..." << endl;
	cout << "Processing..." << endl;

	// extract tables ordered by x-coordinate
	std::vector<Table> tables = extract_tables(sheetImage, params);

	// evaluate tables one by one
	size_t tableNumber = 0;
	for (auto&& table : tables)
	{
		// exracts the cells of the table
		auto tableCells = extract_cells(table.image);

		// for each cell output if it's crossed of not and save the answer
		// the first row and the first column do not contain answers
		for (size_t row = 1; row < TABLE_ROWS; row++)
		{
			for (size_t col = 1; col < TABLE_COLUMNS; col++)
			{
				auto&& cell = tableCells[row][col];

				if (is_cell_crossed(cell, params))
				{
					answers[tableNumber][row - 1][col - 1] = true;
				}
				else
				{
					answers[tableNumber][row - 1][col - 1] = false;
				}
			}
		}

		tableNumber++;
	}

	cout << "Saving answers into a buffer..." << endl;

	// save the answers into the answer array
	for (int table = 0; table < TABLE_COUNT; table++)
	{
		for (int row = 0; row < TABLE_ROWS - 1; row++)
		{
			for (int col = 0; col < TABLE_COLUMNS - 1; col++)
			{
				answerArray[table * (TABLE_ROWS - 1) * (TABLE_COLUMNS - 1) + row * (TABLE_COLUMNS - 1) + col] = answers[table][row][col];
			}
		}
	}

	cout << "Done!" << endl;
}