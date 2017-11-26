namespace KlokanUI
{
	struct TestResult
	{
		public int ScanId { get; set; }
		public bool[,,] StudentComputedValues { get; }
		public bool[,,] StudentExpectedValues { get; }
		public bool[,,] AnswerComputedValues { get; }
		public bool[,,] AnswerExpectedValues { get; }
		public float Correctness { get; }
		public bool Error { get; }

		public TestResult(int scanId, bool[,,] studentComputedValues, bool[,,] studentExpectedValues, 
			bool[,,] answerComputedValues, bool[,,] answerExpectedValues, float correctness, bool error)
		{
			ScanId = scanId;
			StudentComputedValues = studentComputedValues;
			StudentExpectedValues = studentExpectedValues;
			AnswerComputedValues = answerComputedValues;
			AnswerExpectedValues = answerExpectedValues;
			Correctness = correctness;
			Error = error;
		}

		public TestResult(bool error)
		{
			ScanId = -1;
			StudentComputedValues = null;
			StudentExpectedValues = null;
			AnswerComputedValues = null;
			AnswerExpectedValues = null;
			Correctness = -1;
			Error = error;
		}
	}
}
