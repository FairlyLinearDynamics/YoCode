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

        public ProcessRunner(ProcessInfo processInfo){
            procinfo = processInfo;
        }

        public void ExecuteTheCheck()
        {
            var p = new Process();
            ExecuteTheCheck(p);
        }

        public void ExecuteTheCheck(Process p)
        {
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





}
}
