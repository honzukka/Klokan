#ifndef DEBUG_
#define DEBUG_

#include "opencv2\core\core.hpp"
#include "opencv2\highgui\highgui.hpp"
#include "opencv2\imgproc\imgproc.hpp"

#include <vector>

namespace debug 
{
	void show_image(cv::Mat image, std::string windowName);
	void show_cells(std::vector<std::vector<cv::Mat>> table, std::string windowName);
	void show_lines(std::vector<cv::Vec2f> lines, cv::Mat image, std::string windowName);
	void show_points(std::vector<cv::Point> points, cv::Mat image, std::string windowName);

	void draw_lines(cv::Mat source, cv::Mat destination, std::vector<cv::Vec2f> lines);
}


#endif // !DEBUG_
