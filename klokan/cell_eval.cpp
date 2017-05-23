#include "cell_eval.h"
#include "debug.h"

#include <vector>

bool is_line_top_left(cv::Vec2f line, int imageWidth, int imageHeight);
bool is_line_bottom_left(cv::Vec2f line, int imageWidth, int imageHeight);
bool is_line_top_right(cv::Vec2f line, int imageWidth, int imageHeight);
bool is_line_bottom_right(cv::Vec2f line, int imageWidth, int imageHeight);

bool is_cell_crossed(const cv::Mat& cellImage, const Parameters& parameters)
{	
	cv::Mat cellWorkingCopy = cellImage.clone();

	// resize the working copy
	cv::resize(cellWorkingCopy, cellWorkingCopy, cv::Size(parameters.default_cell_width, parameters.default_cell_height));

	// convert cell image to a binary image
	// NECESSARY BECAUSE EXTRACT TABLE FLOODS WITH GREY
	cv::threshold(cellWorkingCopy, cellWorkingCopy, 200, 255, CV_THRESH_BINARY);

	// invert the colors (needed for HoughLines)
	// NOT NECESSARY AFTER EXTRACT_TABLES
	// cv::bitwise_not(cellWorkingCopy, cellWorkingCopy);

	// find all the lines in the image
	std::vector<cv::Vec2f> lines;
	cv::HoughLines(cellWorkingCopy, lines, 1, parameters.cross_line_curvature_limit * (CV_PI / 180), parameters.cross_line_length);

	// if cell is empty
	if (lines.empty())
	{
		return false;
	}

	// counters for lines that form a cross and others
	int topLeftBottomRightCount = 0;
	int bottomLeftTopRightCount = 0;
	int otherCount = 0;

	// for each line check where it belongs
	for (size_t i = 0; i < lines.size(); i++)
	{
		if (is_line_top_left(lines[i], cellWorkingCopy.cols, cellWorkingCopy.rows) &&
			is_line_bottom_right(lines[i], cellWorkingCopy.cols, cellWorkingCopy.rows))
		{
			topLeftBottomRightCount++;
		}
		else if (is_line_bottom_left(lines[i], cellWorkingCopy.cols, cellWorkingCopy.rows) &&
			is_line_top_right(lines[i], cellWorkingCopy.cols, cellWorkingCopy.rows))
		{
			bottomLeftTopRightCount++;
		}
		else
		{
			otherCount++;
		}
	}

	//debug::show_lines(cellWorkingCopy, lines, "" + (int)lines[0][0]);

	// decide if a cell is crossed or not
	if (topLeftBottomRightCount > 0 && bottomLeftTopRightCount > 0 &&
		otherCount <= parameters.rubbish_lines_limit)
	{
		return true;
	}
	else
	{
		//debug::show_lines(cellWorkingCopy, lines, "" + lines.size());
		return false;
	}
}

// returns true if the line intersects the cell image border in the top left corner, false otherwise
bool is_line_top_left(cv::Vec2f line, int imageWidth, int imageHeight)
{
	float rho = line[0];		// the length of the normal (can be negative)
	float theta = line[1];		// the angle that (rho, 0) vector has to be rotated to the right by to get the normal

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

// returns true if the line intersects the cell image border in the bottom left corner, false otherwise
bool is_line_bottom_left(cv::Vec2f line, int imageWidth, int imageHeight)
{
	float rho = line[0];		// the length of the normal (can be negative)
	float theta = line[1];		// the angle that (rho, 0) vector has to be rotated to the right by to get the normal

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

// returns true if the line intersects the cell image border in the top right corner, false otherwise
bool is_line_top_right(cv::Vec2f line, int imageWidth, int imageHeight)
{
	float rho = line[0];		// the length of the normal (can be negative)
	float theta = line[1];		// the angle that (rho, 0) vector has to be rotated to the right by to get the normal

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

// returns true if the line intersects the cell image border in the bottom right corner, false otherwise
bool is_line_bottom_right(cv::Vec2f line, int imageWidth, int imageHeight)
{
	float rho = line[0];		// the length of the normal (can be negative)
	float theta = line[1];		// the angle that (rho, 0) vector has to be rotated to the right by to get the normal

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