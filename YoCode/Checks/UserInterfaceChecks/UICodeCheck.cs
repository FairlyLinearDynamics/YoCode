using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YoCode
{
    internal class UICodeCheck : ICheck
    {
        private readonly string[] keyWords;
        private readonly ICheckConfig checkConfig;

        public UICodeCheck(string[] keyWords, ICheckConfig checkConfig)
        {
            this.keyWords = keyWords;
            this.checkConfig = checkConfig;
            UIEvidence.Feature = Feature.UICodeCheck;
            UIEvidence.HelperMessage = messages.UICodeCheck;
        }

        private void UIContainsFeature(string userFilePath)
        {
            var userFile = File.ReadAllLines(userFilePath);

            for (var i = 0; i < userFile.Length; i++)
            {
                if (ContainsKeyWord(userFile[i], keyWords))
                {
                    UIEvidence.SetPassed(new SimpleEvidenceBuilder($"Found  on line {i + 1} in file \\{new DirectoryInfo(userFilePath).Parent.Name}\\{Path.GetFileName(userFilePath)}"));
                    UIEvidence.FeatureRating = 1;
                }
            }
        }

        private void UIContainsFeature(IEnumerable<string> userFilePaths)
        {
            foreach (var path in userFilePaths)
            {
                UIContainsFeature(path);
            }
        }

        private static bool ContainsKeyWord(string line, IEnumerable<string> keyWords)
        {
            var words = Regex.Split(line, "[^A-Za-z0-9]").ToList();

            return words.Any(word => keyWords.ToList().Any(keyword => word.Equals(keyword, StringComparison.OrdinalIgnoreCase)));
        }

        private FeatureEvidence UIEvidence { get; } = new FeatureEvidence();

        public Task<List<FeatureEvidence>> Execute()
        {
            return Task.Run(() =>
            {
                var modifiedHtmlFiles = checkConfig.PathManager.GetFilesInDirectory(checkConfig.PathManager.ModifiedTestDirPath, FileTypes.html).ToList();
                UIContainsFeature(modifiedHtmlFiles);
                return new List<FeatureEvidence> {UIEvidence};
            });
        }
    }
}