using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using NewMideaProgram;
namespace System
{
    class cAPI
    {
        public static int FindWindowW(string lpClassName, string lpWindowName)
        {
            if (cMain.isComPuter)
            {
                return PcApi.FindWindowW(lpClassName, lpWindowName);
            }
            else
            {
                return CeApi.FindWindowW(lpClassName, lpWindowName);
            }
        }
        public static int ShowWindow(int hWnd, int nCmdShow)
        {
            if (cMain.isComPuter)
            {
                return PcApi.ShowWindow(hWnd, nCmdShow);
            }
            else
            {
                return CeApi.ShowWindow(hWnd, nCmdShow);
            }
        }
        
        public static uint GetLastError()
        {
            if (cMain.isComPuter)
            {
                return PcApi.GetLastError();
            }
            else
            {
                return CeApi.GetLastError();
            }
        }
    }
    class CeApi
    {
        [DllImport("Coredll.dll")]
        public static extern int FindWindowW(string lpClassName, string lpWindowName);
        [DllImport("Coredll.dll")]
        public static extern int ShowWindow(int hWnd, int nCmdShow);
        [DllImport("Coredll.dll")]
        public static extern uint GetLastError(
        );
    }
    class PcApi
    {
        [DllImport("kernel32.dll")]
        public static extern int FindWindowW(string lpClassName, string lpWindowName);
        [DllImport("kernel32.dll")]
        public static extern int ShowWindow(int hWnd, int nCmdShow);
        
        [DllImport("kernel32.dll")]
        public static extern uint GetLastError(
        );
    }
}
