#ifndef CELL_EXTRACT_
#define CELL_EXTRACT_

#include "opencv2\core\core.hpp"
#include "opencv2\highgui\highgui.hpp"
#include "opencv2\imgproc\imgproc.hpp"

#include <vector>

std::vector<std::vector<cv::Mat>> extract_cells(cv::Mat tableImage, int numberOfRows, int numberOfColumns);

#endif // !CELL_EXTRACT_