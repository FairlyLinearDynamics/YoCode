using System;
using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    public class PrintToConsole : IPrint
    {
        TestResultFormater resultsFormatter;

        public void PrintIntroduction()
        {
            Console.WriteLine("Welcome to the YoCode!");
            Console.WriteLine();
            Console.WriteLine();
        }

        public void PrintFinalResults(TestResults results)
        {
            resultsFormatter = new TestResultFormater(results);
            if(results.WrongDirectory)
            {
                PrintWrongDirectory();
                return;
            }
            if (!results.AnyFileChanged)
            {
                LazinessEvidence();
            }
            else
            {
                PrintGitResult();
                PrintUIEvidenceResult();
                SolutionFileFoundResult();
            }
        }

        private void PrintGitResult()
        {
            Console.Write("Git used: ");
            Console.WriteLine(resultsFormatter.GitUsedResult);
        }

        private void SolutionFileFoundResult()
        {
            Console.Write("Solution file found: ");
            Console.WriteLine(resultsFormatter.SolutionFileExistResult);
        }

        private void PrintUIEvidenceResult()
        {
            Console.Write("Feature evidence in UI: ");
            Console.WriteLine(resultsFormatter.UICheckResult);
        }

        private void PrintWrongDirectory()
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

        private static void LazinessEvidence()
        {
            Console.WriteLine("Project unmodified");
        }
    }
}
