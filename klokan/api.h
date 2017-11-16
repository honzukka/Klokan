#ifndef API_
#define API_

#include "opencv2\core\core.hpp"
#include "opencv2\highgui\highgui.hpp"
#include "opencv2\imgproc\imgproc.hpp"

#include "parameters.h"

#include <vector>

using tableAnswers = std::vector<std::vector<bool>>;
using sheetAnswers = std::vector<tableAnswers>;
typedef bool(*cellEvalFunc)(const cv::Mat&, const Parameters&);

// loads the answer sheet image from an array of bytes sent from C# and passes it to the extract_answers function
extern "C" __declspec(dllexport) void __stdcall extract_answers_image_api(unsigned char* imageBytes, int rows, int cols, Parameters params, bool* numberArray, bool* answerArray, bool* success);

// loads the answers sheet image from a file specified by the filename and passes it to the extract_answers function
extern "C" __declspec(dllexport) void __stdcall extract_answers_path_api(char* filename, Parameters params, bool* numberArray, bool* answerArray, bool* success);

// extracts answers from an answer sheet using image recognition
// relies on the caller to provide numberArray of size (params.student_table_rows - 1) * (params.student_table_columns - 1),
// answerArray of size (params.table_count * (params.answer_table_rows - 1) * (params.answer_table_columns - 1)) and the success variable
void extract_answers(cv::Mat& sheetImage, Parameters params, bool* numberArray, bool* answerArray, bool* success);

#endif // !API_
