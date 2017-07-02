#include "cell_extract.h"
#include "parameters.h"

std::vector<std::vector<cv::Mat>> extract_cells(const cv::Mat& tableImage, const Parameters& parameters)
{
	std::vector<std::vector<cv::Mat>> tableCells;
	
	float cellWidth = (float)tableImage.cols / (float)parameters.table_columns;
	float cellHeight = (float)tableImage.rows / (float)parameters.table_rows;

	for (int row = 0; row < parameters.table_rows; row++)
	{
		// create a new row
		tableCells.push_back(std::vector<cv::Mat>());
		
		for (int column = 0; column < parameters.table_columns; column++)
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