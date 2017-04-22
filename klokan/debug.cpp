#include "debug.h"

namespace debug
{
	void show_image(cv::Mat image, std::string windowName)
	{
		cv::imshow(windowName, image);
	}

	void show_cells(std::vector<std::vector<cv::Mat>> table, std::string windowName)
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

		// new image with 3 channels
		//cv::Mat output = cv::Mat(cv::Size(outputWidth, outputHeight), CV_8UC3);

		// new image with 1 channel
		cv::Mat output = cv::Mat(cv::Size(outputWidth, outputHeight), CV_8UC1, cv::Scalar(128));

		for (size_t i = 0; i < numberOfRows; i++)
		{
			for (size_t j = 0; j < numberOfColumns; j++)
			{
				int xInOutput = j * (cellWidth + gap);
				int yInOutput = i * (cellHeight + gap);
				table[i][j].copyTo(output(cv::Rect(xInOutput, yInOutput, cellWidth, cellHeight)));
			}
		}

		imshow(windowName, output);
	}

	void show_lines(std::vector<cv::Vec2f> lines, cv::Mat image, std::string windowName)
	{
		cv::cvtColor(image, image, CV_GRAY2BGR);

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
			line(image, pt1, pt2, cv::Scalar(255, 0, 0), 1, CV_AA);
		}

		imshow(windowName, image);
	}

	void show_points(std::vector<cv::Point> points, cv::Mat image, std::string windowName)
	{
		cv::cvtColor(image, image, CV_GRAY2BGR);

		// draw points
		for (auto&& point : points)
		{
			cv::circle(image, point, 10, cv::Scalar(0, 0, 255), -1);
		}

		imshow(windowName, image);
	}

	void draw_lines_and_show(cv::Mat source, std::vector<cv::Vec2f> lines, std::string windowName)
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
			pt1.x = cvRound(x0 + 1000 * (-b));
			pt1.y = cvRound(y0 + 1000 * (a));
			pt2.x = cvRound(x0 - 1000 * (-b));
			pt2.y = cvRound(y0 - 1000 * (a));
			line(output, pt1, pt2, cv::Scalar(255, 0, 0), 1, CV_AA);
		}

		cv::imshow(windowName, output);
	}
}