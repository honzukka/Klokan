using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace klokantest
{
	class Native
	{
		[DllImport("klokan.dll")]
		public static extern void hello_world();

		[DllImport("klokan.dll")]
		public static extern void string_test(string text);

		[DllImport("klokan.dll", CharSet = CharSet.Unicode)]
		public static extern void string_test_wide(string text);

		[DllImport("klokan.dll")]
		public static extern void structure_test(MyAmazingStructure structure);

		[DllImport("klokan.dll")]
		public unsafe static extern void array_test(int* array);

		[DllImport("klokan.dll")]
		public unsafe static extern void extract_answers_test(string filename, Parameters parameters, bool* answers);
	}

	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	struct MyAmazingStructure
	{
		public int number1;
		public float numberFloat;
		public int number3;
		public int number4;
	}

	// unsafe!!!
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	unsafe struct ArrayWrapper
	{
		public fixed int arr[2 * 3 * 2];
	}

	// unsafe!!!
	unsafe struct AnswerWrapper
	{
		public fixed bool answers[3 * 8 * 5];
	}

	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	struct Parameters
	{
		public int DefaultSheetWidth;
		public int BlackWhiteThreshold;

		public int TableLineLength;
		public float TableLineEccentricityLimit;
		public int TableLineCurvatureLimit;

		public int DefaultCellWidth;
		public int DefaultCellHeight;
		public int CrossLineLength;
		public int CrossLineCurvatureLimit;
		public int RubbishLinesLimit;
	}

	class Program
	{
		static void Main(string[] args)
		{
			Native.hello_world();
			Native.string_test("Tak to jsem teda zvedavej!");
			Native.string_test_wide("Tak to jsem teda zvedavej!");

			MyAmazingStructure structure = new MyAmazingStructure { number1 = 2, numberFloat = 567.98f, number3 = 2000000000, number4 = 0 };
			Native.structure_test(structure);

			// unsafe!!!
			ArrayWrapper array = new ArrayWrapper();
			unsafe
			{
				int* arrayPtr = array.arr;
				Native.array_test(arrayPtr);

				for (int table = 0; table < 2; table++)
				{
					Console.WriteLine("Table " + table + ":");

					for (int row = 0; row < 3; row++)
					{
						for (int col = 0; col < 2; col++)
						{
							Console.Write(arrayPtr[table * 3 * 2 + row * 2 + col] + " ");
						}

						Console.WriteLine();
					}
				}
			}

			Console.WriteLine();

			// ------------------------------------
			// HERE COMES THE ACTUAL KANGAROO TEST!
			// ------------------------------------

			Console.WriteLine("------------------------------------");
			Console.WriteLine("HERE COMES THE ACTUAL KANGAROO TEST!");
			Console.WriteLine("------------------------------------");

			Parameters parameters = new Parameters
			{
				DefaultSheetWidth = 1700,
				BlackWhiteThreshold = 230,
				TableLineLength = 350,
				TableLineEccentricityLimit = (float)(Math.PI / 8),
				TableLineCurvatureLimit = 1,
				DefaultCellWidth = 80,
				DefaultCellHeight = 40,
				CrossLineLength = 35,
				CrossLineCurvatureLimit = 5,
				RubbishLinesLimit = 10
			};

			string filename = "01-varying_size.jpeg";

			AnswerWrapper answerWrapper = new AnswerWrapper();
			unsafe
			{
				bool* answersPtr = answerWrapper.answers;
				Native.extract_answers_test(filename, parameters, answersPtr);

				for (int table = 0; table < 3; table++)
				{
					Console.WriteLine("Table " + (table + 1) + ":");

					for (int row = 0; row < 8; row++)
					{
						for (int col = 0; col < 5; col++)
						{
							if (answersPtr[table * 8 * 5 + row * 5 + col] == true)
							{
								Console.Write("X ");
							}
							else
							{
								Console.Write("  ");
							}
						}

						Console.WriteLine();
					}
				}
			}
		}
	}
}
