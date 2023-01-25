using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Benchmarks.ForeachAsyncFireAndForget;
public class FireAndForget
{
    public async Task FireAndForgetTasks(IEnumerable<string> listToIterate)
    {
        foreach (var systemDestination in listToIterate)
        {
            _ = Task.Run(async () =>
            {
                await new DummyExecutor().DoSomething(CancellationToken.None);
            }, CancellationToken.None);
        }
    }
}
