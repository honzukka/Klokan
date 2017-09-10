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
		/// <exception cref="InvalidOperationException">Thrown when the correct answers haven't been loaded prior to the execution of this function.</exception>
		/// <returns></returns>
		public Result Evaluate(string sheetFilename, bool[,,] correctAnswers, string category)
		{
			if (correctAnswers == null)
			{
				throw new InvalidOperationException("Correct answers haven't been loaded!");
			}

			// get the answers from the sheet
			bool[,,] answers = ExtractAnswers(sheetFilename);

			if (answers == null)
			{
				return new Result(true);
			}

			// correct them
			AnswerType[,,] correctedAnswers = CorrectAnswers(answers, correctAnswers);

			// count the score
			int score = CountScore(correctedAnswers);

			return new Result(category, correctedAnswers, score, sheetFilename,  false);
		}

		/// <summary>
		/// Uses a native library to load an image and extract answers from it.
		/// This method is unsafe.
		/// </summary>
		/// <param name="sheetFilename">The path to the image containing answers to be extracted.</param>
		/// <returns>Answers from an answers sheet or null if there was an error processing the sheet image in the native library.</returns>
		bool[,,] ExtractAnswers(string sheetFilename)
		{
			bool[,,] extractedAnswers = new bool[3, 8, 5];

			// the first row and the first column of the original table were removed as they do not contain any answers
			int answerRows = parameters.TableRows - 1;
			int answerColumns = parameters.TableColumns - 1;

			AnswerWrapper answerWrapper = new AnswerWrapper();
			bool success = false;

			unsafe
			{
				bool* answersPtr = answerWrapper.answers;
				bool* successPtr = &success;

				// call into the native library
				NativeAPIWrapper.extract_answers_api(sheetFilename, parameters, answersPtr, successPtr);

				if (!success)
				{
					return null;
				}

				// convert the answers from a C-style array to a C# multi-dimensional array
				for (int table = 0; table < parameters.TableCount; table++)
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

			return extractedAnswers;
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

			for (int table = 0; table < parameters.TableCount; table++)
			{
				// the first row and the first column of the original table were removed as they do not contain any answers
				for (int row = 0; row < parameters.TableRows - 1; row++)
				{
					for (int col = 0; col < parameters.TableColumns - 1; col++)
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

			for (int table = 0; table < parameters.TableCount; table++)
			{
				// the first row and the first column of the original table were removed as they do not contain any answers
				for (int row = 0; row < parameters.TableRows - 1; row++)
				{
					int correctAnswersCount = 0;
					int incorrectAnswersCount = 0;

					// count the types of answers necessary to assign points
					for (int col = 0; col < parameters.TableColumns - 1; col++)
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
