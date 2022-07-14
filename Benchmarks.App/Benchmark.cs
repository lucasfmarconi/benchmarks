using BenchmarkDotNet.Attributes;
using Benchmarks.JsonOperators;

namespace Benchmarks.App;

[MemoryDiagnoser]
[RankColumn]
public class Benchmark
{
    private const string JsonContentA =
        "{\"machineName\": \"56 Kampf CLS\",\"mainPOrderName\": \"0801998-S\",\"partOrderName\": \"0801998\",\"qsv\": \"7185-846-B5\",\"packs\": 4094000,\"materialName\": \"R1506-583\",\"plannedQuantity\": 185185}";

    private const string JsonContentB =
        "{\"responseHeader\":{\"messageId\":\"442b5a7c-3e72-419a-acb6-aaa282c3ff34\",\"isOk\":true,\"resultMessage\":\"0:\",\"dateCreated\":\"2022-07-0807:50:09.564\",\"dateCreatedLocal\":\"2022-07-0809:50:09.564\",\"sourceName\":\"plannedOrderData\"}}";


    [Benchmark]
    public void AddNodeWithSystemTextJsonStrategy()
    {
        
        new JsonObjectSystemTextJsonStrategy().MergeJsonObjects(JsonContentA, JsonContentB);
    }

    [Benchmark]
    public void AddNodeWithNewtonsoftJsonStrategy()
    {
        new JsonObjectNewtonsoftStrategy().MergeJsonObjects(JsonContentA, JsonContentB);
    }
}
