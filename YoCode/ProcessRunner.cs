﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Management;
using System.Threading;

namespace YoCode
{
    internal class ProcessRunner
    {
        internal bool TimedOut { get; private set; }

        public string Output { get; set; }
        public string ErrorOutput { get; set; }
        public int Pid { get; set; }

        public ProcessInfo procinfo;
        private readonly TimeSpan timeout = TimeSpan.FromSeconds(40);
        private readonly List<string> output = new List<string>();
        private readonly List<string> errorOutput = new List<string>();
        private Process p;

        public ProcessRunner(string processName, string workingDir, string arguments)
        {
            procinfo = SetupProcessInfo(processName, workingDir, arguments);
        }

        private bool ProcessShouldFinishAutomatically(string message) => string.IsNullOrWhiteSpace(message);

        public void ExecuteTheCheck(string waitForMessage = null, bool kill = true)
        {
            p = new Process();
            p.StartInfo = SetProcessStartInfo(procinfo);
            p.EnableRaisingEvents = true;
            p.OutputDataReceived += DataReceived;
            p.ErrorDataReceived += ErrorDataReceived;
            p.Start();
            Pid = p.Id;
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();

            if (ProcessShouldFinishAutomatically(waitForMessage))
            {
                WaitForProcessToFinish();
            }
            else
            {
                WaitForExitCondition(waitForMessage, kill);
            }

            Output = string.Join(Environment.NewLine, output);
            ErrorOutput = string.Join(Environment.NewLine, errorOutput);
        }

        private bool OutputContainsStopKeywords(List<string> outputStopWords, List<string> errorStopWords)
        {
            return output.ListContainsAnyKeywords(outputStopWords) || errorOutput.ListContainsAnyKeywords(errorStopWords);
        }

        private void WaitForProcessToFinish()
        {
            if (!p.WaitForExit((int)timeout.TotalMilliseconds))
            {
                TimedOut = true;
            }
            KillProcessWithChildren(p);
        }

        private void WaitForExitCondition(string wait, bool kill)
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
            if (kill)
            {
                KillProcessWithChildren(p);
            }
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

        private static void KillLiveProcess(Process p)
        {
            if (!p.HasExited)
            {
                try
                {
                    p.Kill();
                    p.Dispose();
                }
                catch
                {

                }
            }
        }

        private static void FindAndKillChildProcesses(int pid)
        {
            using (var mos = new ManagementObjectSearcher(string.Format(CultureInfo.InvariantCulture,
                  "Select * From Win32_Process Where ParentProcessID={0}", pid)))
            {
                foreach (var mo in mos.Get())
                {
                    try
                    {
                        var processById = Process.GetProcessById(Convert.ToInt32(mo["ProcessID"]));
                        FindAndKillChildProcesses(processById.Id);
                        KillLiveProcess(processById);
                    }
                    catch (ArgumentException) { }
                }
            }
        }

        public static void KillProcessWithChildren(Process p)
        {
            FindAndKillChildProcesses(p.Id);
            KillLiveProcess(p);
        }

        public void KillCurrentProcessWithChildren()
        {
            FindAndKillChildProcesses(p.Id);
            KillLiveProcess(p);
        }

        public void FindLeftOverProcess()
        {
            try
            {
                if (!Process.GetProcessById(Pid).HasExited)
                {
                    Console.WriteLine($"Left Over Process Detected with PID: {Pid}");
                }
            }
            catch (ArgumentException) { }
        }
    }
}
