using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace YoCode
{
    class ProcessRunner
    {
        ProcessInfo procinfo;
        public string Output { get; set; }

        public ProcessRunner(string processName,string workingDir,string arguments){
            procinfo = setupProcessInfo(processName, workingDir, arguments);
        }

        public void ExecuteTheCheck()
        {
            var p = new Process();
            p.StartInfo = SetProcessStartInfo(procinfo);
            p.Start();
            Output = p.StandardOutput.ReadToEnd();
        }

        private static ProcessStartInfo SetProcessStartInfo(ProcessInfo procinfo)
        {
            var psi = new ProcessStartInfo
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = procinfo.workingDir,
                FileName = procinfo.processName,
                Arguments = procinfo.arguments,
            };

            return psi;
        }

        public ProcessInfo setupProcessInfo(string processName, string workingDir, string arguments)
        {
            ProcessInfo pi;
            pi.processName = processName;
            pi.workingDir = workingDir;
            pi.arguments = arguments;

            return pi;
        }



    }
}
