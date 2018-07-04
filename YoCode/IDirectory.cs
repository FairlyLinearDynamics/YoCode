using System.Collections.Generic;
using System.IO;

namespace YoCode
{
    public interface IDirectory
    {
        List<string> ModifiedPaths { get; }
        List<string> OriginalPaths { get; }

        List<Stream> ReturnModifiedPathFileStream();
        List<Stream> ReturnOriginalPathFileStream();
    }
}