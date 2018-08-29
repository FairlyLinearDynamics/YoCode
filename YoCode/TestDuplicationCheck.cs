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
            var dupcheck = new DuplicationCheck(dir, dupFinder, p, testFile);
            dupcheck.OrigCodeBaseCost = OrigCodeBaseCost;
            dupcheck.OrigDuplicateCost = OrigDuplicateCost;

            dupcheck.PerformDuplicationCheck();

            TestDuplicationEvidence = dupcheck.DuplicationEvidence;
            TestDuplicationEvidence.FeatureTitle = "Duplication improvement: UnitConverterTests";
            TestDuplicationEvidence.FeatureRating = dupcheck.GetDuplicationCheckRating(OrigDuplicateCost, 0);

        }

        public FeatureEvidence TestDuplicationEvidence { get; } = new FeatureEvidence();

    }
}
