using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace YoCode
{
    public class LoadingAnimation
    {
        static string dots;
        public static void RunLoading()
        {
            while (true)
            {
                Thread.Sleep(1000);

                ClearLine();
                Console.Write($"[Loading{dots}]");
                Console.CursorLeft = 0;
                dots += ".";
                if (dots.Equals("...."))
                {
                    dots = "";
                }
                if (LoadingFinished)
                {
                    ClearLine();
                    break;
                }
            }
        }

        private static void ClearLine()
        {
            Console.CursorLeft = 0;
            Console.Write(new string(' ', Console.WindowWidth));
            Console.CursorTop--;
        }

        public static bool LoadingFinished { get; set; } = false;
    }
}
