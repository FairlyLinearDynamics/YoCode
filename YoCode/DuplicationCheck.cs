using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace YoCode
{
    class DuplicationCheck
    {       
        string CMDtoolsDir;
        string CMDtoolFileName = "dupfinder.exe"; 

        string fileNameChecked = "UnitConverterWebApp.sln";
        string outputFile = "report.xml";
        string outputArg = " -o=\"";

        string processName;
        string workingDir;
        string modiArguments;
        string origArguments;

        bool FinalResult;

        string Output { get; set; }

        int modiCodeBaseCost { get; set; }
        int modiDuplicateCost { get; set; }

        int origCodeBaseCost { get; set; }
        int origDuplicateCost { get; set; }

        string StrCodeBaseCost { get; set; }
        string StrTotalDuplicateCost { get; set; }

        //public static IConfiguration Configuration { get; set; }



        public DuplicationCheck(string modifiedPath,string originalPath, string CMDtoolsDirConfig)
        {
            CMDtoolsDir = CMDtoolsDirConfig;

            processName = Path.Combine(CMDtoolsDir, CMDtoolFileName);
            workingDir = CMDtoolsDir;

            modiArguments = Path.Combine(modifiedPath, fileNameChecked) + outputArg + outputFile;
            origArguments = Path.Combine(originalPath, fileNameChecked) + outputArg + outputFile;
        }

        public void ExecuteTheCheck() {

            RunOneCheck(origArguments);
            origCodeBaseCost = StrCodeBaseCost.GetNumbersInALine()[0];
            origDuplicateCost = StrTotalDuplicateCost.GetNumbersInALine()[0];
            Console.WriteLine(origCodeBaseCost);
            Console.WriteLine(origDuplicateCost);

            RunOneCheck(modiArguments);
            modiCodeBaseCost = StrCodeBaseCost.GetNumbersInALine()[0];
            modiDuplicateCost = StrTotalDuplicateCost.GetNumbersInALine()[0];
            Console.WriteLine(modiCodeBaseCost);
            Console.WriteLine(modiDuplicateCost);

            FinalResult = HasTheCodeImproved();
        }

        public void RunOneCheck(string args)
        {
            ProcessRunner proc = new ProcessRunner(processName, workingDir, args);
            proc.ExecuteTheCheck();

            Output = GetResults(Path.Combine(workingDir,outputFile));

            StrCodeBaseCost = Output.GetLineWithAllKeywords(getCodeBaseCostKeyword());
            StrTotalDuplicateCost = Output.GetLineWithAllKeywords(getTotalDuplicatesCostKeywords());
        }
    
        public string GetResults(string path)
        {
            return File.ReadAllText(path);
        }

        public bool HasTheCodeImproved()
        {
            return origDuplicateCost >= modiDuplicateCost ? true : false;        
        }

        public List<String> getCodeBaseCostKeyword()
        {
            return new List<string> { "<CodebaseCost>" };
        }

        public List<String> getTotalDuplicatesCostKeywords()
        {
            return new List<string> { "<TotalDuplicatesCost>" };
        }
    }
}
