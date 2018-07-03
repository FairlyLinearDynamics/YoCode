using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace YoCode
{
    public class UICheck
    {
        public static bool UIContainsFeature(String userFileURL, String[] keyWords)
        {
            var userFile = File.ReadAllText(userFileURL);
            var comp = StringComparison.OrdinalIgnoreCase;

            foreach(String key in keyWords)
            {
                if (userFile.Contains(key,comp))
                    return true;
            }
            return false;
        }
    }
}
