using Microsoft.Extensions.Configuration;

namespace YoCode
{
    internal interface IAppSettingsBuilder
    {
        IConfiguration ReadJSONFile();

        string GetCMDToolsPath();
        string GetDotCoverDir();
        string ReturnPathByMode(TestType mode);
        (string, string) GetJuniorAppCosts();
        (string, string) GetJuniorTestsCosts();
        (string, string) GetOriginalTestsCosts();
        (string, string) GetOriginalAppCosts();
    }
}