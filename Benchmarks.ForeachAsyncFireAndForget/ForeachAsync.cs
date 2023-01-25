using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Benchmarks.ForeachAsyncFireAndForget;
public class ForeachAsync
{
    public async Task DoForeachAsync(IEnumerable<string> listToIterate)
    {
        var parallelOptions = new ParallelOptions
        {
            CancellationToken = CancellationToken.None,
            MaxDegreeOfParallelism = listToIterate.Count()
        };

        _ = Parallel.ForEachAsync(listToIterate, parallelOptions, async (iteration, cancellationToken) =>
        {
            await new DummyExecutor().DoSomething(cancellationToken);
        });
    }
}
