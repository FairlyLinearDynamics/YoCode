using System;
using System.Collections.Generic;
using System.IO;

namespace YoCode
{
    public class PathManager : IPathManager
    {
        const string HTML = "*.cshtml";
        const string CSS = "*.css";
        const string CS = "*.cs";
        const string SLN = "*.sln";

        public IEnumerable<String> OriginalPaths { get; }
        public IEnumerable<String> ModifiedPaths { get; }

        Dictionary<FileTypes, string> fileExtensions = new Dictionary<FileTypes, string>();
        
        public PathManager(IEnumerable<String> originalPaths, IEnumerable<String> modifiedPaths)
        {
            OriginalPaths = originalPaths;
            ModifiedPaths = modifiedPaths;

            fileExtensions.Add(FileTypes.cs, CS);
            fileExtensions.Add(FileTypes.css, CSS);
            fileExtensions.Add(FileTypes.html, HTML);
            fileExtensions.Add(FileTypes.sln, SLN);
        }

        private IEnumerable<FileContent> ReturnPathFileStream(IEnumerable<string> paths)
        {
            var streamList = new List<FileContent>();

            foreach (var path in paths)
            {
                FileStream fs = File.OpenRead(path);
                streamList.Add(new FileContent { path = path, content = fs });
            }

            return streamList;
        }

        public IEnumerable<FileContent> ReturnOriginalPathFileStream()
        {
            return ReturnPathFileStream(OriginalPaths);
        }

        public IEnumerable<FileContent> ReturnModifiedPathFileStream()
        {
            return ReturnPathFileStream(ModifiedPaths);
        }

        //Will return a list of files from a directory given a pattern
        public IEnumerable<string> GetFilesInDirectory(string PATH, FileTypes type)
        {
            var files = new List<string>();
            var di = new DirectoryInfo(PATH);

            FileImport.AddFileInfoToList(files, di.GetFiles(fileExtensions[type], SearchOption.AllDirectories));
            return files;
        }
    }
}