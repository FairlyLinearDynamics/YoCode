using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    class TestDuplicationCheck
    {
        private int OrigCodeBaseCost = 626;
        private int OrigDuplicateCost = 127;

        private readonly string testFile = "UnitConverterTests\\UnitConverterTests.csproj";

        public TestDuplicationCheck(IPathManager dir,IDupFinder dupFinder,IRunParameterChecker p)
        {
            OrigCodeBaseCost = Int32.Parse(p.TestCodeBaseCost);
            OrigDuplicateCost = Int32.Parse(p.TestDuplicationCost);

            var dupcheck = new DuplicationCheck(dir, dupFinder, p, testFile);
            dupcheck.OrigCodeBaseCost = OrigCodeBaseCost;
            dupcheck.OrigDuplicateCost = OrigDuplicateCost;

            dupcheck.PerformDuplicationCheck();

            TestDuplicationEvidence = dupcheck.DuplicationEvidence;
            TestDuplicationEvidence.FeatureTitle = "Duplication improvement: UnitConverterTests";
            TestDuplicationEvidence.Feature = Feature.TestDuplicationCheck;
            TestDuplicationEvidence.FeatureRating = dupcheck.GetDuplicationCheckRating(OrigDuplicateCost, 0);
        }

        public FeatureEvidence TestDuplicationEvidence { get; } = new FeatureEvidence();

    }
}
