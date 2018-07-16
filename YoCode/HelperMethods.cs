using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace YoCode
{
    public static class HelperMethods
    {
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
            string expr = @"\D+";

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

    }
}