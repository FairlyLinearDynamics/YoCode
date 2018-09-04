using Xunit;
using FluentAssertions;
using YoCode;

namespace YoCode_XUnit
{
    public class UICheckTests
    {
        private readonly string fakeFilePath = @"..\..\..\TestData\MockHTML.cshtml";
        private readonly string[] keyWords = { "miles", "kilometer" };

        // Write better testing mehod
        [Fact]
        public void UICheck_FeatureImplementedBoolCheck()
        {
            var uiCheck = new UICodeCheck(fakeFilePath, keyWords);

            var evidence = uiCheck.UIEvidence;

            evidence.Passed.Should().Be(true);
        }

        [Fact]
        public void UICheck_FeatureTitleSet()
        {
            var uiCheck = new UICodeCheck(fakeFilePath, keyWords);

            var evidence = uiCheck.UIEvidence;

            FeatureTitleStorage.GetFeatureTitle(evidence.Feature).Should().NotBeEmpty();
        }
    }
}