using System;
using System.Diagnostics;

namespace YoCode
{
    internal static class ExceptionHandler
    {
        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ConsoleCloseHandler.CloseProcesses();

            Debug.WriteLine((e.ExceptionObject as Exception)?.Message);
        }
    }
}
