﻿using System;
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

            Console.WriteLine(gc.ExecuteTheCheck());

            Console.Read();
        }
    }
}
