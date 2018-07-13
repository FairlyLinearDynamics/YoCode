using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace YoCode
{
    public class ProcessRunner
    {
        private readonly TimeSpan timeout = TimeSpan.FromSeconds(20);
        public bool TimedOut { get; set; }
        ProcessInfo procinfo;
        public string Output { get; set; }
        public string ErrorOutput { get; set; }

        public ProcessRunner(string processName,string workingDir,string arguments)
        {
            procinfo = SetupProcessInfo(processName, workingDir, arguments);
        }

        public void ExecuteTheCheck()
        {
            var p = new Process();
            p.StartInfo = SetProcessStartInfo(procinfo);
            p.Start();

            if(!p.WaitForExit((int)timeout.TotalMilliseconds))
            {
                TimedOut = true;
            }
            Output = p.StandardOutput.ReadToEnd();
            ErrorOutput = p.StandardError.ReadToEnd();
        }

        private static ProcessStartInfo SetProcessStartInfo(ProcessInfo procinfo)
        {
            return new ProcessStartInfo
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = procinfo.workingDir,
                FileName = procinfo.processName,
                Arguments = procinfo.arguments,
            };
        }

        public ProcessInfo SetupProcessInfo(string processName, string workingDir, string arguments)
        {
            ProcessInfo pi;
            pi.processName = processName;
            pi.workingDir = workingDir;
            pi.arguments = arguments;

            return pi;
        }
    }
}
