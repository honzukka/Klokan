using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlokanUI
{
	class TestKlokanBatch
	{
		public Parameters Parameters { get; set; }
		public List<TestKlokanInstance> TestInstances { get; set; }
	}

	class TestKlokanInstance
	{
		public int ScanId { get; set; }
		public string SheetFilename { get; set; }
		public bool[,,] StudentExpectedValues { get; set; }
		public bool[,,] AnswerExpectedValues { get; set; }
	}
}
