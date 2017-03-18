#include "opencv2\core\core.hpp"
#include "opencv2\highgui\highgui.hpp"
#include "opencv2\imgproc\imgproc.hpp"
#include "opencv2\opencv.hpp"

#include "table_extract.h"
#include "cell_extract.h"
#include "cell_eval.h"

#include <iostream>
#include <sstream>

using namespace std;
using namespace cv;

int main()
{
	// TODO: make parameters easy to tweak
	// TODO: create debugging functions which would show images at crucial points
	
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

	/*
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
	*/

	Mat sheetImage = imread("test_answer_sheet_rotated_filled.jpg", CV_LOAD_IMAGE_GRAYSCALE);

	extract_tables(sheetImage, 1);

	/*
	cv::threshold(sheetImage, sheetImage, 200, 255, CV_THRESH_BINARY);
	cv::bitwise_not(sheetImage, sheetImage);

	Mat kernel = (Mat_<uchar>(3, 3) << 0, 1, 0, 1, 1, 1, 0, 1, 0);
	dilate(sheetImage, sheetImage, kernel);

	int areaMax = -1;
	Point maxPt;

	for (int y = 0; y < sheetImage.size().height; y++)
	{
		uchar* row = sheetImage.ptr(y);
		for (int x = 0; x < sheetImage.size().width; x++)
		{
			if (row[x] >= 128)
			{
				int area = floodFill(sheetImage, Point(x, y), 64);

				if (area > areaMax)
				{
					maxPt = Point(x, y);
					areaMax = area;
				}
			}
		}
	}

	floodFill(sheetImage, maxPt, 255);
	
	areaMax = -1;
	
	for (int y = 0; y < sheetImage.size().height; y++)
	{
		uchar* row = sheetImage.ptr(y);
		for (int x = 0; x < sheetImage.size().width; x++)
		{
			if (row[x] >= 64)
			{
				int area = floodFill(sheetImage, Point(x, y), 32);

				if (area > areaMax)
				{
					maxPt = Point(x, y);
					areaMax = area;
				}
			}
		}
	}

	floodFill(sheetImage, maxPt, 0);

	areaMax = -1;

	for (int y = 0; y < sheetImage.size().height; y++)
	{
		uchar* row = sheetImage.ptr(y);
		for (int x = 0; x < sheetImage.size().width; x++)
		{
			if (row[x] >= 32)
			{
				int area = floodFill(sheetImage, Point(x, y), 16);

				if (area > areaMax)
				{
					maxPt = Point(x, y);
					areaMax = area;
				}
			}
		}
	}

	floodFill(sheetImage, maxPt, 255);
	

	for (int y = 0; y < sheetImage.size().height; y++)
	{
		uchar* row = sheetImage.ptr(y);
		for (int x = 0; x < sheetImage.size().width; x++)
		{
			if (row[x] < 255 && row[x] > 0)
			{
				int area = floodFill(sheetImage, Point(x, y), 0);
			}
		}
	}

	

	std::vector<cv::Vec2f> lines;
	cv::HoughLines(sheetImage, lines, 1, 5 * (CV_PI / 180), 200);

	erode(sheetImage, sheetImage, kernel);

	Mat linesImage = sheetImage.clone();
	cvtColor(linesImage, linesImage, CV_GRAY2BGR);

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

	imshow("ahoj", linesImage);
	*/
	/*
	SimpleBlobDetector::Params params;
	// Change thresholds
	//params.minThreshold = 10;
	//params.maxThreshold = 200;

	// Filter by Area.
	params.filterByArea = true;
	params.minArea = 500;

	// Filter by Circularity
	//params.filterByCircularity = true;
	//params.minCircularity = 0.7;
	//params.maxCircularity = 0.8;

	// Filter by Convexity
	params.filterByConvexity = true;
	params.minConvexity = 0;

	vector<KeyPoint> keypoints;

	Ptr<SimpleBlobDetector> detector = SimpleBlobDetector::create(params);
	detector->detect(sheetImage, keypoints);

	Mat imWithKeypoints;
	drawKeypoints(sheetImage, keypoints, imWithKeypoints, Scalar(0, 0, 255));

	imshow("keypoints", imWithKeypoints);
	*/


	// show the result
	//namedWindow("canny");
	//imshow("source", source);
	//imshow("canny", binaryImage);
	//imshow("detected lines", linesImage);

	waitKey(0);

	return 0;
}