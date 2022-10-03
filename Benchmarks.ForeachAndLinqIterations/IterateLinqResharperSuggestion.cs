using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Benchmarks.ForeachAndLinqIterations;
public class IterateLinqResharperSuggestion : IIterate
{
    public void Iterate(List<double> doubleList)
    {
        foreach (var doubleString in doubleList.Select(value => value.ToString(CultureInfo.InvariantCulture)))
        {
            var yepToDoNothing = doubleString;
        }
    }
}
