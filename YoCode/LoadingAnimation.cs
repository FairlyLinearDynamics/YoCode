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
            PrintIntro();
            var cursorPos = Console.CursorTop;
            var loadingBanner = new StringBuilder();
            loadingBanner.AppendLine(messages.ConsoleFireplaceBanner);
            loadingBanner.AppendLine(messages.ParagraphDivider);
            loadingBanner.AppendLine(messages.LoadingMessage);
            loadingBanner.AppendLine(messages.ParagraphDivider);
            loadingBanner.AppendLine();

            Console.Write(loadingBanner);

            while (true)
            {
                Console.CursorTop--;
                ClearLine();
                Console.WriteLine(String.Format("Loading{0,-3}", dots));
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

        private static void PrintIntro()
        {
            var intro = new StringBuilder();
            intro.Append(messages.Welcome);
            intro.Append(messages.Divider);
            Console.Write(intro);

        }

        public static bool LoadingFinished { get; set; } = false;
    }
}
