﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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

            if (errorReporter == null)
            {
                errOutput = new NullErrorObject();
            }
            else
            {
                errOutput = errorReporter;
            }

            featData = new FeatureData();
        }

        public void PrintFinalResults(IEnumerable<FeatureEvidence> featureList)
        {
            foreach (var feature in featureList)
            {
                featData.title = feature.FeatureTitle;
                featData.featureResult = $"Feature implemented: {((feature.FeatureImplemented) ? "Yes" : "No")}";
                featData.evidence = feature.Evidence;
                featData.featurePass = feature.FeatureImplemented;
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
            outputWriter.AddMessage(string.Format(messages.HelpMessage, CommandNames.ORIGIN, CommandNames.MODIFIED, 
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

        public void ShowLaziness()
        {
            outputWriter.AddMessage("Project unmodified");
            outputWriter.WriteReport();
        }

        public void ShowDirEmptyMsg()
        {
            outputWriter.AddMessage("Specified directory inaccessible");
            outputWriter.WriteReport();
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
