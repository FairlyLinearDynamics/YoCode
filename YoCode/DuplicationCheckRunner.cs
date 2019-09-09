
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoCode
{
    internal class DuplicationCheckRunner : ICheck
    {
        private readonly CheckConfig checkConfig;

        public DuplicationCheckRunner(CheckConfig checkConfig)
        {
            this.checkConfig = checkConfig;
        }

        private static Task<FeatureEvidence> DuplicationCheck(CheckConfig checkConfig, int origCodeBaseCost, int origDuplicateCost, Feature duplicationCheck, string project)
        {
            return Task.Run(() =>
            {
                var duplicationEvidence = RunDuplicationCheck(checkConfig, project, origCodeBaseCost, origDuplicateCost);
                duplicationEvidence.Feature = duplicationCheck;
                return duplicationEvidence;
            });
        }

        private static FeatureEvidence RunDuplicationCheck(CheckConfig checkConfig, string file, int origCodeBaseCost, int origDuplicateCost)
        {
            var dupFinder = new DupFinder(checkConfig.RunParameters.DupFinderPath);

            var dupCheck = new DuplicationCheck(checkConfig.PathManager, dupFinder, file)
            {
                OrigCodeBaseCost = origCodeBaseCost,
                OrigDuplicateCost = origDuplicateCost
            };

            dupCheck.PerformDuplicationCheck();

            return dupCheck.DuplicationEvidence;
        }

        public async Task<List<FeatureEvidence>> Execute()
        {
            var parameters = checkConfig.RunParameters;

            const string appProject = "UnitConverterWebApp\\UnitConverterWebApp.csproj";
            var appCodeBaseCost = int.Parse(parameters.AppCodeBaseCost);
            var appDuplicateCost = int.Parse(parameters.AppDuplicationCost);

            var appEvidence = DuplicationCheck(checkConfig, appCodeBaseCost, appDuplicateCost, Feature.AppDuplicationCheck, appProject);

            const string testProject = "UnitConverterTests\\UnitConverterTests.csproj";
            var testCodeBaseCost = int.Parse(parameters.TestCodeBaseCost);
            var testDuplicateCost = int.Parse(parameters.TestDuplicationCost);

            var testEvidence = DuplicationCheck(checkConfig, testCodeBaseCost, testDuplicateCost, Feature.TestDuplicationCheck, testProject);

            return (await Task.WhenAll(appEvidence, testEvidence)).ToList();
        }
    }
}
