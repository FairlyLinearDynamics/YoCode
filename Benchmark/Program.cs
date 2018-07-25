using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Benchmark
{
    public class DupfinderCheck
    {
        [Benchmark]
        public void TestDupFinder()
        {
            new YoCode.DuplicationCheck();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
