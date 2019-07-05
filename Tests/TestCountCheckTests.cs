using Xunit;
using FluentAssertions;
using YoCode;
using NSubstitute;

namespace YoCode_XUnit
{
    public class TestCountCheckTests
    {
        private readonly TestCountCheck testCountCheck;

        public TestCountCheckTests()
        {
            var checkConfig = Substitute.For<ICheckConfig>();

            testCountCheck = new TestCountCheck(checkConfig, null);
        }

        [Fact]
        public void TestSingleLineOutput()
        {
            var testTestOutput = "Test run for \n" +
            "Microsoft(R) Test Execution Command Line Tool Version 15.7.0\n" +
            "Copyright(c) Microsoft Corporation.All rights reserved.\n" +
            "Starting test execution, please wait...\n" +
            "Total tests: 22. Passed: 19. Failed: 2. Skipped: 1.\n" +
            "Test Run Successful.\n" +
            "Test execution time: 1.6349 Seconds\n";

            testCountCheck.ProcessResult(testTestOutput);

            testCountCheck.Stats.testsPassed.Should().Be(19);
            testCountCheck.Stats.testsFailed.Should().Be(2);
            testCountCheck.Stats.testsSkipped.Should().Be(1);
            testCountCheck.Stats.totalTests.Should().Be(22);
            testCountCheck.Stats.PercentagePassed.Should().BeApproximately(86.37, 0.01);
        }

        [Fact]
        public void TestMultiLineOutput()
        {
            var testTestOutput = "Test run for \n" +
                                 "Microsoft(R) Test Execution Command Line Tool Version 15.7.0\n" +
                                 "Copyright(c) Microsoft Corporation.All rights reserved.\n" +
                                 "Starting test execution, please wait...\n" +
                                 "Total tests: 22.\n" +
                                 "Passed: 19.\n" +
                                 "Failed: 2.\n" +
                                 "Skipped: 1.\n" +
                                 "Test Run Successful.\n" +
                                 "Test execution time: 1.6349 Seconds\n";

            testCountCheck.ProcessResult(testTestOutput);

            testCountCheck.Stats.testsPassed.Should().Be(19);
            testCountCheck.Stats.testsFailed.Should().Be(2);
            testCountCheck.Stats.testsSkipped.Should().Be(1);
            testCountCheck.Stats.totalTests.Should().Be(22);
            testCountCheck.Stats.PercentagePassed.Should().BeApproximately(86.37, 0.01);
        }

        [Fact]
        public void MissingValuesShouldBeTreatedAsZero()
        {
            var testTestOutput = "Test run for \n" +
                                 "Microsoft(R) Test Execution Command Line Tool Version 15.7.0\n" +
                                 "Copyright(c) Microsoft Corporation.All rights reserved.\n" +
                                 "Starting test execution, please wait...\n" +
                                 "Total tests: 22. Failed: 2.\n" +
                                 "Test Run Successful.\n" +
                                 "Test execution time: 1.6349 Seconds\n";

            testCountCheck.ProcessResult(testTestOutput);

            testCountCheck.Stats.testsPassed.Should().Be(0);
            testCountCheck.Stats.testsSkipped.Should().Be(0);
            testCountCheck.Stats.PercentagePassed.Should().Be(0);
        }


    }
}
