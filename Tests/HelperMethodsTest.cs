﻿using Xunit;
using FluentAssertions;
using YoCode;
using System.Collections.Generic;

namespace YoCode_XUnit
{
    public class HelperMethodsTest
    {
        private readonly string testLine, testString;
        private readonly List<string> keywords;

        public HelperMethodsTest()
        {
            testString = "commit fd891f3dbcf0fd935814b56f87b2e0f768fe5bef (HEAD -> i8-gitcheck)\n" +
            "\nAuthor: matas.zilaitis < matas.zilaitis@gmail.com > " +
            "\n Date:   Thu Jul 5 16:57:15 2018 + 0100 " +
            "\nChanged the class logic a bit thanks to Mike";

            testLine = "Author: matas.zilaitis < matas.zilaitis@gmail.com > ";
            keywords = new List<string> { "<", ">", "@", "Author: ", "a", };
        }

        [Fact]
        public void Test_ContainsAny()
        {
            testLine.ContainsAny(keywords).Should().Be(true);
        }

        [Fact]
        public void Test_ContainsAll()
        {
            testLine.ContainsAll(keywords).Should().Be(true);
        }

        [Fact]
        public void Test_GetLineWithAllKeywords()
        {
            string actualString = testString.GetLineWithAllKeywords(keywords);

            testLine.Should().BeEquivalentTo(actualString);
        }

        [Fact]
        public void Test_GetNumbersInALine()
        {
            const string actual = "Lorem2 0 ipsum dolor 17sit amet, consectetur189 adipiscing elit303. Sed id gravida mi.";
            var expected = new List<int>
            {
                2,
                0,
                17,
                189,
                303
            };
            expected.Should().BeEquivalentTo(actual.GetNumbersInALine());
        }

        [Theory]
        [InlineData(20, 20.0000002)]
        [InlineData(19.6666, 19.666600668)]
        [InlineData(0, 0.00000001)]
        [InlineData(49.20, 49.200001)]
        [InlineData(20.20, 20.2000002002)]

        public void Test_SubjectivelyEqual(double a, double b)
        {
            a.ApproximatelyEquals(b).Should().BeTrue();
        }
    }
}
