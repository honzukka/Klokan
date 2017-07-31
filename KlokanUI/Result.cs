using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlokanUI
{
	struct Result
	{
		public Result(List<List<List<AnswerType>>> correctedAnswers, int score, bool error)
		{
			CorrectedAnswers = correctedAnswers;
			Score = score;
			Error = error;
		}

		public Result(bool error)
		{
			CorrectedAnswers = null;
			Score = -1;
			Error = error;
		}

		public List<List<List<AnswerType>>> CorrectedAnswers { get; }
		public int Score { get; }
		public bool Error { get; }
	}
}
