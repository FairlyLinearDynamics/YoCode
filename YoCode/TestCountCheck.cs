using System.Collections.Generic;

namespace YoCode
{
    public class TestCountCheck
    {
        private readonly string processName;
        private readonly string workingDir;
        private readonly string arguments;

        public string StatLine { get; set; }
        public string Output { get; set; }

        private TestStats stats;
        private List<int> tempStats;

        public TestCountCheck(string repositoryPath)
        {
            processName = "dotnet";
            workingDir = repositoryPath;
            arguments = "test";
        }

        public void ExecuteTheCheck()
        {
            ProcessRunner pr = new ProcessRunner(processName, workingDir, arguments);
            pr.ExecuteTheCheck();
            Output = pr.Output;
            StatLine = Output.GetLineWithAllKeywords(GetTestKeyWords());
            tempStats = StatLine.GetNumbersInALine();
            StoreCalculations(tempStats);
        }

        public void StoreCalculations(List<int> tempStats)
        {
            stats.totalTests = tempStats[0];
            stats.testsPassed = tempStats[1];
            stats.testsFailed = tempStats[2];
            stats.testsSkipped = tempStats[3];
            
        }

        public static List<string> GetTestKeyWords()
        {
            return new List<string> { "Total tests:" };
        }
    }
}
