using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    class RunCheck
    {
        private string Output { get; set; } = "No output found";
  

        public void ExecuteTheCheck()
        {

            ProcessRunner pr = new ProcessRunner(setupProcessInfo("cmd.exe", @"C:\Users\ukmzil\source\repos\Tests-Sent-by-People\Real\jacob-millward\UnitConverterWebApp\bin\Debug\netcoreapp2.0", @"dotnet build C:\Users\ukmzil\source\repos\Tests-Sent-by-People\Real\jacob-millward\UnitConverterWebApp\bin\Debug\netcoreapp2.0"));
            pr.ExecuteTheCheck();

            Output = pr.Output;
        }




        public ProcessInfo setupProcessInfo(string processName, string workingDir, string arguments)
        {
            ProcessInfo pi;
            pi.processName = "git.exe";
            pi.workingDir = workingDir;
            pi.arguments = "log";

            return pi;
        }


    }
}
