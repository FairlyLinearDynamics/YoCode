﻿using System;

namespace YoCode
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to YoCode!");

            Console.WriteLine("");
            Console.WriteLine("");

            // Temporary calls to UICheck
            var keys = new String[] { "Miles", "Kilometers", "Km", "km" };
            Console.Write(UICheck.UIContainsFeature("C:\\Users\\ukekar\\source\\repos\\YoCode\\YoCode\\input\\compareTo.cshtml", keys));
            Console.WriteLine();
        }
    }
}
