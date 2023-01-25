using System.Collections.Generic;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Benchmarks.ForeachAsyncFireAndForget;

namespace Benchmarks.App;

[MemoryDiagnoser]
[RankColumn]
public class ForeachAsyncFireAndForgetBenchmark
{
    public IEnumerable<string> ValuesForIterate => new[] { "tiff", "avi" };

    [Benchmark]
    public async Task FireAndForget()
    {
        await new FireAndForget().FireAndForgetTasks(ValuesForIterate);
    }

    [Benchmark]
    public async Task ForeachAsync()
    {
        await new ForeachAsync().DoForeachAsync(ValuesForIterate);
    }
}
