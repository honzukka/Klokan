#ifndef CELL_EVAL_
#define CELL_EVAL_

#include "opencv2\core\core.hpp"
#include "opencv2\highgui\highgui.hpp"
#include "opencv2\imgproc\imgproc.hpp"

#include "parameters.h"

// export for unit tests
#define DLLExport __declspec(dllexport)

// returns if a cell is crossed (true) or if it's empty or corrected (false) - uses shape recognition
DLLExport bool is_cell_crossed_shape(const cv::Mat& cellImage, const Parameters& parameters);

// returns if a cell is crossed (true) or if it's empty or corrected (false) - uses the ratio of white pixels and total pixels
DLLExport bool is_cell_crossed_ratio(const cv::Mat& cellImage, const Parameters& parameters);

#endif // !CELL_EVAL_