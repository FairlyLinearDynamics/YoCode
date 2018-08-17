using System.Collections.Generic;

namespace YoCode
{
    internal interface IPathManager
    {
        string ModifiedTestDirPath { get; set; }
        IEnumerable<string> ModifiedPaths { get; }

        IEnumerable<string> GetFilesInDirectory(string PATH, FileTypes type);
    }
}