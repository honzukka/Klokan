#include "opencv2\core\core.hpp"
#include "opencv2\highgui\highgui.hpp"
#include "opencv2\imgproc\imgproc.hpp"
#include "opencv2\opencv.hpp"

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
	Mat sheetImage = imread("test_answer_sheet_filled.jpg", CV_LOAD_IMAGE_GRAYSCALE);

	if (sheetImage.empty())
	{
		cerr << "No image loaded!" << endl;
		return 1;
	}

	std::vector<Mat> tables = extract_tables(sheetImage, 2);
	
	int i = 1;
	for (auto&& table : tables)
	{
		auto tableCells = extract_cells(table, 9, 6);

		debug::show_cells(tableCells, "cells");

		cout << "Table " << i << ":" << endl;
		
		for (auto&& row : tableCells)
		{
			for (auto&& cell : row)
			{
				if (is_cell_crossed(cell))
				{
					cout << "X";
				}
				else
				{
					cout << " ";
				}
			}

			cout << endl;
		}

		cout << endl;
		i++;
	}

	
	i = 1;
	for (auto&& table : tables)
	{
		debug::show_image(table, "table " + i);
		i++;
	}
	

	waitKey(0);

	return 0;
}