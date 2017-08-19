using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.IO;

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
		/// Creates and starts a task for each sheet in the batch.
		/// Results of those tasks are awaited and later outputted. 
		/// The evaluation form is notified that the process was completed.
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
				foreach (var sheetFilename in categoryBatch.SheetFilenames)
				{
					Task<Result> sheetTask = new Task<Result>(() => evaluator.Evaluate(sheetFilename, categoryBatch.CorrectAnswers));
					tasks.Add(sheetTask);
					sheetTask.Start();
				}
			}

			Result[] results = await Task.WhenAll(tasks);

			OutputResultsTest(results);
			form.ShowMessageBoxInfo("Finished in " + (DateTime.Now - startTime).TotalSeconds + " seconds.\r\n", "Evaluation Completed");
			form.EnableGoButton();
		}

		/// <summary>
		/// A test function which outputs the result into an output file.
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
	}
}
