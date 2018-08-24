using Microsoft.Extensions.Configuration;

namespace YoCode
{
    internal interface IAppSettingsBuilder
    {
        IConfiguration ReadJSONFile();

        string GetCMDToolsPath();
        string GetDotCoverDir();
        string ReturnPathByMode(TestType mode);
        (string, string) GetOriginalCosts();
        (string, string) GetJuniorCosts();
    }
}