﻿using System;
using System.Collections.Generic;
using System.Globalization;
// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator

namespace Benchmarks.ForeachAndLinqIterations;
public class IterateForeach : IIterate
{
    public void Iterate(List<double> doubleList)
    {
        foreach (var value in doubleList)
        {
            var doubleString = value.ToString(CultureInfo.InvariantCulture);
            var yepToDoNothing = doubleString;
        }
    }
}
