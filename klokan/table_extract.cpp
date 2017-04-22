#include "table_extract.h"
#include "parameters.h"
#include "debug.h"

cv::Point find_largest_blob(cv::Mat image, int threshold, int newThreshold);
std::vector<cv::Vec2f> find_blob_lines(cv::Mat blobImage, cv::Point blobPoint);
std::vector<cv::Point> find_corners_of_table(std::vector<cv::Vec2f>& lines);
cv::Mat crop_fix_perspective(cv::Mat image, std::vector<cv::Point> cornerPoints);

void find_extreme_lines(std::vector<cv::Vec2f>& lines, cv::Vec2f& topEdge, cv::Vec2f& bottomEdge, cv::Vec2f& leftEdge, cv::Vec2f& rightEdge);
cv::Point find_intersection(cv::Vec2f line1, cv::Vec2f line2);

// order by x-coordinate
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

std::vector<Table> extract_tables(cv::Mat image, int numberOfTables)
{
	/*
	// resize the image
	const int defaultImageWidth = 1000;
	float heightToWidthRatio = image.rows / image.cols;
	cv::Size newSize(defaultImageWidth, defaultImageWidth * heightToWidthRatio);
	cv::resize(image, image, newSize, 0.0, 0.0);
	*/

	std::vector<Table> tables;
	
	cv::threshold(image, image, BLACK_WHITE_THRESHOLD, 255, CV_THRESH_BINARY);
	cv::bitwise_not(image, image);

	// make the lines in the image thicker
	cv::Mat kernel = (cv::Mat_<uchar>(3, 3) << 0, 1, 0, 1, 1, 1, 0, 1, 0);
	
	cv::Mat dilatedImage;
	cv::dilate(image, dilatedImage, kernel);
	
	for (int i = 0; i < numberOfTables; i++)
	{
		cv::Point maxBlobPoint = find_largest_blob(dilatedImage, 255 - (i * 5), 255 - ((i + 1) * 5));

		cv::Mat workingCopyImage = dilatedImage.clone();
		std::vector<cv::Vec2f> lines = find_blob_lines(workingCopyImage, maxBlobPoint);

		// debug
		debug::show_lines(lines, dilatedImage, "all blob lines " + i);

		std::vector<cv::Point> cornerPoints = find_corners_of_table(lines);

		// debug
		//debug::show_points(cornerPoints, dilatedImage, "corner points " + i);

		cv::Mat table_image = crop_fix_perspective(dilatedImage, cornerPoints);

		Table table{ table_image, maxBlobPoint };
		tables.push_back(std::move(table));

		// hide the processed blob (table)
		cv::floodFill(dilatedImage, maxBlobPoint, 0);
	}

	// sort the tables
	sort(tables.begin(), tables.end(), TableComparer());

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

	// debug
	//debug::show_image(blobImage, "looking for lines here");

	// find the lines
	std::vector<cv::Vec2f> lines;
	cv::HoughLines(blobImage, lines, 1, (CV_PI / 180), TABLE_LINE_LENGTH);

	return lines;
}

std::vector<cv::Point> find_corners_of_table(std::vector<cv::Vec2f>& lines)
{
	// initialize the edges with unrealistic rho and theta respectively
	cv::Vec2f topEdge;
	cv::Vec2f bottomEdge;
	cv::Vec2f leftEdge;
	cv::Vec2f rightEdge;

	find_extreme_lines(lines, topEdge, bottomEdge, leftEdge, rightEdge);

	// find their intersections
	cv::Point topLeftCorner = find_intersection(topEdge, leftEdge);
	cv::Point topRightCorner = find_intersection(topEdge, rightEdge);
	cv::Point bottomLeftCorner = find_intersection(bottomEdge, leftEdge);
	cv::Point bottomRightCorner = find_intersection(bottomEdge, rightEdge);

	return std::vector<cv::Point>{ topLeftCorner, topRightCorner, bottomLeftCorner, bottomRightCorner };
}

cv::Mat crop_fix_perspective(cv::Mat image, std::vector<cv::Point> cornerPoints)
{
	// TODO: pass the points differently so that you don't have to rely on their order?
	// get the top edge length
	int topLength = sqrt((cornerPoints[1].x - cornerPoints[0].x) * (cornerPoints[1].x - cornerPoints[0].x) +
		(cornerPoints[1].y - cornerPoints[0].y) * (cornerPoints[1].y - cornerPoints[0].y));

	// get the side edge length
	int sideLength = sqrt((cornerPoints[2].x - cornerPoints[0].x) * (cornerPoints[2].x - cornerPoints[0].x) +
		(cornerPoints[2].y - cornerPoints[0].y) * (cornerPoints[2].y - cornerPoints[0].y));

	cv::Point2f source[4], destination[4];

	source[0] = cornerPoints[0];	source[1] = cornerPoints[1];
	source[2] = cornerPoints[2];	source[3] = cornerPoints[3];

	destination[0] = cv::Point2f(0, 0);						destination[1] = cv::Point2f(topLength - 1, 0);
	destination[2] = cv::Point2f(0, sideLength - 1);		destination[3] = cv::Point2f(topLength - 1, sideLength - 1);

	cv::Mat fixed = cv::Mat(cv::Size(topLength, sideLength), CV_8UC1);
	cv::warpPerspective(image, fixed, cv::getPerspectiveTransform(source, destination), cv::Size(topLength, sideLength));
	
	return fixed;
}

void find_extreme_lines(std::vector<cv::Vec2f>& lines, cv::Vec2f& topEdge, cv::Vec2f& bottomEdge, cv::Vec2f& leftEdge, cv::Vec2f& rightEdge)
{
	// initialize the edges with unrealistic rho and theta respectively
	//topEdge[0] = 10000, topEdge[1] = 10000;
	//bottomEdge[0] = -10000, bottomEdge[1] = -10000;
	//leftEdge[0] = 10000, leftEdge[1] = 10000;
	//rightEdge[0] = -10000, rightEdge[1] = -10000;

	/*
	// initialize intersections with axes of these edges with unrealistic values as well
	double topYIntersection = 100000;
	double bottomYIntersection = -100000;
	double leftXIntersection = 100000;
	double rightXIntersection = -100000;
	*/

	double topNormal = 100000;
	double bottomNormal = -100000;
	double leftNormal = 100000;
	double rightNormal = -100000;

	// find the all the edges
	for (auto&& line : lines)
	{
		float rho = line[0];
		float theta = line[1];

		// if line is "horizontal"
		if (theta > CV_PI / 2 - TABLE_LINE_ECCENTRICITY_LIMIT && theta < CV_PI / 2 + TABLE_LINE_ECCENTRICITY_LIMIT)
		{
			// get the intersection of the line with y-axis
			//double yIntersection = rho / sin(theta);

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

			/*
			// line is higher up
			if (yIntersection < topYIntersection)
			{
				topEdge = line;
				topYIntersection = yIntersection;
			}
			// line is lower down
			else if (yIntersection > bottomYIntersection)
			{
				bottomEdge = line;
				bottomYIntersection = yIntersection;
			}
			*/
		}
		// line is "vertical"
		else if (theta > -TABLE_LINE_ECCENTRICITY_LIMIT && theta < TABLE_LINE_ECCENTRICITY_LIMIT)
		{
			// get intersection of the line with x-axis
			//double xIntersection = rho / cos(theta);

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

			/*
			// line is further left
			if (xIntersection < leftXIntersection)
			{
				leftEdge = line;
				leftXIntersection = xIntersection;
			}
			// line is further right
			else if (xIntersection > rightXIntersection)
			{
				rightEdge = line;
				rightXIntersection = xIntersection;
			}
			*/
		}
	}
}

// return an intersection of two lines
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

	
	/*
	// lineXContribution is a vector perpendicular to the line, starting at the origin and ending at the line
	// this way, when we add both contributions together, we get the intersections of both lines
	cv::Point line1Contribution;
	line1Contribution.x = rho1 * cos(theta1);
	line1Contribution.y = rho1 * sin(theta1);

	cv::Point line2Contribution;
	line2Contribution.x = rho2 * cos(theta2);
	line2Contribution.y = rho2 * sin(theta2);

	intersection = intersection + line1Contribution + line2Contribution;
	*/
	return intersection;
}
