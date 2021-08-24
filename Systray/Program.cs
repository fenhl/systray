using System;
using System.Windows.Forms;

namespace Systray
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (DiskSpaceProcessIcon diskSpace = new DiskSpaceProcessIcon())
            {
                diskSpace.Display();
                Application.Run();
            }
        }
    }
}
