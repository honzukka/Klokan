#include "parameters.h"

Parameters::Parameters()
{
	default_sheet_width = 1700;
	black_white_threshold = 230;

	table_line_length = 350;
	table_line_eccentricity_limit = CV_PI / 8;
	table_line_curvature_limit = 1;

	default_cell_width = 80;
	default_cell_height = 40;
	cross_line_length = 35;
	cross_line_curvature_limit = 5;
	rubbish_lines_limit = 10;
}

bool Parameters::update_from_file(const std::string& filename)
{
	std::ifstream inputFile;
	inputFile.open(filename);

	// check for errors
	if (!inputFile.is_open()) return false;

	std::string line;

	// read the config file
	while (std::getline(inputFile, line))
	{
		// extract the parameter name and value
		size_t assignmentPosition = line.find('=');
		std::string paramName = line.substr(0, assignmentPosition);
		std::string paramValue = line.substr(assignmentPosition + 1);

		// try to assign the value
		try
		{
			if (paramName == "default_sheet_width")
				default_sheet_width = std::stoi(paramValue);
			else if (paramName == "black_white_threshold")
				black_white_threshold = std::stoi(paramValue);
			else if (paramName == "table_line_length")
				table_line_length = std::stoi(paramValue);
			else if (paramName == "table_line_eccentricity_limit")
				table_line_eccentricity_limit = std::stof(paramValue);
			else if (paramName == "table_line_curvature_limit")
				table_line_curvature_limit = std::stoi(paramValue);
			else if (paramName == "default_cell_width")
				default_cell_width = std::stoi(paramValue);
			else if (paramName == "default_cell_height")
				default_cell_height = std::stoi(paramValue);
			else if (paramName == "cross_line_length")
				cross_line_length = std::stoi(paramValue);
			else if (paramName == "cross_line_curvature_limit")
				cross_line_curvature_limit = std::stoi(paramValue);
			else if (paramName == "rubbish_lines_limit")
				rubbish_lines_limit = std::stoi(paramValue);
			else
			{
				inputFile.close();
				std::cerr << "Incorrect parameter (" << paramName << ") name." << std::endl;
				return false;
			}
		}
		catch (const std::invalid_argument& ex)
		{
			inputFile.close();
			std::cerr << "Incorrect parameter (" << paramName << ") value format." << std::endl;
			return false;
		}
		catch (const std::out_of_range& ex)
		{
			inputFile.close();
			std::cerr << "Parameter (" << paramName << ") value out of range." << std::endl;
			return false;
		}
	}

	// close the file
	inputFile.close();
	if (inputFile.fail())
		return false;
	else
		return true;
}