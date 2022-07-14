using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Benchmarks.JsonOperators;
public class JsonObjectNewtonsoftStrategy
{

    public void MergeJsonObjects(string jsonObjectA, string jsonObjectB)
    {
        var request = JObject.Parse(jsonObjectA);
        var header = JObject.Parse(jsonObjectB);

        request.Merge(header);

        var requestJson = request.ToString();

        var jObject = JsonConvert.DeserializeObject<ExpandoObject>(requestJson);

        var serializer = new JsonSerializer()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        var jo = JObject.FromObject(jObject, serializer);

        var jsonObjectString = jo.ToString();
    }
}
