using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoCode
{
    public class ConsoleWriter : IPrint
    {
        StringBuilder consoleReport = new StringBuilder();

        public void AddErrs(IEnumerable<string> errs)
        {
            foreach(var err in errs)
            {
                consoleReport.AppendLine(err);
            }
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

        public void AddIntro(string intro)
        {
            AddNewBlock(intro);
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
