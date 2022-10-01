using BenchmarkDotNet.Attributes;
using Benchmarks.ByteArrayConverters;

namespace Benchmarks.App;

[MemoryDiagnoser]
[RankColumn]
public class ByteArrayValueSetBenchmark
{
    private readonly byte[] inputArrayBytes =
    {
        2, 0, 0, 0, 78, 79, 163, 235, 220, 126, 63, 156, 216, 1, 0, 2, 0, 0, 0, 78, 79, 2, 0, 0, 0, 78, 79, 2, 0, 0,
        0, 78, 79, 2, 0, 0, 0, 78, 79
    };
    
    [Benchmark]
    public void ConvertByteArrayUsingReflection() => PlcTypeConverter.ConvertFromBytesToUserDefinedType(new TypeExample(), inputArrayBytes);

    [Benchmark]
    public void ConvertByteArrayManually() => ManuallyTypeConverter.ConvertFromBytesToTypesManually(new TypeExample(), inputArrayBytes);
}
