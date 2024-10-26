using System.Numerics;

namespace Sbpc.Serialization;

public class ActorHeader
{
    public required string ClassName { get; set; }
    public required string LevelName { get; set; }
    public required string InstanceName { get; set; }
    public required int NeedTransform { get; set; }
    public required Quaternion Rotation { get; set; }
    public required Vector3 Position { get; set; }
    public required Vector3 Scale { get; set; }
    public required int PlacedInLevel { get; set; }
}