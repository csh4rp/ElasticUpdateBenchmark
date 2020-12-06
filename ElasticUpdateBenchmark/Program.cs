using System;
using System.Threading;
using System.Threading.Tasks;

namespace ElasticUpdateBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkDotNet.Running.BenchmarkRunner.Run<Benchmark>();
        }
    }
}