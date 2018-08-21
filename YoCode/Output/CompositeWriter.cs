using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace YoCode
{
    class CompositeWriter:IPrint
    {
        IEnumerable<IPrint> writers;

        public CompositeWriter(IEnumerable<IPrint> writers)
        {
            this.writers = writers;
        }

        public void AddFeature(FeatureData data)
        {
            foreach(var writer in writers)
            {
                writer.AddFeature(data);
            }
        }

        public void AddMessage(string msg)
        {
            foreach(var writer in writers)
            {
                writer.AddMessage(msg);
            }
        }

        public void AddBanner()
        {
            foreach(var writer in writers)
            {
                writer.AddBanner();
            }
        }

        public void WriteReport()
        {
            foreach(var writer in writers)
            {
                writer.WriteReport();
            }
        }

        public void AddFinalScore(double score)
        {
            foreach(var writer in writers)
            {
                writer.AddFinalScore(score);
            }
        }
    }
}
