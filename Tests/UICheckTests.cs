using System;
using System.IO;
using System.Linq;
using Xunit;
using FluentAssertions;
using YoCode;

namespace YoCode_XUnit
{
    public class UICheckTests
    {
        readonly string fakeFilePath = @"..\..\..\TestData\MockHTML.cshtml";
        readonly string[] keyWords = { "miles", "kilometer" };

        // Write better testing mehod
        [Fact]
        public void UICheck_FeatureImplementedBoolCheck()
        {
            var uiCheck = new UICheck(fakeFilePath, keyWords);

            var evidence = uiCheck.UIEvidence;

            evidence.FeatureImplemented.Should().Be(true);
        }

        [Fact]
        public void UICheck_FeatureEvidencePresent()
        {
            var uiCheck = new UICheck(fakeFilePath, keyWords);

            var evidence = uiCheck.UIEvidence;

            evidence.EvidencePresent.Should().Be(true);
        }

        [Fact]
        public void UICheck_FeatureTitleSet()
        {
            var uiCheck = new UICheck(fakeFilePath, keyWords);

            var evidence = uiCheck.UIEvidence;

            evidence.FeatureTitle.Should().NotBeEmpty();
        }
    }
}
