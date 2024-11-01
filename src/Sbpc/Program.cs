using Sbpc.Core;
using Sbpc.PointGeneration;
using Sbpc.Serialization;
using System.Collections.Generic;
using System.Linq;

Blueprint blueprint = Serialization.ReadBlueprintFile("../../../../../samples/OneBlock.sbp");

ArcGenerator arcGenerator = new();

IEnumerable<Point> points1 = arcGenerator.GeneratePointsAlongXYArc(new(0, 0, 100), 800 * 8 - 400 - 800 * 0, Angle.FromDegrees(0), Angle.FromDegrees(90), 19);
IEnumerable<Point> points2 = arcGenerator.GeneratePointsAlongXYArc(new(0, 0, 100), 800 * 8 - 400 - 800 * 1, Angle.FromDegrees(0), Angle.FromDegrees(90), 19);
IEnumerable<Point> points3 = arcGenerator.GeneratePointsAlongXYArc(new(0, 0, 100), 800 * 8 - 400 - 800 * 2, Angle.FromDegrees(0), Angle.FromDegrees(90), 19);
IEnumerable<Point> points4 = arcGenerator.GeneratePointsAlongXYArc(new(0, 0, 100), 800 * 8 - 400 - 800 * 3, Angle.FromDegrees(0), Angle.FromDegrees(90), 19);

IEnumerable<Point> allPoints = points1.Concat(points2.Concat(points3.Concat(points4)));

foreach (Point point in allPoints)
{
    Actor foundation = Actor.CreateFoundation(point.Position, point.Rotation);
    blueprint.Actors.Add(foundation);
}

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