using System;
using BenchmarkDotNet.Running;

namespace Benchmarks.App;

internal class Program
{
    static void Main(string[] args)
    {
#if DEBUG
        new Benchmark().AddNodeWithSystemTextJsonStrategy();
        new Benchmark().AddNodeWithNewtonsoftJsonStrategy();
#else
        BenchmarkRunner.Run<Benchmark>();
#endif
        Console.ReadKey();
    }
}
