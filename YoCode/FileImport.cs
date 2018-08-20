using System.Collections.Generic;
using System.IO;

namespace YoCode
{
    internal class FileImport
    {
        public static void AddFileInfoToList(List<string> files, IEnumerable<FileInfo> fileinfo, string path)
        {
            foreach (var f in fileinfo)
            {
                if (FileIsNotInBuildDirectory(path, f))
                {
                    files.Add(f.ToString());
                }
            }
        }

        private static bool FileIsNotInBuildDirectory(string path, FileInfo fileinfo)
        {
            return !Path.GetRelativePath(path, fileinfo.DirectoryName).Contains("obj") && !Path.GetRelativePath(path, fileinfo.DirectoryName).Contains("bin");
        }

        public FileStream GetFileStream(string path)
        {
            return new FileStream(path, FileMode.Open);
        }
    }
}
