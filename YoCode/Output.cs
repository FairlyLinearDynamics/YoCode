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
                if (feature.EvidencePresent)
                {
                    printer.PrintDiv();
                    printer.AddNewLine(feature.FeatureTitle);
                    printer.AddNewLine("");
                    printer.AddNewLine($"Feature implemented: {((feature.FeatureImplemented) ? "Yes" : "No")}");
                    printer.AddNewLine("Indication of above: ");
                    printEvidence(feature);
                    printer.PrintDiv();
                }
                else
                {
                    printer.PrintDiv();
                    printer.AddNewLine(feature.FeatureTitle);
                    printer.AddNewLine($"Feature implemented: {((feature.FeatureImplemented) ? "Yes" : "No")}");
                    printer.PrintDiv();
                }
            }

            printer.PrintMessage();
        }

        private void printEvidence(FeatureEvidence feature)
        {
            foreach(var evidence in feature.Evidence)
            {
                printer.AddNewLine(evidence);
            }
        }

        public void PrintWrongDirectory()
        {
            printer.AddNewLine("Invalid directory");
            printer.PrintMessage();
        }

        public void PrintError(List<string> errs)
        {

            printer.AddNewLine("Error detected:");
            foreach(var err in errs)
            {
                printer.AddNewLine(err);
            }
            printer.AddNewLine(messages.AskForHelp);
            printer.PrintMessage();
        }

        public void PrintDupfinderHelp()
        {
            printer.AddNewLine(messages.DupFinderHelp);
            printer.PrintMessage();
        }


        public void PrintHelp()
        {
            PrintFireplace();
            PrintHelpMessage();
            PrintDupfinderHelp();
        }

        public void PrintHelpMessage()
        {
            printer.AddNewLine(string.Format(messages.HelpMessage, CommandNames.ORIGIN, CommandNames.MODIFIED, CommandNames.HELP));
            printer.PrintMessage();
        }

        public void PrintFireplace()
        {
            printer.AddNewLine(messages.Fireplace);
            printer.PrintMessage();
        }

        public void LazinessEvidence()
        {
            printer.AddNewLine("Project unmodified");
            printer.PrintMessage();
        }

        public void NothingInDirectory()
        {
            printer.AddNewLine("Specified directory inaccessible");
            printer.PrintMessage();
        }
    }
}
