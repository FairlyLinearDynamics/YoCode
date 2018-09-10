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
                    WebWriter.LaunchReport(result, outputPath);
                }
                return;
            }

            var modifiedTestDirPath = result.InputFilePath;

            var dir = new PathManager(modifiedTestDirPath);

            CancellationTokenSource source = new CancellationTokenSource();

            if (!result.NoLoadingScreen)
            {
                var t = Task.Run(() =>
                {
                   LoadingAnimation.RunLoading(source.Token);
               },source.Token);

            }

            var checkManager = new CheckManager(new CheckConfig(dir, parameters));

            var evidenceList = await checkManager.PerformChecks();
            var results = new Results(evidenceList, appSettingsBuilder.GetWeightingsPath());

            StopLoadingAnimation(source);

            compositeOutput.PrintFinalResults(new FinalResultsData()
            {
                featureList = evidenceList.OrderBy(a => FeatureTitleStorage.GetFeatureTitle(a.Feature)),
                isJunior = result.JuniorTest,
                finalScore = results.FinalScore,
                finalScorePercentage = results.FinalScorePercentage,
            });

            WebWriter.LaunchReport(result, outputPath);

            Console.WriteLine($"YoCode run time: {stopwatch.Elapsed}");
        }

        public static void StopLoadingAnimation(CancellationTokenSource source)
        {
            LoadingAnimation.LoadingFinished = true;
            source.Cancel();
        }
    }
}
