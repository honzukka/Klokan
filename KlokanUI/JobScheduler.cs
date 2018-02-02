using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Drawing.Imaging;
using System.Data.Entity.Infrastructure;

namespace KlokanUI
{
	class JobScheduler
	{
		#region Fields

		/// <summary>
		/// Batch data for evaluation.
		/// </summary>
		KlokanBatch batch;

		/// <summary>
		/// Batch data for test evaluation.
		/// </summary>
		TestKlokanBatch testBatch;

		/// <summary>
		/// A reference to a progress dialog which informs about the job and also contains a cancel button.
		/// </summary>
		ProgressDialog progressDialog;

		/// <summary>
		/// Used for measuring the time of evaluation.
		/// </summary>
		DateTime evaluationStartTime;
		
		/// <summary>
		/// Used for measuring the time of evaluation.
		/// </summary>
		DateTime evaluationEndTime;

		/// <summary>
		/// Used to create a summary of the evaluation.
		/// </summary>
		int failedSheets;

		#endregion

		public JobScheduler(KlokanBatch batch, ProgressDialog progressDialog)
		{
			this.batch = batch;
			testBatch = null;
			this.progressDialog = progressDialog;
		}

		public JobScheduler(TestKlokanBatch testBatch, ProgressDialog progressDialog)
		{
			batch = null;
			this.testBatch = testBatch;
			this.progressDialog = progressDialog;
		}

		/// <summary>
		/// This method takes the batch (either normal or test) saved in the JobScheduler and runs an asynchronous method
		/// which plans and then starts the processing.
		/// </summary>
		public void Run()
		{
			if (batch != null)
			{
				ProcessBatchAsync();
			}

			if (testBatch != null)
			{
				ProcessTestBatchAsync();
			}
		}

		/// <summary>
		/// Creates and starts a task for each sheet in the batch.
		/// Results of those tasks are awaited and later outputted. 
		/// The progress dialog monitors the process and is able to cancel it too.
		/// </summary>
		async void ProcessBatchAsync()
		{
			evaluationStartTime = DateTime.Now;

			// initialize the progress dialog
			progressDialog.SetTotalTasks(GetNumberOfSheetsInBatch());

			Evaluator evaluator = new Evaluator(batch.Parameters);
			List<Task<Result>> tasks = new List<Task<Result>>();

			foreach (var categoryBatch in batch.CategoryBatches.Values)
			{
				foreach (var sheetFilename in categoryBatch.SheetFilenames)
				{
					Task<Result> sheetTask = new Task<Result>(
						() => {
							var result = evaluator.Evaluate(sheetFilename, categoryBatch.CorrectAnswers, categoryBatch.CategoryName, batch.Year);
							progressDialog.IncrementProgressBarValue();
							return result;
						}, 
						progressDialog.GetCancellationToken()
					);
					tasks.Add(sheetTask);
					sheetTask.Start();
				}
			}

			try
			{
				Result[] results = await Task.WhenAll(tasks);

				evaluationEndTime = DateTime.Now;
				progressDialog.SetProgressLabel(ProgressBarState.Saving);

				await OutputResultsDB(results);

				FinishJob(false);
			}
			catch (Exception ex) when (ex is TaskCanceledException || ex is OperationCanceledException ||
										ex is DbUpdateException || ex is DbUpdateConcurrencyException)
			{
				FinishJob(true);
			}
		}

		/// <summary>
		/// Creates and starts a task for each sheet in the test batch.
		/// Results of those tasks are awaited and later outputted. 
		/// The progress dialog monitors the process and is able to cancel it too.
		/// </summary>
		async void ProcessTestBatchAsync()
		{
			evaluationStartTime = DateTime.Now;

			// initialize the progress dialog
			progressDialog.SetTotalTasks(testBatch.TestInstances.Count);

			Evaluator evaluator = new Evaluator(testBatch.Parameters);
			List<Task<TestResult>> tasks = new List<Task<TestResult>>();

			foreach (var testInstance in testBatch.TestInstances)
			{
				Task<TestResult> instanceTask = new Task<TestResult>(
					() => {
						var result = evaluator.EvaluateTest(testInstance.ScanId, testInstance.Image, testInstance.StudentExpectedValues, testInstance.AnswerExpectedValues);
						progressDialog.IncrementProgressBarValue();
						return result;
					},
					progressDialog.GetCancellationToken()
				);
				tasks.Add(instanceTask);
				instanceTask.Start();
			}

			try
			{
				TestResult[] testResults = await Task.WhenAll(tasks);

				evaluationEndTime = DateTime.Now;
				progressDialog.SetProgressLabel(ProgressBarState.SavingTest);

				await OutputTestResultsDB(testResults);

				FinishJob(false);
			}
			catch (Exception ex) when (ex is TaskCanceledException || ex is OperationCanceledException ||
										ex is DbUpdateException || ex is DbUpdateConcurrencyException)
			{
				FinishJob(true);
			}
		}

		/// <summary>
		/// Asynchronously stores results into a database described by KlokanDBContext.
		/// Instances that already exist in the database are rewritten.
		/// </summary>
		/// <param name="results">Any enumerable structure of evaluation results.</param>
		/// <returns>A void task.</returns>
		async Task OutputResultsDB(IEnumerable<Result> results)
		{
			using (var db = new KlokanDBContext())
			{
				foreach (var result in results)
				{
					if (result.Error == true)
					{
						failedSheets++;
						continue;
					}
					
					// find out if the instance this result belongs to is new or if it already exists
					var query = from instance
								in db.Instances
								where instance.Year == result.Year && instance.Category == result.Category
								select instance;

					KlokanDBInstance currentInstance = query.FirstOrDefault();

					// if the instance isn't saved in the database
					if (currentInstance == default(KlokanDBInstance))
					{
						// try to search locally too (maybe the instance was added in the previous loop cycle)
						var querylocal = from instance
										 in db.Instances.Local
										 where instance.Year == result.Year && instance.Category == result.Category
										 select instance;

						currentInstance = querylocal.FirstOrDefault();
					}
					else
					{
						// remove it completely because the new one will rewrite it
						// lazy loading is used, so we need to load all the relations of an instance if we want Remove() to remove those as well
						var blah = currentInstance.AnswerSheets;
						var blah2 = currentInstance.CorrectAnswers;
						List<ICollection<KlokanDBChosenAnswer>> blah3 = new List<ICollection<KlokanDBChosenAnswer>>();
						foreach (var answerSheetBlah in blah)
						{
							blah3.Add(answerSheetBlah.ChosenAnswers);
						}

						db.Instances.Remove(currentInstance);
						await db.SaveChangesAsync(progressDialog.GetCancellationToken());
						currentInstance = null;
					}

					// if the instance doesn't exist locally either
					if (currentInstance == default(KlokanDBInstance))
					{
						List<KlokanDBCorrectAnswer> correctAnswers = new List<KlokanDBCorrectAnswer>();
						for (int i = 0; i < 3; i++)
						{
							correctAnswers.AddRange(TableArrayHandling.AnswersToDbSet<KlokanDBCorrectAnswer>(result.CorrectAnswers, i, false));
						}

						currentInstance = new KlokanDBInstance
						{
							Year = result.Year,
							Category = result.Category,
							CorrectAnswers = correctAnswers
						};

						db.Instances.Add(currentInstance);
					}

					List<KlokanDBChosenAnswer> chosenAnswers = new List<KlokanDBChosenAnswer>();
					for (int i = 0; i < 3; i++)
					{
						chosenAnswers.AddRange(TableArrayHandling.AnswersToDbSet<KlokanDBChosenAnswer>(result.ChosenAnswers, i, false));
					}

					var answerSheet = new KlokanDBAnswerSheet
					{
						StudentNumber = result.StudentNumber,
						Points = result.Score,
						ChosenAnswers = chosenAnswers,
						Scan = ImageHandling.GetImageBytes(result.SheetFilename, ImageFormat.Png)
					};

					currentInstance.AnswerSheets.Add(answerSheet);
				}

				await db.SaveChangesAsync(progressDialog.GetCancellationToken());
			}
		}

		/// <summary>
		/// Asynchronously stores test results into a database described by KlokanTestDBContext.
		/// Results that already exist in the database are rewritten.
		/// </summary>
		/// <param name="results">Any enumerable structure of evaluation test results.</param>
		/// <returns>A void task.</returns>
		async Task OutputTestResultsDB(IEnumerable<TestResult> testResults)
		{
			using (var testDB = new KlokanTestDBContext())
			{
				foreach (var testResult in testResults)
				{
					if (testResult.Error == true)
					{
						failedSheets++;
						continue;
					}

					var scanQuery = from scan in testDB.Scans
									where scan.ScanId == testResult.ScanId
									select scan;

					var oldComputedAnswersQuery = from answer in testDB.ComputedValues
												  where answer.ScanId == testResult.ScanId
												  select answer;

					// delete old computed values
					foreach (var answer in oldComputedAnswersQuery)
					{
						testDB.ComputedValues.Remove(answer);
					}

					// assign new computed values
					KlokanTestDBScan correspondingScan = scanQuery.FirstOrDefault();

					var computedValuesDbSet = new List<KlokanTestDBComputedAnswer>();
					computedValuesDbSet.AddRange(TableArrayHandling.AnswersToDbSet<KlokanTestDBComputedAnswer>(testResult.StudentComputedValues, 0, true));
					for (int i = 0; i < 3; i++)
					{
						computedValuesDbSet.AddRange(TableArrayHandling.AnswersToDbSet<KlokanTestDBComputedAnswer>(testResult.AnswerComputedValues, i, false));
					}

					correspondingScan.ComputedValues = computedValuesDbSet;
					correspondingScan.Correctness = testResult.Correctness;

					await testDB.SaveChangesAsync(progressDialog.GetCancellationToken());
				}
			}
		}

		/// <summary>
		/// Notifies the evaluation form that the job has ended.
		/// Additional information about the job's completion is displayed in a message box.
		/// </summary>
		void FinishJob(bool wasCancelled)
		{
			progressDialog.DisableCancelButton();

			if (wasCancelled)
			{
				progressDialog.SetProgressLabel(ProgressBarState.Cancelled);
				progressDialog.SetResultLabel();
			}
			else
			{
				progressDialog.SetProgressLabel(ProgressBarState.Done);
				progressDialog.SetResultLabel(failedSheets, (evaluationEndTime - evaluationStartTime).TotalSeconds, (DateTime.Now - evaluationEndTime).TotalSeconds);
			}

			progressDialog.EnableOkButton();
		}

		private int GetNumberOfSheetsInBatch()
		{
			int totalTasks = 0;
			foreach (var categoryBatch in batch.CategoryBatches.Values)
			{
				totalTasks += categoryBatch.SheetFilenames.Count;
			}

			return totalTasks;
		}
	}
}
