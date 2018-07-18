using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace YoCode
{
    class Output
    {
        IPrint consoleWritter;

        public Output()
        {
            consoleWritter = new PrintToConsole();
            PrintIntroduction();
        }

        public void PrintIntroduction()
        {
            consoleWritter.AddNewLine(messages.Welcome);
            consoleWritter.PrintMessage();
        }


        public void PrintFinalResults(List<FeatureEvidence> featureList)
        {
            var resultOutput = "";
            foreach (FeatureEvidence feature in featureList)
            {
                if (feature.EvidencePresent)
                {
                    consoleWritter.PrintDiv();
                    consoleWritter.AddNewLine(feature.FeatureTitle);
                    consoleWritter.AddNewLine($"Feature implemented: {((feature.FeatureImplemented) ? "Yes" : "No")}");
                    consoleWritter.AddNewLine("Indication of above: ");
                    consoleWritter.AddNewLine(FormatEvidence(feature));
                    consoleWritter.PrintDiv();
                }
                else
                {
                    consoleWritter.PrintDiv();
                    consoleWritter.AddNewLine(feature.FeatureTitle);
                    consoleWritter.AddNewLine($"Feature implemented: {((feature.FeatureImplemented) ? "Yes" : "No")}");
                    consoleWritter.PrintDiv();
                }
            }

            consoleWritter.PrintMessage();
        }

        private string FormatEvidence(FeatureEvidence evidence)
        {
            return (evidence.EvidencePresent) ?
                evidence.Evidence.Aggregate((a, b) => $"{a}{Environment.NewLine}{b}")
                : "No evidence present";
        }

        public void PrintWrongDirectory()
        {
            consoleWritter.AddNewLine("Invalid directory");
            consoleWritter.PrintMessage();
        }

        public void PrintError(List<string> errs)
        {
            var resultOutput = "";

            resultOutput += ("Error detected:")
                +Environment.NewLine + errs.Select(row => row + Environment.NewLine)
                +Environment.NewLine + messages.AskForHelp;

            consoleWritter.AddNewLine(resultOutput);
            consoleWritter.PrintMessage();
        }

        public void PrintDupfinderHelp()
        {
            consoleWritter.AddNewLine(messages.DupFinderHelp);
            consoleWritter.PrintMessage();
        }


        public void PrintHelp()
        {
            PrintFireplace();
            PrintHelpMessage();
            PrintDupfinderHelp();
        }

        public void PrintHelpMessage()
        {
            consoleWritter.AddNewLine(string.Format(messages.HelpMessage, CommandNames.ORIGIN, CommandNames.MODIFIED, CommandNames.HELP));
            consoleWritter.PrintMessage();
        }

        public void PrintFireplace()
        {
            consoleWritter.AddNewLine(messages.Fireplace);
            consoleWritter.PrintMessage();
        }

        public void LazinessEvidence()
        {
            consoleWritter.AddNewLine("Project unmodified");
            consoleWritter.PrintMessage();
        }

        public void NothingInDirectory()
        {
            consoleWritter.AddNewLine("Specified directory inaccessible");
            consoleWritter.PrintMessage();
        }
    }
}
