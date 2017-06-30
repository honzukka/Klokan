#include <iostream>
#include <string>
#include <array>

using namespace std;

using testTableAnswers = array<array<bool, 2>, 3>;
using testSheetAnswers = array<testTableAnswers, 2>;

#pragma pack(push, 8)
struct MyStructure
{
	int number1;
	float number2;
	int number3;
	int number4;
};
#pragma pack(pop)

extern "C" __declspec(dllexport) void __stdcall hello_world()
{
	cout << "Hello from C++!!!" << endl;
	cout << endl;
}

// conversion happens on the .NET side (during marshalling)
extern "C" __declspec(dllexport) void __stdcall string_test(char* text)
{
	string textString(text);

	cout << "This was sent to C++: " << textString << endl;
	cout << endl;
}

// conversion happens right here
extern "C" __declspec(dllexport) void __stdcall string_test_wide(wchar_t* text)
{
	wstring textWString(text);
	string textString(textWString.begin(), textWString.end());

	cout << "This was sent to C++: " << textString << endl;
	cout << endl;
}

extern "C" __declspec(dllexport) void __stdcall structure_test(MyStructure structure)
{
	cout << "This was sent to C++:" << endl;
	cout << "Number 1: " << structure.number1 << endl;
	cout << "Number 2: " << structure.number2 << endl;
	cout << "Number 3: " << structure.number3 << endl;
	cout << "Number 4: " << structure.number4 << endl;
	cout << endl;
}

// assumming I get a buffer of int[2*3*2]...
extern "C" _declspec(dllexport) void __stdcall array_test(int* answers)
{
	const int rows = 3;
	const int cols = 2;
	
	// first table
	for (size_t row = 0; row < rows; row++)
	{
		for (size_t col = 0; col < 2; col++)
		{
			answers[0 * rows * cols + row * cols + cols] = row + col;
		}
	}

	// second table
	for (size_t row = 0; row < cols; row++)
	{
		for (size_t col = 0; col < 2; col++)
		{
			answers[1 * rows * cols + row * cols + cols] = row + col + 5;
		}
	}
}