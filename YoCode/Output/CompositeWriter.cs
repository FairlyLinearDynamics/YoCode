﻿using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    class CompositeWriter:IPrint
    {
        IEnumerable<IPrint> writers;

        public CompositeWriter(IEnumerable<IPrint> writers)
        {
            this.writers = writers;
        }

        public void AddErrs(IEnumerable<string> errors)
        {
            foreach(var writer in writers)
            {
                writer.AddErrs(errors);
            }
        }

        public void AddFeature(FeatureData data)
        {
            foreach(var writer in writers)
            {
                writer.AddFeature(data);
            }
        }

        public void AddIntro(string intro)
        {
            foreach(var writer in writers)
            {
                writer.AddIntro(intro);
            }
        }

        public void AddMessage(string msg)
        {
            foreach(var writer in writers)
            {
                writer.AddMessage(msg);
            }
        }

        public void WriteReport()
        {
            foreach(var writer in writers)
            {
                writer.WriteReport();
            }
        }
    }
}