using System;
using System.IO;

namespace YoCode
{
    public class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to YoCode!");

            Console.WriteLine("");
            Console.WriteLine("");

            GitCheck gc = new GitCheck();
            gc.ExecuteTheCheck();
            //gc.printDetailedResults();
            Console.Write(gc.Output);
            Console.Read();
        }
    }
}
