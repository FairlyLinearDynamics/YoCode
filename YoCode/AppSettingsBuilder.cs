
using Microsoft.Extensions.Configuration;
using System.IO;

namespace YoCode
{
    internal class AppSettingsBuilder : IAppSettingsBuilder
    {
        private static IConfiguration configuration;
        private readonly bool juniorTest;

        public AppSettingsBuilder(bool juniorTest)
        {
            this.juniorTest = juniorTest;
        }

        public IConfiguration ReadJSONFile()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            configuration = builder.Build();
            return configuration;
        }

        public string GetCMDToolsPath()
        {
            return configuration["duplicationCheckSetup:CMDtoolsDir"];
        }

        public string GetDotCoverDir()
        {
            return configuration["codeCoverageCheckSetup:dotCoverDir"];
        }

        public string GetWeightingsPath()
        {
            return juniorTest ? configuration["featureWeightings:Junior"] : configuration["featureWeightings:Original"];
        }

        public (string,string) GetCodebaseCosts()
        {
            return juniorTest ? (configuration["JuniorTest:CodeBaseCost"], configuration["JuniorTest:DuplicationCost"]) : (configuration["OriginalTest:CodeBaseCost"], configuration["OriginalTest:DuplicationCost"]);
        }

    }
}
