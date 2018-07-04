using System;
using System.Collections.Generic;

namespace YoCode
{

    
    public class Directory
    {
        public List<String> OriginalPaths { get; } = null;
        public List<String> ModifiedPaths { get; } = null;

        public Directory(List<String> originalPaths, List<String> modifiedPaths)
        {
            OriginalPaths = originalPaths;
            ModifiedPaths = modifiedPaths;
        }

    }
}
