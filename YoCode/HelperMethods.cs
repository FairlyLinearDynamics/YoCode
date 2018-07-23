﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace YoCode
{
    public static class HelperMethods
    {
        const int RoundTo = 6;

        public static bool ContainsAny(this string line, IEnumerable<string> keywords)
        {
            return keywords.Any(line.Contains);
        }

        public static bool ContainsAll(this string line, IEnumerable<string> keywords)
        {
            return keywords.All(line.Contains);
        }

        public static string GetLineWithAllKeywords(this string output, IEnumerable<String> keywords)
        {
            var sr = new StringReader(output);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.ContainsAll(keywords))
                {
                    return line;
                }
            }
            return "";
        }

        public static List<int> GetNumbersInALine(this string line)
        {
            const string expr = @"\D+";

            string[] numbers = Regex.Split(line, expr);
            var list = new List<int>();
            for (int i = 0; i < numbers.Length; i++)
            {
                int temp;
                if (Int32.TryParse(numbers[i], out temp))
                {
                    list.Add(temp);
                }
            }
            return list;
        }

        public static string GetLineWithOneKeyword(this string line, string keyword)
        {
            return line.GetLineWithAllKeywords(new List<string> { keyword });
        }

        public static bool ListContainsAnyKeywords(this List<string> list, IEnumerable<string> keywords)
        {
            foreach (var keyword in keywords)
            {
                if (list.Contains(keyword))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<string> GetMultipleLinesWithAllKeywords(this string text, List<string> keywords)
        {
            var list = new List<string>();
            var sr = new StringReader(text);
            string line;
            while ((line = sr.ReadLine()) != null){
                if(line.ContainsAll(keywords))
                {
                    list.Add(line);
                }

            }
            return list;
        }
    
        public static string GetStringBetweenStrings(this string line, string fromString,string toString)
        {
            int pFrom = line.IndexOf(fromString) + fromString.Length;
            int pTo = line.LastIndexOf(toString);

            return line.Substring(pFrom, pTo - pFrom);
        }

        public static bool SubjectivelyEquals(this double a, double b)
        {
            double aComp = Math.Round(a, RoundTo);
            double bComp = Math.Round(b, RoundTo);

            if(Math.Abs(aComp - bComp) > 0.000001)
            {
                return false;
            }
            return true;
        }

    }
}