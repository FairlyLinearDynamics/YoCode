﻿using Xunit;
using FluentAssertions;
using YoCode;

namespace YoCode_XUnit
{
    public class TestCountCheckTests
    {
        private readonly string testStatLine;
        private readonly string testTestOutput;

        public TestCountCheckTests()
        {
            testTestOutput = "Build started, please wait...\n" +
            "Build started, please wait...\n" +
            "Build completed.\n" +
            "Test run for)\n" +
            "Microsoft(R) Test Execution Command Line Tool Version 15.7.0\n" +
            "Copyright(c) Microsoft Corporation.All rights reserved.\n" +
            "Starting test execution, please wait...\n" +
            "Build completed.\n" +
            "Test run for \n" +
            "Microsoft(R) Test Execution Command Line Tool Version 15.7.0\n" +
            "Copyright(c) Microsoft Corporation.All rights reserved.\n" +
            "Starting test execution, please wait...\n" +
            "Total tests: 22.Passed: 22.Failed: 0.Skipped: 0.\n" +
            "Test Run Successful.\n" +
            "Test execution time: 1.6349 Seconds\n";
            testStatLine = "Total tests: 22.Passed: 22.Failed: 0.Skipped: 0.";

        }

        [Fact]
        public void Test_GetLineWithAllKeywords()
        {
            //testLastAuthor.ContainsAll(GitCheck.GetKeyWords()).Should().Be(true);
            string testResult = testTestOutput.GetLineWithAllKeywords(TestCountCheck.GetTestKeyWords());

            testStatLine.Should().BeEquivalentTo(testResult);

        }
    }
}
