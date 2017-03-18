#include "table_extract.h"

cv::Point find_largest_blob(cv::Mat image, int threshold, int newThreshold);
std::vector<cv::Vec2f> find_blob_lines(cv::Mat blobImage, cv::Point blobPoint);
std::vector<cv::Point> find_corners_of_table(std::vector<cv::Vec2f> lines);
cv::Mat crop_fix_perspective(cv::Mat image, std::vector<cv::Point> cornerPoints);

std::vector<cv::Mat> extract_tables(cv::Mat image, int numberOfTables)
{
	std::vector<cv::Mat> tables;
	
	cv::threshold(image, image, 200, 255, CV_THRESH_BINARY);
	cv::bitwise_not(image, image);

	// make the lines in the image thicker
	cv::Mat kernel = (cv::Mat_<uchar>(3, 3) << 0, 1, 0, 1, 1, 1, 0, 1, 0);
	dilate(image, image, kernel);
	
	for (int i = 0; i < numberOfTables; i++)
	{
		cv::Point maxBlobPoint = find_largest_blob(image, 255 - (i * 5), 255 - ((i + 1) * 5));

		cv::Mat workingCopyImage = image.clone();
		std::vector<cv::Vec2f> lines = find_blob_lines(workingCopyImage, maxBlobPoint);

		std::vector<cv::Point> cornerPoints = find_corners_of_table(lines);

		cv::Mat table = crop_fix_perspective(image, cornerPoints);

		tables.push_back(table);

		// hide the processed blob (table)
		cv::floodFill(image, maxBlobPoint, 0);
	}

	return tables;
}


cv::Point find_largest_blob(cv::Mat image, int threshold, int newThreshold)
{
	int maxArea = -1;
	cv::Point maxBlobPoint;

	for (int y = 0; y < image.size().height; y++)
	{
		uchar* row = image.ptr(y);
		for (int x = 0; x < image.size().width; x++)
		{
			if (row[x] >= threshold)
			{
				int area = cv::floodFill(image, cv::Point(x, y), newThreshold);

				if (area > maxArea)
				{
					maxBlobPoint = cv::Point(x, y);
					maxArea = area;
				}
			}
		}
	}

	return maxBlobPoint;
}

// finds all lines in a blob (presumably table) in the input image
// the input image will be modified
std::vector<cv::Vec2f> find_blob_lines(cv::Mat blobImage, cv::Point blobPoint)
{
	cv::floodFill(blobImage, blobPoint, 255);
	
	// hide the rest of the image (flood it with black)
	for (int y = 0; y < blobImage.size().height; y++)
	{
		uchar* row = blobImage.ptr(y);
		for (int x = 0; x < blobImage.size().width; x++)
		{
			if (row[x] < 255 && row[x] > 0)
			{
				int area = cv::floodFill(blobImage, cv::Point(x, y), 0);
			}
		}
	}

	// find the lines
	std::vector<cv::Vec2f> lines;
	cv::HoughLines(blobImage, lines, 1, 5 * (CV_PI / 180), 200);

	return lines;
}

std::vector<cv::Point> find_corners_of_table(std::vector<cv::Vec2f> lines)
{
	// initialize the edges with unrealistic rho and theta respectively
	cv::Vec2f topEdge(10000, 10000);
	cv::Vec2f bottomEdge(-10000, -10000);
	cv::Vec2f leftEdge(10000, 10000);
	cv::Vec2f rightEdge(-10000, -10000);

	// initialize different parameters of these edges with unrealistic values as well
	double topXIntersection = 0, topYIntersection = 100000;
	double bottomXIntersection = 0, bottomYIntersection = -100000;
	double leftXIntersection = 100000, leftYIntersection = 0;
	double rightXIntersection = -100000, rightYIntersection = 0;

	// find the all the edges
	for (auto&& line : lines)
	{
		float rho = line[0];
		float theta = line[1];

		double xIntersection = rho / cos(theta);
		double yIntersection = rho / sin(theta);
		
		// if line is "horizontal"
		if (theta > CV_PI / 4 && theta < 3 * CV_PI / 4)
		{
			// line is higher up
			if (yIntersection < topYIntersection)
			{
				topEdge = line;
				topYIntersection = yIntersection;
				topXIntersection = xIntersection;
			}
			// line is lower down
			else if (yIntersection > bottomYIntersection)
			{
				bottomEdge = line;
				bottomYIntersection = yIntersection;
				bottomXIntersection = xIntersection;
			}
		}
		// line is "vertical"
		else
		{
			// line is further left
			if (xIntersection < leftXIntersection)
			{
				leftEdge = line;
				leftXIntersection = xIntersection;
				leftYIntersection = yIntersection;
			}
			// line is further right
			else if (xIntersection > rightXIntersection)
			{
				rightEdge = line;
				rightXIntersection = xIntersection;
				rightYIntersection = yIntersection;
			}
		}
	}

	// find their intersections
	// TODO: finish intersections
}

cv::Mat crop_fix_perspective(cv::Mat image, std::vector<cv::Point> cornerPoints)
{
	return cv::Mat();
}