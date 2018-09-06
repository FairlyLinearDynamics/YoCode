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
                    LaunchReport(result, outputPath);
                }
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

            evidenceList = await checkManager.PerformChecks(projectRunner);
            var results = new Results(evidenceList, appSettingsBuilder.GetWeightingsPath());

            if (projectRunner == null)
            {
                StopLoadingAnimation(workThreads);
                compositeOutput.PrintFinalResults(new FinalResultsData()
                {
                    featureList = evidenceList,
                    isJunior = result.JuniorTest,
                    finalScore = 0,
                    finalScorePercentage = 0,
                });
                return;
            }

            StopLoadingAnimation(workThreads);

            compositeOutput.PrintFinalResults(new FinalResultsData()
            {
                featureList = evidenceList.OrderBy(a => FeatureTitleStorage.GetFeatureTitle(a.Feature)),
                isJunior = result.JuniorTest,
                finalScore = results.FinalScore,
                finalScorePercentage = results.FinalScorePercentage,
            });

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
