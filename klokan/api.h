#ifndef API_
#define API_

#include "opencv2\core\core.hpp"
#include "opencv2\highgui\highgui.hpp"
#include "opencv2\imgproc\imgproc.hpp"

#include "parameters.h"

#include <vector>

using tableAnswers = std::vector<std::vector<bool>>;
using sheetAnswers = std::vector<tableAnswers>;

// extracts answers from an answer sheet using image recognition
// relies on the caller to provide answerArray of size (params.table_count * (params.table_rows - 1) * (params.table_columns - 1)) and the success variable
extern "C" __declspec(dllexport) void __stdcall extract_answers_api(char* filename, Parameters params, bool* answerArray, bool* success);

#endif // !API_
