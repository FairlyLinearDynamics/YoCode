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
            var dupcheck = new DuplicationCheck(dir,dupFinder,p,testFile);
            TestDuplicationEvidence = dupcheck.DuplicationEvidence;
            TestDuplicationEvidence.FeatureTitle = "Test Code Duplication";
        }

        public FeatureEvidence TestDuplicationEvidence { get; } = new FeatureEvidence();

    }
}
