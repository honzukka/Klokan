using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlokanUI
{
	interface IEvaluationForm
	{
		void EnableGoButton();
		void ShowMessageBoxInfo(string message, string caption);
	}
}
