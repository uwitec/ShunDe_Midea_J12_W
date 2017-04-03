using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PcProgram
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            cMain.strLanguage ="zh-CN";
            System.Globalization.CultureInfo UICulture = new System.Globalization.CultureInfo(cMain.strLanguage);
            System.Threading.Thread.CurrentThread.CurrentUICulture = UICulture;
            Application.Run(new frmMain());
        }
    }
}