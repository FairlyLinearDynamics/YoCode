using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    class TestResultFormater
    {
        TestResults results;

        public TestResultFormater(TestResults results)
        {
            this.results = results;
        }

        public string UICheckResult => (results.UiCheck) ? "Yes" : "No";
        public string GitUsedResult => (results.GitUsed) ? "Yes" : "No";
        public string SolutionFileExistResult => (results.SolutionFileExist) ? "Yes" : "No";

        public List<int> UIEvidence { get=>results.Lines; }
    }
}
