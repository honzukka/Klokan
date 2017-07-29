using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlokanUI
{
	struct KlokanBatch
	{
		public Parameters Parameters;
		public Dictionary<string, KlokanCategoryBatch> CategoryBatches;
	}

	struct KlokanCategoryBatch
	{
		public string CategoryName;
		public string CorrectSheetFilename;
		public List<string> SheetFilenames;
	}
}
