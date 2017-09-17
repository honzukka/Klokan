using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace KlokanUI
{
	class JobScheduler
	{
		/// <summary>
		/// Data structure containing algorithm parameters, correct answers and sheets to be processed for multiple categories.
		/// </summary>
		KlokanBatch batch;

		/// <summary>
		/// A reference to the evaluation form, so that new events can be added to its event loop.
		/// </summary>
		EvaluationForm form;

		// just for information
		DateTime evaluationStartTime;
		DateTime evaluationEndTime;

		public JobScheduler(KlokanBatch batch, EvaluationForm form)
		{
			this.batch = batch;
			this.form = form;
		}

		/// <summary>
		/// This method takes the batch saved in the JobScheduler and runs an asynchronous method
		/// which plans and then starts the processing.
		/// </summary>
		public void Run()
		{
			ProcessBatchAsync();
		}

		/// <summary>
		/// Creates and starts a task for each sheet in the batch.
		/// Results of those tasks are awaited and later outputted. 
		/// The evaluation form is notified that the process was completed.
		/// </summary>
		async void ProcessBatchAsync()
		{
			evaluationStartTime = DateTime.Now;

			Evaluator evaluator = new Evaluator(batch.Parameters);
			List<Task<Result>> tasks = new List<Task<Result>>();

			foreach (var categoryBatch in batch.CategoryBatches.Values)
			{
				foreach (var sheetFilename in categoryBatch.SheetFilenames)
				{
					Task<Result> sheetTask = new Task<Result>(() => evaluator.Evaluate(sheetFilename, categoryBatch.CorrectAnswers, categoryBatch.CategoryName, batch.Year));
					tasks.Add(sheetTask);
					sheetTask.Start();
				}
			}

			Result[] results = await Task.WhenAll(tasks);
			evaluationEndTime = DateTime.Now;

			await OutputResultsDB(results);

			FinishJob();
		}

		/// <summary>
		/// A test method which outputs results into an output file.
		/// </summary>
		/// <param name="results">Any enumerable structure of evaluation results.</param>
		void OutputResultsTest(IEnumerable<Result> results)
		{
			using (var sw = new StreamWriter("output-test.txt"))
			{
				foreach (var result in results)
				{
					if (result.Error)
					{
						sw.WriteLine("ERROR");
						sw.WriteLine("--------------");
						continue;
					}

					for (int table = 0; table < batch.Parameters.TableCount; table++)
					{
						sw.WriteLine("Table " + (table + 1) + ":");

						for (int row = 0; row < batch.Parameters.TableRows - 1; row++)
						{
							for (int col = 0; col < batch.Parameters.TableColumns - 1; col++)
							{
								switch (result.CorrectedAnswers[table, row, col])
								{
									case AnswerType.Correct:
										sw.Write("X ");
										break;
									case AnswerType.Incorrect:
										sw.Write("! ");
										break;
									case AnswerType.Void:
										sw.Write("  ");
										break;
									case AnswerType.Corrected:
										sw.Write("O ");
										break;
								}
							}

							sw.WriteLine();
						}

						sw.WriteLine();
					}

					sw.WriteLine("Score: " + result.Score);
					sw.WriteLine("--------------");
				}
			}
		}

		/// <summary>
		/// Asynchronously stores results into a database described by KlokanDBContext.
		/// </summary>
		/// <param name="results">Any enumerable structure of evaluation results.</param>
		/// <returns>A void task.</returns>
		async Task OutputResultsDB(IEnumerable<Result> results)
		{
			using (var db = new KlokanDBContext())
			{
				foreach (var result in results)
				{
					var answerSheet = new AnswerSheet {
						Year = result.Year,
						Category = result.Category,
						Points = result.Score,
						Answers = GetAnswers(result.CorrectedAnswers),
						Scan = GetImageBytes(result.SheetFilename, ImageFormat.Png)
					};

					db.AnswerSheets.Add(answerSheet);
				}

				await db.SaveChangesAsync();
			}
		}

		/// <summary>
		/// Transform the "table view" of the answers (multidimensional array) into a list of objects 
		/// which correspond to a database record described in the Answer class.
		/// </summary>
		/// <param name="correctedAnswers">Answers as stored in the Result structure.</param>
		List<Answer> GetAnswers(AnswerType[,,] correctedAnswers)
		{
			List<Answer> answers = new List<Answer>();

			for (int table = 0; table < batch.Parameters.TableCount; table++)
			{
				// the first row and the first column of the original table were removed as they do not contain any answers
				for (int row = 0; row < batch.Parameters.TableRows - 1; row++)
				{
					char enteredValue = '\0';
					char correctValue = '\0';
					
					// find out the entered and correct value (entered value can stay '\0' in case the question wasn't answered)
					// the first row and the first column of the original table were removed as they do not contain any answers
					for (int col = 0; col < batch.Parameters.TableColumns - 1; col++)
					{
						switch (correctedAnswers[table, row, col])
						{
							case AnswerType.Correct:
								enteredValue = (char)('a' + col);
								correctValue = enteredValue;
								break;
							case AnswerType.Incorrect:
								enteredValue = (char)('a' + col);
								break;
							case AnswerType.Corrected:
								correctValue = (char)('a' + col);
								break;
							default:
								break;
						}
					}

					answers.Add(new Answer
					{
						QuestionNumber = (row + 1) + (table * (batch.Parameters.TableRows - 1)),
						EnteredValue = new String(enteredValue, 1),
						CorrectValue = new string(correctValue, 1)
					});
				}
			}

			return answers;
		}

		/// <summary>
		/// Transforms an image into an array of bytes in a specified format.
		/// </summary>
		/// <param name="imageFilename">Path to the image that will be transformed.</param>
		/// <param name="imageFormat">The format in which the image will be stored into the array.</param>
		byte[] GetImageBytes(string imageFilename, ImageFormat imageFormat)
		{
			using (var memoryStream = new MemoryStream())
			using (var sheetImage = Image.FromFile(imageFilename))
			{
				sheetImage.Save(memoryStream, imageFormat);
				return memoryStream.ToArray();
			}
		}

		/// <summary>
		/// Notifies the evaluation form that the job has ended.
		/// Additional information about the job's completion is displayed in a message box.
		/// </summary>
		void FinishJob()
		{
			form.ShowMessageBoxInfo("Evaluation finished in " + (evaluationEndTime - evaluationStartTime).TotalSeconds + " seconds.\r\n" +
				"Results saved in " + (DateTime.Now - evaluationEndTime).TotalSeconds + " seconds.", "Evaluation Completed"
			);
			form.EnableGoButton();
		}
	}
}
