using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace YoCode
{
    class DuplicationCheck
    {
        string CMDtoolsDir = @"C:\Users\ukmzil\source\repos\Tools\CMD";
        string CMDtoolFileName = "dupfinder.exe"; 
        string fileNameChecked = "UnitConverterWebApp.sln";
        string outputFile = " -o=\"report.xml\"";

        string processName;
        string workingDir;
        string modiArguments;
        string origArguments;

        string Output { get; set; }

        int modiCodeBaseCost { get; set; }
        int modiDuplicateCost { get; set; }

        int origCodeBaseCost { get; set; }
        int origDuplicateCost { get; set; }

        string StrCodeBaseCost { get; set; }
        string StrTotalDuplicateCost { get; set; }

        public DuplicationCheck(string modifiedPath,string originalPath)
        {
            processName = Path.Combine(CMDtoolsDir, CMDtoolFileName);
            workingDir = CMDtoolsDir;

            modiArguments = Path.Combine(modifiedPath, fileNameChecked) + outputFile;
            origArguments = Path.Combine(originalPath, fileNameChecked) + outputFile;


        }

        public void ExecuteTheCheck() {

            RunOneCheck(modiArguments);
            modiCodeBaseCost = StrCodeBaseCost.GetNumbersInALine()[0];
            modiDuplicateCost = StrTotalDuplicateCost.GetNumbersInALine()[0];
              
            RunOneCheck(origArguments);
            origCodeBaseCost = StrCodeBaseCost.GetNumbersInALine()[0];
            origDuplicateCost = StrTotalDuplicateCost.GetNumbersInALine()[0];

            Console.WriteLine(HasTheCodeImproved());

        }

        public void RunOneCheck(string args)
        {

            ProcessRunner proc = new ProcessRunner(processName, workingDir, args);
            proc.ExecuteTheCheck();
            Output = GetResults(workingDir + @"\report.xml");

            StrCodeBaseCost = Output.GetLineWithAllKeywords(getCodeBaseCostKeyword());
            StrTotalDuplicateCost = Output.GetLineWithAllKeywords(getTotalDuplicatesCostKeywords());






        }
    
        public string GetResults(string path)
        {
            return File.ReadAllText(path);

        }

        public bool HasTheCodeImproved()
        {
            return origCodeBaseCost >= modiCodeBaseCost ? true : false;        
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
