using System.Threading;
using System.Threading.Tasks;

namespace Benchmarks.ForeachAsyncFireAndForget;
internal class DummyExecutor
{
    internal async Task DoSomething(CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            Thread.Sleep(45);
        }, cancellationToken);
    }
}
