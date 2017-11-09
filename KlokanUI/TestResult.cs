using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlokanUI
{
	struct TestResult
	{
		public bool[,,] StudentComputedValues { get; }
		public bool[,,] StudentExpectedValues { get; }
		public bool[,,] AnswerComputedValues { get; }
		public bool[,,] AnswerExpectedValues { get; }
		public float Correctness { get; }
		public bool Error { get; }

		public TestResult(bool[,,] studentComputedValues, bool[,,] studentExpectedValues, 
			bool[,,] answerComputedValues, bool[,,] answerExpectedValues, float correctness, bool error)
		{
			StudentComputedValues = studentComputedValues;
			StudentExpectedValues = studentExpectedValues;
			AnswerComputedValues = answerComputedValues;
			AnswerExpectedValues = answerExpectedValues;
			Correctness = correctness;
			Error = error;
		}

		public TestResult(bool error)
		{
			StudentComputedValues = null;
			StudentExpectedValues = null;
			AnswerComputedValues = null;
			AnswerExpectedValues = null;
			Correctness = -1;
			Error = error;
		}
	}
}
