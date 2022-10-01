using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Benchmarks.ForeachAndLinqIterations;
// ReSharper disable MemberCanBePrivate.Global

namespace Benchmarks.App;

[MemoryDiagnoser]
[RankColumn]
public class IterateForeachLinqBenchmark
{
    [Params(1000, 100000, 1000000)]
    public int N;

    private double[]? data;

    [GlobalSetup]
    public void GlobalSetup()
    {
        data = new double[N];

        Random randNum = new Random();
        for (int i = 0; i < data.Length; i++)
            data[i] = randNum.Next();
    }

    [Benchmark]
    public void IterateUsingForeach()
    {
        new IterateForeach().Iterate(data.ToList());
    }

    [Benchmark]
    public void IterateUsingResharperSuggestedLinq()
    {
        new IterateLinqResharperSuggestion().Iterate(data.ToList());
    }
}
