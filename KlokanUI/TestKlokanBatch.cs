using System.Collections.Generic;

namespace KlokanUI
{
	/// <summary>
	/// Test batch data structure to be fed to the evaluator.
	/// </summary>
	class TestKlokanBatch
	{
		public Parameters Parameters { get; set; }
		public List<TestKlokanInstance> TestInstances { get; set; }
	}

	/// <summary>
	/// Test data for one Klokan scan.
	/// Consists of the image data and a set of expected answers (values).
	/// </summary>
	class TestKlokanInstance
	{
		public int ScanId { get; set; }
		public byte[] Image { get; set; }
		public bool[,,] StudentExpectedValues { get; set; }
		public bool[,,] AnswerExpectedValues { get; set; }
	}
}
