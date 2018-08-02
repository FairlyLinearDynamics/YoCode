using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace YoCode
{
    public class FileImport
    {
        //Will return a list of all files from a directory
        public static IEnumerable<string> GetAllFilesInDirectory(string path)
        {
            var files = new List<string>();
            var di = new DirectoryInfo(path);
            try
            {
                var fileinfo = di.GetFiles("*", SearchOption.AllDirectories);

                AddFileInfoToList(files, fileinfo, path);

                return files;
            }
            catch (Exception)
            {
                return null;
            }
        }

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
            return new FileStream(path,FileMode.Open);
        }
    }
}
