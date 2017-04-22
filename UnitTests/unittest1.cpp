#include "stdafx.h"
#include "CppUnitTest.h"

#include "..\klokan\table_extract.h"
#include "..\klokan\cell_extract.h"
#include "..\klokan\cell_eval.h"

#include <array>

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace UnitTests
{		
	const int table_rows = 9;
	const int table_columns = 6;

	using cell_vector = std::vector<std::vector<cv::Mat>>;
	using result_array = std::array<std::array<bool, table_columns - 1>, table_rows - 1>;
	
	void assert_table(const cell_vector& tableCells, const result_array& results);
	
	TEST_CLASS(UnitTest1)
	{
	public:
		/*
		TEST_METHOD(TestMethodTest)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\test_answer_sheet_filled.jpg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);
			std::vector<result_array> results;

			// correct results for table 1
			result_array results1{ {
				{ true, false, false, false, false },
				{ false, true, false, false, false },
				{ false, true, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false }
			} };

			// correct results for table 2
			result_array results2{ {
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false }
			} };

			// correct results for table 3
			result_array results3{ {
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, true, false, false, false },
				{ false, false, false, false, true },
				{ true, false, false, false, false }
			} };
			
			results.push_back(results1);
			results.push_back(results2);
			results.push_back(results3);

			int i = 0;
			for (auto&& table : tables)
			{
				auto tableCells = extract_cells(table.image, table_rows, table_columns);
				assert_table(tableCells, results[i]);
				i++;
			}
		}
		*/
		TEST_METHOD(TestBigCrosses)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\01-varying_size.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);
			
			// correct results for table 2 (big crosses)
			result_array results{ {
				{ true, false, false, false, false },
				{ false, true, false, false, false },
				{ false, true, false, false, false },
				{ false, false, false, false, true },
				{ false, false, true, false, false },
				{ true, false, false, false, false },
				{ false, true, false, false, false },
				{ false, false, false, false, true }
			} };

			auto tableCells = extract_cells(tables[1].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestMediumCrosses)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\01-varying_size.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 1 (medium crosses)
			result_array results{ {
				{ false, false, false, true, false },
				{ false, false, true, false, false },
				{ false, false, false, true, false },
				{ false, true, false, false, false },
				{ true, false, false, false, false },
				{ false, true, false, false, false },
				{ false, true, false, false, false },
				{ false, false, false, false, true }
				} };

			auto tableCells = extract_cells(tables[0].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestSmallCrosses)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\01-varying_size.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 3 (small crosses)
			result_array results{ {
				{ false, true, false, false, false },
				{ false, false, false, false, true },
				{ false, false, false, true, false },
				{ false, false, true, false, false },
				{ false, false, true, false, false },
				{ true, false, false, false, false },
				{ true, false, false, false, false },
				{ false, true, false, false, false }
				} };

			auto tableCells = extract_cells(tables[2].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestThickCorrectionsSmallCrosses)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\02-corrections.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 1
			result_array results{ {
				{ false, false, false, false, true },
				{ false, false, true, false, false },
				{ false, false, false, true, false },
				{ true, false, false, false, false },
				{ false, false, false, false, true },
				{ false, false, false, true, false },
				{ false, false, false, true, false },
				{ false, true, false, false, false }
				} };

			auto tableCells = extract_cells(tables[0].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestThickCorrectionsSmallCrosses2)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\02-corrections.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 3
			result_array results{ {
				{ false, true, false, false, false },
				{ false, false, true, false, false },
				{ false, false, true, false, false },
				{ false, false, false, true, false },
				{ false, false, false, false, true },
				{ false, true, false, false, false },
				{ true, false, false, false, false },
				{ false, true, false, false, false }
				} };

			auto tableCells = extract_cells(tables[2].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestThinCorrectionsBigCrosses)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\02-corrections.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 2
			result_array results{ {
				{ false, true, false, false, false },
				{ false, true, false, false, false },
				{ false, false, true, false, false },
				{ false, false, false, false, true },
				{ false, false, true, false, false },
				{ true, false, false, false, false },
				{ false, true, false, false, false },
				{ false, false, false, true, false }
				} };

			auto tableCells = extract_cells(tables[1].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestFullColumnCrosses)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\03-full_column.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 1
			result_array results{ {
				{ false, true, false, false, false },
				{ false, true, false, false, false },
				{ false, true, false, false, false },
				{ false, true, false, false, false },
				{ false, true, false, false, false },
				{ false, true, false, false, false },
				{ false, true, false, false, false },
				{ false, true, false, false, false }
				} };

			auto tableCells = extract_cells(tables[0].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestFullColumnCorrectionsMiddle)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\03-full_column.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 2
			result_array results{ {
				{ false, false, false, false, true },
				{ false, true, false, false, false },
				{ true, false, false, false, false },
				{ false, true, false, false, false },
				{ false, false, false, true, false },
				{ false, true, false, false, false },
				{ false, false, false, true, false },
				{ true, false, false, false, false }
				} };

			auto tableCells = extract_cells(tables[1].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestFullColumnCorrectionsRight)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\03-full_column.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 3
			result_array results{ {
				{ false, true, false, false, false },
				{ false, false, true, false, false },
				{ false, false, true, false, false },
				{ false, false, false, true, false },
				{ false, true, false, false, false },
				{ true, false, false, false, false },
				{ true, false, false, false, false },
				{ false, false, true, false, false }
				} };

			auto tableCells = extract_cells(tables[2].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestColoursGreyOdd)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\04-colours.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 1
			result_array results{ {
				{ false, false, false, false, true },
				{ false, false, false, true, false },
				{ false, false, false, false, true },
				{ false, false, false, true, false },
				{ false, false, true, false, false },
				{ true, false, false, false, false },
				{ false, false, false, false, true },
				{ false, false, true, false, false }
				} };

			auto tableCells = extract_cells(tables[0].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestColoursRed)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\04-colours.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 2
			result_array results{ {
				{ false, false, false, false, true },
				{ false, true, false, false, false },
				{ false, false, false, true, false },
				{ false, true, false, false, false },
				{ false, false, true, false, false },
				{ false, false, true, false, false },
				{ true, false, false, false, false },
				{ false, true, false, false, false }
				} };

			auto tableCells = extract_cells(tables[1].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestColoursBlue)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\04-colours.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 3
			result_array results{ {
				{ false, true, false, false, false },
				{ false, false, true, false, false },
				{ false, false, false, true, false },
				{ false, false, false, false, true },
				{ false, false, false, true, false },
				{ false, false, true, false, false },
				{ false, true, false, false, false },
				{ true, false, false, false, false }
				} };

			auto tableCells = extract_cells(tables[2].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestVaryingThicknessMedium)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\05-varying_thickness.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 1
			result_array results{ {
				{ false, true, false, false, false },
				{ false, false, true, false, false },
				{ false, false, false, false, true },
				{ false, true, false, false, false },
				{ false, false, false, false, true },
				{ true, false, false, false, false },
				{ false, false, true, false, false },
				{ false, false, true, false, false }
				} };

			auto tableCells = extract_cells(tables[0].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestVaryingThicknessBold)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\05-varying_thickness.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 2
			result_array results{ {
				{ false, false, false, true, false },
				{ false, false, false, false, true },
				{ true, false, false, false, false },
				{ false, false, true, false, false },
				{ false, false, false, true, false },
				{ false, true, false, false, false },
				{ false, true, false, false, false },
				{ false, false, false, false, true }
				} };

			auto tableCells = extract_cells(tables[1].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestVaryingThicknessLight)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\05-varying_thickness.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 3
			result_array results{ {
				{ true, false, false, false, false },
				{ false, true, false, false, false },
				{ false, true, false, false, false },
				{ false, false, false, true, false },
				{ false, false, true, false, false },
				{ false, false, true, false, false },
				{ true, false, false, false, false },
				{ false, false, false, false, true }
				} };

			auto tableCells = extract_cells(tables[2].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestNoCrosses1)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\06-no_crosses.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 1
			result_array results{ {
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false }
				} };

			auto tableCells = extract_cells(tables[0].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestNoCrosses2)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\06-no_crosses.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 2
			result_array results{ {
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false }
				} };

			auto tableCells = extract_cells(tables[1].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestNoCrosses3)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\06-no_crosses.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 3
			result_array results{ {
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false },
				{ false, false, false, false, false }
				} };

			auto tableCells = extract_cells(tables[2].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestSloppy1)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\07-sloppy.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 1
			result_array results{ {
				{ false, true, false, false, false },
				{ false, false, true, false, false },
				{ true, false, false, false, false },
				{ false, false, false, true, false },
				{ false, false, false, false, false },
				{ false, false, true, false, false },
				{ true, false, false, false, false },
				{ false, true, false, false, false }
				} };

			auto tableCells = extract_cells(tables[0].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestSloppy2)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\07-sloppy.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 2
			result_array results{ {
				{ false, false, false, false, true },
				{ false, true, false, false, false },
				{ false, false, false, true, false },
				{ false, false, true, false, false },
				{ true, false, false, false, false },
				{ true, false, false, false, false },
				{ false, false, false, false, true },
				{ false, false, true, false, false }
				} };

			auto tableCells = extract_cells(tables[1].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestSloppy3)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\07-sloppy.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 3
			result_array results{ {
				{ true, false, false, false, false },
				{ false, false, true, false, false },
				{ false, false, false, true, false },
				{ false, false, false, true, false },
				{ false, true, false, false, false },
				{ false, false, false, false, true },
				{ false, false, false, true, false },
				{ true, false, false, false, false }
				} };

			auto tableCells = extract_cells(tables[2].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestRotatedThickCorrectionsSmallCrosses)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\08-corrections_rotated.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 1
			result_array results{ {
				{ false, false, false, false, true },
				{ false, false, true, false, false },
				{ false, false, false, true, false },
				{ true, false, false, false, false },
				{ false, false, false, false, true },
				{ false, false, false, true, false },
				{ false, false, false, true, false },
				{ false, true, false, false, false }
				} };

			auto tableCells = extract_cells(tables[0].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestRotatedThickCorrectionsSmallCrosses2)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\08-corrections_rotated.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 3
			result_array results{ {
				{ false, true, false, false, false },
				{ false, false, true, false, false },
				{ false, false, true, false, false },
				{ false, false, false, true, false },
				{ false, false, false, false, true },
				{ false, true, false, false, false },
				{ true, false, false, false, false },
				{ false, true, false, false, false }
				} };

			auto tableCells = extract_cells(tables[2].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestRotatedThinCorrectionsBigCrosses)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\08-corrections_rotated.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 2
			result_array results{ {
				{ false, true, false, false, false },
				{ false, true, false, false, false },
				{ false, false, true, false, false },
				{ false, false, false, false, true },
				{ false, false, true, false, false },
				{ true, false, false, false, false },
				{ false, true, false, false, false },
				{ false, false, false, true, false }
				} };

			auto tableCells = extract_cells(tables[1].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestFullColumnCrossesRotated)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\09-full_column_rotated.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 1
			result_array results{ {
				{ false, true, false, false, false },
				{ false, true, false, false, false },
				{ false, true, false, false, false },
				{ false, true, false, false, false },
				{ false, true, false, false, false },
				{ false, true, false, false, false },
				{ false, true, false, false, false },
				{ false, true, false, false, false }
				} };

			auto tableCells = extract_cells(tables[0].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestFullColumnCorrectionsMiddleRotated)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\09-full_column_rotated.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 2
			result_array results{ {
				{ false, false, false, false, true },
				{ false, true, false, false, false },
				{ true, false, false, false, false },
				{ false, true, false, false, false },
				{ false, false, false, true, false },
				{ false, true, false, false, false },
				{ false, false, false, true, false },
				{ true, false, false, false, false }
				} };

			auto tableCells = extract_cells(tables[1].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}

		TEST_METHOD(TestFullColumnCorrectionsRightRotated)
		{
			cv::Mat sheetImage = cv::imread("D:\\Projects\\OpenCV\\Klokan\\klokan\\UnitTests\\09-full_column_rotated.jpeg", CV_LOAD_IMAGE_GRAYSCALE);
			Assert::AreEqual(sheetImage.empty(), false, L"Image not loaded.");

			std::vector<Table> tables = extract_tables(sheetImage, 3);

			// correct results for table 3
			result_array results{ {
				{ false, true, false, false, false },
				{ false, false, true, false, false },
				{ false, false, true, false, false },
				{ false, false, false, true, false },
				{ false, true, false, false, false },
				{ true, false, false, false, false },
				{ true, false, false, false, false },
				{ false, false, true, false, false }
				} };

			auto tableCells = extract_cells(tables[2].image, table_rows, table_columns);
			assert_table(tableCells, results);
		}
	};

	void assert_table(const cell_vector& tableCells, const result_array& results)
	{
		for (size_t row = 1; row < table_rows; row++)
		{
			for (size_t column = 1; column < table_columns; column++)
			{
				auto&& cell = tableCells[row][column];
				Assert::AreEqual(results[row - 1][column - 1], is_cell_crossed(cell));
			}
		}
	}
}