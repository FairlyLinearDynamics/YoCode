using System;

namespace YoCode
{
    public class ProjectRunner
    {
        private string Process { get; } = "dotnet";
        private string Argument { get; } = "run";
        public string Output { get; }
        public string ErrorOutput { get; set; }

        public ProjectRunner(string workingDir)
        {
            ProcessRunner processRunner = new ProcessRunner(Process, workingDir, Argument);
            processRunner.ExecuteTheCheck();
            Output = processRunner.Output;
            ErrorOutput = processRunner.ErrorOutput;

            Console.WriteLine(Output);
            Console.WriteLine("========================ERROR OUTPUT=================================");
            Console.WriteLine(ErrorOutput);
        }

        //public static 
    }
}
