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
        // Write better testing mehod
        [Theory]
        [InlineData(@"..\..\..\TestData\MockHTML.cshtml",
            new string[]{"miles", "kilometer"})]
        public void UICheck_FoundMatchesOnOneLine(string userFile, string[] keyWords)
        {
            var uiCheck = new UICheck(userFile, keyWords);

            var listSize = uiCheck.UIEvidence.Count;

            var singleString = File.ReadAllText(userFile);

            var containsLine = keyWords.Any(key => singleString.ToLower().Contains(key));

            listSize.Should().Be(1);
            containsLine.Should().Be(true);
        }
        // TODO: Add tests for: Multiple files with one line of keywords; 
        // multiple files with multiple lines of keywords; one file with multiple lines of keywords
    }
}
