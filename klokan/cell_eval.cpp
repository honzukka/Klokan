#include "cell_eval.h"

#include <vector>

bool is_line_top_left(cv::Vec2f line, int imageWidth, int imageHeight);
bool is_line_bottom_left(cv::Vec2f line, int imageWidth, int imageHeight);
bool is_line_top_right(cv::Vec2f line, int imageWidth, int imageHeight);
bool is_line_bottom_right(cv::Vec2f line, int imageWidth, int imageHeight);

bool is_cell_crossed(cv::Mat cellImage)
{
	// default size of the working copy
	const int workingCopyWidth = 80;
	const int workingCopyHeight = 40;
	
	cv::Mat cellWorkingCopy = cellImage.clone();

	// resize the working copy
	cv::resize(cellWorkingCopy, cellWorkingCopy, cv::Size(workingCopyWidth, workingCopyHeight));

	// convert cell image to a binary image
	cv::threshold(cellWorkingCopy, cellWorkingCopy, 200, 255, CV_THRESH_BINARY);

	// invert the colors (needed for HoughLines)
	cv::bitwise_not(cellWorkingCopy, cellWorkingCopy);

	// find all the lines in the image
	std::vector<cv::Vec2f> lines;
	cv::HoughLines(cellWorkingCopy, lines, 1, 5 * (CV_PI / 180), 50);

	if (lines.empty())
	{
		return false;
	}

	// check if lines form a cross
	int topLeftBottomRightCount = 0;
	int bottomLeftTopRightCount = 0;
	int otherCount = 0;

	for (size_t i = 0; i < lines.size(); i++)
	{
		if (is_line_top_left(lines[i], workingCopyWidth, workingCopyHeight) &&
			is_line_bottom_right(lines[i], workingCopyWidth, workingCopyHeight))
		{
			topLeftBottomRightCount++;
		}
		else if (is_line_bottom_left(lines[i], workingCopyWidth, workingCopyHeight) &&
			is_line_top_right(lines[i], workingCopyWidth, workingCopyHeight))
		{
			bottomLeftTopRightCount++;
		}
		else
		{
			otherCount++;
		}
	}

	// decide
	if (topLeftBottomRightCount > 0 && bottomLeftTopRightCount > 0 &&
		otherCount <= 3)
	{
		return true;
	}
	else
	{
		return false;
	}
}

// returns true of the line intersects with the image border in the top left corner, false otherwise
bool is_line_top_left(cv::Vec2f line, int imageWidth, int imageHeight)
{
	float rho = line[0];		// perpendicular distance from the origin
	float theta = line[1];		// angle between x-axis and normal vector

	// find x for y = 0
	double xTopIntersecton = rho / cos(theta);

	// does it lie in the left half of the top edge of the image?
	if (xTopIntersecton >= 0 && xTopIntersecton < imageWidth / 2)
	{
		return true;
	}
	else
	{
		// find y for x = 0
		double yLeftIntersection = rho / sin(theta);

		// does it lie in the upper half of the left edge of the image?
		if (yLeftIntersection >= 0 && yLeftIntersection < imageHeight / 2)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}

// returns true of the line intersects with the image border in the bottom left corner, false otherwise
bool is_line_bottom_left(cv::Vec2f line, int imageWidth, int imageHeight)
{
	float rho = line[0];		// perpendicular distance from the origin
	float theta = line[1];		// angle between x-axis and normal vector

	// find x for y = imageHeight
	double xBottomIntersecton = (rho - imageHeight * sin(theta)) / cos(theta);

	// does it lie in the left half of the bottom edge of the image?
	if (xBottomIntersecton >= 0 && xBottomIntersecton < imageWidth / 2)
	{
		return true;
	}
	else
	{
		// find y for x = 0
		double yLeftIntersection = rho / sin(theta);

		// does it lie in the lower half of the left edge of the image?
		if (yLeftIntersection > imageHeight / 2 && yLeftIntersection <= imageHeight)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}

// returns true of the line intersects with the image border in the top right corner, false otherwise
bool is_line_top_right(cv::Vec2f line, int imageWidth, int imageHeight)
{
	float rho = line[0];		// perpendicular distance from the origin
	float theta = line[1];		// angle between x-axis and normal vector

	// find x for y = 0
	double xTopIntersection = rho / cos(theta);

	// does it lie in the right half of the top edge of the image?
	if (xTopIntersection > imageWidth / 2 && xTopIntersection <= imageWidth)
	{
		return true;
	}
	else
	{
		// find y for x = imageWidth
		double rightIntersection = (rho - imageWidth * cos(theta)) / sin(theta);

		// does it lie in the upper half of the right edge of the image?
		if (rightIntersection >= 0 && rightIntersection < imageHeight / 2)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}

// returns true of the line intersects with the image border in the bottom right corner, false otherwise
bool is_line_bottom_right(cv::Vec2f line, int imageWidth, int imageHeight)
{
	float rho = line[0];		// perpendicular distance from the origin
	float theta = line[1];		// angle between x-axis and normal vector

	// find x for y = imageHeight
	double xBottomIntersection = (rho - imageHeight * sin(theta)) / cos(theta);

	// does it lie in the right half of the bottom edge of the image?
	if (xBottomIntersection > imageWidth / 2 && xBottomIntersection <= imageWidth)
	{
		return true;
	}
	else
	{
		// find y for x = imageWidth
		double rightIntersection = (rho - imageWidth * cos(theta)) / sin(theta);

		// does it lie in the lower half of the right edge of the image?
		if (rightIntersection > imageHeight / 2 && rightIntersection <= imageHeight)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}