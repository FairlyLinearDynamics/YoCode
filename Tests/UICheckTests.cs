using Xunit;
using FluentAssertions;
using Moq;
using YoCode;

namespace YoCode_XUnit
{
    public class UICheckTests
    {
        private readonly string fakeFilePath = @"..\..\..\TestData\MockHTML.cshtml";
        private readonly string[] keyWords = { "miles", "kilometer" };
        private readonly Mock<ICheckConfig> checkConfig;

        public UICheckTests()
        {
            var pathManager = new Mock<IPathManager>();
            pathManager.Setup(m => m.GetFilesInDirectory(It.IsAny<string>(), It.IsAny<FileTypes>())).Returns(new []{fakeFilePath});

            checkConfig = new Mock<ICheckConfig>();
            checkConfig.Setup(m => m.PathManager).Returns(pathManager.Object);
        }

        [Fact]
        public void UICheck_FeatureImplementedBoolCheck()
        {
            var uiCheck = new UICodeCheck(keyWords, checkConfig.Object);

            var evidence = uiCheck.UIEvidence;

            evidence.Passed.Should().Be(true);
        }

        [Fact]
        public void UICheck_FeatureTitleSet()
        {
            var uiCheck = new UICodeCheck(keyWords, checkConfig.Object);

            var evidence = uiCheck.UIEvidence;

            FeatureTitleStorage.GetFeatureTitle(evidence.Feature).Should().NotBeEmpty();
        }
    }
}