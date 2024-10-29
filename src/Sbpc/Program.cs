using Sbpc.Serialization;
using System.Numerics;

Blueprint blueprint = Serialization.ReadBlueprintFile("../../../../../samples/OneBlock.sbp");

Actor foundation = Actor.CreateFoundation(new Vector3(6400, -6400, 200));
blueprint.Actors.Add(foundation);

Serialization.WriteBlueprintFile("../../../../../samples/TEST.sbp", blueprint);

//JsonSerializerOptions serializerOptions = new()
//{
//    WriteIndented = true,
//    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
//    PropertyNameCaseInsensitive = false,
//    Converters =
//    {
//        new JsonConverterVector3(),
//        new JsonConverterQuaternion(),
//    },
//};

//await File.WriteAllTextAsync("../../../../../samples/test.json", JsonSerializer.Serialize(blueprint, serializerOptions));