using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlokanUI
{
	struct Result
	{
		public Result(string category, AnswerType[,,] correctedAnswers, int score, string sheetFilename, bool error)
		{
			Category = category;
			CorrectedAnswers = correctedAnswers;
			Score = score;
			SheetFilename = sheetFilename;
			Error = error;
		}

		public Result(bool error)
		{
			Category = null;
			CorrectedAnswers = null;
			Score = -1;
			SheetFilename = null;
			Error = error;
		}

		public string Category { get; }
		public AnswerType[,,] CorrectedAnswers { get; }
		public int Score { get; }
		public string SheetFilename { get; }
		public bool Error { get; }
	}
}
