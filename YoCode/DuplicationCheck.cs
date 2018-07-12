using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace YoCode
{
    class DuplicationCheck
    {
        string processName;
        string workingDir;
        string arguments;

        string Output { get; set; }

        int CodeBaseCost { get; set; }
        int TotalDuplicateCost { get; set; }


        public DuplicationCheck(string repositoryPath)
        {
            workingDir = @"C:\Users\ukmzil\source\repos\Tools\CMD";
            processName = @"C:\Users\ukmzil\source\repos\Tools\CMD\dupfinder.exe";
            //arguments = @"C:\Users\ukmzil\source\repos\junior-test\UnitConverterWebApp.sln -o=""report.xml""";
            arguments = @"C:\Users\ukmzil\source\repos\Tests-Sent-by-People\Real\drew-gibbon\UnitConverterWebApp.sln -o=""report.xml""";
        }


        public void ExecuteTheCheck() {

            ProcessRunner pr = new ProcessRunner(processName, workingDir, arguments);
            pr.ExecuteTheCheck();
            Output = GetResults(workingDir + @"\report.xml");
            string StrCodeBaseCost = Output.GetLineWithAllKeywords(getCodeBaseCostKeyword());
            string StrTotalDuplicateCost = Output.GetLineWithAllKeywords(getTotalDuplicatesCostKeywords());

            CodeBaseCost = StrCodeBaseCost.GetNumbersInALine()[0];
            TotalDuplicateCost = StrTotalDuplicateCost.GetNumbersInALine()[0];

            Console.WriteLine(CodeBaseCost);
            Console.WriteLine(TotalDuplicateCost);


        }

        public string GetResults(string path)
        {
            return File.ReadAllText(path);

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
