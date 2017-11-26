using System.Collections.Generic;

namespace KlokanUI
{
	/// <summary>
	/// Batch data structure to be fed to the evaluator.
	/// </summary>
	class KlokanBatch
	{
		public Parameters Parameters { get; set; }
		public Dictionary<string, KlokanCategoryBatch> CategoryBatches { get; set; }
		public int Year { get; set; }
	}

	/// <summary>
	/// Batch data for one Klokan category.
	/// Consists of one correct answer sheet and multiple answer sheets to be evaluated.
	/// </summary>
	class KlokanCategoryBatch
	{
		public string CategoryName { get; set; }
		public string CorrectSheetFilename { get; set; }
		public bool[,,] CorrectAnswers { get; set; }
		public List<string> SheetFilenames { get; set; }
	}
}
