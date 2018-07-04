using System;

namespace YoCode
{
    public class PrintToConsole : IPrint
    {
        TestResults results;

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
            Console.Write(results.anyFileChanged+"\n");
        }

        private void PrintSolutionFileResult()
        {
            Console.Write("Solution file was found: ");
            Console.Write(results.solutionExists+"\n");
        }

        private void PrintUIEvidenceResult()
        {
            Console.Write("Feature evidence in UI: ");
            Console.Write(results.uiCheck+"\n");
        }

        // Possibly will need to add more print methods to corespond to 
        // Performed tests.
    }
}
