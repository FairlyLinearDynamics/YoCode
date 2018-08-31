using Xunit;
using FluentAssertions;
using System.IO;
using YoCode;
using Moq;
using System.Text;

namespace YoCode_XUnit
{
    public class DuplicationCheckTests
    {
        private FeatureEvidence SetUpFeatureEvidence(string outputToBeSet)
        {
            return new FeatureEvidence()
            {
                Output = outputToBeSet,
                FeatureImplemented = true,
            };
        }

        [Fact]
        public void DuplicationCheck_FeatureImplemented_TRUE()
        {
            IPathManager fakeDir;
            IDupFinder fakeDupFinder;
            IRunParameterChecker fakeRunCheck;
            DuplicationCheck dupCheck;

            var mockDir = new Mock<IPathManager>();
            var mockDupFinder = new Mock<IDupFinder>();

            const string fakeModified = @"\fake\modified\dir";

            StringBuilder fakeModifiedCodeScore = new StringBuilder();
            fakeModifiedCodeScore.Append("<CodebaseCost>0");
            fakeModifiedCodeScore.Append("TotalDuplicatesCost>10");

            const string fileNameChecked =  "UnitConverterWebApp\\UnitConverterWebApp.csproj";

            mockDir.Setup(w => w.ModifiedTestDirPath).Returns(fakeModified);

            mockDir.Setup(w => w.ModifiedTestDirPath).Returns(fakeModified);

            mockDupFinder.Setup(w => w.Execute(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new FeatureEvidence()
                {
                    FeatureImplemented = true,
                    Output = fakeModifiedCodeScore.ToString()
                });

            fakeDir = mockDir.Object;
            fakeDupFinder = mockDupFinder.Object;

            dupCheck = new DuplicationCheck(fakeDir, fakeDupFinder,fileNameChecked);

            dupCheck.PerformDuplicationCheck();

            dupCheck.DuplicationEvidence.FeatureImplemented.Should()
                .BeTrue($"Feature implemented: {dupCheck.DuplicationEvidence.FeatureImplemented}, " +
                $"Feature evidence: {dupCheck.DuplicationEvidence.Evidence}");
        }
    }
}