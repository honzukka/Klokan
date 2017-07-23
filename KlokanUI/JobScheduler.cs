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
		Form1 form;

		public JobScheduler(KlokanBatch batch, Form1 form)
		{
			this.batch = batch;
			this.form = form;
		}

		public void Run()
		{
			ProcessBatchAsync(batch, form);

			/* SYNCHRONOUS
			DateTime startTime = DateTime.Now;

			Evaluator evaluator = new Evaluator(batch.Parameters);

			foreach (var categoryBatch in batch.CategoryBatches)
			{
				var correctAnswers = evaluator.LoadCorrectAnswers(categoryBatch.CorrectSheetFilename);

				if (correctAnswers == null)
				{
					form.AddTextToTextbox("ERROR\r\n--------------\r\n");
					continue;
				}

				foreach (var sheetFilename in categoryBatch.SheetFilenames)
				{
					Result result = evaluator.Evaluate(sheetFilename, correctAnswers);

					if (result.Error)
					{
						form.AddTextToTextbox("ERROR\r\n--------------\r\n");
						continue;
					}	
				}
			}

			form.AddTextToTextbox("Finished in " + (DateTime.Now - startTime).TotalSeconds + " seconds.\r\n");
			form.EnableGoButton();
			*/
		}

		async void ProcessBatchAsync(KlokanBatch batch, Form1 form)
		{
			DateTime startTime = DateTime.Now;

			Evaluator evaluator = new Evaluator(batch.Parameters);
			List<Task<Result>> tasks = new List<Task<Result>>();

			foreach (var categoryBatch in batch.CategoryBatches)
			{
				// this is still synchronous...
				var correctAnswers = evaluator.LoadCorrectAnswers(categoryBatch.CorrectSheetFilename);

				if (correctAnswers == null)
				{
					form.AddTextToTextbox("ERROR\r\n--------------\r\n");
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
			form.AddTextToTextbox("Finished in " + (DateTime.Now - startTime).TotalSeconds + " seconds.\r\n");
			form.EnableGoButton();
		}

		void OutputResultsTest(IEnumerable<Result> results)
		{
			foreach (var result in results)
			{
				if (result.Error)
				{
					form.AddTextToTextbox("ERROR\r\n--------------\r\n");
					continue;
				}

				for (int table = 0; table < batch.Parameters.TableCount; table++)
				{
					form.AddTextToTextbox("Table " + (table + 1) + ":\r\n");

					for (int row = 0; row < batch.Parameters.TableRows - 1; row++)
					{
						for (int col = 0; col < batch.Parameters.TableColumns - 1; col++)
						{
							switch (result.CorrectedAnswers[table][row][col])
							{
								case AnswerType.Correct:
									form.AddTextToTextbox("X\t");
									break;
								case AnswerType.Incorrect:
									form.AddTextToTextbox("!\t");
									break;
								case AnswerType.Void:
									form.AddTextToTextbox("\t");
									break;
								case AnswerType.Corrected:
									form.AddTextToTextbox("O\t");
									break;
							}
						}

						form.AddTextToTextbox("\r\n");
					}

					form.AddTextToTextbox("\r\n");
				}

				form.AddTextToTextbox("Score: " + result.Score + "\r\n--------------\r\n");
			}
		}
	}
}
