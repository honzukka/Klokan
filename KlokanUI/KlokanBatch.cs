using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlokanUI
{
	class KlokanBatch
	{
		public Parameters Parameters { get; set; }
		public Dictionary<string, KlokanCategoryBatch> CategoryBatches { get; set; }
	}

	class KlokanCategoryBatch
	{
		public string CategoryName { get; set; }
		public string CorrectSheetFilename { get; set; }
		public bool[,,] CorrectAnswers { get; set; }
		public List<string> SheetFilenames { get; set; }
	}
}
