using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YoCode
{
    public class UICheck
    {
        public UICheck(List<String> userFilePaths, String[] keyWords)
        {
            UIContainsFeature(userFilePaths, keyWords);
        }

        public UICheck(String userFilePath, String[] keyWords)
        {
            UIContainsFeature(userFilePath, keyWords);
        }

        private void UIContainsFeature(String userFilePath, String[] keyWords)
        {
            var userFile = File.ReadAllLines(userFilePath);

            for(var i=0; i<userFile.Length; i++)
            {
                // Output code line where this keyword was found.
                if (ContainsKeyWord(userFile[i], keyWords)) 
                {
                    ListOfMatches.Add(i+1);
                }
            }
        }

        private void UIContainsFeature(List<string> userFilePaths, String[] keyWords)
        {
            foreach (string path in userFilePaths)
            {
                UIContainsFeature(path, keyWords);
            }
        }

        private bool ContainsKeyWord(string line, string[] keyWords)
        {
            foreach (string key in keyWords)
            {
                if (line.ToLower().Contains(key))
                    return true;
            }
            return false;
        }

        public bool ContainsFeature => ListOfMatches.Any();

        public List<int> ListOfMatches { get; private set; } = new List<int>();
        
    }
}
