using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    class TestDuplicationCheck
    {
        private readonly string testFile = "UnitConverterTests\\UnitConverterTests.csproj";

        public TestDuplicationCheck(IPathManager dir,IDupFinder dupFinder,IRunParameterChecker p)
        {
            var dupcheck = new DuplicationCheck(dir, dupFinder, p, testFile);
            dupcheck.OrigCodeBaseCost = Int32.Parse(p.TestCodeBaseCost);
            dupcheck.OrigDuplicateCost = Int32.Parse(p.TestDuplicationCost);

            dupcheck.PerformDuplicationCheck();

            TestDuplicationEvidence = dupcheck.DuplicationEvidence;
            TestDuplicationEvidence.FeatureTitle = "Duplication improvement: UnitConverterTests";
            TestDuplicationEvidence.Feature = Feature.TestDuplicationCheck;
            TestDuplicationEvidence.FeatureRating = dupcheck.GetDuplicationCheckRating(dupcheck.OrigDuplicateCost, 0);
        }

        public FeatureEvidence TestDuplicationEvidence { get; } = new FeatureEvidence();
    }
}
