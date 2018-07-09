using System;
using System.Linq;

namespace YoCode
{
    public class PrintToConsole : IPrint
    {
        TestResultFormater results;

        public void PrintIntroduction()
        {
            Console.WriteLine("Welcome to the YoCode, the best code checkup software on the marker!");
            Console.WriteLine();
            Console.WriteLine();
        }

        public void PrintFinalResults(TestResults results)
        {
            this.results = new TestResultFormater(results);
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
            Console.WriteLine(results.GitUsedResult);
        }

        private void SolutionFileFoundResult()
        {
            Console.Write("Solution file found: ");
            Console.WriteLine(results.SolutionFileExistResult);
        }

        private void PrintUIEvidenceResult()
        {
            Console.Write("Feature evidence in UI: ");
            Console.WriteLine(results.UICheckResult);
            if (results.UIEvidence.Any())
            {
                Console.Write("Found on lines: ");
                foreach (int line in results.UIEvidence)
                {
                    Console.Write(line+" ");
                }
                Console.WriteLine();
            }
        }

        private void LazinessEvidence()
        {
            Console.WriteLine("Project unmodified");
        }
        // Possibly will need to add more print methods to corespond to 
        // Performed tests.
    }
}
