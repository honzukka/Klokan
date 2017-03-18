#ifndef TABLE_EXTRACT_
#define TABLE_EXTRACT_

#include "opencv2\core\core.hpp"
#include "opencv2\highgui\highgui.hpp"
#include "opencv2\imgproc\imgproc.hpp"

#include <vector>

std::vector<cv::Mat> extract_tables(cv::Mat image, int numberOfTables);

#endif // !TABLE_EXTRACT_
