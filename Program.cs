using System;
using System.Text;
using System.Windows.Forms;

namespace Nsr.Planner
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MDIMain());
        }

        internal static NsrDataAdapter NsrDataAdapter => new(Application.ExecutablePath, Properties.Resources.ExcelPath);
    }
}
