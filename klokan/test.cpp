#include <iostream>

extern "C" __declspec(dllexport) void __stdcall hello_world()
{
	std::cout << "Hello from C++!!!" << std::endl;
}