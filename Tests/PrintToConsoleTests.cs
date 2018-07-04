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
            results.anyFileChanged = true;
            consolePrinter.PrintFinalResults(results);
            String expectedOutput = "Any files changed: True\nSolution file was found: False\nFeature evidence in UI: False\n";


            testOutput.ToString().Should().Be(expectedOutput);
        }
        [Fact]
        public void PrintToConsole_PrintFilesChangedResult_Incorrect()
        {
            results.anyFileChanged = false;
            consolePrinter.PrintFinalResults(results);
            String expectedOutput = "Any files changed: False\nSolution file was found: False\nFeature evidence in UI: False\n";

            testOutput.ToString().Should().Be(expectedOutput);
        }

        [Fact]
        public void PrintToConsole_PrintSolutionFileResult_Correct()
        {
            results.solutionExists = true;
            consolePrinter.PrintFinalResults(results);
            String expectedOutput = "Any files changed: False\nSolution file was found: True\nFeature evidence in UI: False\n";

            testOutput.ToString().Should().Be(expectedOutput);
        }
        [Fact]
        public void PrintToConsole_PrintSolutionFileResult_Incorrect()
        {
            results.solutionExists = false;
            consolePrinter.PrintFinalResults(results);
            String expectedOutput = "Any files changed: False\nSolution file was found: False\nFeature evidence in UI: False\n";

            testOutput.ToString().Should().Be(expectedOutput);
        }

        [Fact]
        public void PrintToConsole_PrintUIEvidenceResult_Correct()
        {
            results.uiCheck = true;
            consolePrinter.PrintFinalResults(results);
            String expectedOutput = "Any files changed: False\nSolution file was found: False\nFeature evidence in UI: True\n";

            testOutput.ToString().Should().Be(expectedOutput);
        }
        [Fact]
        public void PrintToConsole_PrintUIEvidenceResult_Incorrect()
        {
            results.uiCheck = false;
            consolePrinter.PrintFinalResults(results);
            String expectedOutput = "Any files changed: False\nSolution file was found: False\nFeature evidence in UI: False\n";

            testOutput.ToString().Should().Be(expectedOutput);
        }
    }
}
