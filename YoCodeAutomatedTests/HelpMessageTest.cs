using Xunit;
using FluentAssertions;
using YoCode;
using System.IO;
using System;
using Microsoft.Extensions.Configuration;

namespace YoCodeAutomatedTests
{
    public class HelpMessageTest
    {
        private string TestPath;
        private string dllPath;
        public static IConfiguration Configuration;
 
        public HelpMessageTest()
        {
            var builder1 = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("testappsettings.json");
            Configuration = builder1.Build();
        }

        [Fact]
        public void CheckHelpMessage()
        {
            TestPath = Configuration["AutomatedTesting:TestDataPath"];
            dllPath = Configuration["YoCodeLocation:DLLFolderPath"];

            const string argument = "YoCode.dll --help";
            ProcessRunner pr = new ProcessRunner("dotnet", dllPath, argument);
            pr.ExecuteTheCheck();

            var actualPath = TestPath+@"\Outputs\helpMessageActualOutput.txt";
            var expectedPath = TestPath+@"\Outputs\helpMessage.txt";

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

            (pr.Output).Should().BeEquivalentTo(output+Environment.NewLine);
        }
    }
}
