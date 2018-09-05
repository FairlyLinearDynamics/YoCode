using System.Collections.Generic;
using System.IO;

namespace YoCode
{
    internal class PathManager : IPathManager
    {
        private const string HTML = "*.cshtml";
        private const string CSS = "*.css";
        private const string CS = "*.cs";
        private const string SLN = "*.sln";

        public string ModifiedTestDirPath { get; }

        private readonly Dictionary<FileTypes, string> fileExtensions = new Dictionary<FileTypes, string>();

        public PathManager(string modifiedTestDir)
        {
            ModifiedTestDirPath = modifiedTestDir;

            fileExtensions.Add(FileTypes.cs, CS);
            fileExtensions.Add(FileTypes.css, CSS);
            fileExtensions.Add(FileTypes.html, HTML);
            fileExtensions.Add(FileTypes.sln, SLN);
        }

        //Will return a list of files from a directory given a pattern
        public IEnumerable<string> GetFilesInDirectory(string path, FileTypes type)
        {
            var files = new List<string>();
            var di = new DirectoryInfo(path);

            FileImport.AddFileInfoToList(files, di.GetFiles(fileExtensions[type], SearchOption.AllDirectories), path);
            return files;
        }
    }
}