using Xunit;
using FluentAssertions;
using YoCode;
using System.IO;
using System;

namespace YoCodeAutomatedTests
{
    public class SuccessfulBuilds
    {
        [Fact]
        public void RunProject1()
        {
            const string dir = @"C:\Users\ukmaug\source\repos\YoCode\YoCode\bin\Debug\netcoreapp2.1";
            string argument = @"YoCode.dll --original=C:\YoCodeTestData\TestProjects\junior-test --modified=C:\YoCodeTestData\TestProjects\Project1";
            ProcessRunner pr = new ProcessRunner("dotnet", dir, argument);
            pr.ExecuteTheCheck("Minimum test count:");

            var actualPath = @"C:\YoCodeTestData\Outputs\P1ActualOutput.txt";
            var expectedPath = @"C:\YoCodeTestData\Outputs\P1.txt";

            if (!File.Exists(actualPath))
            {
                using (FileStream fs = File.Create(actualPath)) { }
            }

            string output = File.ReadAllText(expectedPath);

            //WriteAllLines leaves a newline at the end
            File.WriteAllLines(actualPath, new string[] { pr.Output });

            //===================================================
            var fcc = new FileChangeChecker();

            using (FileStream f1 = File.OpenRead(actualPath))
            using (FileStream f2 = File.OpenRead(expectedPath))
            {
                //fcc.FileIsModified(f1, f2).Should().BeFalse();
            }
            //===================================================

            (pr.Output).Should().BeEquivalentTo(output + Environment.NewLine);
        }
    }
}

