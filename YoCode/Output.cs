using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace YoCode
{
    class Output
    {
        IPrint outputWriter;

        public Output(IPrint printTo)
        {
            outputWriter = printTo;
        }

        public void PrintIntroduction()
        {
            outputWriter.AddNewLine(messages.Welcome);
            outputWriter.PrintMessage();
        }

        public void PrintFinalResults(List<FeatureEvidence> featureList)
        {
            foreach (var feature in featureList)
            {
                outputWriter.PrintDiv();
                if (feature.EvidencePresent)
                {
                    OutputForEvidencePresnt(feature);
                }
                else
                {
                    OutputForEvidenceAbsent(feature);
                }
                outputWriter.PrintDiv();
            }

            outputWriter.PrintMessage();
        }

        public void OutputForEvidencePresnt(FeatureEvidence feature)
        {
            outputWriter.AddNewLine(feature.FeatureTitle);
            outputWriter.AddNewLine("");
            outputWriter.AddNewLine($"Feature implemented: {((feature.FeatureImplemented) ? "Yes" : "No")}");
            outputWriter.AddNewLine("Indication of above: ");
            ShowEvidence(feature);
        }

        public void OutputForEvidenceAbsent(FeatureEvidence feature)
        {
            outputWriter.AddNewLine(feature.FeatureTitle);
            outputWriter.AddNewLine($"Feature implemented: {((feature.FeatureImplemented) ? "Yes" : "No")}");
        }

        private void ShowEvidence(FeatureEvidence feature)
        {
            foreach(var evidence in feature.Evidence)
            {
                outputWriter.AddNewLine(evidence);
            }
        }

        public void ShowWrongDirectoryMsg()
        {
            outputWriter.AddNewLine("Invalid directory");
            outputWriter.PrintMessage();
        }

        public void ShowErrors(List<string> errs)
        {

            outputWriter.AddNewLine("Error detected:");
            foreach(var err in errs)
            {
                outputWriter.AddNewLine(err);
            }
            outputWriter.AddNewLine(messages.AskForHelp);
            outputWriter.PrintMessage();
        }

        public void ShowHelp()
        {
            ShowBanner();
            ShowHelpMsg();
            ShowDupfinderHelp();
        }

        public void ShowBanner()
        {
            outputWriter.AddNewLine(messages.Fireplace);
            outputWriter.PrintMessage();
        }

        public void ShowHelpMsg()
        {
            outputWriter.AddNewLine(string.Format(messages.HelpMessage, CommandNames.ORIGIN, CommandNames.MODIFIED, CommandNames.HELP));
            outputWriter.PrintMessage();
        }

        public void ShowDupfinderHelp()
        {
            outputWriter.AddNewLine(messages.DupFinderHelp);
            outputWriter.PrintMessage();
        }

        public void ShowLaziness()
        {
            outputWriter.AddNewLine("Project unmodified");
            outputWriter.PrintMessage();
        }

        public void ShowDirEmptyMsg()
        {
            outputWriter.AddNewLine("Specified directory inaccessible");
            outputWriter.PrintMessage();
        }
    }
}
