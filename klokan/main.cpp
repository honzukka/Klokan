#include "table_extract.h"
#include "cell_extract.h"
#include "cell_eval.h"
#include "debug.h"
#include "klokan.h"
#include "parameters.h"

#include <iostream>
#include <sstream>

using namespace std;
using namespace cv;

int main(int argc, char** argv)
{
	// get names of the sheets from the command line
	// the first argument is the name of the correct sheet
	// the rest of the arguments are names of the sheets to be processed
	string correctSheetName(argv[1]);
	vector<string> sheetNames(argv + 2, argv + argc);

	/* ENTER THE NAMES OF THE SHEETS MANUALLY
	string correctSheetName = "01-varying_size.jpeg";
	vector<string> sheetNames;

	sheetNames.push_back("01-varying_size.jpeg");
	sheetNames.push_back("01-varying_size-one_wrong.jpeg");
	sheetNames.push_back("01-varying_size-one_empty.jpeg");
	sheetNames.push_back("09-full_column_rotated.jpeg");
	*/
	
	Parameters parameters;
	if (!parameters.update_from_file("config.txt")) 
		cerr << "There was an error loading the config file. Using default parameters where necessary." << endl;

	Klokan klokan(parameters);
	
	klokan.run(correctSheetName, sheetNames);

	cerr << "Ready!" << endl;

	waitKey(0);

	return 0;
}