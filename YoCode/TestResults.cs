using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    public class TestResults
    {
        // ============================================================ Directory evidence

        public bool AnyFileChanged { get; set; }
        public FeatureEvidence AnyFileChangedEvidence { get; set; }

        public bool WrongDirectory { get; set; }
        public FeatureEvidence WrongDirectoryEvidence { get; set; }

        // ============================================================ UI evidence

        public bool UICheckExists { get; set; }
        public FeatureEvidence UICheckExistsEvidence { get; set; }

        // ============================================================ Git evidence

        public bool GitUsed { get; set; }
        public FeatureEvidence GitUsedEvidence { get; set; }

        // ============================================================

        public bool SolutionFileExists { get; set; }
        public FeatureEvidence SolutionFileExistsEvidence { get; set; }

        // ============================================================
    }
}