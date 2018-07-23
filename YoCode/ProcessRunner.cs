using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace YoCode
{
    public class ProcessRunner
    {
        internal bool TimedOut { get; private set; }

        public string Output { get; set; }
        public string ErrorOutput { get; set; }

        private ProcessInfo procinfo;
        private readonly TimeSpan timeout = TimeSpan.FromSeconds(40);
        private readonly List<string> output = new List<string>();
        private readonly List<string> errorOutput = new List<string>();

        public ProcessRunner(string processName, string workingDir, string arguments)
        {
            procinfo = SetupProcessInfo(processName, workingDir, arguments);
        }

        private bool ProcessShouldFinishAutomatically(string message) => string.IsNullOrWhiteSpace(message);

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

            if (ProcessShouldFinishAutomatically(waitForMessage))
            {
                WaitForProcessToFinish(p);
            } else
            {
                WaitForExitCondition(p, waitForMessage);
            }

            Output = string.Join(Environment.NewLine, output);
            ErrorOutput = string.Join(Environment.NewLine, errorOutput);
        }

        private bool OutputContainsStopKeywords(List<string> outputStopWords, List<string> errorStopWords)
        {
            return output.ListContainsAnyKeywords(outputStopWords) || errorOutput.ListContainsAnyKeywords(errorStopWords);
        }

        private void WaitForProcessToFinish(Process p)
        {
            if (!p.WaitForExit((int)timeout.TotalMilliseconds))
            {
                TimedOut = true;
            }
            KillLiveProcess(p);
        }

        private void WaitForExitCondition(Process p, string wait)
        {
            var keywords = ExpectedStopConditions(wait);
            var errorKeywords = ExpectedErrorStopConditions();

            var loopRetryDelay = TimeSpan.FromSeconds(0.5);
            var numberOfTimesToRetry = (int)(timeout / loopRetryDelay);
            var numberOfRetries = 0;

            while (!p.HasExited
                && !OutputContainsStopKeywords(keywords, errorKeywords)
                && numberOfRetries < numberOfTimesToRetry)
            {
                Thread.Sleep(loopRetryDelay);
                numberOfRetries++;
            }

            TimedOut = numberOfRetries == numberOfTimesToRetry;

            KillLiveProcess(p);
        }

        private List<string> ExpectedStopConditions(string extraCondition) => new List<string>
            {
                extraCondition,
                "Unable to start Kestrel",
                "Failed to bind to address"
            };

        private List<string> ExpectedErrorStopConditions() => new List<string>
            {
                "'appsettings.json' was not found and is not optional."
            };

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
            output.Add(e.Data);
        }

        private void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            errorOutput.Add(e.Data);
        }

        private void KillLiveProcess(Process p)
        {
            if (!p.HasExited)
            {
                p.Kill();
            }
        }
    }
}
