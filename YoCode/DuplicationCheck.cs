using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace YoCode
{
    public class DuplicationCheck
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

        string Output { get; set; }

        int modiCodeBaseCost { get; set; }
        int modiDuplicateCost { get; set; }

        int origCodeBaseCost { get; set; }
        int origDuplicateCost { get; set; }

        string StrCodeBaseCost { get; set; }
        string StrTotalDuplicateCost { get; set; }

        public DuplicationCheck(PathManager dir, string CMDtoolsDirConfig)
        {
            CMDtoolsDir = CMDtoolsDirConfig;

            DuplicationEvidence.FeatureTitle = "Code quality improvement";
            processName = Path.Combine(CMDtoolsDir, CMDtoolFileName);
            workingDir = CMDtoolsDir;

            modiArguments = Path.Combine(dir.modifiedTestDirPath, fileNameChecked) + outputArg + outputFile;
            origArguments = Path.Combine(dir.originalTestDirPath, fileNameChecked) + outputArg + outputFile;            
            
            ExecuteTheCheck();
        }

        public void ExecuteTheCheck() {

            RunOneCheck(origArguments);
            origCodeBaseCost = StrCodeBaseCost.GetNumbersInALine()[0];
            origDuplicateCost = StrTotalDuplicateCost.GetNumbersInALine()[0];

            RunOneCheck(modiArguments);
            modiCodeBaseCost = StrCodeBaseCost.GetNumbersInALine()[0];
            modiDuplicateCost = StrTotalDuplicateCost.GetNumbersInALine()[0];

            DuplicationEvidence.FeatureImplemented = HasTheCodeImproved();
            DuplicationEvidence.GiveEvidence($"Original code score: {origCodeBaseCost}\nModified code score: {modiCodeBaseCost}" +
                $"\nOriginal code duplication score: {origDuplicateCost}\nModified code duplication score: {modiDuplicateCost}");
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
            return origDuplicateCost > modiDuplicateCost ? true : false;        
        }

        public List<String> getCodeBaseCostKeyword()
        {
            return new List<string> { "<CodebaseCost>" };
        }

        public List<String> getTotalDuplicatesCostKeywords()
        {
            return new List<string> { "<TotalDuplicatesCost>" };
        }

        public FeatureEvidence DuplicationEvidence { get; private set; } = new FeatureEvidence();
    }
}
