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
		public List<KlokanCategoryBatch> CategoryBatches;
	}

	struct KlokanCategoryBatch
	{
		public Category Category;
		public string CorrectSheetFilename;
		public List<string> SheetFilenames;
	}
}
