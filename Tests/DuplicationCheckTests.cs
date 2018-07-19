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
        Mock<IPathManager> fakeDir = new Mock<IPathManager>();
        string fakeCMD = @"C:\Users\ukekar\source\repos\Tools\CMD";
        Mock<IFeatureRunner> fakeFeatureRunner = new Mock<IFeatureRunner>();

        string fakeModifiedDirPath = @""

        public DuplicationCheckTests()
        {

        }

        [Fact]
        public void DuplicationCheck_FeatureImplemented()
        {
            fakeDir.Setup(w => w.modifiedTestDirPath).Returns();
            var dupCheck = new DuplicationCheck(fakeDir,fakeCMD,fakeFeatureRunner);

        }

    }
}
