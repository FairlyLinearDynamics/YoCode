using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace YoCode
{
    internal class UICodeCheck
    {
        // -------------------------------------------------------------------------------------------- Constructors
        public UICodeCheck(IEnumerable<string> userFilePaths, string[] keyWords)
        {
            UIContainsFeature(userFilePaths, keyWords);
            UIEvidence.FeatureTitle = "Found feature keyword in UI implementation";
            UIEvidence.Feature = Feature.UICodeCheck;
        }

        public UICodeCheck(string userFilePath, string[] keyWords) : this(new List<string> { userFilePath }, keyWords)
        {
            UIContainsFeature(userFilePath, keyWords);
        }

        // -------------------------------------------------------------------------------------------- Single UI check
        private void UIContainsFeature(string userFilePath, string[] keyWords)
        {
            var userFile = File.ReadAllLines(userFilePath);

            //ListOfMatches.add

            for (var i = 0; i < userFile.Length; i++)
            {
                if (ContainsKeyWord(userFile[i], keyWords))
                {
                    UIEvidence.FeatureImplemented = true;
                    UIEvidence.FeatureRating = 1;

                    UIEvidence.GiveEvidence($"Found  on line {i + 1} in file \\{new DirectoryInfo(userFilePath).Parent.Name}\\{Path.GetFileName(userFilePath)}");
                }
            }
        }

        // -------------------------------------------------------------------------------------------- List of UI to check
        private void UIContainsFeature(IEnumerable<string> userFilePaths, string[] keyWords)
        {
            foreach (var path in userFilePaths)
            {
                UIContainsFeature(path, keyWords);
            }
        }

        // -------------------------------------------------------------------------------------------- Check logic
        private static bool ContainsKeyWord(string line, IEnumerable<string> keyWords)
        {
            //Console.WriteLine(keyWords.Select(key => line.ToLower().Contains(key)));
            var words = Regex.Split(line, "[^A-Za-z0-9]").ToList();

            return words.Any(word => keyWords.ToList().Any(keyword => word.Equals(keyword, StringComparison.OrdinalIgnoreCase)));
        }

        // -------------------------------------------------------------------------------------------- Return methods
        public FeatureEvidence UIEvidence { get; } = new FeatureEvidence();
    }
}