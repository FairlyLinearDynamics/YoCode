using System;
using System.IO;
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
            new String[]{"miles", "kilometer"})]
        public void UICheck_FoundMatchesOnOneLine(String userFile, String[] keyWords)
        {
            UICheck uiCheck = new UICheck(userFile, keyWords);

            var listSize = uiCheck.ListOfMatches.Count;
            var containsLine = false;

            var singleString = File.ReadAllText(userFile);
            
            foreach(string key in keyWords)
            {
                if (singleString.ToLower().Contains(key))
                {
                    containsLine = true;
                    break;
                }
            }

            listSize.Should().Be(1);
            containsLine.Should().Be(true);
        }
        // TODO: Add tests for: Multiple files with one line of keywords; 
        // multiple files with multiple lines of keywords; one file with multiple lines of keywords
    }
}
