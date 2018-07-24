using Microsoft.Extensions.Configuration;
using System.IO;
using YoCode;

namespace YoCodeAutomatedTests
{
    public class TestHelperMethods
    {
        private IConfiguration Configuration;
        public string testPath { get; set; }
        public string dllPath { get; set; }

        public TestHelperMethods()
        {
            Setup();
        }

        public void Setup()
        {
            var builder1 = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("testappsettings.json");
            Configuration = builder1.Build();

            testPath = Configuration["AutomatedTesting:TestDataPath"];
            dllPath = Configuration["YoCodeLocation:DLLFolderPath"];
        }

        public static void WriteToFile(string path, string content)
        {
            File.WriteAllLines(path, new string[] { content });
        }

        public static bool FilesAreDifferent(string path1, string path2)
        {
            var fcc = new FileChangeChecker();

            using (FileStream f1 = File.OpenRead(path1))
            using (FileStream f2 = File.OpenRead(path2))
            {
                return fcc.FileIsModified(f1, f2);
            }
        }
    }
}
