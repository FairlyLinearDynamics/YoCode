using System;
using System.Text;
using System.Threading;

namespace YoCode
{
    public static class LoadingAnimation
    {
        private static string dots;
        static int cursorStartPos;
        static int cursorStopPos;

        public static void RunLoading()
        {
            PrintIntro();
            cursorStartPos = Console.CursorTop;

            var loadingBanner = new StringBuilder();
            var fireplaceFrames = new string[] { messages.ConsoleFireplaceBannerFrame1,
                messages.ConsoleFireplaceBannerFrame2, messages.ConsoleFireplaceBannerFrame3};

            var fireplaceInd = 0;

            while (true)
            {
                Console.CursorTop = cursorStartPos;
                Console.WriteLine(fireplaceFrames[fireplaceInd]);
                Console.WriteLine(messages.ParagraphDivider);
                Console.WriteLine(messages.LoadingMessage);
                Console.WriteLine(messages.ParagraphDivider);
                Console.WriteLine();

                //Console.Write(loadingBanner);

                Console.Write(String.Format("Loading{0,-3}", dots));
                cursorStopPos = Console.CursorTop;
                dots += ".";
                if (dots.Equals("...."))
                {
                    dots = "";
                }
                if (LoadingFinished)
                {
                    ClearLine();
                    Console.CursorTop = cursorStartPos;
                    break;
                }

                if (fireplaceInd > 1)
                {
                    fireplaceInd = 0;
                }
                else
                {
                    fireplaceInd++;
                }
                Thread.Sleep(1000);
            }
        }

        private static void ClearLine()
        {
            Console.CursorTop = cursorStopPos;
            while (Console.CursorTop >= cursorStartPos)
            {
                Console.CursorLeft = 0;
                Console.Write(new string(' ', Console.WindowWidth));
                Console.CursorTop--;
                Console.CursorTop--;
            }
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
