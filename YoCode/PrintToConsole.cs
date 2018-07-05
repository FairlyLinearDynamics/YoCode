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
            PrintFilesChangedResult();
            PrintSolutionFileResult();
            PrintUIEvidenceResult();
        }

        private void PrintFilesChangedResult()
        {
            Console.Write("Any files changed: ");
            Console.Write(results.AnyFileChangedResult()+"\n");
        }

        private void PrintSolutionFileResult()
        {
            Console.Write("Solution file was found: ");
            Console.Write(results.SolutionExistsResult()+"\n");
        }

        private void PrintUIEvidenceResult()
        {
            Console.Write("Feature evidence in UI: ");
            Console.Write(results.UiCheckResult()+"\n");
        }

        // Possibly will need to add more print methods to corespond to 
        // Performed tests.
    }
}
