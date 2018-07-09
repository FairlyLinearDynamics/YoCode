using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YoCode
{
    public class UICheck
    {
        public UICheck(IEnumerable<string> userFilePaths, string[] keyWords)
        {
            UIContainsFeature(userFilePaths, keyWords);
        }

        public UICheck(string userFilePath, string[] keyWords)
        {
            UIContainsFeature(userFilePath, keyWords);
        }

        private void UIContainsFeature(string userFilePath, string[] keyWords)
        {
            var userFile = File.ReadAllLines(userFilePath);

            for(var i=0; i<userFile.Length; i++)
            {
                if (ContainsKeyWord(userFile[i], keyWords)) 
                {
                    // TODO: Show from which file below lines are taken
                    ListOfMatches.Add(i + 1);
                }
            }
        }

        private void UIContainsFeature(IEnumerable<string> userFilePaths, string[] keyWords)
        {
            foreach (var path in userFilePaths)
            {
                UIContainsFeature(path, keyWords);
            }
        }

        public static bool ContainsKeyWord(string line, IEnumerable<string> keyWords)
        {
            return keyWords.Any(key => line.ToLower().Contains(key));
        }

        public List<int> ListOfMatches { get; } = new List<int>();
        
    }
}
