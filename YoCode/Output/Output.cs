using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace YoCode
{
    class Output
    {
        IPrint outputWriter;
        FeatureData featData;

        public Output(IPrint printTo)
        {
            outputWriter = printTo;
            featData = new FeatureData();
        }

        public void PrintIntroduction()
        {
            outputWriter.AddIntro(messages.Welcome);
            outputWriter.WriteReport();
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
            outputWriter.AddErrs(errs);
            outputWriter.AddMessage(messages.AskForHelp);
            outputWriter.WriteReport();
        }

        public void ShowHelp()
        {
            //ShowBanner();
            ShowHelpMsg();
            ShowDupfinderHelp();
            outputWriter.WriteReport();
        }

        private void ShowBanner()
        {
            outputWriter.AddMessage(messages.Fireplace);
        }

        private void ShowHelpMsg()
        {
            outputWriter.AddMessage(string.Format(messages.HelpMessage, CommandNames.ORIGIN, CommandNames.MODIFIED, CommandNames.HELP));
        }

        private void ShowDupfinderHelp()
        {
            outputWriter.AddMessage(messages.DupFinderHelp);
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
