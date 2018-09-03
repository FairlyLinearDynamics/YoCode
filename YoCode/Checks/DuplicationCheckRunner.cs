﻿
using System;

namespace YoCode
{
    class DuplicationCheckRunner
    {
        private readonly string webAppFile = "UnitConverterWebApp\\UnitConverterWebApp.csproj";
        private readonly string testFile = "UnitConverterTests\\UnitConverterTests.csproj";

        IRunParameterChecker p;
        IPathManager dir;
        IDupFinder dupFinder;

        public DuplicationCheckRunner(IPathManager dir,IDupFinder dupFinder,IRunParameterChecker p)
        {
            this.dir = dir;
            this.dupFinder = dupFinder;
            this.p = p;

            AppDuplicationEvidence = RunAppDuplicationCheck(webAppFile,Int32.Parse(p.AppCodeBaseCost),Int32.Parse(p.AppDuplicationCost));
            SetFeatureAndTitle(AppDuplicationEvidence, "Duplication improvement: UnitConverterWebApp", Feature.AppDuplicationCheck);

            TestDuplicationEvidence = RunAppDuplicationCheck(testFile,Int32.Parse(p.TestCodeBaseCost), Int32.Parse(p.TestDuplicationCost));
            SetFeatureAndTitle(TestDuplicationEvidence, "Duplication improvement: UnitConverterTests", Feature.TestDuplicationCheck);
        }

        public FeatureEvidence RunAppDuplicationCheck(string file, int OrigCodeBaseCost,int OrigDuplicateCost)
        {
            var dupcheck = new DuplicationCheck(dir, dupFinder,file);
            dupcheck.OrigCodeBaseCost = OrigCodeBaseCost;
            dupcheck.OrigDuplicateCost = OrigDuplicateCost;
            dupcheck.PerformDuplicationCheck();

            return dupcheck.DuplicationEvidence;
        }

        public void SetFeatureAndTitle(FeatureEvidence evidence,string title,Feature feature)
        {
            evidence.FeatureTitle = title;
            evidence.Feature = feature;
        }

        public FeatureEvidence AppDuplicationEvidence { get; set; } = new FeatureEvidence();
        public FeatureEvidence TestDuplicationEvidence { get; set; } = new FeatureEvidence();

    }
}