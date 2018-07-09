using System.Collections.Generic;

namespace YoCode
{
    public interface IPathManager
    {
        IEnumerable<string> ModifiedPaths { get; }
        IEnumerable<string> OriginalPaths { get; }

        IEnumerable<FileContent> ReturnModifiedPathFileStream();
        IEnumerable<FileContent> ReturnOriginalPathFileStream();
    }
}