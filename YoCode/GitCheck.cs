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
            open();

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




        public void open()
        {
            process = new Process();
            process.StartInfo = psi;        
            process.Start();

            //string errString = process.StandardError.ReadToEnd();
            //string stdString = process.StandardOutput.ReadToEnd();

            var isRunning = !process.HasExited;

            //while(isRunning = true) { 
                string line = process.StandardOutput.ReadToEnd();
                Console.WriteLine(line);
                //process.WaitForExit();
            //}

            // Console.WriteLine(stdString);
            Console.ReadLine();

        }


    }
}
