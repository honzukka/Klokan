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
		public unsafe static extern void array_test(int* answers);
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
	unsafe struct AnswerWrapper
	{
		public fixed int answers[2 * 3 * 2];
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
			AnswerWrapper answers = new AnswerWrapper();
			unsafe
			{
				int* answerPtr = answers.answers;
				Native.array_test(answerPtr);

				for (int table = 0; table < 2; table++)
				{
					Console.WriteLine("Table " + table + ":");

					for (int row = 0; row < 3; row++)
					{
						for (int col = 0; col < 2; col++)
						{
							Console.Write(answerPtr[table * 3 * 2 + row * 2 + col] + " ");
						}

						Console.WriteLine();
					}
				}
			}
		}
	}
}
