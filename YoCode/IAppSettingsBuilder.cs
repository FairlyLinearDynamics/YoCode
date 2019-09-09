using Microsoft.Extensions.Configuration;

namespace YoCode
{
    internal interface IAppSettingsBuilder
    {
        IConfiguration ReadJSONFile();

        ToolPath GetDupFinderPath();
        ToolPath GetDotCoverPath();
        string GetWeightingsPath();
        (string, string) GetWebAppCosts();
        (string, string) GetTestsCosts();

    }
}