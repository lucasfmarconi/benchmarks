using System.Collections.Generic;

namespace Benchmarks.ForeachAndLinqIterations;

public interface IIterate
{
    public void Iterate(List<double> doubleList);
}
