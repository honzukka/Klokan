#ifndef CELL_EVAL_
#define CELL_EVAL_

#include "opencv2\core\core.hpp"
#include "opencv2\highgui\highgui.hpp"
#include "opencv2\imgproc\imgproc.hpp"

#define DLLExport __declspec(dllexport)

// returns if a cell is crossed (true) or if it's empty or corrected (false)
DLLExport bool is_cell_crossed(const cv::Mat& cellImage);

#endif // !CELL_EVAL_