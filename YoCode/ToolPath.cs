using System;
using System.IO;
using System.Reflection;

namespace YoCode
{
    internal class ToolPath
    {
        private const string dupFinder = "dupfinder.exe";
        private const string dotCover = "dotcover.exe";

        private string ToolFileName { get; }

        private ToolPath(string dir, string fileName)
        {
            Dir = dir;
            ToolFileName = fileName;

            if (Exists())
            {
                return;
            }

            // If we don't find it, it might be because the combination of working dir and relative path isn't doing what the user thought it would
            // Try harder by finding the absolute path by combining the relative path in the setting with the path to YoCode.dll
            var testPath = Path.GetFullPath(Path.Combine(AssemblyDirectory, dir));
            if (File.Exists(Path.Combine(testPath, ToolFileName)))
            {
                Dir = testPath;
            }
        }

        public bool Exists()
        {
            return File.Exists(FullPath);
        }

        public string FullPath => Path.Combine(Dir, ToolFileName);
        public string Dir { get; }

        public static ToolPath CreateDupFinderPath(string dir)
        {
            return new ToolPath(dir, dupFinder);
        }

        public static ToolPath CreateDotCoverPath(string dir)
        {
            return new ToolPath(dir, dotCover);
        }

        private static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}