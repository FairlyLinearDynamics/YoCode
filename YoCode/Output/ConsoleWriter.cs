using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    public class ConsoleWriter : IPrint,IErrorReporter
    {
        StringBuilder consoleReport = new StringBuilder();

        public void PrintErrors(IEnumerable<string> errs)
        {
            foreach(var err in errs)
            {
                consoleReport.AppendLine(err);
            }
            consoleReport.AppendLine(messages.Divider);
            consoleReport.AppendLine(messages.AskForHelp);
            WriteReport();
        }

        public void AddFinalScore(double score)
        {
            consoleReport.AppendLine("Total Score: "+score);
            consoleReport.AppendLine(messages.Divider);
        }

        public void AddFeature(FeatureData data)
        {
            consoleReport.AppendLine(data.title);
            consoleReport.AppendLine(data.featureResult);
            foreach(var evidence in data.evidence)
            {
                consoleReport.AppendLine(evidence);
            }
            consoleReport.AppendLine(messages.Divider);
        }

        public void AddMessage(string msg)
        {
            AddNewBlock(msg);
        }

        public void AddBanner()
        {
            AddNewBlock(messages.ConsoleFireplaceBanner);
        }

        private void AddNewBlock(string text)
        {
            consoleReport.AppendLine(text);
            consoleReport.AppendLine(messages.Divider);
        }

        public void WriteReport()
        {
            Console.Write(consoleReport);
            consoleReport.Clear();
        }
    }
}
