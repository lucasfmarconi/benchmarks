using System;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.App;

[MemoryDiagnoser]
[RankColumn]
public class RegexBenchmark
{
    public string StringToExtractPattern = "P2WebService/service/orders/orderinfo?p2ReelId={injector.frompayload[.p2ReelId]}";

    [Benchmark]
    public void ExtractUsingRegex()
    {
        var stringArray = System.Text.RegularExpressions.Regex.Split(StringToExtractPattern, @"=");
        if (stringArray.Length > 0)
            StringToExtractPattern = stringArray[0];
    }

    [Benchmark]
    public void ExtractUsingCSharp()
    {
        var indexOfDelimiter = StringToExtractPattern.IndexOf("=", StringComparison.Ordinal);
        if (indexOfDelimiter >= 0)
            StringToExtractPattern = StringToExtractPattern[..(indexOfDelimiter + 1)];
    }
}
