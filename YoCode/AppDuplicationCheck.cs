
using System;

namespace YoCode
{
    class AppDuplicationCheck
    {
        private readonly string webAppFile = "UnitConverterWebApp\\UnitConverterWebApp.csproj";
        private readonly string testFile = "UnitConverterTests\\UnitConverterTests.csproj";

        IRunParameterChecker p;
        IPathManager dir;
        IDupFinder dupFinder;

        public AppDuplicationCheck(IPathManager dir,IDupFinder dupFinder,IRunParameterChecker p)
        {
            this.dir = dir;
            this.dupFinder = dupFinder;
            this.p = p;

            AppDuplicationEvidence = RunAppDuplicationCheck(webAppFile,AppDuplicationEvidence,Int32.Parse(p.AppCodeBaseCost),Int32.Parse(p.AppDuplicationCost));
            AppDuplicationEvidence.FeatureTitle = "Duplication improvement: UnitConverterWebApp";
            AppDuplicationEvidence.Feature = Feature.AppDuplicationCheck;

            TestDuplicationEvidence = RunAppDuplicationCheck(testFile, TestDuplicationEvidence, Int32.Parse(p.TestCodeBaseCost), Int32.Parse(p.TestDuplicationCost));
            TestDuplicationEvidence.FeatureTitle = "Duplication improvement: UnitConverterTests";
            TestDuplicationEvidence.Feature = Feature.TestDuplicationCheck;
        }

        public FeatureEvidence RunAppDuplicationCheck(string webAppFile,FeatureEvidence evidence, int OrigCodeBaseCost,int OrigDuplicateCost)
        {
            var dupcheck = new DuplicationCheck(dir, dupFinder, p, webAppFile);
            dupcheck.OrigCodeBaseCost = OrigCodeBaseCost;
            dupcheck.OrigDuplicateCost = OrigDuplicateCost;
            dupcheck.PerformDuplicationCheck();

            evidence = dupcheck.DuplicationEvidence;

            evidence.FeatureRating = dupcheck.GetDuplicationCheckRating(dupcheck.OrigDuplicateCost, 0);

            return evidence;
        }

        public FeatureEvidence AppDuplicationEvidence { get; } = new FeatureEvidence();
        public FeatureEvidence TestDuplicationEvidence { get; } = new FeatureEvidence();

    }
}
