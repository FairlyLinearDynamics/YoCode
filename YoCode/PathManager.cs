using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibGit2Sharp;

namespace YoCode
{
    internal class PathManager : IPathManager
    {
        private const string HTML = "*.cshtml";
        private const string CSS = "*.css";
        private const string CS = "*.cs";
        private const string SLN = "*.sln";

        public string ModifiedTestDirPath { get; set; }

        public IEnumerable<string> ModifiedPaths { get; }

        private readonly Dictionary<FileTypes, string> fileExtensions = new Dictionary<FileTypes, string>();

        public PathManager(string modifiedTestDir)
        {
            ModifiedTestDirPath = modifiedTestDir;

            if (Repository.IsValid(modifiedTestDir))
            {
                var repo = new Repository(modifiedTestDir);
                ModifiedPaths = FileImport.GetAllFilesInDirectory(ModifiedTestDirPath)
                    .Where(a => !repo.Ignore.IsPathIgnored(Path.GetRelativePath(ModifiedTestDirPath, a))
                                && repo.RetrieveStatus(a) != FileStatus.Ignored && !a.Contains(".git"));
            }
            else
            {
                ModifiedPaths = FileImport.GetAllFilesInDirectory(ModifiedTestDirPath);
            }

            fileExtensions.Add(FileTypes.cs, CS);
            fileExtensions.Add(FileTypes.css, CSS);
            fileExtensions.Add(FileTypes.html, HTML);
            fileExtensions.Add(FileTypes.sln, SLN);
        }

        //Will return a list of files from a directory given a pattern
        public IEnumerable<string> GetFilesInDirectory(string PATH, FileTypes type)
        {
            var files = new List<string>();
            var di = new DirectoryInfo(PATH);

            FileImport.AddFileInfoToList(files, di.GetFiles(fileExtensions[type], SearchOption.AllDirectories), PATH);
            return files;
        }
    }
}