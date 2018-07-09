using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YoCode
{
    public class UICheck
    {
        public UICheck(List<string> userFilePaths, string[] keyWords)
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

        private void UIContainsFeature(List<string> userFilePaths, string[] keyWords)
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

        public List<int> ListOfMatches { get; private set; } = new List<int>();
        
    }
}
