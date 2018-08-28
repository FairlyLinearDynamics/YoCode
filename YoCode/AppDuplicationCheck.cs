using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    class AppDuplicationCheck
    {
        private readonly string testFile = "UnitConverterWebApp.sln";

        public AppDuplicationCheck(IPathManager dir,IDupFinder dupFinder,IRunParameterChecker p)
        {
            var dupcheck = new DuplicationCheck(dir,dupFinder,p,testFile);
            AppDuplicationEvidence = dupcheck.DuplicationEvidence;
            AppDuplicationEvidence.FeatureTitle = "Application Code Duplication";
        }

        public FeatureEvidence AppDuplicationEvidence { get; } = new FeatureEvidence();

    }
}
