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

        [Fact]
        public void DuplicationCheck_Should_Set_Inconclusive_DueToNoDupfinderOutput()
        {
            IPathManager fakeDir;
            IDupFinder fakeDupFinder;
            DuplicationCheck dupCheck;

            var mockDir = new Mock<IPathManager>();
            var mockDupFinder = new Mock<IDupFinder>();

            const string fakeModified = @"\fake\modified\dir";

            const string fileNameChecked =  "UnitConverterWebApp\\UnitConverterWebApp.csproj";

            mockDir.Setup(w => w.ModifiedTestDirPath).Returns(fakeModified);


            fakeDir = mockDir.Object;
            fakeDupFinder = mockDupFinder.Object;

            dupCheck = new DuplicationCheck(fakeDir, fakeDupFinder,fileNameChecked);

            dupCheck.PerformDuplicationCheck();

            dupCheck.DuplicationEvidence.Inconclusive.Should()
                .BeTrue($"Feature implemented: {dupCheck.DuplicationEvidence.Passed}, " +
                $"Feature evidence: {dupCheck.DuplicationEvidence.Evidence}");
        }
    }
}