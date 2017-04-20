#ifndef CELL_EVAL_
#define CELL_EVAL_

#include "opencv2\core\core.hpp"
#include "opencv2\highgui\highgui.hpp"
#include "opencv2\imgproc\imgproc.hpp"

#define DLLExport __declspec(dllexport)

DLLExport bool is_cell_crossed(cv::Mat cellImage);

#endif // !CELL_EVAL_