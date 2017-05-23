#include "table_extract.h"
#include "cell_extract.h"
#include "cell_eval.h"
#include "debug.h"
#include "klokan.h"

#include <iostream>
#include <sstream>

using namespace std;
using namespace cv;

int main()
{
	Klokan klokan;

	std::string correctSheetName = "01-varying_size.jpeg";
	std::vector<std::string> sheetNames;

	sheetNames.push_back("01-varying_size.jpeg");
	sheetNames.push_back("01-varying_size-one_wrong.jpeg");
	sheetNames.push_back("01-varying_size-one_empty.jpeg");
	sheetNames.push_back("09-full_column_rotated.jpeg");

	klokan.run(correctSheetName, sheetNames);

	cout << "Ready!" << endl;

	/*
	Mat sheetImage = imread("09-full_column_rotated.jpeg", CV_LOAD_IMAGE_GRAYSCALE);

	const int sheetRows = 9;
	const int sheetColumns = 6;

	if (sheetImage.empty())
	{
		cerr << "No image loaded!" << endl;
		return 1;
	}

	// extract tables ordered by x-coordinate
	std::vector<Table> tables = extract_tables(sheetImage, 3);
	
	// evaluate tables one by one
	int tableNumber = 1;
	for (auto&& table : tables)
	{
		auto tableCells = extract_cells(table.image, sheetRows, sheetColumns);

		//debug::show_cells(tableCells, "cells" + tableNumber);

		cout << "Table " << tableNumber << ":" << endl;
		cout << "-----------------------" << endl;
		cout << "\tA B C D E" << endl;
		
		// for each cell output if it's crossed of not
		for (size_t i = 1; i < sheetRows; i++)
		{
			cout << (tableNumber - 1) * 8 + i << "\t";
			
			for (size_t j = 1; j < sheetColumns; j++)
			{
				auto&& cell = tableCells[i][j];
				
				if (is_cell_crossed(cell))
				{
					cout << "X ";
				}
				else
				{
					cout << "  ";
				}
			}

			cout << endl;
		}

		cout << endl;
		tableNumber++;
	}
	*/
	waitKey(0);

	return 0;
}