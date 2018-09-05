using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace YoCode
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();
            AppDomain.CurrentDomain.UnhandledException += ExceptionHandler.CurrentDomain_UnhandledException;

            var commandLineHandler = new CommandLineParser(args);
            var result = commandLineHandler.Parse();

            const string reportFilename = "YoCodeReport.html";
            var outputPath = result.OutputFilePath != null ? Path.Combine(result.OutputFilePath, reportFilename) : reportFilename;

            var outputs = new List<IPrint>();

            if (result.CreateHtmlReport)
            {
                outputs.Add(new WebWriter(outputPath));
            }

            outputs.Add(new ConsoleWriter());

            var compositeOutput = new Output(new CompositeWriter(outputs), outputPath, (IErrorReporter)outputs.Find(a => a is ConsoleWriter));

            var appSettingsBuilder = new AppSettingsBuilder(result.JuniorTest);
            var parameters = new RunParameterChecker(compositeOutput, result, appSettingsBuilder);

            if (!parameters.ParametersAreValid())
            {
                if (!result.HelpAsked)
                {
                    compositeOutput.ShowInputErrors(parameters.Errs);
                }
                else
                {
                    compositeOutput.ShowHelp();
                }
                LaunchReport(result, outputPath);
                return;
            }

            var modifiedTestDirPath = result.InputFilePath;

            var dir = new PathManager(modifiedTestDirPath);

            var workThreads = new List<Thread>();

            if (!result.NoLoadingScreen)
            {
                var loadingThread = new Thread(LoadingAnimation.RunLoading)
                {
                    IsBackground = true
                };
                loadingThread.Name = "loadingThread";
                workThreads.Add(loadingThread);
                loadingThread.Start();
            }

            var evidenceList = new List<FeatureEvidence>();

            var checkManager = new CheckManager(new CheckConfig(dir, parameters));

            var projectRunner = await checkManager.PassGatewayChecksAsync(evidenceList);

            if (projectRunner == null)
            {
                StopLoadingAnimation(workThreads);
                compositeOutput.PrintFinalResults(evidenceList, 0);
                return;
            }

            evidenceList = await checkManager.PerformChecks(projectRunner);

            StopLoadingAnimation(workThreads);

            var results = new Results(evidenceList, appSettingsBuilder.GetWeightingsPath());

            //var fd = new FileDifference(dir.ModifiedTestDirPath);


            evidenceList.Add(new ResultSummary(evidenceList).ResultEvidence);

            compositeOutput.PrintFinalResults(evidenceList.OrderBy(a => FeatureTitleStorage.GetFeatureTitle(a.Feature)),
                results.FinalScore);

            LaunchReport(result, outputPath);

            Console.WriteLine($"YoCode run time: {stopwatch.Elapsed}");
        }

        private static void StopLoadingAnimation(List<Thread> workThreads)
        {
            LoadingAnimation.LoadingFinished = true;
            workThreads.ForEach(a => a.Join());
        }

        private static void LaunchReport(InputResult result, string outputPath)
        {
            if (result.CreateHtmlReport && result.OpenHtmlReport)
            {
                HtmlReportLauncher.LaunchReport(outputPath);
            }
        }
    }
}
