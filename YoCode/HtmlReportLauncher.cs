using System.Diagnostics;
using System.Runtime.InteropServices;

namespace YoCode
{
    static class HtmlReportLauncher
    {
        public static bool LaunchReport(string nameOfReportFile)
        {
            try
            {
                Process.Start(nameOfReportFile);
                return true;
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {nameOfReportFile}") { CreateNoWindow = true });
                    return true;
                }
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", nameOfReportFile);
                    return true;
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", nameOfReportFile);
                    return true;
                }
                else
                {
                    return false;
                    throw;
                }
            }
        }
    }
}
