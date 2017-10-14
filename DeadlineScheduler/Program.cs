using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeadlineScheduler
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new StartMenu());

			var main = new StartMenu();
			main.FormClosed += new FormClosedEventHandler(FormClosed);
			main.Show();
			Application.Run();
		}

		static void FormClosed(object sender, FormClosedEventArgs e)
		{
			((Form)sender).FormClosed -= FormClosed;
			if (Application.OpenForms.Count == 0) Application.ExitThread();
			else Application.OpenForms[0].FormClosed += FormClosed;
		}

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool SetProcessDPIAware();
	}
}
