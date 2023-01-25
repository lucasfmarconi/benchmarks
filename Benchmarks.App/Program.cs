using System;
using BenchmarkDotNet.Running;

namespace Benchmarks.App;

internal class Program
{
    static void Main(string[] args)
    {
        //new RegexBenchmark().ExtractUsingRegex();
        //new RegexBenchmark().ExtractUsingCSharp();
        //BenchmarkRunner.Run<JsonMergeBenchmark>();
        //BenchmarkRunner.Run<ByteArrayValueSetBenchmark>();
        //BenchmarkRunner.Run<IterateForeachLinqBenchmark>();
        BenchmarkRunner.Run<RegexBenchmark>();
        //BenchmarkRunner.Run<ForeachAsyncFireAndForgetBenchmark>();
        Console.ReadKey();
    }
}
