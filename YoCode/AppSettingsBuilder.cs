
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

        public string ReturnPathByMode(TestType mode)
        {
            return mode == TestType.Junior ? Configuration["featureWeightings:Junior"] : Configuration["featureWeightings:Original"];
        }

        public (string,string) GetJuniorTestsCosts()
        {
            return (Configuration["JuniorTest-Tests:CodeBaseCost"], Configuration["JuniorTest-Tests:DuplicationCost"]);
        }

        public (string, string) GetJuniorAppCosts()
        {
            return (Configuration["JuniorTest-App:CodeBaseCost"], Configuration["JuniorTest-App:DuplicationCost"]);
        }

        public (string, string) GetOriginalTestsCosts()
        {
            return (Configuration["OriginalTest-Tests:CodeBaseCost"], Configuration["OriginalTest-Tests:DuplicationCost"]);
        }

        public (string, string) GetOriginalAppCosts()
        {
            return (Configuration["OriginalTest-App:CodeBaseCost"], Configuration["OriginalTest-App:DuplicationCost"]);
        }

    }
}
