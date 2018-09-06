using OpenQA.Selenium;
using System.Collections.Generic;
using System.IO;

namespace YoCode
{
    internal class Output
    {
        IPrint outputWriter;
        IErrorReporter errOutput;
        FeatureData featData;
        static string writeTo;


        public Output(IPrint printTo, string outputPath = null, IErrorReporter errorReporter = null)
        {
            outputWriter = printTo;

            errOutput = errorReporter ?? new NullErrorObject();

            featData = new FeatureData();

            writeTo = outputPath;
        }

        public void PrintFinalResults(IEnumerable<FeatureEvidence> featureList, bool isJunior, double finalScore, double finalScorePercentage)
        {
            outputWriter.AddFinalScore(finalScorePercentage);
            outputWriter.AddTestType(isJunior);
            foreach (var feature in featureList)
            {
                featData.title = FeatureTitleStorage.GetFeatureTitle(feature.Feature);
                featData.featureEvidence = feature.Evidence;
                featData.featurePass = feature.Inconclusive ? (bool?)null : feature.Passed;
                featData.score = finalScore == 0 ? 0 : (int)(feature.WeightedRating * 100 / finalScore);
                var result = feature.Inconclusive
                    ? "Could not perform check"
                    : $"Feature implemented: {(feature.Passed ? "Yes" : "No")}";
                featData.featureResult = result;
                featData.featureHelperMessage = feature.HelperMessage;
                featData.weighting = feature.FeatureWeighting;
                featData.rawScore = feature.FeatureRating * 100;
                outputWriter.AddFeature(featData);
            }
            outputWriter.WriteReport();
        }

        public static void PrintScreenShot(Screenshot screenshot, string fileName)
        {
            screenshot.SaveAsFile(Path.Combine(Path.GetDirectoryName(writeTo), fileName),ScreenshotImageFormat.Png);
        }

        public void ShowInputErrors(List<string> errs)
        {
            errOutput.PrintErrors(errs);
        }

        public void ShowHelp()
        {
            ShowBanner();
            ShowHelpMsg();
            ShowDupfinderHelp();
            ShowCodeCoverageHelp();
            outputWriter.WriteReport();
        }

        private void ShowBanner()
        {
            outputWriter.AddBanner();
        }

        private void ShowHelpMsg()
        {
            outputWriter.AddMessage(string.Format(messages.HelpMessage, CommandNames.INPUT, 
                CommandNames.HELP, CommandNames.NOLOADINGSCREEN, CommandNames.SILENTREPORT, CommandNames.OUTPUT, CommandNames.NOHTML));
        }

        private void ShowDupfinderHelp()
        {
            outputWriter.AddMessage(messages.DupFinderHelp);
        }

        private void ShowCodeCoverageHelp()
        {
            outputWriter.AddMessage(messages.CodeCoverageHelp);
        }

        public void AppsettingsHelp()
        {
            outputWriter.AddMessage(messages.AppsettingsHelp);
            outputWriter.WriteReport();
        }

        public void ShowSettingHelp()
        {
            AppsettingsHelp();
            ShowHelp();
        }
    }
}
