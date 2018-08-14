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
            var cursorPos = Console.CursorTop;
            Console.WriteLine(messages.Fireplace);
            Console.WriteLine(messages.ParagraphDivider);
            Console.WriteLine($"Get comfortable, YoCode is about to finish gathering{Environment.NewLine}your results");
            Console.WriteLine(messages.ParagraphDivider);

            while (true)
            {
                ClearLine();
                Console.Write(String.Format("-----------------------[Loading{0,-3}]----------------------", dots));
                Console.CursorLeft = 0;
                dots += ".";
                if (dots.Equals("...."))
                {
                    dots = "";
                }
                if (LoadingFinished)
                {
                    ClearLine();
                    while (Console.CursorTop > cursorPos)
                    {
                        Console.CursorTop--;
                        ClearLine();
                    }
                    break;
                }
                Thread.Sleep(1000);
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
