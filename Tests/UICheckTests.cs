using System;
using Xunit;
using FluentAssertions;

namespace YoCode_XUnit
{
    public class UICheckTests
    {
        // Write better testing mehod
        [Theory]
        [InlineData("C:\\Users\\ukekar\\source\\repos\\YoCode\\YoCode\\input\\compareTo.cshtml",
            new String[]{"mile", "kilometer"})]
        public void UICheck_FeatureImplementedInUI(String userFile, String[] keyWords)
        {
            YoCode.UICheck.UIContainsFeature(userFile, keyWords).Should().Be(true);
        }
    }
}
