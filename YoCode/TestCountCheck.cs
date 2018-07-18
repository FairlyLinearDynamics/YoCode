using System;
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
        private readonly IFeatureRunner featureRunner;

        public TestCountCheck(string repositoryPath, IFeatureRunner featureRunner)
        {
            UnitTestEvidence.FeatureTitle = "All unit tests have passed";
            processName = "dotnet";
            workingDir = repositoryPath;
            arguments = "test";
            this.featureRunner = featureRunner;
            ExecuteTheCheck();
        }

        public void ExecuteTheCheck()
        {
            var pr = new ProcessDetails(processName, workingDir, arguments);
            var evidence = featureRunner.Execute(pr, "Unit test check");
            if (evidence.FeatureFailed)
            {
                return;
            }

            Output = evidence.Output;
            StatLine = Output.GetLineWithAllKeywords(GetTestKeyWords());
            tempStats = StatLine.GetNumbersInALine();
            StoreCalculations(tempStats);

            UnitTestEvidence.FeatureImplemented = stats.percentagePassed == 100 && stats.totalTests > 10 ? true :false;
            UnitTestEvidence.GiveEvidence(StatLine);
            UnitTestEvidence.GiveEvidence("Percentage: "+ (stats.percentagePassed).ToString());

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

        public FeatureEvidence UnitTestEvidence { get; } = new FeatureEvidence();
    }
}
