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
        IFeatureRunner fakeFeatureRunner;
        string fakeCMD;

        private readonly ITestOutputHelper _output;

        string fakeOriginal = @"\fake\original\dir";
        string fakeModified = @"\fake\modified\dir";

        public DuplicationCheckTests(ITestOutputHelper output)
        {
            var mockDir = new Mock<IPathManager>();
            var mockFeatureRunner = new Mock<IFeatureRunner>();
            fakeCMD = @"C:\Users\ukekar\source\repos\Tools\CMD";

            mockDir.Setup(w => w.originalTestDirPath).Returns(fakeOriginal);
            mockDir.Setup(w => w.modifiedTestDirPath).Returns(fakeModified);

            mockFeatureRunner.Setup(w => w.Execute(It.IsAny<ProcessDetails>(),It.IsAny<string>())).Returns(new FeatureEvidence()
            {
                Output = "<CodebaseCost>50</CodebaseCost>"
            });

            _output = output;

            fakeDir = mockDir.Object;
            fakeFeatureRunner = mockFeatureRunner.Object;
        }

        [Fact]
        public void DuplicationCheck_FeatureImplemented_TRUE()
        {
            var dupCheck = new DuplicationCheck(fakeDir, fakeCMD, fakeFeatureRunner);
            foreach(var evidence in dupCheck.DuplicationEvidence.Evidence)
            {
                _output.WriteLine(evidence);
            }

            dupCheck.DuplicationEvidence.FeatureImplemented.Should().BeTrue();
        }

        
    }
}
