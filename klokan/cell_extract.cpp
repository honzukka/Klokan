#include "cell_extract.h"
#include "parameters.h"

std::vector<std::vector<cv::Mat>> extract_cells(const cv::Mat& tableImage)
{
	std::vector<std::vector<cv::Mat>> tableCells;
	
	float cellWidth = (float)tableImage.cols / (float)TABLE_COLUMNS;
	float cellHeight = (float)tableImage.rows / (float)TABLE_ROWS;

	for (int row = 0; row < TABLE_ROWS; row++)
	{
		// create a new row
		tableCells.push_back(std::vector<cv::Mat>());
		
		for (int column = 0; column < TABLE_COLUMNS; column++)
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