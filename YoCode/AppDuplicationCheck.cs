
namespace YoCode
{
    class AppDuplicationCheck
    {
        public int OrigCodeBaseCost = 1755;
        public int OrigDuplicateCost = 388;

        private readonly string testFile = "UnitConverterWebApp\\UnitConverterWebApp.csproj";

        public AppDuplicationCheck(IPathManager dir,IDupFinder dupFinder,IRunParameterChecker p)
        {
            var dupcheck = new DuplicationCheck(dir, dupFinder, p, testFile);

            dupcheck.OrigCodeBaseCost = OrigCodeBaseCost;
            dupcheck.OrigDuplicateCost = OrigDuplicateCost;

            dupcheck.PerformDuplicationCheck();

            AppDuplicationEvidence = dupcheck.DuplicationEvidence;
            AppDuplicationEvidence.FeatureTitle = "Duplication improvement: UnitConverterWebApp";
            AppDuplicationEvidence.FeatureRating = dupcheck.GetDuplicationCheckRating(OrigDuplicateCost, 0);
        }

        public FeatureEvidence AppDuplicationEvidence { get; } = new FeatureEvidence();

    }
}
