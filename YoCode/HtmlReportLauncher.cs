using System.Diagnostics;
using System.Runtime.InteropServices;

namespace YoCode
{
    static class HtmlReportLauncher
    {
        public static void LaunchReport(string nameOfReportFile)
        {
            try
            {
                Process.Start(nameOfReportFile);
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {nameOfReportFile}") { CreateNoWindow = true });
                    return;
                }
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", nameOfReportFile);
                    return;
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", nameOfReportFile);
                    return;
                }
                else
                {
                    return;
                }
            }
        }
    }
}
