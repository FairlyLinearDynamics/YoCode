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
        public string UICheckResultEvidence => FormatEvidence(results.UICheckExistsEvidence);

        public string GitUsedResult => (results.GitUsed) ? "Yes" : "No";
        public string GitUsedResultEvidence => FormatEvidence(results.GitUsedEvidence);

        public string SolutionFileExistResult => (results.SolutionFileExists) ? "Yes" : "No";

        //public IEnumerable<int> UIEvidence => results.Lines;

        public string FormatEvidence(FeatureEvidence evidence)
        {
            return (evidence.EvidencePresent) ?
                evidence.Evidence.Aggregate((a, b) => $"\n{a}\n{b}")
                : "No evidence present";
        }

    }
}
