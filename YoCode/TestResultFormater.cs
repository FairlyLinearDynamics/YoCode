using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace YoCode
{
    class TestResultFormater
    {
        private readonly TestResults results;

        public TestResultFormater(TestResults results)
        {
            this.results = results;
        }

        public string UICheckResult => (results.UICheckExists) ? "Yes" : "No";
        public string UICheckResultEvidence => FormatUIEvidence(results.UICheckExistsEvidence.Evidence);

        public string GitUsedResult => (results.GitUsed) ? "Yes" : "No";

        public string SolutionFileExistResult => (results.SolutionFileExist) ? "Yes" : "No";

        //public IEnumerable<int> UIEvidence => results.Lines;

        public string FormatUIEvidence(List<string> evidence)
        {
            return evidence.Aggregate((a, b) => $"\n{a}\n{b}");
        }
    }
}
