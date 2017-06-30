using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace klokantest
{
	class Native
	{
		[DllImport("klokan.dll")]
		public static extern void hello_world();
	}

	class Program
	{
		static void Main(string[] args)
		{
			Native.hello_world();
		}
	}
}
