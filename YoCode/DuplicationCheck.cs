using System;
using System.Collections.Generic;
using System.IO;

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

        private string Output { get; set; }

        private int ModiCodeBaseCost { get; set; }
        private int ModiDuplicateCost { get; set; }

        private int OrigCodeBaseCost { get; set; }
        private int OrigDuplicateCost { get; set; }

        private string StrCodeBaseCost { get; set; }
        private string StrTotalDuplicateCost { get; set; }

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
            OrigCodeBaseCost = StrCodeBaseCost.GetNumbersInALine()[0];
            OrigDuplicateCost = StrTotalDuplicateCost.GetNumbersInALine()[0];

            RunOneCheck(modiArguments);
            ModiCodeBaseCost = StrCodeBaseCost.GetNumbersInALine()[0];
            ModiDuplicateCost = StrTotalDuplicateCost.GetNumbersInALine()[0];

            if(DuplicationEvidence.Evidence.Count > 0)
            {
                return;
            }

            DuplicationEvidence.FeatureImplemented = HasTheCodeImproved();
            DuplicationEvidence.GiveEvidence($"Original\nCodebase cost: {OrigCodeBaseCost}\nDuplicate cost: {OrigDuplicateCost}" +
                $"\n\nModified\nCodebase cost {ModiCodeBaseCost}\nDuplicate cost: {ModiDuplicateCost}");
        }

        public void RunOneCheck(string args)
        {
            ProcessRunner proc = new ProcessRunner(processName, workingDir, args);
            proc.ExecuteTheCheck();

            Output = GetResults(Path.Combine(workingDir,outputFile));

            StrCodeBaseCost = Output.GetLineWithAllKeywords(GetCodeBaseCostKeyword());
            StrTotalDuplicateCost = Output.GetLineWithAllKeywords(GetTotalDuplicatesCostKeywords());

            if(proc.TimedOut && DuplicationEvidence.Evidence.Count == 0)
            {
                DuplicationEvidence.FeatureImplemented = false;
                DuplicationEvidence.GiveEvidence("Timed Out");
            }
        }

        public string GetResults(string path)
        {
            return File.ReadAllText(path);
        }

        public bool HasTheCodeImproved()
        {
            return OrigDuplicateCost > ModiDuplicateCost;
        }

        public List<String> GetCodeBaseCostKeyword()
        {
            return new List<string> { "<CodebaseCost>" };
        }

        public List<String> GetTotalDuplicatesCostKeywords()
        {
            return new List<string> { "<TotalDuplicatesCost>" };
        }

        public FeatureEvidence DuplicationEvidence { get; } = new FeatureEvidence();
    }
}
