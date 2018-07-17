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

        const int TEST_PASS_THRESHOLD = 80;

        public TestCountCheck(string repositoryPath)
        {
            TestCountEvidence.FeatureTitle = "Tests";
            processName = "dotnet";
            workingDir = repositoryPath;
            arguments = "test";
            ExecuteTheCheck();
        }

        public void ExecuteTheCheck()
        {
            ProcessRunner pr = new ProcessRunner(processName, workingDir, arguments);
            pr.ExecuteTheCheck();
            Output = pr.Output;
            StatLine = Output.GetLineWithAllKeywords(GetTestKeyWords());
            tempStats = StatLine.GetNumbersInALine();
            StoreCalculations(tempStats);

            TestCountEvidence.FeatureImplemented = stats.percentagePassed > TEST_PASS_THRESHOLD;

            TestCountEvidence.GiveEvidence($"Tests passed: {stats.testsPassed}\nTests failed: {stats.testsFailed}\nTests skipped: {stats.testsSkipped}" +
                $"\nTotal number of Tests: {stats.totalTests}\nTests passed(%): {stats.percentagePassed}");
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

        public FeatureEvidence TestCountEvidence { get; private set; } = new FeatureEvidence();
    }
}
