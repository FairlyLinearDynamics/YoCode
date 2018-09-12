using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

// This is warning about things that are done implicitly by Newtonsoft (I think)
#pragma warning disable 649

namespace YoCode
{
    public static class DotCover
    {
        private struct ReportNode
        {
            public string Kind;
            public string Name;
            public int CoveredStatements;
            public int TotalStatements;
            public int CoveragePercent;
            public List<ReportNode> Children;
        }

        private struct ReportRoot
        {
            public string DotCoverVersion;
            public string Kind;
            public int CoveredStatements;
            public int TotalStatements;
            public int CoveragePercent;
            public List<ReportNode> Children;
        }

        public static int CalculateCoverageFromJsonReport(string json)
        {
            var coverageReport = JsonConvert.DeserializeObject<ReportRoot>(json);

            var webAppAssembly = coverageReport.Children.Single(c => c.Kind == "Assembly" && c.Name == "UnitConverterWebApp");
            var allTypesInWebapp = webAppAssembly.Children.SelectMany(n => n.Children.Where(c => c.Kind == "Type"));
            var filteredTypesInWebApp = allTypesInWebapp.Where(t => t.Name != "Program" && t.Name != "Startup").ToList();
            var totalRelevantStatements = filteredTypesInWebApp.Sum(t => t.TotalStatements);
            var coveredRelevantStatements = filteredTypesInWebApp.Sum(t => t.CoveredStatements);

            return (int)(coveredRelevantStatements * 100.0 / totalRelevantStatements);
        }
    }
}
