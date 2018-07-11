using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace YoCode
{
    public class TestCountCheck
    {
        string processName;
        string workingDir;
        string arguments;

        public string statLine { get; set; }
        public string Output { get; set; }

        TestStats stats;
        List<int> tempStats;


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
            statLine = Output.GetLineWithAllKeywords(GetTestKeyWords());
            tempStats = CountNumberOfTests(statLine);
            StoreCalculations(tempStats); 

        }

        public List<int> CountNumberOfTests(String statLine)
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

        public void StoreCalculations(List<int> tempStats)
        {
            stats.totalTests = tempStats[0];
            stats.testsPassed = tempStats[1];
            stats.testsFailed = tempStats[2];
            stats.testsSkipped = tempStats[3];
        }

        public static List<string> GetTestKeyWords()
        {
            return new List<string> { "Total tests:" };
        }



    }
}
