using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace YoCode
{
    public class DuplicationCheck
    {
        private readonly string CMDtoolsDir;
        private readonly string CMDtoolFileName = "dupfinder.exe"; 

        private readonly string fileNameChecked = "UnitConverterWebApp.sln";
        private readonly string outputFile = "report.xml";
        private readonly string outputArg = " -o=\"";

        private readonly string processName;
        private readonly string workingDir;
        private readonly string modiArguments;
        private readonly string origArguments;

        string Output { get; set; }

        int modiCodeBaseCost { get; set; }
        int modiDuplicateCost { get; set; }

        int origCodeBaseCost { get; set; }
        int origDuplicateCost { get; set; }

        string StrCodeBaseCost { get; set; }
        string StrTotalDuplicateCost { get; set; }

        public DuplicationCheck(PathManager dir, string CMDtoolsDirConfig)
        {
            try
            {
            CMDtoolsDir = CMDtoolsDirConfig;
            DuplicationEvidence.FeatureTitle = "Code quality improvement";
            processName = Path.Combine(CMDtoolsDir, CMDtoolFileName);
            workingDir = CMDtoolsDir;

            modiArguments = Path.Combine(dir.modifiedTestDirPath, fileNameChecked) + outputArg + outputFile;
            origArguments = Path.Combine(dir.originalTestDirPath, fileNameChecked) + outputArg + outputFile;            
            
            ExecuteTheCheck();
            }
            catch(Exception e)
            {
            DuplicationEvidence.FeatureImplemented = false;
            DuplicationEvidence.GiveEvidence("To run this application you will have to install Command Line Tools by Jetbrains\n" +
                "Direct download link here: https://www.jetbrains.com/resharper/download/download-thanks.html?platform=windows&code=RSCLT" +
                "After you downloaded it please specify its location in appsetting.json file, which lives in the root directory of this  project ");
            }
        }

        public void ExecuteTheCheck() {

            RunOneCheck(origArguments);
            origCodeBaseCost = StrCodeBaseCost.GetNumbersInALine()[0];
            origDuplicateCost = StrTotalDuplicateCost.GetNumbersInALine()[0];

            RunOneCheck(modiArguments);
            modiCodeBaseCost = StrCodeBaseCost.GetNumbersInALine()[0];
            modiDuplicateCost = StrTotalDuplicateCost.GetNumbersInALine()[0];

            DuplicationEvidence.FeatureImplemented = HasTheCodeImproved();
            DuplicationEvidence.GiveEvidence($"Original\nCodebase cost: {origCodeBaseCost}\nDuplicate cost: {origDuplicateCost}" +
                $"\n\nModified\nCodebase cost {modiCodeBaseCost}\nDuplicate cost: {modiDuplicateCost}");
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

        public FeatureEvidence DuplicationEvidence { get; } = new FeatureEvidence();
    }
}
