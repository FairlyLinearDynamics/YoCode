using Xunit;
using FluentAssertions;
using YoCode;
using System.IO;
using System;

namespace YoCodeAutomatedTests
{
    public class HelpMessageTest
    {
        [Fact]
        public void CheckHelpMessage()
        {
            const string dir = @"C:\Users\ukmaug\source\repos\YoCode\YoCode\bin\Debug\netcoreapp2.1";
            string argument = "YoCode.dll --help";
            ProcessRunner pr = new ProcessRunner("dotnet", dir, argument);
            pr.ExecuteTheCheck();

            var actualPath = @"C:\YoCodeTestData\ExpectedOutput\helpMessageActualOutput.txt";
            var expectedPath = @"C:\YoCodeTestData\ExpectedOutput\helpMessage.txt";

            if (!File.Exists(actualPath))
            {
                using (FileStream fs = File.Create(actualPath)) { }
            }

            string output = File.ReadAllText(expectedPath);


            File.WriteAllLines(actualPath, new string[] { pr.Output });

            //===================================================
            var fcc = new FileChangeChecker();


            using (FileStream f1 = File.OpenRead(actualPath))
            using (FileStream f2 = File.OpenRead(expectedPath))
            {
                //fcc.FileIsModified(f1, f2).Should().BeFalse();
            }
            //===================================================

            (pr.Output).Should().BeEquivalentTo(output);
        }
    }
}
