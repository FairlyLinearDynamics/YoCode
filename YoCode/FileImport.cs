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
        public string ORIGINAL_PATH { get; set; } = @"C:\Users\ukmzil\source\repos\junior-test";
        public string MODIFIED_PATH { get; set; } = @"C:\Users\ukmzil\source\repos\original-test";

        public List<String> SearchPatterns { get; set; } = new List<string>{ "*.cs", "*.cshtml", "*.js" };
        
        //Will return a list of all files from a directory

        public List<string> GetAllFilesInDirectory(String PATH)
        {
            List<string> files = new List<string>();
            DirectoryInfo di = new DirectoryInfo(PATH);
            FileInfo[] fileinfo = di.GetFiles("*", SearchOption.AllDirectories);

            AddFileInfoToList(files, fileinfo);

            return files;

        }

        //Will return a list of files from a directory given a pattern
        public List<string> GetFilesInDirectory(String PATH, String pattern)
        {
            List<string> files = new List<string>();
            DirectoryInfo di = new DirectoryInfo(PATH);
            FileInfo[] fileinfo = di.GetFiles(pattern, SearchOption.AllDirectories);

            AddFileInfoToList(files, fileinfo);

            return files;

        }

        //Will return a list of files from a directory given a list of patterns
        public List<string> GetFilesInDirectory(String PATH, List<String> searchPatterns){

            DirectoryInfo di = new DirectoryInfo(PATH);
            int len = searchPatterns.Count;
            List<String> files = new List<String>();

            for (int i = 0; i < len; i++)
            {
                FileInfo[] fileinfo = di.GetFiles(searchPatterns[i], SearchOption.AllDirectories);

                AddFileInfoToList(files, fileinfo);

            }

            return files;
        }

        //Helper method to convert FileInfo[] elements to string and add them to a list 
        private static void AddFileInfoToList(List<string> files, FileInfo[] fileinfo)
        {
            for (int j = 0; j < fileinfo.Length; j++)
            {
                files.Add(fileinfo[j].ToString());
            }
        }


        public FileStream GetFileStream(String PATH)
        {
            FileStream fs = new FileStream(PATH,FileMode.Open);
            return fs; 
        }

      
        public void Print()
        {
            List<String> files = GetFilesInDirectory(ORIGINAL_PATH,SearchPatterns);
            for (int i = 0; i < files.Count; i++)
            {
                Console.WriteLine(files[i].ToString());
            }
            Console.ReadLine();

        }


    }
}