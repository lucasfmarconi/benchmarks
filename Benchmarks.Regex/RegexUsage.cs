using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Benchmarks.Regex;

public class RegexUsage
{
    public void Filter(IEnumerable<string> namesList)
    {
        var reg = new System.Text.RegularExpressions.Regex("(.+)(_done)(.json)$");

        var filteredList = namesList.Where(item => !reg.IsMatch(item));
    }
}
