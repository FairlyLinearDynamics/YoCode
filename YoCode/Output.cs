using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace YoCode
{
    class Output
    {
        IPrint printer;

        public Output(IPrint printTo)
        {
            printer = printTo;
        }

        public void PrintIntroduction()
        {
            printer.AddNewLine(messages.Welcome);
            printer.PrintMessage();
        }

        public void PrintFinalResults(List<FeatureEvidence> featureList)
        {
            foreach (var feature in featureList)
            {
                printer.PrintDiv();
                if (feature.EvidencePresent)
                {
                    OutputForEvidencePresnt(feature);
                }
                else
                {
                    OutputForEvidenceAbsent(feature);
                }
                printer.PrintDiv();
            }

            printer.PrintMessage();
        }

        public void OutputForEvidencePresnt(FeatureEvidence feature)
        {
            printer.AddNewLine(feature.FeatureTitle);
            printer.AddNewLine("");
            printer.AddNewLine($"Feature implemented: {((feature.FeatureImplemented) ? "Yes" : "No")}");
            printer.AddNewLine("Indication of above: ");
            ShowEvidence(feature);
        }

        public void OutputForEvidenceAbsent(FeatureEvidence feature)
        {
            printer.AddNewLine(feature.FeatureTitle);
            printer.AddNewLine($"Feature implemented: {((feature.FeatureImplemented) ? "Yes" : "No")}");
        }

        private void ShowEvidence(FeatureEvidence feature)
        {
            foreach(var evidence in feature.Evidence)
            {
                printer.AddNewLine(evidence);
            }
        }

        public void ShowWrongDirectoryMsg()
        {
            printer.AddNewLine("Invalid directory");
            printer.PrintMessage();
        }

        public void ShowErrors(List<string> errs)
        {

            printer.AddNewLine("Error detected:");
            foreach(var err in errs)
            {
                printer.AddNewLine(err);
            }
            printer.AddNewLine(messages.AskForHelp);
            printer.PrintMessage();
        }

        public void ShowHelp()
        {
            ShowFireplace();
            ShowHelpMsg();
            ShowDupfinderHelp();
        }

        public void ShowFireplace()
        {
            printer.AddNewLine(messages.Fireplace);
            printer.PrintMessage();
        }

        public void ShowHelpMsg()
        {
            printer.AddNewLine(string.Format(messages.HelpMessage, CommandNames.ORIGIN, CommandNames.MODIFIED, CommandNames.HELP));
            printer.PrintMessage();
        }

        public void ShowDupfinderHelp()
        {
            printer.AddNewLine(messages.DupFinderHelp);
            printer.PrintMessage();
        }

        public void ShowLaziness()
        {
            printer.AddNewLine("Project unmodified");
            printer.PrintMessage();
        }

        public void ShowDirEmptyMsg()
        {
            printer.AddNewLine("Specified directory inaccessible");
            printer.PrintMessage();
        }
    }
}
