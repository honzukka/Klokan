#include "cell_extract.h"
#include "debug.h"
#include "parameters.h"

std::vector<std::vector<cv::Mat>> extract_cells(const cv::Mat& tableImage, int tableColumns, int tableRows)
{
	std::vector<std::vector<cv::Mat>> tableCells;
	
	float cellWidth = (float)tableImage.cols / (float)tableColumns;
	float cellHeight = (float)tableImage.rows / (float)tableRows;

	for (int row = 0; row < tableRows; row++)
	{
		// create a new row
		tableCells.push_back(std::vector<cv::Mat>());
		
		for (int column = 0; column < tableColumns; column++)
		{
			// extract the cell
			float x = column * cellWidth;
			float y = row * cellHeight;
			cv::Mat cell = tableImage(cv::Rect(x, y, cellWidth, cellHeight));
			
			// add it into a new column
			tableCells[row].push_back(cell);
		}
	}

	return tableCells;
}