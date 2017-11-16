#include "table_extract.h"
#include "cell_extract.h"
#include "cell_eval.h"
#include "debug.h"
#include "parameters.h"
#include "api.h"

#include <iostream>
#include <sstream>

using namespace std;
using namespace cv;

int main(int argc, char** argv)
{
	Parameters params;
	bool numberArray[5 * 10];
	bool answersArray[3 * 8 * 5];
	char* filename = "sheet1.jpeg";
	bool success = false;
	bool* successPtr = &success;

	extract_answers_path_api(filename, params, numberArray, answersArray, successPtr);

	cout << "Number:" << endl;

	for (int row = 0; row < 5; row++)
	{
		for (int col = 0; col < 10; col++)
		{
			if (numberArray[row * 10 + col] == true)
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

	waitKey(0);
	return 0;
}