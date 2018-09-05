using System.Collections.Generic;

namespace YoCode
{
    internal interface IPathManager
    {
        string ModifiedTestDirPath { get; }

        IEnumerable<string> GetFilesInDirectory(string path, FileTypes type);
    }
}