using Sbpc.Core;
using Sbpc.Core.JsonConverters;
using Sbpc.PointGeneration;
using Sbpc.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.Json;

Blueprint blueprint = new()
{
    Dimensions = (24, 24, 6),
};

ArcGenerator arcGenerator = new();

IEnumerable<Point> points1 = arcGenerator.GeneratePointsAlongXYArc(new(0, 0, 100), 800 * 8 - 400, Angle.FromDegrees(0), Angle.FromDegrees(90), 15);
IEnumerable<Point> points2 = arcGenerator.GeneratePointsAlongXYArc(new(0, 0, 100), 800 * 7 - 400, Angle.FromDegrees(0), Angle.FromDegrees(90), 15);
IEnumerable<Point> points3 = arcGenerator.GeneratePointsAlongXYArc(new(0, 0, 100), 800 * 6 - 400, Angle.FromDegrees(0), Angle.FromDegrees(90), 15);
IEnumerable<Point> points4 = arcGenerator.GeneratePointsAlongXYArc(new(0, 0, 100), 800 * 5 - 400, Angle.FromDegrees(0), Angle.FromDegrees(90), 15);

IEnumerable<Point> allPoints = points1.Concat(points2.Concat(points3.Concat(points4)));

foreach (Point point in allPoints)
{
    Actor foundation = Foundation.CreateFoundationFloor(FoundationMaterial.Asphalt, FoundationThickness.TwoMeter, point.Position, point.Rotation);
    blueprint.Actors.Add(foundation);
}

IEnumerable<LineSegment> outerCurbLineSegments = arcGenerator.GenerateLineSegmentsAlongXYArc(new(0, 0, 200), 800 * 8, Angle.FromDegrees(0), Angle.FromDegrees(90), 16);
IEnumerable<LineSegment> innerCurbLineSegments = arcGenerator.GenerateLineSegmentsAlongXYArc(new(0, 0, 200), 800 * 4, Angle.FromDegrees(0), Angle.FromDegrees(90), 16);

IEnumerable<LineSegment> allCurbLineSegments = outerCurbLineSegments.Concat(innerCurbLineSegments);

foreach (LineSegment curbLineSegment in allCurbLineSegments)
{
    Actor innerCurb = Beam.CreateConcreteBeam(curbLineSegment.StartPosition, curbLineSegment.Rotation, curbLineSegment.Length);
    blueprint.Actors.Add(innerCurb);
}

IEnumerable<LineSegment> centerLineLineSegments1 = arcGenerator.GenerateLineSegmentsAlongXYArc(new(0, 0, 200), 800 * 6 - 20, Angle.FromDegrees(0), Angle.FromDegrees(90), 16);
IEnumerable<LineSegment> centerLineLineSegments2 = arcGenerator.GenerateLineSegmentsAlongXYArc(new(0, 0, 200), 800 * 6 + 20, Angle.FromDegrees(0), Angle.FromDegrees(90), 16);

IEnumerable<LineSegment> centerLineLineSegments = centerLineLineSegments1.Concat(centerLineLineSegments2);

foreach (LineSegment centerLineSegment in centerLineLineSegments)
{
    Actor centerLine = Beam.CreateEmmisiveBeam(centerLineSegment.StartPosition, centerLineSegment.Rotation, centerLineSegment.Length, new Vector4(1.0f, 1.0f, 0.0f, 1.0f));
    blueprint.Actors.Add(centerLine);
}

IEnumerable<LineSegment> sideLineLineSegments1 = arcGenerator.GenerateLineSegmentsAlongXYArc(new(0, 0, 200), 800 * 8 - 220, Angle.FromDegrees(0), Angle.FromDegrees(90), 16);
IEnumerable<LineSegment> sideLineLineSegments2 = arcGenerator.GenerateLineSegmentsAlongXYArc(new(0, 0, 200), 800 * 4 + 220, Angle.FromDegrees(0), Angle.FromDegrees(90), 16);

IEnumerable<LineSegment> sideLineLineSegments = sideLineLineSegments1.Concat(sideLineLineSegments2);

foreach (LineSegment sideLineSegment in sideLineLineSegments)
{
    Actor sideLine = Beam.CreateEmmisiveBeam(sideLineSegment.StartPosition, sideLineSegment.Rotation, sideLineSegment.Length, new Vector4(0.8f, 0.8f, 1.0f, 1.0f));
    blueprint.Actors.Add(sideLine);
}

Serialization.WriteBlueprintFile("../../../../../samples/TEST.sbp", blueprint);

JsonSerializerOptions serializerOptions = new()
{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    PropertyNameCaseInsensitive = false,
    Converters =
    {
        new JsonConverterVector3(),
        new JsonConverterQuaternion(),
        new JsonConverterPropertyList(),
    },
};

await File.WriteAllTextAsync("../../../../../samples/TEST.json", JsonSerializer.Serialize(blueprint, serializerOptions));