
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

        public ToolPath GetDupFinderPath()
        {
            return ToolPath.CreateDupFinderPath(configuration["duplicationCheckSetup:CMDtoolsDir"]);
        }

        public ToolPath GetDotCoverPath()
        {
            return ToolPath.CreateDotCoverPath(configuration["codeCoverageCheckSetup:dotCoverDir"]);
        }

        public string GetWeightingsPath()
        {
            return juniorTest ? configuration["featureWeightings:Junior"] : configuration["featureWeightings:Original"];
        }

        public (string,string) GetWebAppCosts()
        {
            return juniorTest ? (configuration["JuniorTest-App:CodeBaseCost"], configuration["JuniorTest-App:DuplicationCost"]) : (configuration["OriginalTest-App:CodeBaseCost"], configuration["OriginalTest-App:DuplicationCost"]);
        }

        public (string,string) GetTestsCosts()
        {
            return juniorTest ? (configuration["JuniorTest-Tests:CodeBaseCost"], configuration["JuniorTest-Tests:DuplicationCost"]) : (configuration["OriginalTest-Tests:CodeBaseCost"], configuration["OriginalTest-Tests:DuplicationCost"]);
        }

    }
}
