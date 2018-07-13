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

                AddFileInfoToList(files, fileinfo);

                return files;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //Helper method to convert FileInfo[] elements to string and add them to a list 
        public static void AddFileInfoToList(List<string> files, IEnumerable<FileInfo> fileinfo)
        {
            files.AddRange(fileinfo.Select(fi => fi.ToString()));
        }

        public FileStream GetFileStream(string path)
        {
            var fs = new FileStream(path,FileMode.Open);
            return fs; 
        }
    }
}
