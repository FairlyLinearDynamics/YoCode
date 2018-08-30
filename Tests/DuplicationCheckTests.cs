using Xunit;
using FluentAssertions;
using System.IO;
using YoCode;
using Moq;

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
            var mockRunCheck = new Mock<IRunParameterChecker>();

            const string fakeModified = @"\fake\modified\dir";

            const string fakeModifiedCodeScore = "<CodebaseCost>10 <TotalDuplicatesCost>10";

            const string fileNameChecked =  "UnitConverterWebApp\\UnitConverterWebApp.csproj";

            mockDir.Setup(w => w.ModifiedTestDirPath).Returns(fakeModified);

            mockRunCheck.Setup(w => w.AppCodeBaseCost).Returns("69");
            mockRunCheck.Setup(w => w.AppDuplicationCost).Returns("0");
            mockRunCheck.Setup(w => w.TestCodeBaseCost).Returns("69");
            mockRunCheck.Setup(w => w.TestDuplicationCost).Returns("0");

            mockDupFinder.Setup(w => w.Execute(It.IsAny<string>(), Path.Combine(fakeModified, fileNameChecked)))
                .Returns(SetUpFeatureEvidence(fakeModifiedCodeScore));

            fakeDir = mockDir.Object;
            fakeDupFinder = mockDupFinder.Object;
            fakeRunCheck = mockRunCheck.Object;

            dupCheck = new DuplicationCheck(fakeDir, fakeDupFinder, fakeRunCheck,fileNameChecked);

            dupCheck.DuplicationEvidence.FeatureImplemented.Should()
                .BeTrue($"Feature implemented: {dupCheck.DuplicationEvidence.FeatureImplemented}, " +
                $"Feature evidence: {dupCheck.DuplicationEvidence.Evidence}");
        }
    }
}