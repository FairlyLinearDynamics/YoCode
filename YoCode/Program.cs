using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace YoCode
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
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

            var compositeOutput = new Output(new CompositeWriter(outputs), (IErrorReporter)outputs.Find(a => a is ConsoleWriter));

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

            var checkManager = new CheckManager(dir, workThreads);

            var projectRunner = checkManager.PassGatewayChecks(evidenceList);

            if (projectRunner == null)
            {
                LoadingAnimation.LoadingFinished = true;
                workThreads.ForEach(a => a.Join());
                compositeOutput.PrintFinalResults(evidenceList, 0);
                return;
            }

            evidenceList = checkManager.PerformChecks(parameters, projectRunner);

            var results = new Results(evidenceList, appSettingsBuilder.GetWeightingsPath());

            //var fd = new FileDifference(dir.ModifiedTestDirPath);


            evidenceList.Add(new ResultSummary(evidenceList).ResultEvidence);

            compositeOutput.PrintFinalResults(evidenceList.OrderBy(a => FeatureTitleStorage.GetFeatureTitle(a.Feature)),
                results.FinalScore);

            LaunchReport(result, outputPath);
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
