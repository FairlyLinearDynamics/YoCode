
using System;

namespace YoCode
{
    class AppDuplicationCheck
    {
        private readonly string testFile = "UnitConverterWebApp\\UnitConverterWebApp.csproj";

        public AppDuplicationCheck(IPathManager dir,IDupFinder dupFinder,IRunParameterChecker p)
        {
            var dupcheck = new DuplicationCheck(dir, dupFinder, p, testFile);

            dupcheck.OrigCodeBaseCost = Int32.Parse(p.AppCodeBaseCost);
            dupcheck.OrigDuplicateCost = Int32.Parse(p.AppDuplicationCost);

            dupcheck.PerformDuplicationCheck();

            AppDuplicationEvidence = dupcheck.DuplicationEvidence;
            AppDuplicationEvidence.FeatureTitle = "Duplication improvement: UnitConverterWebApp";
            AppDuplicationEvidence.Feature = Feature.AppDuplicationCheck;
            AppDuplicationEvidence.FeatureRating = dupcheck.GetDuplicationCheckRating(dupcheck.OrigDuplicateCost, 0);
        }

        public FeatureEvidence AppDuplicationEvidence { get; } = new FeatureEvidence();
    }
}
