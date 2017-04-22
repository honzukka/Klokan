#ifndef DEBUG_
#define DEBUG_

#include "opencv2\core\core.hpp"
#include "opencv2\highgui\highgui.hpp"
#include "opencv2\imgproc\imgproc.hpp"

#include <vector>

namespace debug 
{
	// simply shows the image given
	void show_image(const cv::Mat& image, const std::string& windowName);
	
	// shows cells in a single 1-channel image with a gap in between them
	void show_cells(const std::vector<std::vector<cv::Mat>>& table, const std::string& windowName);

	// copies the source, draws colourful lines into it and shows it
	void show_lines(const cv::Mat& source, const std::vector<cv::Vec2f>& lines, const std::string& windowName);

	// copies the source, draws colourful points into the image and shows it
	void show_points(const cv::Mat& source, const std::vector<cv::Point>& points, const std::string& windowName);
}


#endif // !DEBUG_
