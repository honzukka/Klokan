//#include "opencv2\core\core.hpp"
//#include "opencv2\highgui\highgui.hpp"
//#include "opencv2\imgproc\imgproc.hpp"
//#include "opencv2\opencv.hpp"

#include "table_extract.h"
#include "cell_extract.h"
#include "cell_eval.h"
#include "debug.h"

#include <iostream>
#include <sstream>

using namespace std;
using namespace cv;

int main()
{
	Mat sheetImage = imread("09-full_column_rotated.jpeg", CV_LOAD_IMAGE_GRAYSCALE);

	const int sheetRows = 9;
	const int sheetColumns = 6;

	if (sheetImage.empty())
	{
		cerr << "No image loaded!" << endl;
		return 1;
	}

	std::vector<Table> tables = extract_tables(sheetImage, 3);
	
	int tableNumber = 1;
	for (auto&& table : tables)
	{
		auto tableCells = extract_cells(table.image, sheetRows, sheetColumns);

		// debug
		//debug::show_cells(tableCells, "cells" + tableNumber);

		cout << "Table " << tableNumber << ":" << endl;
		cout << "-----------------------" << endl;
		cout << "\tA B C D E" << endl;
		
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

	/*
	i = 1;
	for (auto&& table : tables)
	{
		debug::show_image(table.image, "table " + i);
		i++;
	}
	*/

	waitKey(0);

	return 0;
}