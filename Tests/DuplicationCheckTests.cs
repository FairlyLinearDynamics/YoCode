using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using System.IO;
using YoCode;
using System.Text;
using Moq;
using System.Collections.Generic;

namespace YoCode_XUnit
{
    public class DuplicationCheckTests
    {
        IPathManager fakeDir;
        IDupFinder fakeDupFinder;

        private readonly ITestOutputHelper _output;

        public DuplicationCheckTests(ITestOutputHelper output)
        {
            var mockDir = new Mock<IPathManager>();
            var mockDupFinder = new Mock<IDupFinder>();

            var fakeOriginal = @"\fake\original\dir";
            var fakeModified = @"\fake\modified\dir";

            var fileNameChecked = "UnitConverterWebApp.sln";

            mockDir.Setup(w => w.originalTestDirPath).Returns(fakeOriginal);
            mockDir.Setup(w => w.modifiedTestDirPath).Returns(fakeModified);

            mockDupFinder.Setup(w => w.Execute(It.IsAny<string>(),Path.Combine(fakeOriginal,fileNameChecked))).Returns(new FeatureEvidence()
            {
                Output = "<CodebaseCost>50 <TotalDuplicatesCost>50",
            });

            mockDupFinder.Setup(w => w.Execute(It.IsAny<string>(),Path.Combine(fakeModified,fileNameChecked))).Returns(new FeatureEvidence()
            {
                Output = "<CodebaseCost>10 <TotalDuplicatesCost>10",
            });

            _output = output;

            fakeDir = mockDir.Object;
            fakeDupFinder = mockDupFinder.Object;
        }

        [Fact]
        public void DuplicationCheck_FeatureImplemented_TRUE()
        {
            var dupCheck = new DuplicationCheck(fakeDir, fakeDupFinder);
            foreach(var evidence in dupCheck.DuplicationEvidence.Evidence)
            {
                _output.WriteLine(evidence);
            }

            dupCheck.DuplicationEvidence.FeatureImplemented.Should().BeTrue();
        }

        
    }
}
