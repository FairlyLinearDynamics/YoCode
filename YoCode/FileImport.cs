using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace YoCode
{
    // To import:
    // UnitConverter.cs from junior-test/UnitConverterWebApp
    // Program.cs from junior-test/UnitConverterWebApp
    // Startup.cs from junior-test/UnitConverterWebApp
    // HomeController.cs from junior-test/UnitConverterWebApp/Controllers

    public class FileImport
    {
        // This shouldn't live in FileImport class

        public string ORIGINAL_PATH { get; set; } = @"..\..\..\..\..\junior-test";

        public string MODIFIED_PATH { get; set; } = @"..\..\..\..\..\original-test";

        List<string> SearchPatterns = new List<string>{ "*.cs", "*.cshtml", "*.js" };
        
        //Will return a list of all files from a directory
        public List<string> GetAllFilesInDirectory(string PATH)
        {
            var files = new List<string>();
            var di = new DirectoryInfo(PATH);
            var fileinfo = di.GetFiles("*", SearchOption.AllDirectories);

            AddFileInfoToList(files, fileinfo);

            return files;
        }

        //Helper method to convert FileInfo[] elements to string and add them to a list 
        public static void AddFileInfoToList(List<string> files, FileInfo[] fileinfo)
        {
            foreach(var fi in fileinfo)
            {
                files.Add(fi.ToString());
            }
        }

        public FileStream GetFileStream(string PATH)
        {
            var fs = new FileStream(PATH,FileMode.Open);
            return fs; 
        }
    }
}
