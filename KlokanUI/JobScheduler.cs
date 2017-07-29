using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

namespace KlokanUI
{
	class JobScheduler
	{
		KlokanBatch batch;
		EvaluationForm form;

		public JobScheduler(KlokanBatch batch, EvaluationForm form)
		{
			this.batch = batch;
			this.form = form;
		}

		/// <summary>
		/// This method takes the batch saved in the JobScheduler and passes it to an asynchronous method
		/// which plans and then starts the processing.
		/// </summary>
		public void Run()
		{
			ProcessBatchAsync(batch, form);
		}

		/// <summary>
		/// Synchronously loads correct answers and then creates and starts a task for each sheet in the batch.
		/// Results of those tasks are awaited and later outputted. The evaluation form is notified that the process was completed.
		/// </summary>
		/// <param name="batch">Data structure containing algorithm parameters, correct answers and sheets to be processed for multiple categories.</param>
		/// <param name="form">A reference to the evaluation form, so that new events can be added to its event loop.</param>
		async void ProcessBatchAsync(KlokanBatch batch, EvaluationForm form)
		{
			DateTime startTime = DateTime.Now;

			Evaluator evaluator = new Evaluator(batch.Parameters);
			List<Task<Result>> tasks = new List<Task<Result>>();

			foreach (var categoryBatch in batch.CategoryBatches.Values)
			{
				// this is still synchronous...
				var correctAnswers = evaluator.LoadCorrectAnswers(categoryBatch.CorrectSheetFilename);

				if (correctAnswers == null)
				{
					//form.AddTextToTextbox("ERROR\r\n--------------\r\n");
					continue;
				}

				foreach (var sheetFilename in categoryBatch.SheetFilenames)
				{
					Task<Result> sheetTask = new Task<Result>(() => evaluator.Evaluate(sheetFilename, correctAnswers));
					tasks.Add(sheetTask);
					sheetTask.Start();
				}
			}

			Result[] results = await Task.WhenAll(tasks);

			//OutputResultsTest(results);
			//form.AddTextToTextbox("Finished in " + (DateTime.Now - startTime).TotalSeconds + " seconds.\r\n");
			form.EnableGoButton();
		}

		/// <summary>
		/// A test function which outputs the result into the evaluation form. (slow and blocks the event loop)
		/// </summary>
		/// <param name="results">Any enumerable structure of evaluation results.</param>
		void OutputResultsTest(IEnumerable<Result> results)
		{
			foreach (var result in results)
			{
				if (result.Error)
				{
					//form.AddTextToTextbox("ERROR\r\n--------------\r\n");
					continue;
				}

				for (int table = 0; table < batch.Parameters.TableCount; table++)
				{
					//form.AddTextToTextbox("Table " + (table + 1) + ":\r\n");

					for (int row = 0; row < batch.Parameters.TableRows - 1; row++)
					{
						for (int col = 0; col < batch.Parameters.TableColumns - 1; col++)
						{
							switch (result.CorrectedAnswers[table][row][col])
							{
								case AnswerType.Correct:
									//form.AddTextToTextbox("X\t");
									break;
								case AnswerType.Incorrect:
									//form.AddTextToTextbox("!\t");
									break;
								case AnswerType.Void:
									//form.AddTextToTextbox("\t");
									break;
								case AnswerType.Corrected:
									//form.AddTextToTextbox("O\t");
									break;
							}
						}

						//form.AddTextToTextbox("\r\n");
					}

					//form.AddTextToTextbox("\r\n");
				}

				//form.AddTextToTextbox("Score: " + result.Score + "\r\n--------------\r\n");
			}
		}
	}
}
