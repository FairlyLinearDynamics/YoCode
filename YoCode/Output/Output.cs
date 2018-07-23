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
        }

        public void PrintFinalResults(List<FeatureEvidence> featureList)
        {
            foreach (var feature in featureList)
            {
                featData.title = feature.FeatureTitle;
                featData.featureResult = $"Feature implemented: {((feature.FeatureImplemented) ? "Yes" : "No")}";
                featData.evidence = feature.Evidence;
                outputWriter.AddFeature(featData);
            }
            outputWriter.WriteReport();
        }

        public void ShowErrors(List<string> errs)
        {

            outputWriter.AddMessage("Error detected:");
            foreach(var err in errs)
            {
                outputWriter.AddErr(err);
            }
            outputWriter.AddMessage(messages.AskForHelp);
        }

        public void ShowHelp()
        {
            ShowBanner();
            ShowHelpMsg();
            ShowDupfinderHelp();
        }

        public void ShowBanner()
        {
            outputWriter.AddMessage(messages.Fireplace);
        }

        public void ShowHelpMsg()
        {
            outputWriter.AddMessage(string.Format(messages.HelpMessage, CommandNames.ORIGIN, CommandNames.MODIFIED, CommandNames.HELP));
        }

        public void ShowDupfinderHelp()
        {
            outputWriter.AddMessage(messages.DupFinderHelp);
        }

        public void ShowLaziness()
        {
            outputWriter.AddMessage("Project unmodified");
        }

        public void ShowDirEmptyMsg()
        {
            outputWriter.AddMessage("Specified directory inaccessible");
        }
    }
}
