#ifndef KLOKAN_
#define KLOKAN_

#include "opencv2\core\core.hpp"
#include "opencv2\highgui\highgui.hpp"
#include "opencv2\imgproc\imgproc.hpp"

#include "parameters.h"

#include <array>
#include <string>
#include <vector>
#include <iostream>
#include <fstream>

enum AnswerType
{
	CORRECT,
	CORRECTED,
	INCORRECT,
	VOID
};

using tableAnswers = std::array<std::array<bool, TABLE_COLUMNS - 1>, TABLE_ROWS - 1>;
using correctedTableAnswers = std::array<std::array<AnswerType, TABLE_COLUMNS - 1>, TABLE_ROWS - 1>;
using sheetAnswers = std::array<tableAnswers, TABLE_COUNT>;
using correctedSheetAnswers = std::array<correctedTableAnswers, TABLE_COUNT>;

class Klokan
{
public:
	Klokan(Parameters parameters) : parameters_(parameters) {}

	Klokan(const Klokan& other) = delete;
	Klokan(Klokan&& other) = delete;

	Klokan& operator=(const Klokan& other) = delete;
	Klokan& operator=(Klokan&& other) = delete;

	void run(const std::string& correctAnswerSheetName, const std::vector<std::string>& answerSheetNames);

private:
	sheetAnswers extract_answers(cv::Mat& sheetImage);
	correctedSheetAnswers correct_answers(const sheetAnswers& answers);
	size_t count_score(const correctedSheetAnswers& correctedAnswers);
	bool output_results(size_t score, const correctedSheetAnswers& correctedAnswers, const std::string& originalFilename);

	Parameters parameters_;
	sheetAnswers correctAnswers_;
};

#endif // !KLOKAN_
