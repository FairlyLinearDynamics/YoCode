using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YoCode
{
    public class UICheck
    {
        // -------------------------------------------------------------------------------------------- Constructors
        public UICheck(IEnumerable<string> userFilePaths, string[] keyWords)
        {
            UIContainsFeature(userFilePaths, keyWords);
            UIEvidence.FeatureTitle = "Evidence present in UI";
        }

        public UICheck(string userFilePath, string[] keyWords) : this( new List<string> { userFilePath }, keyWords)
        {
            UIContainsFeature(userFilePath, keyWords);
        }

        // -------------------------------------------------------------------------------------------- Single UI check
        private void UIContainsFeature(string userFilePath, string[] keyWords)
        {
            var userFile = File.ReadAllLines(userFilePath);

            //ListOfMatches.add

            for(var i=0; i<userFile.Length; i++)
            {
                if (ContainsKeyWord(userFile[i], keyWords)) 
                {
                    UIEvidence.FeatureImplemented = true;
                    UIEvidence.GiveEvidence($"Found  on line {i+1} in file \\{new DirectoryInfo(userFilePath).Parent.Name}\\{Path.GetFileName(userFilePath)}");

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
            
            return keyWords.Any(key => line.ToLower().Contains(key));
        }

        // -------------------------------------------------------------------------------------------- Return methods
        public FeatureEvidence UIEvidence { get; private set; } = new FeatureEvidence();
    }
}
