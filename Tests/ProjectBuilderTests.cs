using Xunit;
using YoCode;
using FluentAssertions;
using System;

namespace YoCode_XUnit
{
    public class ProjectBuilderTests
    {
        private string sampleOutput;

        [Fact]
        public void CheckIfCorrectlySetsErrorOutputToEmpty()
        {
            sampleOutput = "Restoring packages for C:\\Users\\ukmaug\\Downloads\no - to - interview" + Environment.NewLine +
                           "Restoring packages for C:\\Users\\ukmaug\\Downloads\no - to - interview" + Environment.NewLine +
                           "Restoring packages for C:\\Users\\ukmaug\\Downloads\no - to - interview" + Environment.NewLine +
                           "Restore completed in 813.89 ms for C:\\Users\\ukmaug\\Downloads\no -" + Environment.NewLine +
                           "Generating MSBuild file C:\\Users\\ukmaug\\Downloads\no" + Environment.NewLine +
                           "Generating MSBuild file C:\\Users\\ukmaug\\Downloads\no" + Environment.NewLine +
                           "Generating MSBuild file C:\\Users\\ukmaug\\Downloads\no" + Environment.NewLine +
                           "Restore completed in 1.13 sec for C:\\Users" + Environment.NewLine +
                           "UnitConverterWebApp->C:\\Us" + Environment.NewLine + Environment.NewLine +

                           "Build Succeeded." + Environment.NewLine+
                           "    56 Warning(s)" + Environment.NewLine +
                           "    3 Error(s)" + Environment.NewLine + Environment.NewLine +

                           "Time Elapsed 00:00:03.44";
            ProjectBuilder.GetErrorOutput(sampleOutput).Should().BeEmpty();
        }

        [Fact]
        public void CheckIfErrorOutputCapturedCorrectly()
        {
            sampleOutput =  "Restoring packages for C:\\Users\\ukmaug\\Downloads\no - to - interview" + Environment.NewLine +
                            "Restoring packages for C:\\Users\\ukmaug\\Downloads\no - to - interview" + Environment.NewLine +
                            "Restoring packages for C:\\Users\\ukmaug\\Downloads\no - to - interview" + Environment.NewLine +
                            "Restore completed in 813.89 ms for C:\\Users\\ukmaug\\Downloads\no -" + Environment.NewLine +
                            "Generating MSBuild file C:\\Users\\ukmaug\\Downloads\no" + Environment.NewLine +
                            "Generating MSBuild file C:\\Users\\ukmaug\\Downloads\no" + Environment.NewLine +
                            "Generating MSBuild file C:\\Users\\ukmaug\\Downloads\no" + Environment.NewLine +
                            "Restore completed in 1.13 sec for C:\\Users" + Environment.NewLine +
                            "UnitConverterWebApp->C:\\Us" + Environment.NewLine +
                            "Tests.cs(22, 11): error CS0117: 'Assert' does not contain a definition" + Environment.NewLine +
                            "Tests.cs(46, 11): error CS0117: 'Assert' does not contain a definition" + Environment.NewLine +
                            "Tests.cs(68, 11): error CS0117: 'Assert' does not contain a definition" + Environment.NewLine + Environment.NewLine +

                            "Build FAILED." + Environment.NewLine + Environment.NewLine +
                            "Tests.cs(22, 11): error CS0117: 'Assert' does not contain a definition" + Environment.NewLine +
                            "Tests.cs(46, 11): error CS0117: 'Assert' does not contain a definition" + Environment.NewLine +
                            "Tests.cs(68, 11): error CS0117: 'Assert' does not contain a definition" + Environment.NewLine +

                            "    56 Warning(s)" + Environment.NewLine +
                            "    3 Error(s)" + Environment.NewLine + Environment.NewLine +

                            "Time Elapsed 00:00:03.44";

            string sampleErrorOutput = "Tests.cs(22, 11): error CS0117: 'Assert' does not contain a definition" + Environment.NewLine +
                                       "Tests.cs(46, 11): error CS0117: 'Assert' does not contain a definition" + Environment.NewLine +
                                       "Tests.cs(68, 11): error CS0117: 'Assert' does not contain a definition";

            ProjectBuilder.GetErrorOutput(sampleOutput).Should().BeEquivalentTo(sampleErrorOutput);
        }
    }
}
