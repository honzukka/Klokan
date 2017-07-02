#include "table_extract.h"
#include "parameters.h"
#include "debug.h"

cv::Point find_largest_blob(cv::Mat& image, int threshold, int newThreshold);
std::vector<cv::Vec2f> find_blob_lines(cv::Mat& blobImage, cv::Point blobPoint, const Parameters& parameters);
std::vector<cv::Point> find_corners_of_table(const std::vector<cv::Vec2f>& lines, const Parameters& parameters);
void find_extreme_lines(const std::vector<cv::Vec2f>& lines, cv::Vec2f& topEdge, cv::Vec2f& bottomEdge, cv::Vec2f& leftEdge, cv::Vec2f& rightEdge, const Parameters& parameters);
cv::Point find_intersection(cv::Vec2f line1, cv::Vec2f line2);
cv::Mat crop_fix_perspective(const cv::Mat& image, const std::vector<cv::Point>& cornerPoints);

bool TableComparer::operator() (const Table& table1, const Table& table2)
{
	if (table1.origin.x < table2.origin.x)
	{
		return true;
	}
	else
	{
		return false;
	}
}

std::vector<Table> extract_tables(cv::Mat& sheetImage, const Parameters& parameters)
{
	// resize the sheet
	float heightToWidthRatio = (float)sheetImage.rows / (float)sheetImage.cols;
	cv::Size newSize(parameters.default_sheet_width, parameters.default_sheet_width * heightToWidthRatio);
	cv::resize(sheetImage, sheetImage, newSize, 0.0, 0.0);
	
	std::vector<Table> tables;
	
	// convert the image to a binary image and invert it
	cv::threshold(sheetImage, sheetImage, parameters.black_white_threshold, 255, CV_THRESH_BINARY);
	cv::bitwise_not(sheetImage, sheetImage);

	// make the lines in the image thicker
	cv::Mat kernel = (cv::Mat_<uchar>(3, 3) << 0, 1, 0, 1, 1, 1, 0, 1, 0);
	cv::dilate(sheetImage, sheetImage, kernel);
	
	// find tables one by one
	for (int i = 0; i < parameters.table_count; i++)
	{
		cv::Point maxBlobPoint = find_largest_blob(sheetImage, 255 - (i * 5), 255 - ((i + 1) * 5));

		cv::Mat sheetImageCopy = sheetImage.clone();
		std::vector<cv::Vec2f> lines = find_blob_lines(sheetImageCopy, maxBlobPoint, parameters);

		//debug::show_lines(sheetImage, lines, "all blob lines " + i);

		std::vector<cv::Point> cornerPoints = find_corners_of_table(lines, parameters);

		//debug::show_points(sheetImage, cornerPoints, "corner points " + i);

		cv::Mat tableImage = crop_fix_perspective(sheetImage, cornerPoints);

		// save the table
		Table table{ tableImage, maxBlobPoint };
		tables.push_back(std::move(table));

		// hide the processed blob (table)
		cv::floodFill(sheetImage, maxBlobPoint, 0);
	}

	// sort the tables by the x-coordinate
	sort(tables.begin(), tables.end(), TableComparer());

	return tables;
}

// returns the upper left corner of the largest white blob in the image (it is expected to be a table)
// modifies the input image - white parts (of colour threshold) will be flooded with a newThreshold colour
cv::Point find_largest_blob(cv::Mat& image, int threshold, int newThreshold)
{
	int maxArea = -1;
	cv::Point maxBlobPoint;

	for (int y = 0; y < image.size().height; y++)
	{
		uchar* row = image.ptr(y);
		for (int x = 0; x < image.size().width; x++)
		{
			// if we see a white pixel that hasn't been flooded yet (=> has to be the upper left corner of an isolated blob)
			if (row[x] >= threshold)
			{
				int area = cv::floodFill(image, cv::Point(x, y), newThreshold);

				// if it's the largest we've seen so far
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
// modifies the input image
std::vector<cv::Vec2f> find_blob_lines(cv::Mat& blobImage, cv::Point blobPoint, const Parameters& parameters)
{
	// make the largest blob white again
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

	//debug::show_image(blobImage, "looking for lines here");

	// find the lines
	std::vector<cv::Vec2f> lines;
	cv::HoughLines(blobImage, lines, 1, parameters.table_line_curvature_limit * (CV_PI / 180), parameters.table_line_length);

	return lines;
}

// finds the intersections of the extreme lines of the collection
// returns them in a specific order
std::vector<cv::Point> find_corners_of_table(const std::vector<cv::Vec2f>& lines, const Parameters& parameters)
{
	cv::Vec2f topEdge;
	cv::Vec2f bottomEdge;
	cv::Vec2f leftEdge;
	cv::Vec2f rightEdge;

	find_extreme_lines(lines, topEdge, bottomEdge, leftEdge, rightEdge, parameters);

	// find their intersections
	cv::Point topLeftCorner = find_intersection(topEdge, leftEdge);
	cv::Point topRightCorner = find_intersection(topEdge, rightEdge);
	cv::Point bottomLeftCorner = find_intersection(bottomEdge, leftEdge);
	cv::Point bottomRightCorner = find_intersection(bottomEdge, rightEdge);

	return std::vector<cv::Point>{ topLeftCorner, topRightCorner, bottomLeftCorner, bottomRightCorner };
}

// returns the edges of the table
void find_extreme_lines(const std::vector<cv::Vec2f>& lines, cv::Vec2f& topEdge, cv::Vec2f& bottomEdge, cv::Vec2f& leftEdge, cv::Vec2f& rightEdge, const Parameters& parameters)
{
	// set impossible values
	double topNormal = 100000;
	double bottomNormal = -100000;
	double leftNormal = 100000;
	double rightNormal = -100000;

	// find the all the edges
	for (auto&& line : lines)
	{
		float rho = line[0];		// the length of the normal (can be negative)
		float theta = line[1];		// the angle that (rho, 0) vector has to be rotated to the right by to get the normal

		// if line is "horizontal"
		if (theta > CV_PI / 2 - parameters.table_line_eccentricity_limit && theta < CV_PI / 2 + parameters.table_line_eccentricity_limit)
		{
			// if the line is higher up
			if (abs(rho) < topNormal)
			{
				topEdge = line;
				topNormal = abs(rho);
			}
			
			// line is lower down
			if (abs(rho) > bottomNormal)
			{
				bottomEdge = line;
				bottomNormal = abs(rho);
			}
		}
		// line is "vertical"
		else if (theta >= 0 && theta < parameters.table_line_eccentricity_limit
					|| theta < CV_PI && theta > CV_PI - parameters.table_line_eccentricity_limit)
		{
			// line is further to the left
			if (abs(rho) < leftNormal)
			{
				leftEdge = line;
				leftNormal = abs(rho);
			}
			
			// line is further to the right
			if (abs(rho) > rightNormal)
			{
				rightEdge = line;
				rightNormal = abs(rho);
			}
		}
	}
}

// returns an intersection of two lines
cv::Point find_intersection(cv::Vec2f line1, cv::Vec2f line2)
{
	float rho1 = line1[0], theta1 = line1[1];
	float rho2 = line2[0], theta2 = line2[1];
	float cosTheta1 = cos(theta1);
	float cosTheta2 = cos(theta2);
	float sinTheta1 = sin(theta1);
	float sinTheta2 = sin(theta2);

	cv::Point intersection(0, 0);

	// calculate the intersection using homogenous coordinates
	// coordinates of line 1
	float a1 = cosTheta1;
	float b1 = sinTheta1;
	float c1 = -rho1;

	// coordinates of line 2
	float a2 = cosTheta2;
	float b2 = sinTheta2;
	float c2 = -rho2;

	// the intersection (i1, i2, i3) is the cross-product of the two vectors (a1, b1, c1) and (a2, b2, c2)
	float i1 = b1 * c2 - b2 * c1;
	float i2 = a2 * c1 - a1 * c2;
	float i3 = a1 * b2 - a2 * b1;

	// convert back to Euclidian coordinates
	intersection.x = i1 / i3;
	intersection.y = i2 / i3;

	return intersection;
}

// transforms image into a square image which it then returns
// corner points have to be passed in a specific order!
cv::Mat crop_fix_perspective(const cv::Mat& image, const std::vector<cv::Point>& cornerPoints)
{
	// get the top edge length
	int topLength = sqrt((cornerPoints[1].x - cornerPoints[0].x) * (cornerPoints[1].x - cornerPoints[0].x) +
		(cornerPoints[1].y - cornerPoints[0].y) * (cornerPoints[1].y - cornerPoints[0].y));

	// get the side edge length
	int sideLength = sqrt((cornerPoints[2].x - cornerPoints[0].x) * (cornerPoints[2].x - cornerPoints[0].x) +
		(cornerPoints[2].y - cornerPoints[0].y) * (cornerPoints[2].y - cornerPoints[0].y));

	cv::Point2f source[4], destination[4];

	// source points of the corners of the image
	source[0] = cornerPoints[0];	source[1] = cornerPoints[1];
	source[2] = cornerPoints[2];	source[3] = cornerPoints[3];

	// destination points of the corners of the image
	destination[0] = cv::Point2f(0, 0);						destination[1] = cv::Point2f(topLength - 1, 0);
	destination[2] = cv::Point2f(0, sideLength - 1);		destination[3] = cv::Point2f(topLength - 1, sideLength - 1);

	cv::Mat squareImage = cv::Mat(cv::Size(topLength, sideLength), CV_8UC1);
	cv::warpPerspective(image, squareImage, cv::getPerspectiveTransform(source, destination), cv::Size(topLength, sideLength));

	return squareImage;
}