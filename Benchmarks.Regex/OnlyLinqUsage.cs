using System.Collections.Generic;
using System.Linq;

namespace Benchmarks.Regex;

public class OnlyLinqUsage
{
    public void Filter(IEnumerable<string> namesList)
    {
        var filteredList = namesList.Where(item => item.Contains(".json") && !item.Contains("_done"));
    }
}
