using System;

using System.Drawing;
using System.Drawing.Imaging;

namespace KlokanUI
{
	class Evaluator
	{
		/// <summary>
		/// Parameters to be used in this evaluation instance.
		/// </summary>
		Parameters parameters;

		public Evaluator(Parameters parameters)
		{
			this.parameters = parameters;
		}

		/// <summary>
		/// Evaluates answers contained in an image of an answer sheet against a set of correct answers.
		/// </summary>
		/// <param name="sheetFilename">The path to the image containing answers to be evaluated.</param>
		/// <param name="correctAnswers">Correct answers to evaluate against.</param>
		/// <param name="category">The category this sheet belongs to.</param>
		/// <param name="year">The year this sheet belongs to.</param>
		/// <exception cref="InvalidOperationException">Thrown when the correct answers haven't been loaded prior to the execution of this function.</exception>
		public Result Evaluate(string sheetFilename, bool[,,] correctAnswers, string category, int year)
		{
			if (correctAnswers == null)
			{
				throw new InvalidOperationException("Correct answers haven't been loaded!");
			}

			// get the answers from the sheet
			bool[,,] studentNumberAnswers;
			bool[,,] answers;
			if (ExtractAnswers(sheetFilename, out studentNumberAnswers, out answers) == false)
			{
				return new Result(true);
			}

			// get the student number
			int studentNumber = StudentTableToNumber(studentNumberAnswers);

			// count the score
			int score = TableArrayHandling.CountScore(answers, correctAnswers);

			return new Result(year, category, studentNumber, answers, correctAnswers, score, sheetFilename, false);
		}

		/// <summary>
		/// Evaluates the similarity between answers in an answer sheet (a scan) and a set of expected answers.
		/// </summary>
		/// <param name="scanId">Id of the scan entry in the database.</param>
		/// <param name="sheetImage">Scan image bytes.</param>
		/// <param name="studentExpectedValues">A set of expected answers (values) for the student number table.</param>
		/// <param name="answerExpectedValues">A set of expected answers (values) for the answer tables.</param>
		/// <exception cref="InvalidOperationException">Thrown when the expected value arrays are not set.</exception>
		public TestResult EvaluateTest(int scanId, byte[] sheetImage, bool[,,] studentExpectedValues, bool[,,] answerExpectedValues)
		{
			if (studentExpectedValues == null || answerExpectedValues == null)
			{
				throw new InvalidOperationException("Expected answers haven't been loaded!");
			}

			// get the answers from the sheet
			bool[,,] studentComputedValues;
			bool[,,] answerComputedValues;
			if (ExtractAnswers(sheetImage, out studentComputedValues, out answerComputedValues) == false)
			{
				return new TestResult(true);
			}

			// compute the correctness
			float correctness = ComputeCorrectness(studentExpectedValues, studentComputedValues, answerExpectedValues, answerComputedValues);

			return new TestResult(scanId, studentComputedValues, studentExpectedValues, answerComputedValues, answerExpectedValues, correctness, false);
		}
		
		/// <summary>
		/// Uses a native library to load an image and extract answers from it.
		/// Works with both image bytes as well as just an image file path.
		/// This method is unsafe.
		/// </summary>
		/// <param name="sheet">The path to the image containing answers to be extracted as a string or an array of image bytes.</param>
		/// <param name="studentNumberAnswers">This parameter will contain the student number answers extracted from the student number table.</param>
		/// <param name="extractedAnswers">This parameter will contain the answers extracted from the answer tables.</param>
		/// <returns>True if the process has succeeded and false otherwise.</returns>
		bool ExtractAnswers<T>(T sheet, out bool[,,] studentNumberAnswers, out bool[,,] extractedAnswers)
		{
			extractedAnswers = new bool[3, 8, 5];
			studentNumberAnswers = new bool[1, 5, 10];

			// the first row and the first column of the original tables were removed as they do not contain any answers
			int studentNumberRows = parameters.StudentTableRows - 1;
			int studentNumberColumns = parameters.StudentTableColumns - 1;
			int answerRows = parameters.AnswerTableRows - 1;
			int answerColumns = parameters.AnswerTableColumns - 1;

			NumberWrapper numberWrapper = new NumberWrapper();
			AnswerWrapper answerWrapper = new AnswerWrapper();
			bool success = false;

			unsafe
			{
				bool* numberPtr = numberWrapper.number;
				bool* answersPtr = answerWrapper.answers;
				bool* successPtr = &success;

				string sheetFilename;
				byte[] sheetImageBytes;

				// if the sheet has been passed as a filename
				if ((sheetFilename = sheet as string) != null)
				{
					NativeAPIWrapper.extract_answers_path_api(sheetFilename, parameters, numberPtr, answersPtr, successPtr);
				}
				// if the sheet has been passed as image bytes
				else if ((sheetImageBytes = sheet as byte[]) != null)
				{
					// the image is saved in PNG (or another format) so we need to convert it to BMP which the library can read
					Image sheetImage = ImageHandling.GetBitmap(sheetImageBytes);
					int imageRows = sheetImage.Height;
					int imageCols = sheetImage.Width;

					byte[] sheetImageBitmapBytes = ImageHandling.GetImageBytes(sheetImage, ImageFormat.Bmp);
					sheetImage.Dispose();

					fixed (byte* imageBitmapBytesPtr = sheetImageBitmapBytes)
					{
						NativeAPIWrapper.extract_answers_image_api(imageBitmapBytesPtr, imageRows, imageCols, parameters, numberPtr, answersPtr, successPtr);
					}
				}
				else
				{
					return false;
				}

				if (!success)
				{
					return false;
				}

				// convert the student number values from a C-style array of answers to a C# multi-dimensional array
				for (int row = 0; row < studentNumberRows; row++)
				{
					for (int col = 0; col < studentNumberColumns; col++)
					{
						if (numberPtr[row * studentNumberColumns + col] == true)
						{
							studentNumberAnswers[0, row, col] = true;
						}
						else
						{
							studentNumberAnswers[0, row, col] = false;
						}
					}
				}

				// convert the answer table values from a C-style array to a C# multi-dimensional array
				for (int table = 0; table < parameters.TableCount - 1; table++)
				{
					for (int row = 0; row < answerRows; row++)
					{
						for (int col = 0; col < answerColumns; col++)
						{
							if (answersPtr[table * answerRows * answerColumns + row * answerColumns + col] == true)
							{
								extractedAnswers[table, row, col] = true;
							}
							else
							{
								extractedAnswers[table, row, col] = false;
							}
						}
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Converts the answers in the student number table into an actual number.
		/// </summary>
		/// <returns>Returns -1 if the conversion failed.</returns>
		private int StudentTableToNumber(bool[,,] studentTableValues)
		{
			int tableRows = studentTableValues.GetUpperBound(1) + 1;
			int tableColumns = studentTableValues.GetUpperBound(2) + 1;

			int studentNumber = 0;

			for (int row = 0; row < tableRows; row++)
			{
				bool numberInRow = false;

				for (int col = 0; col < tableColumns; col++)
				{
					if (studentTableValues[0, row, col] == true)
					{
						// if a row contains two numbers (answers)
						if (numberInRow == true)
						{
							return -1;
						}

						numberInRow = true;
						studentNumber *= 10;
						studentNumber += col;
					}
				}

				// if a row doesn't contain a number (an answer)
				if (numberInRow == false)
				{
					return -1;
				}
			}

			return studentNumber;
		}

		/// <summary>
		/// Computes a ratio of correctly identified answers (values) among all answers from an answer sheet.
		/// </summary>
		private float ComputeCorrectness(bool[,,] studentExpectedValues, bool[,,] studentComputedValues, bool[,,] answerExpectedValues, bool[,,] answerComputedValues)
		{
			int studentTableRows = studentExpectedValues.GetUpperBound(1) + 1;
			int studentTableColumns = studentExpectedValues.GetUpperBound(2) + 1;

			int answerTables = answerExpectedValues.GetUpperBound(0) + 1;
			int answerTableRows = answerExpectedValues.GetUpperBound(1) + 1;
			int answerTableColumns = answerExpectedValues.GetUpperBound(2) + 1;

			int correctValues = 0;
			int totalValues = studentTableRows + (answerTableRows * 3);

			// check the student table values
			for (int row = 0; row < studentTableRows; row++)
			{
				bool valueCorrect = true;

				for (int col = 0; col < studentTableColumns; col++)
				{
					if (studentExpectedValues[0, row, col] != studentComputedValues[0, row, col])
					{
						valueCorrect = false;
						break;
					}
				}

				if (valueCorrect)
				{
					correctValues++;
				}
			}

			// check the answer table values
			for (int table = 0; table < answerTables; table++)
			{
				for (int row = 0; row < answerTableRows; row++)
				{
					bool valueCorrect = true;

					for (int col = 0; col < answerTableColumns; col++)
					{
						if (answerExpectedValues[table, row, col] != answerComputedValues[table, row, col])
						{
							valueCorrect = false;
							break;
						}
					}

					if (valueCorrect)
					{
						correctValues++;
					}
				}
			}

			return (float)correctValues / (float)totalValues;
		}
	}
}
