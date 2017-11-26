﻿using System;
using System.Windows.Forms;

namespace KlokanUI
{
	// TODO: watch out for memory leaks!
	// TODO: handle database exceptions when saving changes!

	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
