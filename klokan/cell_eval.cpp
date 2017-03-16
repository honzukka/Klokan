#include <iostream>
#include "opencv2\highgui\highgui.hpp"
#include "opencv2\imgproc\imgproc.hpp"

using namespace std;
using namespace cv;

int main()
{
	Mat src = imread("test_cross.jpg", CV_LOAD_IMAGE_GRAYSCALE);
	Mat dst, cdst;

	threshold(src, dst, 200, 255, CV_THRESH_BINARY);
	bitwise_not(dst, dst);
	cvtColor(dst, cdst, CV_GRAY2BGR);

	vector<Vec2f> lines;

	// detect lines
	HoughLines(dst, lines, 3, 5 * (CV_PI / 180), 20, 0, 0);

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
		line(cdst, pt1, pt2, Scalar(0, 0, 255), 3, CV_AA);
	}

	// show the result
	imshow("source", src);
	//imshow("canny", dst);
	imshow("detected lines", cdst);

	waitKey();

	return 0;
}