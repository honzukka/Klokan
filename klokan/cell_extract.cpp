#include "cell_extract.h"

std::vector<std::vector<cv::Mat>> extract_cells(cv::Mat tableImage, int numberOfRows, int numberOfColumns)
{
	std::vector<std::vector<cv::Mat>> tableCells;
	
	int cellWidth = tableImage.cols / numberOfColumns;
	int cellHeight = tableImage.rows / numberOfRows;

	for (int row = 0; row < numberOfRows; row++)
	{
		// create a new row
		tableCells.push_back(std::vector<cv::Mat>());
		
		for (int column = 0; column < numberOfColumns; column++)
		{
			// extract the cell
			cv::Mat cell = tableImage(cv::Rect(column * cellWidth, row * cellHeight, cellWidth, cellHeight));
			
			// add it into a new column
			tableCells[row].push_back(cell);
		}
	}

	return tableCells;
}