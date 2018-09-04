using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace YoCode
{
    internal class UICodeCheck : ICheck
    {
        // -------------------------------------------------------------------------------------------- Constructors
        public UICodeCheck(string[] keyWords, ICheckConfig checkConfig)
        {
            var modifiedHtmlFiles = checkConfig.PathManager.GetFilesInDirectory(checkConfig.PathManager.ModifiedTestDirPath, FileTypes.html).ToList();
            UIContainsFeature(modifiedHtmlFiles, keyWords);
            UIEvidence.Feature = Feature.UICodeCheck;
            UIEvidence.HelperMessage = messages.UICodeCheck;

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
                    UIEvidence.SetPassed(new SimpleEvidenceBuilder($"Found  on line {i + 1} in file \\{new DirectoryInfo(userFilePath).Parent.Name}\\{Path.GetFileName(userFilePath)}"));
                    UIEvidence.FeatureRating = 1;
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
        private FeatureEvidence UIEvidence { get; } = new FeatureEvidence();

        public IEnumerable<FeatureEvidence> Execute()
        {
            return new[] { UIEvidence };
        }
    }
}