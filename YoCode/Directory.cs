using System;
using System.Collections.Generic;
using System.IO;

namespace YoCode
{
    public class Directory : IDirectory
    {
        const string HTML = "*.html";
        const string CSS = "*.css";
        const string CS = "*.cs";

        public IEnumerable<String> OriginalPaths { get; }
        public IEnumerable<String> ModifiedPaths { get; }
        Dictionary<FileTypes, string> fileExtensions = new Dictionary<FileTypes, string>();
        
        public Directory(IEnumerable<String> originalPaths, IEnumerable<String> modifiedPaths)
        {
            fileExtensions.Add(FileTypes.cs, CS);
            fileExtensions.Add(FileTypes.css, CSS);
            fileExtensions.Add(FileTypes.html, HTML);
        }

        private IEnumerable<FileContent> ReturnPathFileStream(IEnumerable<string> paths)
        {
            var streamList = new List<FileContent>();

            foreach (var path in paths)
            {
                FileStream fs = File.OpenWrite(path);
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
        public IEnumerable<string> GetFilesInDirectory(String PATH, FileTypes type)
        {
            var files = new List<string>();
            var di = new DirectoryInfo(PATH);

            FileImport.AddFileInfoToList(files, di.GetFiles(fileExtensions[type], SearchOption.AllDirectories));
            return files;
        }
    }
}