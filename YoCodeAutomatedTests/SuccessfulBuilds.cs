using Xunit;
using FluentAssertions;
using YoCode;
using System.IO;
using System;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace YoCodeAutomatedTests
{
    public class SuccessfulBuilds
    {
        private string dllPath;
        private string TestPath;
        public static IConfiguration Configuration;

        public SuccessfulBuilds()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("testappsettings.json");
            Configuration = builder.Build();
        }

        [Fact]
        public void RunProject1()
        {
            TestPath = Configuration["AutomatedTesting:TestDataPath"];
            dllPath = Configuration["YoCodeLocation:DLLFolderPath"];

            const string argument = @"YoCode.dll --original=C:\YoCodeTestData\TestProjects\junior-test --modified=C:\YoCodeTestData\TestProjects\Project1";
            ProcessRunner pr = new ProcessRunner("dotnet", dllPath, argument);
            pr.ExecuteTheCheck("Minimum test count:");

            var actualPath = TestPath+@"\Outputs\P1ActualOutput.txt";
            var expectedPath =TestPath+ @"\Outputs\P1.txt";

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

