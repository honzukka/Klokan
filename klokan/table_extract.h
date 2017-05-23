#ifndef TABLE_EXTRACT_
#define TABLE_EXTRACT_

#include "opencv2\core\core.hpp"
#include "opencv2\highgui\highgui.hpp"
#include "opencv2\imgproc\imgproc.hpp"

#include "parameters.h"

#include <vector>

#define DLLExport __declspec(dllexport)

struct DLLExport Table
{
	cv::Mat image;		// table image
	cv::Point origin;	// where the upper left corner of the table is in the resized sheet
};

class DLLExport TableComparer
{
public:
	TableComparer() {}
	bool operator() (const Table& table1, const Table& table2);		// compares the x-coordinate
};

// finds the requested number of tables in the sheet, fixes their perspective and sorts them by the x-coordinate
// modifies the sheet image!
DLLExport std::vector<Table> extract_tables(cv::Mat& sheetImage, const Parameters& parameters);

#endif // !TABLE_EXTRACT_
