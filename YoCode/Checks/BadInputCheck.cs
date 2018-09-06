using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoCode
{
    internal class BadInputCheck : ICheck
    {
        private readonly string port;
        private Dictionary<string, string> badInputs;

        List<bool> BadInputBoolResults;

        private List<string> actions;
        
        private const int TitleColumnFormatter = -30;
        private const int ValueColumnFormatter = -15;

        private StringBuilder badInputResultsOutput= new StringBuilder();

        public BadInputCheck(string port)
        {
            this.port = port;
            InitializeDataStructures();
        }

        private void InitializeDataStructures()
        {
            var fetcher = new HTMLFetcher(port);
            var htmlCode = fetcher.GetHTMLCodeAsString();

            var handler = new BackEndHelperFunctions();

            actions = handler.GetListOfActions(htmlCode, "value=\"", "\"");

            badInputs = new Dictionary<string, string>
            {
                { "Empty input", " " },
                { "Blank lines at the start", "\n10" },
                { "Blank lines at the middle", "10 \n\n 10" },
                { "Blank lines at the end", "10 \n\n" },
                { "Not numbers", "Y..@" }
            };

            BadInputBoolResults = new List<bool>();
        }        

        public bool BadInputsAreFixed(List<string> badInputResults)
        {
            bool ret = true;

            badInputResultsOutput.AppendLine(string.Format($"\n{"Input name",TitleColumnFormatter} {"FIXED",ValueColumnFormatter}"));
            badInputResultsOutput.AppendLine(messages.ParagraphDivider);

            foreach (var a in badInputs)
            {
                bool isFixed = !badInputResults.Contains(a.Key);
                BadInputBoolResults.Add(isFixed);

                if (!isFixed)
                {
                    ret = false;
                }

                badInputResultsOutput.AppendLine(string.Format($"{a.Key,TitleColumnFormatter} {isFixed,ValueColumnFormatter}"));
            }
            return ret;
        }

        private double GetBadInputCheckRating()
        {
            return HelperMethods.GetRatingFromBoolList(BadInputBoolResults);
        }

        public Task<List<FeatureEvidence>> Execute()
        {
            return Task.Run(() =>
            {
                BadInputCheckEvidence.Feature = Feature.BadInputCheck;
                BadInputCheckEvidence.HelperMessage = messages.BadInputCheck;

                if (string.IsNullOrEmpty(port))
                {
                    BadInputCheckEvidence.SetInconclusive(new SimpleEvidenceBuilder(messages.BadPort));
                    return new List<FeatureEvidence> { BadInputCheckEvidence };
                }

                try
                {
                    var fetcher = new HTMLFetcher(port);

                    var htmlCode = fetcher.GetHTMLCodeAsString();
                    InitializeDataStructures();

                    var badInputResults = fetcher.GetBadInputs(badInputs, actions[0]);

                    if (BadInputsAreFixed(badInputResults))
                    {
                        BadInputCheckEvidence.SetPassed(new SimpleEvidenceBuilder(badInputResultsOutput.ToString()));
                    }
                    else
                    {
                        BadInputCheckEvidence.SetFailed(new SimpleEvidenceBuilder(badInputResultsOutput.ToString()));
                    }

                    BadInputCheckEvidence.FeatureRating = GetBadInputCheckRating();
                }
                catch (Exception)
                {
                    BadInputCheckEvidence.SetInconclusive(new SimpleEvidenceBuilder(badInputResultsOutput.ToString()));
                }

                return new List<FeatureEvidence> { BadInputCheckEvidence};
            });
        }

        private FeatureEvidence BadInputCheckEvidence { get; } = new FeatureEvidence();
    }
}