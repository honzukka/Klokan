using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlokanUI
{
	struct Result
	{
		public List<List<List<AnswerType>>> CorrectedAnswers;
		public int Score;
		public bool Error;
	}
}
