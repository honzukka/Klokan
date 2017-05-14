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

using tableAnswers = std::array<std::array<bool, TABLE_COLUMNS>, TABLE_ROWS>;
using correctedTableAnswers = std::array<std::array<AnswerType, TABLE_COLUMNS>, TABLE_ROWS>;
using sheetAnswers = std::array<tableAnswers, TABLE_COUNT>;
using correctedSheetAnswers = std::array<correctedTableAnswers, TABLE_COUNT>;

enum AnswerType
{
	CORRECT,
	CORRECTED,
	INCORRECT,
	VOID
};

class Klokan
{
public:
	Klokan() {}

	Klokan(const Klokan& other) = delete;
	Klokan(Klokan&& other) = delete;

	Klokan& operator=(const Klokan& other) = delete;
	Klokan& operator=(Klokan&& other) = delete;

	void run(const std::string& correctAnswerSheetName, const std::vector<const std::string&>& answerSheetNames);

private:
	sheetAnswers extract_answers(cv::Mat& sheetImage);
	correctedSheetAnswers correct_answers(const sheetAnswers& answers);
	size_t count_score(const correctedSheetAnswers& correctedAnswers);
	bool output_results(size_t score, const correctedSheetAnswers& correctedAnswers, const std::string& originalFilename);

	sheetAnswers correctAnswers_;
};

#endif // !KLOKAN_
