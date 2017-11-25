using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace KlokanUI
{
	class JobScheduler
	{
		KlokanBatch batch;
		TestKlokanBatch testBatch;
		
		/// <summary>
		/// A reference to the evaluation form, so that new events can be added to its event loop.
		/// </summary>
		IEvaluationForm callingForm;

		// just for information
		DateTime evaluationStartTime;
		DateTime evaluationEndTime;

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
		/// This method takes the batch saved in the JobScheduler and runs an asynchronous method
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

		async void ProcessTestBatchAsync()
		{
			evaluationStartTime = DateTime.Now;

			Evaluator evaluator = new Evaluator(testBatch.Parameters);
			List<Task<TestResult>> tasks = new List<Task<TestResult>>();

			foreach (var testInstance in testBatch.TestInstances)
			{
				Task<TestResult> instanceTask = new Task<TestResult>(() => evaluator.EvaluateTest(testInstance.ScanId, testInstance.Image, testInstance.StudentExpectedValues, testInstance.AnswerExpectedValues));
				tasks.Add(instanceTask);
				instanceTask.Start();
			}

			TestResult[] testResults = await Task.WhenAll(tasks);
			evaluationEndTime = DateTime.Now;

			// output results
			await OutputTestResultsDB(testResults);

			FinishJob();
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
					// TODO: let the user know that it failed
					if (result.Error == true)
					{
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
						currentInstance = new KlokanDBInstance
						{
							Year = result.Year,
							Category = result.Category,
							CorrectAnswers = GetCorrectAnswers(result.CorrectedAnswers)
						};

						db.Instances.Add(currentInstance);
					}

					var answerSheet = new KlokanDBAnswerSheet
					{
						StudentNumber = result.StudentNumber,
						Points = result.Score,
						ChosenAnswers = GetChosenAnswers(result.CorrectedAnswers),
						Scan = HelperFunctions.GetImageBytes(result.SheetFilename, ImageFormat.Png)
					};

					currentInstance.AnswerSheets.Add(answerSheet);
				}

				await db.SaveChangesAsync();
			}
		}

		async Task OutputTestResultsDB(IEnumerable<TestResult> testResults)
		{
			using (var testDB = new KlokanTestDBContext())
			{
				foreach (var testResult in testResults)
				{
					if (testResult.Error == true)
					{
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
					computedValuesDbSet.AddRange(HelperFunctions.AnswersToDbSet<KlokanTestDBComputedAnswer>(testResult.StudentComputedValues, 0, true));
					for (int i = 0; i < 3; i++)
					{
						computedValuesDbSet.AddRange(HelperFunctions.AnswersToDbSet<KlokanTestDBComputedAnswer>(testResult.AnswerComputedValues, i, false));
					}

					correspondingScan.ComputedValues = computedValuesDbSet;
					correspondingScan.Correctness = testResult.Correctness;

					await testDB.SaveChangesAsync();
				}
			}
		}

		/// <summary>
		/// Transform the "table view" of the answers (multidimensional array) into a list of objects 
		/// which correspond to a database record described in the Answer class.
		/// </summary>
		/// <param name="correctedAnswers">Answers as stored in the Result structure.</param>
		List<KlokanDBChosenAnswer> GetChosenAnswers(AnswerType[,,] correctedAnswers)
		{
			List<KlokanDBChosenAnswer> chosenAnswers = new List<KlokanDBChosenAnswer>();

			for (int table = 0; table < batch.Parameters.TableCount - 1; table++)
			{
				// the first row and the first column of the original table were removed as they do not contain any answers
				for (int row = 0; row < batch.Parameters.AnswerTableRows - 1; row++)
				{
					char enteredValue = '\0';
					
					// find out the entered value (entered value can stay '\0' in case the question wasn't answered)
					// the first row and the first column of the original table were removed as they do not contain any answers
					for (int col = 0; col < batch.Parameters.AnswerTableColumns - 1; col++)
					{
						int numberOfSelectedAnswers = 0;

						if (correctedAnswers[table, row, col] == AnswerType.Correct ||
							correctedAnswers[table, row, col] == AnswerType.Incorrect)
						{
							enteredValue = (char)('a' + col);
							numberOfSelectedAnswers++;
						}

						// keep in mind that more answers can be selected
						if (numberOfSelectedAnswers > 1)
						{
							enteredValue = 'x';
						}
					}

					chosenAnswers.Add(new KlokanDBChosenAnswer
					{
						QuestionNumber = (row + 1) + (table * (batch.Parameters.AnswerTableRows - 1)),
						Value = new String(enteredValue, 1)
					});
				}
			}

			return chosenAnswers;
		}

		/// <summary>
		/// Transform the "table view" of the correct answers (multidimensional array) into a list of objects 
		/// which correspond to a database record described in the CorrectAnswer class.
		/// </summary>
		/// <param name="correctedAnswers">Answers as stored in the Result structure.</param>
		List<KlokanDBCorrectAnswer> GetCorrectAnswers(AnswerType[,,] correctedAnswers)
		{
			List<KlokanDBCorrectAnswer> correctAnswers = new List<KlokanDBCorrectAnswer>();

			for (int table = 0; table < batch.Parameters.TableCount - 1; table++)
			{
				// the first row and the first column of the original table were removed as they do not contain any answers
				for (int row = 0; row < batch.Parameters.AnswerTableRows - 1; row++)
				{
					char correctValue = '\0';

					// find out the entered and correct value (entered value can stay '\0' in case the question wasn't answered)
					// the first row and the first column of the original table were removed as they do not contain any answers
					for (int col = 0; col < batch.Parameters.AnswerTableColumns - 1; col++)
					{
						if (correctedAnswers[table, row, col] == AnswerType.Correct ||
							correctedAnswers[table, row, col] == AnswerType.Corrected)
						{
							correctValue = (char)('a' + col);
							break;		// only one answer can be selected as correct thanks to the category edit form
						}
					}

					correctAnswers.Add(new KlokanDBCorrectAnswer
					{
						QuestionNumber = (row + 1) + (table * (batch.Parameters.AnswerTableRows - 1)),
						Value = new string(correctValue, 1)
					});
				}
			}

			return correctAnswers;
		}

		/// <summary>
		/// Notifies the evaluation form that the job has ended.
		/// Additional information about the job's completion is displayed in a message box.
		/// </summary>
		void FinishJob()
		{
			callingForm.ShowMessageBoxInfo("Evaluation finished in " + (evaluationEndTime - evaluationStartTime).TotalSeconds + " seconds.\r\n" +
				"Results saved in " + (DateTime.Now - evaluationEndTime).TotalSeconds + " seconds.", "Evaluation Completed"
			);
			callingForm.EnableGoButton();
		}
	}
}
