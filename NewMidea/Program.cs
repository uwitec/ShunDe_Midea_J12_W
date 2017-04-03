using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
namespace NewMideaProgram
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //FE AA 0A 00 FF 00 08 00 00 20 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 CF 55 FE
            //FE AA 0A 00 FF 00 08 00 00 20 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 CF 55 FE
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!File.Exists(cMain.AppPath + "\\SystemInfo.txt"))
            {
                frmSys.DataClassToFile(cMain.mSysSet);
            }
            else
            {
                if (!frmSys.DataFileToClass((cMain.AppPath + "\\SystemInfo.txt"), out cMain.mSysSet, true))
                {
                    frmSys.DataClassToFile(cMain.mSysSet);
                }
            }
            cMain.IndexLanguage = 1;
            Application.Run(new frmMain());
        }
    }
}