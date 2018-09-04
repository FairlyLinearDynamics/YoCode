
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YoCode
{
    internal class DuplicationCheckRunner : ICheck
    {
        private const string webAppFile = "UnitConverterWebApp\\UnitConverterWebApp.csproj";
        private const string testFile = "UnitConverterTests\\UnitConverterTests.csproj";

        private readonly CheckConfig checkConfig;

        public DuplicationCheckRunner(CheckConfig checkConfig)
        {
            IRunParameterChecker parameters = checkConfig.RunParameters;
            this.checkConfig = checkConfig;

            AppDuplicationEvidence = RunAppDuplicationCheck(webAppFile, Int32.Parse(parameters.AppCodeBaseCost), Int32.Parse(parameters.AppDuplicationCost));
            AppDuplicationEvidence.Feature = Feature.AppDuplicationCheck;

            TestDuplicationEvidence = RunAppDuplicationCheck(testFile, Int32.Parse(parameters.TestCodeBaseCost), Int32.Parse(parameters.TestDuplicationCost));
            TestDuplicationEvidence.Feature = Feature.TestDuplicationCheck;
        }

        private FeatureEvidence RunAppDuplicationCheck(string file, int origCodeBaseCost, int origDuplicateCost)
        {
            var dupFinder = new DupFinder(checkConfig.RunParameters.CMDToolsPath);

            var dupCheck = new DuplicationCheck(checkConfig.PathManager, dupFinder, file)
            {
                OrigCodeBaseCost = origCodeBaseCost,
                OrigDuplicateCost = origDuplicateCost
            };

            dupCheck.PerformDuplicationCheck();

            return dupCheck.DuplicationEvidence;
        }

        public Task<List<FeatureEvidence>> Execute()
        {
            // TODO Background
            return Task.FromResult(new List<FeatureEvidence>{ AppDuplicationEvidence, TestDuplicationEvidence});
        }

        private FeatureEvidence AppDuplicationEvidence { get; }
        private FeatureEvidence TestDuplicationEvidence { get; }
    }
}
