#include "table_extract.h"
#include "cell_extract.h"
#include "cell_eval.h"
#include "debug.h"
#include "klokan.h"
#include "parameters.h"
#include "api.h"

#include <iostream>
#include <sstream>

using namespace std;
using namespace cv;

int main(int argc, char** argv)
{
	/*
	// get names of the sheets from the command line
	// the first argument is the name of the correct sheet
	// the rest of the arguments are names of the sheets to be processed
	if (argc < 3)
	{
		cerr << "Too few arguments!" << endl;
		return 0;
	}
	string correctSheetName(argv[1]);
	vector<string> sheetNames(argv + 2, argv + argc);

	//ENTER THE NAMES OF THE SHEETS MANUALLY
	//string correctSheetName = "01-varying_size.jpeg";
	//vector<string> sheetNames;

	//sheetNames.push_back("01-varying_size.jpeg");
	//sheetNames.push_back("01-varying_size-one_wrong.jpeg");
	//sheetNames.push_back("01-varying_size-one_empty.jpeg");
	//sheetNames.push_back("09-full_column_rotated.jpeg");
	
	Parameters parameters;
	if (!parameters.update_from_file("config.txt")) 
		cerr << "There was an error loading the config file. Using default parameters where necessary." << endl;

	Klokan klokan(parameters);
	
	klokan.run(correctSheetName, sheetNames);

	cerr << "Ready!" << endl;

	waitKey(0);
	*/
	/*
	Parameters params;
	bool answersArray[3 * 8 * 5];
	char* filename = "01-varying_size.jpeg";
	bool success = false;
	bool* successPtr = &success;

	extract_answers_api(filename, params, answersArray, successPtr);

	for (int table = 0; table < 3; table++)
	{
		cout << "Table " << (table + 1) << ":" << endl;

		for (int row = 0; row < 8; row++)
		{
			for (int col = 0; col < 5; col++)
			{
				if (answersArray[table * 8 * 5 + row * 5 + col] == true)
				{
					cout << "X ";
				}
				else
				{
					cout << "  ";
				}
			}

			cout << endl;
		}
	}
	*/
	Parameters params;

	// load the answer sheet
	cv::Mat sheetImage = cv::imread("sheet1.jpeg", CV_LOAD_IMAGE_GRAYSCALE);

	// extract tables ordered by x-coordinate
	std::vector<Table> tables = extract_tables(sheetImage, params);

	int i = 0;
	for (auto&& table : tables)
	{
		debug::show_image(table.image, "table" + i);
		i++;
	}

	waitKey(0);
	return 0;
}