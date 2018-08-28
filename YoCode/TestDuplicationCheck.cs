using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    class TestDuplicationCheck
    {
        public double OrigCodeBaseCost = 626;
        public double OrigDuplicateCost = 127;

        public double ModiCodeBaseCost = 585;
        public double ModiDuplicateCost = 43;

        private readonly string testFile = "UnitConverterTests\\UnitConverterTests.csproj";

        public TestDuplicationCheck(IPathManager dir,IDupFinder dupFinder,IRunParameterChecker p)
        {
            var dupcheck = new DuplicationCheck(dir,dupFinder,p,testFile);
            TestDuplicationEvidence = dupcheck.DuplicationEvidence;
            TestDuplicationEvidence.FeatureTitle = "Duplication improvement: UnitConverterTests";
            TestDuplicationEvidence.FeatureRating = GetTestDuplicationCheckRating();
        }

        public double GetTestDuplicationCheckRating()
        {
            double UpperBound = 127;
            double LowerBound = 0;
            double range = UpperBound - LowerBound;

            return ModiDuplicateCost >= UpperBound ? 0 : 1-Math.Round((ModiDuplicateCost - LowerBound) / range,2);
        }

        public FeatureEvidence TestDuplicationEvidence { get; } = new FeatureEvidence();

    }
}
