using System;

namespace YoCode
{
    public class PrintToConsole : IPrint
    {
        TestResults results;

        public void PrintIntroduction()
        {
            Console.WriteLine("Welcome to the YoCode, the best code checkup software on the marker!");
            Console.WriteLine();
            Console.WriteLine();
        }

        public void PrintFinalResults(TestResults results)
        {
            this.results = results;
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
            Console.Write(results.GitUsedResult() + "\n");
        }

        private void SolutionFileFoundResult()
        {
            Console.Write("Solution file found: ");
            Console.Write(results.SolutionFileExistResult() + "\n");
        }

        private void PrintUIEvidenceResult()
        {
            Console.Write("Feature evidence in UI: ");
            Console.Write(results.UiCheckResult()+"\n");
        }

        private void LazinessEvidence()
        {
            Console.Write("Project unmodified\n");
        }
        // Possibly will need to add more print methods to corespond to 
        // Performed tests.
    }
}
