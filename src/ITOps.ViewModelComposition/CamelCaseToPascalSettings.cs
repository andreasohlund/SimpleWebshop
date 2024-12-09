namespace ITOps.ViewModelComposition;

using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class CamelCaseToPascalSettings
{
    static readonly JsonSerializerSettings settings;

    static CamelCaseToPascalSettings()
    {
        settings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new List<JsonConverter> { new PascalCaseExpandoObjectConverter() }
        };
    }

    public static JsonSerializerSettings GetSerializerSettings()
    {
        return settings;
    }
}