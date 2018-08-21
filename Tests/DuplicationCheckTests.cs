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
            };
        }

        [Fact]
        public void DuplicationCheck_FeatureImplemented_TRUE()
        {
            IPathManager fakeDir;
            IDupFinder fakeDupFinder;
            DuplicationCheck dupCheck;

            var mockDir = new Mock<IPathManager>();
            var mockDupFinder = new Mock<IDupFinder>();

            const string fakeModified = @"\fake\modified\dir";

            const string fakeModifiedCodeScore = "<CodebaseCost>10 <TotalDuplicatesCost>10";

            const string fileNameChecked = "UnitConverterWebApp.sln";

            mockDir.Setup(w => w.ModifiedTestDirPath).Returns(fakeModified);

            mockDupFinder.Setup(w => w.Execute(It.IsAny<string>(), Path.Combine(fakeModified, fileNameChecked)))
                .Returns(SetUpFeatureEvidence(fakeModifiedCodeScore));

            fakeDir = mockDir.Object;
            fakeDupFinder = mockDupFinder.Object;

            dupCheck = new DuplicationCheck(fakeDir, fakeDupFinder, true);

            dupCheck.DuplicationEvidence.FeatureImplemented.Should()
                .BeTrue($"Feature implemented: {dupCheck.DuplicationEvidence.FeatureImplemented}, " +
                $"Feature evidence: {dupCheck.DuplicationEvidence.Evidence}");
        }
    }
}
