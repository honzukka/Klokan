#include "cell_extract.h"

std::vector<std::vector<cv::Mat>> extract_cells(const cv::Mat& tableImage, int numberOfRows, int numberOfColumns)
{
	std::vector<std::vector<cv::Mat>> tableCells;
	
	float cellWidth = (float)tableImage.cols / (float)numberOfColumns;
	float cellHeight = (float)tableImage.rows / (float)numberOfRows;

	for (int row = 0; row < numberOfRows; row++)
	{
		// create a new row
		tableCells.push_back(std::vector<cv::Mat>());
		
		for (int column = 0; column < numberOfColumns; column++)
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