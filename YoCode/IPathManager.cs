using System.Collections.Generic;

namespace YoCode
{
    internal interface IPathManager
    {
        string OriginalTestDirPath { get; set; }
        string ModifiedTestDirPath { get; set; }
        IEnumerable<string> ModifiedPaths { get; }
        IEnumerable<string> OriginalPaths { get; }

        IEnumerable<FileContent> ReturnModifiedPathFileStream();
        IEnumerable<FileContent> ReturnOriginalPathFileStream();

        IEnumerable<string> GetFilesInDirectory(string PATH, FileTypes type);
    }
}