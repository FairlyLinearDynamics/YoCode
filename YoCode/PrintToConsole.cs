using System;
using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    public class PrintToConsole : IPrint
    {

        public PrintToConsole()
        {
            PrintIntroduction();
        }

        public void PrintIntroduction()
        {
            Console.WriteLine("Welcome to YoCode!");
            Console.WriteLine();
            Console.WriteLine();
        }


        public void PrintFinalResults(List<FeatureEvidence> featureList)
        {
            foreach(FeatureEvidence feature in featureList)
            {
                Console.WriteLine("==========================================");
                Console.Write($"{feature.FeatureTitle}: ");
                Console.WriteLine((feature.FeatureImplemented)?"Yes":"No");
                Console.WriteLine();
                if (feature.EvidencePresent)
                {
                    Console.WriteLine("Indication of above result: ");
                    Console.WriteLine(FormatEvidence(feature));
                    Console.WriteLine("==========================================");
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

            foreach(string err in errs)
            {
                Console.WriteLine(err);
            }
            Console.WriteLine("\nIf you would like to see list of commands, type: --help");
        }

        public void PrintDupfinderHelp()
        {          
            Console.WriteLine("\nTo run this application you will have to install Command Line Tools by Jetbrains\n" +
                "Direct download link here: https://www.jetbrains.com/resharper/download/download-thanks.html?platform=windows&code=RSCLT" +
                "After you downloaded it please specify its location in appsetting.json file, which lives in the root directory of this  project ");
        }


        public void PrintHelp()
        {
            PrintFireplace();
            Console.WriteLine("Application takes 2 parameters: path to original test directory and path to modified test directory" +
                $"\nPossible commands: --{CommandNames.ORIGIN}; --{CommandNames.MODIFIED}; --{CommandNames.HELP}" +
                $"\nExample use: --{CommandNames.ORIGIN}=<path-to-original-test> --{CommandNames.MODIFIED}=<path-to-modified-test>");
            PrintDupfinderHelp();
        }

        public void PrintFireplace()
        {
            Console.WriteLine("   ___________________________\r\n   _|__ _________________ __|__\r\n  _|___||               ||_|__\r\n  ___|_||       )  '    ||___|_\r\n   _|__||    ( ()\\(     ||_|___\r\n  ___|_||  ( ,|,(X)'    ||___#_\r\n  _|___|| /,)/|`\\``\\\\\\  |||__/\\\r\n       ''---------------''  /  `--#\r\n       . - ------------ . #/      |\r\n     (( (((  (( ))) ))))  )\\      |\r\n       `  -   ----  __ -/\\  `.__.-#\r\n                  C(__)`\\ \\____\r\n                      /_`\\/___/\n");
          
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
