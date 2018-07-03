using System;
using Xunit;
using FluentAssertions;

namespace YoCode_XUnit
{
    public class UICheckTests
    {
        [Theory]
        [InlineData("C:\\Users\\ukekar\\source\\repos\\YoCode\\YoCode\\input\\compareTo.cshtml",new String[]{"mile", "kilometers"})]
        public void UICheck_FeatureImplementedInUI(String userFile, String[] keyWords)
        {
            YoCode.UICheck.UIContainsFeature(userFile, keyWords).Should().Be(true);
        }
    }
}
