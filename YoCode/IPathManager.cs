using System.Collections.Generic;
using System.IO;

namespace YoCode
{
    internal interface IPathManager
    {
        string ModifiedTestDirPath { get; }

        IEnumerable<string> GetFilesInDirectory(string path, FileTypes type, SearchOption option = SearchOption.AllDirectories);
    }
}