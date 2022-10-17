using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Benchmarks.Regex;

namespace Benchmarks.App;

[MemoryDiagnoser]
[RankColumn]
public class RegexBenchmark
{
    [ParamsSource(nameof(ValuesForFileName))]
    public string FileName { get; set; }

    public IEnumerable<string> ValuesForFileName => new[] { "Accumsan.tiff",
        "InFaucibus.avi",
        "NuncCommodoPlacerat.jpeg"
    };

    [Benchmark]
    public void FilterUsingRegex()
    {
        new RegexUsage().Filter(ValuesForFileName);
    }

    [Benchmark]
    public void FilterUsingOnlyLinq()
    {
        new OnlyLinqUsage().Filter(ValuesForFileName);
    }
}
