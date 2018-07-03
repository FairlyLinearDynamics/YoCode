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


    class FileImport
    {
        public string ORIGINAL_PATH { get; set; } = @"C:\Users\ukmzil\source\repos\junior-test";
        public string MODIFIED_PATH { get; set; } = @"C:\Users\ukmzil\source\repos\original-test";

        FileInfo[] originalFiles { get; set; }
        FileInfo[] modifiedFiles { get; set; }



        public FileStream getFile(String PATH)
        {
            FileStream fs = new FileStream(PATH,FileMode.Open);
            return fs; 

        }
       


        

        


    }
}
