using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Drawing.Imaging;

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
		/// A reference to the evaluation form, so that new events can be added to its event loop.
		/// </summary>
		IEvaluationForm callingForm;

		/// <summary>
		/// Used for measuring the time of evaluation.
		/// </summary>
		DateTime evaluationStartTime;
		
		/// <summary>
		/// Used for measuring the time of evaluation.
		/// </summary>
		DateTime evaluationEndTime;

		/// <summary>
		/// Information about the result of the evaluation.
		/// </summary>
		string evaluationSummary;

		#endregion

		public JobScheduler(KlokanBatch batch, IEvaluationForm callingForm)
		{
			this.batch = batch;
			testBatch = null;
			this.callingForm = callingForm;
		}

		public JobScheduler(TestKlokanBatch testBatch, IEvaluationForm callingForm)
		{
			batch = null;
			this.testBatch = testBatch;
			this.callingForm = callingForm;
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
		/// The calling form is notified that the process was completed.
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
					Task<Result> sheetTask = new Task<Result>(
						() => evaluator.Evaluate(sheetFilename, categoryBatch.CorrectAnswers, categoryBatch.CategoryName, batch.Year)
					);
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
		/// Creates and starts a task for each sheet in the test batch.
		/// Results of those tasks are awaited and later outputted. 
		/// The calling form is notified that the process was completed.
		/// </summary>
		async void ProcessTestBatchAsync()
		{
			evaluationStartTime = DateTime.Now;

			Evaluator evaluator = new Evaluator(testBatch.Parameters);
			List<Task<TestResult>> tasks = new List<Task<TestResult>>();

			foreach (var testInstance in testBatch.TestInstances)
			{
				Task<TestResult> instanceTask = new Task<TestResult>(
					() => evaluator.EvaluateTest(testInstance.ScanId, testInstance.Image, testInstance.StudentExpectedValues, testInstance.AnswerExpectedValues)
				);
				tasks.Add(instanceTask);
				instanceTask.Start();
			}

			TestResult[] testResults = await Task.WhenAll(tasks);

			evaluationEndTime = DateTime.Now;

			await OutputTestResultsDB(testResults);

			FinishJob();
		}

		/// <summary>
		/// Asynchronously stores results into a database described by KlokanDBContext.
		/// Instances that already exist in the database are rewritten.
		/// </summary>
		/// <param name="results">Any enumerable structure of evaluation results.</param>
		/// <returns>A void task.</returns>
		async Task OutputResultsDB(IEnumerable<Result> results)
		{
			int failedSheets = 0;

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
						await db.SaveChangesAsync();
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

				await db.SaveChangesAsync();
			}

			if (failedSheets == 0)
			{
				evaluationSummary = Properties.Resources.SummaryTextEvaluationSuccessful;
			}
			else if (failedSheets == 1)
			{
				evaluationSummary = failedSheets + " " + Properties.Resources.SummaryTextFailedSheetPart2;
			}
			else
			{
				evaluationSummary = failedSheets + " " + Properties.Resources.SummaryTextFailedSheetsPart2;
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
			int failedScans = 0;

			using (var testDB = new KlokanTestDBContext())
			{
				foreach (var testResult in testResults)
				{
					if (testResult.Error == true)
					{
						failedScans++;
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

					// TODO: reflection used in batch processing
					var computedValuesDbSet = new List<KlokanTestDBComputedAnswer>();
					computedValuesDbSet.AddRange(TableArrayHandling.AnswersToDbSet<KlokanTestDBComputedAnswer>(testResult.StudentComputedValues, 0, true));
					for (int i = 0; i < 3; i++)
					{
						computedValuesDbSet.AddRange(TableArrayHandling.AnswersToDbSet<KlokanTestDBComputedAnswer>(testResult.AnswerComputedValues, i, false));
					}

					correspondingScan.ComputedValues = computedValuesDbSet;
					correspondingScan.Correctness = testResult.Correctness;

					await testDB.SaveChangesAsync();
				}
			}

			if (failedScans == 0)
			{
				evaluationSummary = Properties.Resources.SummaryTextEvaluationSuccessful;
			}
			else if (failedScans == 1)
			{
				evaluationSummary = failedScans + " " + Properties.Resources.SummaryTextFailedSheetPart2;
			}
			else
			{
				evaluationSummary = failedScans + " " + Properties.Resources.SummaryTextFailedSheetsPart2;
			}
		}

		/// <summary>
		/// Notifies the evaluation form that the job has ended.
		/// Additional information about the job's completion is displayed in a message box.
		/// </summary>
		void FinishJob()
		{
			callingForm.ShowMessageBoxInfo(evaluationSummary + "\r\n\r\n" +
				Properties.Resources.SummaryTextEvaluationTimePart1 + " " + (evaluationEndTime - evaluationStartTime).TotalSeconds + " " + Properties.Resources.SummaryTextDatabaseTimePart3 + "\r\n" +
				Properties.Resources.SummaryTextDatabaseTimePart1 + " " + (DateTime.Now - evaluationEndTime).TotalSeconds + " " + Properties.Resources.SummaryTextDatabaseTimePart3,
				Properties.Resources.SummaryCaption
			);
			callingForm.EnableGoButton();
		}
	}
}
