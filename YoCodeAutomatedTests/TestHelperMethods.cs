using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.IO;
using Xunit.Abstractions;
using YoCode;
using System;

namespace YoCodeAutomatedTests
{
    public class TestHelperMethods
    {
        public string OutputFilename { get; } = "YoCodeReport.html";
        private IConfiguration configuration;
        public string TestPath { get; private set; }
        public string DllPath { get; private set; }
        public string DefaultYoCodeReportPath { get; }

        public TestHelperMethods()
        {
            Setup();
            DefaultYoCodeReportPath = Path.Combine(DllPath, "YoCodeReport.html");
        }

        private void Setup()
        {
            var builder1 = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("testappsettings.json");
            configuration = builder1.Build();

            TestPath = configuration["AutomatedTesting:TestDataPath"];
            DllPath = configuration["YoCodeLocation:DLLFolderPath"];
        }

        public static bool FilesAreDifferent(string path1, string path2)
        {
            using (var f1 = File.OpenRead(path1))
            using (var f2 = File.OpenRead(path2))
            {
                return f1.FileIsModified(f2);
            }
        }

        public static void RunProcessAndGatherOutput(string processName, string workingDir, string arguments, ITestOutputHelper testOutputHelper)
        {
            var pr = new ProcessRunner(processName, workingDir, arguments);
            pr.ExecuteTheCheck("Units were converted successfully");

            testOutputHelper.WriteLine("Error Output: ");
            testOutputHelper.WriteLine(pr.ErrorOutput);

            testOutputHelper.WriteLine("Standard Output: ");
            testOutputHelper.WriteLine(pr.Output);
        }

        public void OutputTestDebugInfo(ITestOutputHelper testOutputHelper, string argument)
        {
            testOutputHelper.WriteLine($"Arguments to dotnet: {argument}");
            testOutputHelper.WriteLine($"YoCode DLL path: {DllPath}");
            testOutputHelper.WriteLine($"Working directory: {Directory.GetCurrentDirectory()}");
            testOutputHelper.WriteLine($"Test assembly version: {ThisAssembly.AssemblyInformationalVersion}");
            testOutputHelper.WriteLine($"YoCode assembly version: {FileVersionInfo.GetVersionInfo(Path.Combine(DllPath, "YoCode.dll")).ProductVersion}"
                + Environment.NewLine);
        }
    }
}
