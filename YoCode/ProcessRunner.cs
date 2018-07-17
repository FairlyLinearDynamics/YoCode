using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace YoCode
{
    public class ProcessRunner
    {
        public bool TimedOut { get; private set; }
        public string Output { get; set; }
        public string ErrorOutput { get; set; }

        private ProcessInfo procinfo;
        private readonly TimeSpan timeout = TimeSpan.FromSeconds(15);
        private readonly StringBuilder output = new StringBuilder();
        private readonly StringBuilder errorOutput = new StringBuilder();


        public ProcessRunner(string processName, string workingDir, string arguments)
        {
            procinfo = SetupProcessInfo(processName, workingDir, arguments);
        }

        public void ExecuteTheCheck(string waitForMessage = null)
        {
                var p = new Process();
                p.StartInfo = SetProcessStartInfo(procinfo);
                p.EnableRaisingEvents = true;
                p.OutputDataReceived += DataReceived;
                p.ErrorDataReceived += ErrorDataReceived;
                p.Start();
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();

                WaitForExitCondition(p, waitForMessage);

                Output = output.ToString();
                ErrorOutput = errorOutput.ToString();

        }

        private void WaitForExitCondition(Process p, string wait)
        {
            if (string.IsNullOrEmpty(wait))
            {
                if (!p.WaitForExit((int)timeout.TotalMilliseconds))
                {
                    TimedOut = true;
                }
                return;
            }

            /*If code gets to this point, it should also look for "Unable to start Kestrel"
             *in case kestrel fails (Fails to bind to address)*/
            List<string> keywords = new List<string>
            {
                wait,
                "Unable to start Kestrel"
            };

            /*Check if 'appsettings.json file is missing*/
            List<string> errorKeywords = new List<string>
            {
                "'appsettings.json' was not found and is not optional."
            };

            var loopRetryDelay = TimeSpan.FromSeconds(0.5);
            var numberOfTimesToRetry = (int)(timeout / loopRetryDelay);
            var numberOfRetries = 0;

            while (!p.HasExited
                && !output.ToString().ContainsAny(keywords)
                && numberOfRetries < numberOfTimesToRetry
                && !errorOutput.ToString().ContainsAny(errorKeywords))
            {
                Thread.Sleep(loopRetryDelay);
                numberOfRetries++;
            }
            if(numberOfRetries == numberOfTimesToRetry)
            {
                TimedOut = true;
            }

            if(!p.HasExited)
            {
                p.Kill();
            }
        }

        private static ProcessStartInfo SetProcessStartInfo(ProcessInfo procinfo)
        {
            return new ProcessStartInfo
            {
                UseShellExecute = false,
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

        private void DataReceived(object sender, DataReceivedEventArgs e)
        {
            output.Append(e.Data + Environment.NewLine);
        }

        private void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            errorOutput.Append(e.Data + Environment.NewLine);
        }
    }
}
