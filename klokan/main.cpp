#include "opencv2\core\core.hpp"
#include "opencv2\highgui\highgui.hpp"
#include "opencv2\imgproc\imgproc.hpp"

#include "cell_extract.h"
#include "cell_eval.h"

#include <iostream>
#include <sstream>

using namespace std;
using namespace cv;

int main()
{
	/*
	for (size_t i = 1; i < 12; i++)
	{
		stringstream ss;
		if (i < 10)
		{
			ss << "0" << i << "test_cell" << ".jpg";
		}
		else
		{
			ss << i << "test_cell" << ".jpg";
		}

		Mat source = imread(ss.str(), CV_LOAD_IMAGE_GRAYSCALE);

		if (is_cell_crossed(source))
		{
			cout << i << ": yes" << endl;
		}
		else
		{
			cout << i << ": no" << endl;
		}
		
		
		// draw lines
		for (size_t i = 0; i < lines.size(); i++)
		{
			float rho = lines[i][0];
			float theta = lines[i][1];
			Point pt1, pt2;
			double a = cos(theta);
			double b = sin(theta);
			double x0 = a * rho;
			double y0 = b * rho;
			pt1.x = cvRound(x0 + 1000 * (-b));
			pt1.y = cvRound(y0 + 1000 * (a));
			pt2.x = cvRound(x0 - 1000 * (-b));
			pt2.y = cvRound(y0 - 1000 * (a));
			line(linesImage, pt1, pt2, Scalar(0, 0, 255), 1, CV_AA);
		}
		

		// show the result
		ss.str("");
		ss.clear();
		ss << i << " detected lines";
		namedWindow(ss.str());
		imshow(ss.str(), linesImage);
		
	}
	*/

	Mat tableImage = imread("test_answer_table_filled.jpg", CV_LOAD_IMAGE_GRAYSCALE);

	if (tableImage.empty())
	{
		cerr << "Image not loaded!" << endl;
		return 1;
	}

	const int numberOfColumns = 6;
	const int numberOfRows = 9;

	auto tableCells = extract_cells(tableImage, numberOfRows, numberOfColumns);

	int i = 0;
	for (auto&& row : tableCells)
	{
		for (Mat cell : row)
		{
			stringstream ss;
			ss << i;
			namedWindow(ss.str());
			imshow(ss.str(), cell);
			i++;
			
			if (is_cell_crossed(cell))
			{
				cout << "X";
			}
			else
			{
				cout << " ";
			}
		}

		cout << ";" << endl;
	}
	// show the result
	//namedWindow("canny");
	//imshow("source", source);
	//imshow("canny", binaryImage);
	//imshow("detected lines", linesImage);

	waitKey(0);

	return 0;
}