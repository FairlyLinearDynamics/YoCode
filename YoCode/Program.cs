using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace YoCode
{
    internal static class Program
    {
        private static bool showLoadingAnim;
        private static bool isJunior;

        public static bool OpenHTMLOnFinish { get; set; }
        public static string OutputTo { get; set; }
        public static bool GenerateHtml { get; set; }

        private static void Main(string[] args)
        {
            var outputs = new List<IPrint> { new WebWriter(), new ConsoleWriter() };

            var compositeOutput = new Output(new CompositeWriter(outputs), (IErrorReporter)outputs.Find(a => a is ConsoleWriter));

            var commandLinehandler = new CommandLineParser(args);
            var result = commandLinehandler.Parse();

            var parameters = new RunParameterChecker(compositeOutput, result, new AppSettingsBuilder());

            OpenHTMLOnFinish = !result.Silent;
            OutputTo = result.OutputFilePath;
            GenerateHtml = !result.NoHtml;
            isJunior = result.JuniorTest;

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
        }
    }
}
