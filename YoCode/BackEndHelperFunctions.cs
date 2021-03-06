﻿using System;
using System.Collections.Generic;
namespace YoCode
{
    internal static class BackEndHelperFunctions
    {     
        public static List<string> GetActionLines(string file)
        {
            return file.GetMultipleLinesWithAllKeywords(GetActionKeywords());
        }

        public static List<string> GetListOfActions(string HTMLfile,string from,string to)
        {
            var actionlines = GetActionLines(HTMLfile);
            return ExtractActionsFromList(actionlines,from,to);
        }

        public static List<string> ExtractActionsFromList(List<string> actionLines,string from,string to)
        {
            var list = new List<string>();

            foreach (var line in actionLines)
            {
                var res = line.GetStringBetweenStrings(from, to);

                list.Add(res);
            }
            return list;
        }

        public static List<double> MakeConversion(List<double> inputs, double mult)
        {
            var list = new List<double>();
            foreach (var x in inputs)
            {
                list.Add(x * mult);
            }
            return list;
        }

        public static List<string> GetActionKeywords()
        {
            return new List<string> { "action", "value" };
        }

        public static List<double> CheckActions(string action,Dictionary<List<string>, List<double>> KeywordMap)
        {
            foreach (var keywords in KeywordMap)
            {
                if (action.ToLower().ContainsAny(keywords.Key))
                {
                    return keywords.Value;
                }
            }
            return new List<double> { 0.1 };
        }

    }
}