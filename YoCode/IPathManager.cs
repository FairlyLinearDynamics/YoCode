using System.Collections.Generic;

namespace YoCode
{
    public interface IPathManager
    {
        string originalTestDirPath { get; set; }
        string modifiedTestDirPath { get; set; }
        IEnumerable<string> ModifiedPaths { get; }
        IEnumerable<string> OriginalPaths { get; }

        IEnumerable<FileContent> ReturnModifiedPathFileStream();
        IEnumerable<FileContent> ReturnOriginalPathFileStream();

        IEnumerable<string> GetFilesInDirectory(string PATH, FileTypes type);
    }
}