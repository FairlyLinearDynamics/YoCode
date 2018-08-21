﻿using System.Collections.Generic;

namespace YoCode
{
    class Output
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
                featData.featureResult = $"Feature implemented: {((feature.FeatureImplemented) ? "Yes" : "No")}";
                featData.evidence = feature.Evidence;
                featData.featurePass = feature.FeatureImplemented;
                featData.score = feature.FeatureRating;
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
            outputWriter.AddMessage(string.Format(messages.HelpMessage, CommandNames.MODIFIED, 
                CommandNames.HELP, CommandNames.NOLOADINGSCREEN, CommandNames.SILENTREPORT));
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
