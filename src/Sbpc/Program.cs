using Sbpc.Serialization;
using Sbpc.Serialization.JsonConverters;
using System.Text.Json;

Blueprint blueprint = Serialization.ReadBlueprintFile("../../../../../samples/Swatches.sbp");

JsonSerializerOptions serializerOptions = new()
{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    PropertyNameCaseInsensitive = false,
    Converters =
    {
        new JsonConverterVector3(),
        new JsonConverterQuaternion(),
    },
};

await File.WriteAllTextAsync("../../../../../samples/test.json", JsonSerializer.Serialize(blueprint, serializerOptions));

Serialization.WriteBlueprintFile("../../../../../samples/TEST.sbp", blueprint);