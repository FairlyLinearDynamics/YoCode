using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace YoCode
{
    class GitCheck
    {
        public ProcessStartInfo psi { get; set; }
        public Process process { get; set; }
        public string GIT_INSTALL_DIRECTORY { get; set; } = @"C:\Program Files\Git";


        public GitCheck()
        {
            setProcessStartInfo(@"..\..\..\..\..\YoCode");
            openProcess();
            //printOutput(getOutput());
            parseOutput(getOutput());

        }

        public void setProcessStartInfo(String PATH)
        {
            psi = new ProcessStartInfo();
            psi.CreateNoWindow = true;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            psi.WorkingDirectory = PATH;
            psi.FileName = GIT_INSTALL_DIRECTORY + @"\bin\git.exe";
            psi.Arguments = "log ";


        }




        public void openProcess()
        {
            process = new Process();
            process.StartInfo = psi;
            process.Start();
        }


        public string getOutput()
        {
            return process.StandardOutput.ReadToEnd();
        }

        public void printOutput(String output)
        {
            Console.Write(output);
        }

        public void parseOutput(String output)
        {
            //getAuthorList(output);
            Console.WriteLine(getLastAuthor(output));
            Console.WriteLine(gitHasBeenUsed(getLastAuthor(output)));


        }

        private List<String> getAuthorList(string output)
        {
            StringReader sr = new StringReader(output);
            List<String> authors = new List<String>();
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains("Author"))
                {
                    authors.Add(line);
                }
            }
            return authors;
        }

        private string getLastAuthor(string output)
        {
            StringReader sr = new StringReader(output);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains("Author"))
                {
                    return line;
                }
            }
            return "No commits/authors found";
        }

        private bool gitHasBeenUsed(string lastAuthor)
        {
            if(lastAuthor.Contains("@nonlinear.com") || lastAuthor.Contains("@waters.com"))
            {
                return false;
            }
            else
            {
                return true;
            }


        }



    }
}
