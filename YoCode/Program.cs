using System;
using System.Collections.Generic;

namespace YoCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var list1 = new List<String>();
            var list2 = new List<String>();

            list1.Add("dummy");
            list2.Add("awdoghlasdkghsdl;fg;adofgh;sdofghado;fghadf;ghadfo;gh;adfog;adofg");

            var dir = new Directory(list1,list2);
            var fc = FileChangeChecker.ProjectIsModified(dir);
            Console.Write(fc);
            FileImport di = new FileImport();

        }
    }
}
