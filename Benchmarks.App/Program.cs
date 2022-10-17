using System;
using BenchmarkDotNet.Running;

namespace Benchmarks.App;

internal class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<JsonMergeBenchmark>();
        BenchmarkRunner.Run<ByteArrayValueSetBenchmark>();
        BenchmarkRunner.Run<IterateForeachLinqBenchmark>();
        BenchmarkRunner.Run<RegexBenchmark>();
        Console.ReadKey();
    }
}
