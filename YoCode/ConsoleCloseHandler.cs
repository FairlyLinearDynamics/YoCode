using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace YoCode
{
    internal static class ConsoleCloseHandler
    {
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(SetConsoleCtrlEventHandler handler, bool add);

        private delegate bool SetConsoleCtrlEventHandler(CtrlType sig);

        private static ProjectRunner Pr { get; set; }

        public static void StartHandler(ProjectRunner projectRunner)
        {
            Pr = projectRunner;
            SetConsoleCtrlHandler(Handler, true);
        }

        private static bool Handler(CtrlType signal)
        {
            switch (signal)
            {
                case CtrlType.CTRL_BREAK_EVENT:
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:
                    Console.WriteLine("Closing");

                    try
                    {
                        Pr.KillProject();

                        if (Array.Find(Process.GetProcesses(), x => x.ProcessName == "geckodriver") != null)
                        {
                            var process = Array.Find(Process.GetProcesses(), x => x.ProcessName == "geckodriver");
                            ProcessRunner.KillProcessWithChildren(process);
                        }
                    }
                    catch (NullReferenceException) { }

                    Environment.Exit(0);
                    return false;

                default:
                    return false;
            }
        }
    }
}
