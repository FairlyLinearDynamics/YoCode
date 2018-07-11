using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace YoCode
{
    class TestCountCheck
    {
        string processName;
        string workingDir;
        string arguments;

        public string statLine { get; set; }
        public string Output { get; set; }

        TestStats stats;


        public TestCountCheck(string repositoryPath)
        {
            processName = "dotnet";
            workingDir = repositoryPath;
            arguments = "test";
        }

        public void ExecuteTheCheck()
        {
            ProcessRunner pr = new ProcessRunner(processName, workingDir, arguments);
            pr.ExecuteTheCheck();
           
            Output = pr.Output;
            statLine = getStatLine(Output);
            countNumberOfTests(statLine);
            
            Console.WriteLine(statLine);
        }      

        public IEnumerable<> countNumberOfTests(String statLine)
        {
            string[] numbers = Regex.Split(statLine, @"\D+");
            var tempStats = new List<int>();
            for(int i = 0; i < numbers.Length; i++)
            {
                int temp;
                if(Int32.TryParse(numbers[i],out temp) == true)
                {
                    tempStats.Add(temp);
                }
            }
            return tempStats;

        }

        public String getStatLine(string output)
        {
            var sr = new StringReader(output);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.ContainsAll(GetTestKeyWords()))
                {
                    return line;
                }
            }
            return "";
        }



        public static IEnumerable<string> GetTestKeyWords()
        {
            return new List<string> { "Total tests:" };
        }



    }
}
