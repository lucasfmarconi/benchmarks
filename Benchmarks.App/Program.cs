using System;
using BenchmarkDotNet.Running;

namespace Benchmarks.App;

internal class Program
{
    static void Main(string[] args)
    {
#if DEBUG
        new JsonMergeBenchmark().AddNodeWithSystemTextJsonStrategy();
        new JsonMergeBenchmark().AddNodeWithNewtonsoftJsonStrategy();
        new ByteArrayValueSetBenchmark().ConvertByteArrayUsingReflection();
        new ByteArrayValueSetBenchmark().ConvertByteArrayManually();
#else
        BenchmarkRunner.Run<JsonMergeBenchmark>();
        BenchmarkRunner.Run<ByteArrayValueSetBenchmark>();
        BenchmarkRunner.Run<IterateForeachLinqBenchmark>();
#endif
        Console.ReadKey();
    }
}
