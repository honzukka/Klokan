#include "debug.h"

namespace debug
{
	void show_image(const cv::Mat& image, const std::string& windowName)
	{
		// show the image
		cv::imshow(windowName, image);
	}

	void show_cells(const std::vector<std::vector<cv::Mat>>& table, const std::string& windowName)
	{
		const int gap = 5;
		
		int numberOfRows = table.size();
		int numberOfColumns = table[0].size();
		int cellWidth = table[0][0].cols;
		int cellHeight = table[0][0].rows;

		int extraWidth = (numberOfColumns - 1) * gap;
		int extraHeight = (numberOfRows - 1) * gap;

		int outputWidth = (numberOfColumns * cellWidth) + extraWidth;
		int outputHeight = (numberOfRows * cellHeight) + extraHeight;

		// new 1-channel image
		cv::Mat output = cv::Mat(cv::Size(outputWidth, outputHeight), CV_8UC1, cv::Scalar(128));

		// copy cells into the output image one by one
		for (size_t i = 0; i < numberOfRows; i++)
		{
			for (size_t j = 0; j < numberOfColumns; j++)
			{
				int xInOutput = j * (cellWidth + gap);
				int yInOutput = i * (cellHeight + gap);
				table[i][j].copyTo(output(cv::Rect(xInOutput, yInOutput, cellWidth, cellHeight)));
			}
		}

		// show the image
		cv::imshow(windowName, output);
	}

	void show_lines(const cv::Mat& source, const std::vector<cv::Vec2f>& lines, const std::string& windowName)
	{
		cv::Mat output;

		cv::cvtColor(source, output, CV_GRAY2BGR);

		// draw lines
		for (size_t i = 0; i < lines.size(); i++)
		{
			float rho = lines[i][0];
			float theta = lines[i][1];
			cv::Point pt1, pt2;
			double a = cos(theta);
			double b = sin(theta);
			double x0 = a * rho;
			double y0 = b * rho;
			pt1.x = cvRound(x0 + 2000 * (-b));
			pt1.y = cvRound(y0 + 2000 * (a));
			pt2.x = cvRound(x0 - 2000 * (-b));
			pt2.y = cvRound(y0 - 2000 * (a));
			line(output, pt1, pt2, cv::Scalar(255, 0, 0), 1, CV_AA);
		}

		// show the image
		cv::imshow(windowName, output);
	}

	void show_points(const cv::Mat& source, const std::vector<cv::Point>& points, const std::string& windowName)
	{
		cv::Mat output;
		
		cv::cvtColor(source, output, CV_GRAY2BGR);

		// draw points
		for (auto&& point : points)
		{
			cv::circle(output, point, 10, cv::Scalar(0, 0, 255), -1);
		}

		// show the image
		cv::imshow(windowName, output);
	}
}