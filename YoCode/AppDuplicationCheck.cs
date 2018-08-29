
using System;

namespace YoCode
{
    class AppDuplicationCheck
    {
        private readonly string webAppFile = "UnitConverterWebApp\\UnitConverterWebApp.csproj";
        FeatureEvidence AppEvidence;
        FeatureEvidence TestEvidence;
        IRunParameterChecker p;
        IPathManager dir;
        IDupFinder dupFinder;

        public AppDuplicationCheck(IPathManager dir,IDupFinder dupFinder,IRunParameterChecker p)
        {
            this.dir = dir;
            this.dupFinder = dupFinder;
            this.p = p;

            AppEvidence = RunAppDuplicationCheck(webAppFile);
        }

        public FeatureEvidence RunAppDuplicationCheck(string webAppFile)
        {
            var dupcheck = new DuplicationCheck(dir, dupFinder, p, webAppFile);
            dupcheck.OrigCodeBaseCost = Int32.Parse(p.AppCodeBaseCost);
            dupcheck.OrigDuplicateCost = Int32.Parse(p.AppDuplicationCost);
            dupcheck.PerformDuplicationCheck();

            var evidence = dupcheck.DuplicationEvidence;
            evidence.FeatureTitle = "Duplication improvement: UnitConverterWebApp";
            evidence.Feature = Feature.AppDuplicationCheck;
            evidence.FeatureRating = dupcheck.GetDuplicationCheckRating(dupcheck.OrigDuplicateCost, 0);

            return evidence;

        }

        public FeatureEvidence RunTestDuplicationCheck(string testFile)
        {
            var dupcheck = new DuplicationCheck(dir, dupFinder, p, testFile);
            dupcheck.OrigCodeBaseCost = Int32.Parse(p.TestCodeBaseCost);
            dupcheck.OrigDuplicateCost = Int32.Parse(p.TestDuplicationCost);
            dupcheck.PerformDuplicationCheck();

            var evidence = dupcheck.DuplicationEvidence;
            evidence.FeatureTitle = "Duplication improvement: UnitConverterTests";
            evidence.Feature = Feature.AppDuplicationCheck;
            evidence.FeatureRating = dupcheck.GetDuplicationCheckRating(dupcheck.OrigDuplicateCost, 0);

            return evidence;
        }






        public FeatureEvidence AppDuplicationEvidence { get; } = new FeatureEvidence();
    }
}
