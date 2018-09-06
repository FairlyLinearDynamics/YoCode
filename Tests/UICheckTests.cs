using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Moq;
using YoCode;
using System.IO;

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
            pathManager.Setup(m => m.GetFilesInDirectory(It.IsAny<string>(), It.IsAny<FileTypes>(), It.IsAny<SearchOption>())).Returns(new []{fakeFilePath});

            checkConfig = new Mock<ICheckConfig>();
            checkConfig.Setup(m => m.PathManager).Returns(pathManager.Object);
        }

        [Fact]
        public async Task UICheck_FeatureImplementedBoolCheck()
        {
            var uiCheck = new UICodeCheck(keyWords, checkConfig.Object);

            var evidence = await uiCheck.Execute();

            evidence.Single().Passed.Should().Be(true);
        }

        [Fact]
        public async Task UICheck_FeatureTitleSet()
        {
            var uiCheck = new UICodeCheck(keyWords, checkConfig.Object);

            var evidence = await uiCheck.Execute();

            FeatureTitleStorage.GetFeatureTitle(evidence.Single().Feature).Should().NotBeEmpty();
        }
    }
}