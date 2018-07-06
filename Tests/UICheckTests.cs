using System;
using Xunit;
using FluentAssertions;
using YoCode;

namespace YoCode_XUnit
{
    public class UICheckTests
    {
        // Write better testing mehod
        [Theory]
        [InlineData(@"..\..\..\TestData\MockHTML.cshtml",
            new String[]{"mile", "kilometer"})]
        public void UICheck_FeatureImplementedInUI(String userFile, String[] keyWords)
        {
            UICheck uiCheck = new UICheck(userFile, keyWords);
            //uiCheck.ContainsFeature.Should().Be(true);
        }
    }
}
