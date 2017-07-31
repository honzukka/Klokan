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
		/// Loads the correct answers to evaluate against.
		/// </summary>
		/// <param name="correctSheetFilename">Image name containing the correct answers.</param>
		/// <returns>A multi-dimensional list of correct answers or null if there was an error loading the answers.</returns>
		public List<List<List<bool>>> LoadCorrectAnswers(string correctSheetFilename)
		{
			return ExtractAnswers(correctSheetFilename);
		}
		
		/// <summary>
		/// Evaluates answers contained in an image of an answer sheet.
		/// </summary>
		/// <param name="sheetFilename">The name of the image containing answers to be evaluated.</param>
		/// <exception cref="InvalidOperationException">Thrown when the correct answers haven't been loaded prior to the execution of this function.</exception>
		/// <returns></returns>
		public Result Evaluate(string sheetFilename, List<List<List<bool>>> correctAnswers)
		{
			if (correctAnswers == null)
			{
				throw new InvalidOperationException("Correct answers haven't been loaded!");
			}

			// get the answers from the sheet
			List<List<List<bool>>> answers = ExtractAnswers(sheetFilename);

			if (answers == null)
			{
				return new Result(true);
			}

			// correct them
			List<List<List<AnswerType>>> correctedAnswers = CorrectAnswers(answers, correctAnswers);

			// count the score
			int score = CountScore(correctedAnswers);

			return new Result(correctedAnswers, score, false);
		}

		/// <summary>
		/// Uses a native library to load an image and extract answers from it.
		/// This method is unsafe.
		/// </summary>
		/// <param name="sheetFilename">The name of the image containing answers to be extracted.</param>
		/// <returns>Answers from an answers sheet or null if there was an error processing the sheet image in the native library.</returns>
		List<List<List<bool>>> ExtractAnswers(string sheetFilename)
		{
			List<List<List<bool>>> extractedAnswers = new List<List<List<bool>>>();

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

				// convert the answers from a C-style array to a multi-dimensional list
				for (int table = 0; table < parameters.TableCount; table++)
				{
					// add a new table
					extractedAnswers.Add(new List<List<bool>>());

					for (int row = 0; row < answerRows; row++)
					{
						// add a new row
						extractedAnswers[table].Add(new List<bool>());

						for (int col = 0; col < answerColumns; col++)
						{
							if (answersPtr[table * answerRows * answerColumns + row * answerColumns + col] == true)
							{
								extractedAnswers[table][row].Add(true);
							}
							else
							{
								extractedAnswers[table][row].Add(false);
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
		/// <param name="answers">Answers to be checked against the set of correct answers stored in the object.</param>
		/// <returns>A multi-dimensional list of corrected answers (AnswerType).</returns>
		List<List<List<AnswerType>>> CorrectAnswers(List<List<List<bool>>> answers, List<List<List<bool>>> correctAnswers)
		{
			List<List<List<AnswerType>>> correctedAnswers = new List<List<List<AnswerType>>>();

			for (int table = 0; table < parameters.TableCount; table++)
			{
				// add a new table
				correctedAnswers.Add(new List<List<AnswerType>>());

				// the first row and the first column of the original table were removed as they do not contain any answers
				for (int row = 0; row < parameters.TableRows - 1; row++)
				{
					// add a new row
					correctedAnswers[table].Add(new List<AnswerType>());

					for (int col = 0; col < parameters.TableColumns - 1; col++)
					{
						// if the answer is no and correct
						if (answers[table][row][col] == false && correctAnswers[table][row][col] == false)
							correctedAnswers[table][row].Add(AnswerType.Void);

						// if the answer is yes and correct
						else if (answers[table][row][col] == true && correctAnswers[table][row][col] == true)
							correctedAnswers[table][row].Add(AnswerType.Correct);

						// if the answer is yes and incorrect
						else if (answers[table][row][col] == true && correctAnswers[table][row][col] == false)
							correctedAnswers[table][row].Add(AnswerType.Incorrect);

						// if the answer is no and incorrect
						else if (answers[table][row][col] == false && correctAnswers[table][row][col] == true)
							correctedAnswers[table][row].Add(AnswerType.Corrected);
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
		int CountScore(List<List<List<AnswerType>>> correctedAnswers)
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
						switch (correctedAnswers[table][row][col])
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
