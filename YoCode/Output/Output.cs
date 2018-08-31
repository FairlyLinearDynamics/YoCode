using System.Collections.Generic;

namespace YoCode
{
    internal class Output
    {
        IPrint outputWriter;
        IErrorReporter errOutput;
        FeatureData featData;

        public Output(IPrint printTo, IErrorReporter errorReporter = null)
        {
            outputWriter = printTo;

            errOutput = errorReporter ?? new NullErrorObject();

            featData = new FeatureData();
        }

        public void PrintFinalResults(IEnumerable<FeatureEvidence> featureList,double finalScore)
        {
            outputWriter.AddFinalScore(finalScore);
            foreach (var feature in featureList)
            {
                featData.title = feature.FeatureTitle;
                featData.evidence = feature.Evidence;
                featData.featurePass = feature.Inconclusive ? (bool?)null : feature.Passed;
                featData.score = feature.FeatureRating;
                var result = feature.Inconclusive
                    ? "Could not perform check"
                    : $"Feature implemented: {(feature.Passed ? "Yes" : "No")}";
                featData.featureResult = result;
                featData.featureHelperMessage = feature.HelperMessage;

                outputWriter.AddFeature(featData);
            }
            outputWriter.WriteReport();
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
