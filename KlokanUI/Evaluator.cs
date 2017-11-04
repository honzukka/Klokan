using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlokanUI
{
	class Evaluator
	{
		Parameters parameters;

		public Evaluator(Parameters parameters)
		{
			this.parameters = parameters;
		}

		/// <summary>
		/// Evaluates answers contained in an image of an answer sheet.
		/// </summary>
		/// <param name="sheetFilename">The path to the image containing answers to be evaluated.</param>
		/// <param name="correctAnswers">Correct answers to evaluate against.</param>
		/// <param name="category">The category this sheet belongs to.</param>
		/// <param name="year">The year this sheet belongs to.</param>
		/// <exception cref="InvalidOperationException">Thrown when the correct answers haven't been loaded prior to the execution of this function.</exception>
		/// <returns></returns>
		public Result Evaluate(string sheetFilename, bool[,,] correctAnswers, string category, int year)
		{
			if (correctAnswers == null)
			{
				throw new InvalidOperationException("Correct answers haven't been loaded!");
			}

			// get the answers from the sheet
			int studentNumber;
			bool[,,] answers;
			if (ExtractAnswers(sheetFilename, out studentNumber, out answers) == false)
			{
				return new Result(true);
			}

			// correct them
			AnswerType[,,] correctedAnswers = CorrectAnswers(answers, correctAnswers);

			// count the score
			int score = CountScore(correctedAnswers);

			return new Result(year, category, studentNumber, correctedAnswers, score, sheetFilename,  false);
		}

		/// <summary>
		/// Uses a native library to load an image and extract answers from it.
		/// This method is unsafe.
		/// </summary>
		/// <param name="sheetFilename">The path to the image containing answers to be extracted.</param>
		/// <param name="studentNumber">This parameter will contain the student number extracted from the student number table.</param>
		/// <param name="extractedAnswers">This parameter will contain the answers extracted from the answer tables.</param>
		/// <returns>True if the process has succeeded and false otherwise.</returns>
		bool ExtractAnswers(string sheetFilename, out int studentNumber, out bool[,,] extractedAnswers)
		{
			extractedAnswers = new bool[3, 8, 5];
			studentNumber = 0;

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

				// call into the native library
				NativeAPIWrapper.extract_answers_api(sheetFilename, parameters, numberPtr, answersPtr, successPtr);

				if (!success)
				{
					return false;
				}

				// convert the number from a C-style array of answers to an actual number
				for (int row = 0; row < studentNumberRows; row++)
				{
					bool numberInRow = false;

					for (int col = 0; col < studentNumberColumns; col++)
					{
						if (numberPtr[row * studentNumberColumns + col] == true)
						{
							// if a row contains two numbers (answers)
							if (numberInRow == true)
							{
								return false;
							}

							numberInRow = true;
							studentNumber *= 10;
							studentNumber += col;
						}
					}

					// if a row doesn't contain a number (an answer)
					if (numberInRow == false)
					{
						return false;
					}
				}

				// convert the answers from a C-style array to a C# multi-dimensional array
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
		/// Checks the answers in the input against the set of correct answers stored in the object
		/// and outputs the corrections in the form of a multi-dimensional array of AnswerType.
		/// </summary>
		/// <param name="answers">Answers to be checked.</param>
		/// <param name="correctAnswers">The set of correct answers to check against.</param>
		/// <returns>A multi-dimensional array of corrected answers (AnswerType).</returns>
		AnswerType[,,] CorrectAnswers(bool[,,] answers, bool[,,] correctAnswers)
		{
			AnswerType[,,] correctedAnswers = new AnswerType[3,8,5];

			for (int table = 0; table < parameters.TableCount - 1; table++)
			{
				// the first row and the first column of the original table were removed as they do not contain any answers
				for (int row = 0; row < parameters.AnswerTableRows - 1; row++)
				{
					for (int col = 0; col < parameters.AnswerTableColumns - 1; col++)
					{
						// if the answer is no and correct
						if (answers[table, row, col] == false && correctAnswers[table, row, col] == false)
							correctedAnswers[table, row, col] = AnswerType.Void;

						// if the answer is yes and correct
						else if (answers[table, row, col] == true && correctAnswers[table, row, col] == true)
							correctedAnswers[table, row, col] = AnswerType.Correct;

						// if the answer is yes and incorrect
						else if (answers[table, row, col] == true && correctAnswers[table, row, col] == false)
							correctedAnswers[table, row, col] = AnswerType.Incorrect;

						// if the answer is no and incorrect
						else if (answers[table, row, col] == false && correctAnswers[table, row, col] == true)
							correctedAnswers[table, row, col] = AnswerType.Corrected;
					}
				}
			}

			return correctedAnswers;
		}

		/// <summary>
		/// Counts the score based on corrected answers according to the standard rules of the Mathematical Kangaroo
		/// </summary>
		/// <param name="correctedAnswers">Corrected answers to be rated.</param>
		/// <returns>The score.</returns>
		int CountScore(AnswerType[,,] correctedAnswers)
		{
			int score = 24;

			for (int table = 0; table < parameters.TableCount - 1; table++)
			{
				// the first row and the first column of the original table were removed as they do not contain any answers
				for (int row = 0; row < parameters.AnswerTableRows - 1; row++)
				{
					int correctAnswersCount = 0;
					int incorrectAnswersCount = 0;

					// count the types of answers necessary to assign points
					for (int col = 0; col < parameters.AnswerTableColumns - 1; col++)
					{
						switch (correctedAnswers[table, row, col])
						{
							case AnswerType.Correct: correctAnswersCount++; break;
							case AnswerType.Incorrect: incorrectAnswersCount++; break;
							default: break;
						}
					}

					// assign points for the question (row)
					// if it's correct
					// NOTE: here we test if only one question has been selected!!!
					if (correctAnswersCount == 1 && incorrectAnswersCount == 0)
					{
						switch (table)
						{
							case 0: score += 3; break;
							case 1: score += 4; break;
							case 2: score += 5; break;
						}
					}
					// if it's incorrect
					else if (incorrectAnswersCount > 0)
					{
						score--;
					}
					// otherwise the score doesn't change
				}
			}

			return score;
		}
	}
}
