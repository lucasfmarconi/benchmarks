using System.Text.Json;
using System.Text.Json.Nodes;

namespace Benchmarks.JsonOperators;

public class JsonObjectSystemTextJsonStrategy
{

    public void MergeJsonObjects(string jsonObjectA, string jsonObjectB)
    {
        var request = JsonNode.Parse(json: jsonObjectA);
        var requestHeaderPartNode = JsonSerializer.SerializeToNode(jsonObjectB);

        request?.AsObject().Add("requestHeader", requestHeaderPartNode);

        var json = request?.ToJsonString();
    }
}
