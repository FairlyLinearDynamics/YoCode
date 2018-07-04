using System;
using System.IO;

namespace YoCode
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to YoCode!");

            Console.WriteLine("Current directory: " + Directory.GetCurrentDirectory() + "\n");


            FileImport fi = new FileImport();

            fi.Print();


        }
    }
}
