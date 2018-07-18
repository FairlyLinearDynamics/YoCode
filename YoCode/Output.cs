using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace YoCode
{
    class Output
    {
        PrintToConsole consoleWritter;

        public Output()
        {
            consoleWritter = new PrintToConsole();
            PrintIntroduction();
        }

        public void PrintIntroduction()
        {
            Console.WriteLine(YoCode.messages.Welcome);
        }


        public void PrintFinalResults(List<FeatureEvidence> featureList)
        {
            foreach (FeatureEvidence feature in featureList)
            {
                Console.WriteLine(YoCode.messages.equalsFormat);
                Console.Write($"{feature.FeatureTitle}: ");
                Console.WriteLine((feature.FeatureImplemented) ? "Yes" : "No");
                Console.WriteLine();
                if (feature.EvidencePresent)
                {
                    Console.WriteLine("Indication of above result: ");
                    Console.WriteLine(FormatEvidence(feature));
                    Console.WriteLine(YoCode.messages.equalsFormat);
                }
            }
        }

        private string FormatEvidence(FeatureEvidence evidence)
        {
            return (evidence.EvidencePresent) ?
                evidence.Evidence.Aggregate((a, b) => $"{a}\n{b}")
                : "No evidence present";
        }

        public void PrintWrongDirectory()
        {
            Console.WriteLine("Invalid directory");
        }

        public void PrintError(List<string> errs)
        {
            Console.WriteLine("Error detected:");

            foreach (string err in errs)
            {
                Console.WriteLine(err);
            }
            Console.WriteLine("\nIf you would like to see list of commands, type: --help");
        }

        public static void PrintDupfinderHelp()
        {
            Console.WriteLine(YoCode.messages.DupFinderHelp);
        }


        public void PrintHelp()
        {
            PrintFireplace();
            PrintHelpMessage();
            PrintDupfinderHelp();
        }

        public void PrintHelpMessage()
        {
            var x = String.Format(YoCode.messages.HelpMessage, CommandNames.ORIGIN, CommandNames.MODIFIED, CommandNames.HELP);
            Console.Write(x);
        }

        public void PrintFireplace()
        {
            Console.WriteLine(YoCode.messages.fireplace);
        }

        public void LazinessEvidence()
        {
            Console.WriteLine("Project unmodified");
        }

        public void NothingInDirectory()
        {
            Console.WriteLine("Specified directory inaccessible");
        }
    }
}
