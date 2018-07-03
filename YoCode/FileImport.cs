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

    class FileImport
    {
        public string ORIGINAL_PATH { get; set; } = @"C:\Users\ukmzil\source\repos\junior-test";
        public string MODIFIED_PATH { get; set; } = @"C:\Users\ukmzil\source\repos\original-test";

        List<String> searchPatterns = new List<string>{ "*.cs", "*.cshtml", "*.js" };


        public List<string> getAllFilesInDirectory(String PATH)
        {
            List<string> files = new List<string>();
            DirectoryInfo di = new DirectoryInfo(PATH);
            FileInfo[] fileinfo = di.GetFiles("*", SearchOption.AllDirectories);

            addfileInfoToList(files, fileinfo);

            return files;

        }

        public List<string> getFilesInDirectory(String PATH, List<String> searchPatterns){

            DirectoryInfo di = new DirectoryInfo(PATH);
            int len = searchPatterns.Count;
            List<String> files = new List<String>();

            for (int i = 0; i < len; i++)
            {
                FileInfo[] fileinfo = di.GetFiles(searchPatterns[i], SearchOption.AllDirectories);

                addfileInfoToList(files, fileinfo);

            }

            return files;
        }

        private static void addfileInfoToList(List<string> files, FileInfo[] fileinfo)
        {
            for (int j = 0; j < fileinfo.Length; j++)
            {
                files.Add(fileinfo[j].ToString());
            }
        }


        public FileStream getFileStream(String PATH)
        {
            FileStream fs = new FileStream(PATH,FileMode.Open);
            return fs; 
        }

      
        public void print()
        {
            List<String> files = getAllFilesInDirectory(ORIGINAL_PATH);
            for (int i = 0; i < files.Count; i++)
            {
                Console.WriteLine(files[i].ToString());
            }
            Console.ReadLine();

        }








    }
}