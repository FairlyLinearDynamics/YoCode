
using Microsoft.Extensions.Configuration;
using System.IO;

namespace YoCode
{
    internal class AppSettingsBuilder : IAppSettingsBuilder
    {
        private static IConfiguration Configuration;

        public IConfiguration ReadJSONFile()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            return Configuration;
        }

        public string GetCMDToolsPath()
        {
            return Configuration["duplicationCheckSetup:CMDtoolsDir"];
        }

        public string GetDotCoverDir()
        {
            return Configuration["codeCoverageCheckSetup:dotCoverDir"];
        }
    }
}
