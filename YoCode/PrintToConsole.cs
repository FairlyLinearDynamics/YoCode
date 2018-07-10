using System;
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
            Console.WriteLine("==========================================");
            Console.Write("Git used: ");
            Console.WriteLine(resultsFormatter.GitUsedResult);
            Console.WriteLine("Indication of above result: ");
            Console.WriteLine(resultsFormatter.GitUsedResultEvidence);
            Console.WriteLine("==========================================");
        }

        private void SolutionFileFoundResult()
        {
            Console.WriteLine("==========================================");
            Console.Write("Solution file found: ");
            Console.WriteLine(resultsFormatter.SolutionFileExistResult);
            Console.WriteLine("==========================================");
        }

        private void PrintUIEvidenceResult()
        {
            Console.WriteLine("==========================================");
            Console.Write("Feature evidence in UI: ");
            Console.WriteLine(resultsFormatter.UICheckResult);
            Console.WriteLine("Indication of above result: ");
            Console.WriteLine(resultsFormatter.UICheckResultEvidence);
            Console.WriteLine("==========================================");
        }

        private void PrintWrongDirectory()
        {
            Console.WriteLine("Invalid directory");
        }

        private static void LazinessEvidence()
        {
            Console.WriteLine("Project unmodified");
        }
        // Possibly will need to add more print methods to corespond to 
        // Performed tests.
    }
}
