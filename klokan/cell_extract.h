#ifndef CELL_EXTRACT_
#define CELL_EXTRACT_

#include "opencv2\core\core.hpp"
#include "opencv2\highgui\highgui.hpp"
#include "opencv2\imgproc\imgproc.hpp"

#include "parameters.h"

#include <vector>

// export for unit tests
#define DLLExport __declspec(dllexport)

// splits the tableImage into cells based on the number of rows and columns
// cells returned are only contain pointers to the original tableImage!
DLLExport std::vector<std::vector<cv::Mat>> extract_cells(const cv::Mat& tableImage, const Parameters& parameters);

#endif // !CELL_EXTRACT_