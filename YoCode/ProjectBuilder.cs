using System;
using System.IO;
using System.Text;

namespace YoCode
{
    public class ProjectBuilder
    {
        private readonly ProcessRunner processRunner;
        private string ProcessName { get; } = "dotnet";
        private string Arguments { get; } = "build";

        public ProjectBuilder(string workingDir)
        {
            processRunner = new ProcessRunner(ProcessName, workingDir, Arguments);
            processRunner.ExecuteTheCheck();
        }

        public string ShowErrorOutput()
        {
            string[] buildKeywords = { "Build succeeded." + Environment.NewLine + Environment.NewLine,
                "Build FAILED." + Environment.NewLine + Environment.NewLine };
            var properOutput = processRunner.Output.Split(buildKeywords, StringSplitOptions.None);

            try
            {
                var result = properOutput[1].Split(Environment.NewLine+"    ");
                if (result != null && properOutput != null)
                {
                    return result[0];
                }
            }
            catch (IndexOutOfRangeException) { }
            return "";
        }

        public bool BuildSuccessful()
        {
            var sr = new StringReader(processRunner.Output);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains("Build succeeded"))
                {
                    return true;
                }
            }
            return false;
        }

        public int GetNumberOfWarnings()
        {
            string warningLine = GetLineWithKeyword("Warning(s)");
            return ParseStringToInt(warningLine);
        }

        public int GetNumberOfErrors()
        {
            string errorLine = GetLineWithKeyword("Error(s)");
            return ParseStringToInt(errorLine);
        }

        private string GetLineWithKeyword(string keyword)
        {
            var sr = new StringReader(processRunner.Output);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains(keyword))
                {
                    return line;
                }
            }
            return "";
        }

        private int ParseStringToInt(string line)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in line)
            {
                if (char.IsNumber(c))
                {
                    sb.Append(c);
                }
            }
            return Int32.Parse(sb.ToString());
        }
    }
}
