#ifndef TABLE_EXTRACT_
#define TABLE_EXTRACT_

#include "opencv2\core\core.hpp"
#include "opencv2\highgui\highgui.hpp"
#include "opencv2\imgproc\imgproc.hpp"

#include <vector>

#define DLLExport __declspec(dllexport)

struct DLLExport Table
{
	cv::Mat image;
	cv::Point origin;
};

class DLLExport TableComparer
{
public:
	TableComparer() {}
	bool operator() (const Table& table1, const Table& table2);
};

DLLExport std::vector<Table> extract_tables(cv::Mat image, int numberOfTables);

#endif // !TABLE_EXTRACT_
