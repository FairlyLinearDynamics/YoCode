using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace YoCode
{
    internal static class Program
    {
        private static bool showLoadingAnim;
        private static bool isJunior;

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += ExceptionHandler.CurrentDomain_UnhandledException;

            var commandLineHandler = new CommandLineParser(args);
            var result = commandLineHandler.Parse();

            isJunior = result.JuniorTest;

            const string reportFilename = "YoCodeReport.html";
            var outputPath = result.OutputFilePath != null ? Path.Combine(result.OutputFilePath, reportFilename) : reportFilename;

            var outputs = new List<IPrint> { new ConsoleWriter() };

            if (result.CreateHtmlReport)
            {
                outputs.Add(new WebWriter(outputPath));
            }

            var compositeOutput = new Output(new CompositeWriter(outputs), (IErrorReporter)outputs.Find(a => a is ConsoleWriter));

            var parameters = new RunParameterChecker(compositeOutput, result, new AppSettingsBuilder());

            if (!parameters.ParametersAreValid(isJunior))
            {
                if (!result.HelpAsked)
                {
                    compositeOutput.ShowInputErrors(parameters.Errs);
                }
                else
                {
                    compositeOutput.ShowHelp();
                }
                return;
            }

            var modifiedTestDirPath = result.InputFilePath;

            var dir = new PathManager(modifiedTestDirPath);

            showLoadingAnim = !result.NoLoadingScreen;

            var workThreads = new List<Thread>();

            if (showLoadingAnim)
            {
                var loadingThread = new Thread(LoadingAnimation.RunLoading)
                {
                    IsBackground = true
                };
                workThreads.Add(loadingThread);
                loadingThread.Start();
            }

            var evidenceList = new List<FeatureEvidence>();

            var checkManager = new CheckManager(dir, workThreads, isJunior);

            var projectRunner = checkManager.PassGatewayChecks(evidenceList);

            if (projectRunner == null)
            {
                LoadingAnimation.LoadingFinished = true;
                workThreads.ForEach(a => a.Join());
                compositeOutput.PrintFinalResults(evidenceList, 0);
                return;
            }

            evidenceList = checkManager.PerformChecks(parameters, projectRunner);

            compositeOutput.PrintFinalResults(evidenceList.OrderBy(a => a.FeatureTitle),
                new Results(evidenceList, isJunior ? TestType.Junior : TestType.Original).FinalScore);

            if (result.CreateHtmlReport && result.OpenHtmlReport)
            {
                HtmlReportLauncher.LaunchReport(outputPath);
            }
        }
    }
}
