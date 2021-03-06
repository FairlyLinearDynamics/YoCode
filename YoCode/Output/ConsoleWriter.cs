﻿using System;
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

        public void AddTestType(bool isJunior)
        {
            consoleReport.AppendLine($"Test type: { (isJunior ? "Junior" : "Original")}");
            consoleReport.AppendLine(messages.Divider);
        }

        public void AddFeature(FeatureData data)
        {
            consoleReport.AppendLine(data.title);
            consoleReport.AppendLine(data.featureResult);
            consoleReport.AppendLine(data.featureEvidence.BuildEvidenceForConsole());
            consoleReport.AppendLine(messages.Divider);
        }

        public void AddMessage(string msg)
        {
            AddNewBlock(msg);
        }

        public void AddBanner()
        {
            AddNewBlock(messages.ConsoleFireplaceBannerFrame1);
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
