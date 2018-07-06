using System;
using Xunit;
using FluentAssertions;
using System.IO;
using YoCode;

namespace YoCode_XUnit
{
    public class PrintToConsoleTests
    {
        TextWriter testOutput = new StringWriter();
        TestResults results = new TestResults();
        PrintToConsole consolePrinter = new PrintToConsole();

        public PrintToConsoleTests()
        {
            Console.SetOut(testOutput);
        }

        [Fact]
        public void PrintToConsole_PrintFilesChangedResult_Correct()
        {
            results.AnyFileChanged = true;
            consolePrinter.PrintFinalResults(results);
            String expectedOutput = "Any files changed: Yes\nSolution file was found: No\nFeature evidence in UI: No\n";


            testOutput.ToString().Should().Be(expectedOutput);
        }
        [Fact]
        public void PrintToConsole_PrintFilesChangedResult_Incorrect()
        {
            results.AnyFileChanged = false;
            consolePrinter.PrintFinalResults(results);
            String expectedOutput = "Any files changed: No\nSolution file was found: No\nFeature evidence in UI: No\n";

            testOutput.ToString().Should().Be(expectedOutput);
        }

        [Fact]
        public void PrintToConsole_PrintSolutionFileResult_Correct()
        {
            results.GitUsed = true;
            consolePrinter.PrintFinalResults(results);
            String expectedOutput = "Any files changed: No\nSolution file was found: Yes\nFeature evidence in UI: No\n";

            testOutput.ToString().Should().Be(expectedOutput);
        }
        [Fact]
        public void PrintToConsole_PrintSolutionFileResult_Incorrect()
        {
            results.GitUsed = false;
            consolePrinter.PrintFinalResults(results);
            String expectedOutput = "Any files changed: No\nSolution file was found: No\nFeature evidence in UI: No\n";

            testOutput.ToString().Should().Be(expectedOutput);
        }

        [Fact]
        public void PrintToConsole_PrintUIEvidenceResult_Correct()
        {
            results.UiCheck = true;
            consolePrinter.PrintFinalResults(results);
            String expectedOutput = "Any files changed: No\nSolution file was found: No\nFeature evidence in UI: Yes\n";

            testOutput.ToString().Should().Be(expectedOutput);
        }
        [Fact]
        public void PrintToConsole_PrintUIEvidenceResult_Incorrect()
        {
            results.UiCheck = false;
            consolePrinter.PrintFinalResults(results);
            String expectedOutput = "Any files changed: No\nSolution file was found: No\nFeature evidence in UI: No\n";

            testOutput.ToString().Should().Be(expectedOutput);
        }
    }
}
