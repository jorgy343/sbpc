using System.Numerics;

namespace Sbpc.Serialization;

public class Actor
{
    public required string ClassName { get; init; }
    public required string LevelName { get; init; }
    public required string InstanceName { get; init; }

    public Quaternion Rotation { get; set; } = new(0, 0, 0, 1);
    public Vector3 Position { get; set; } = new(0, 0, 0);
    public Vector3 Scale { get; set; } = new(1, 1, 1);

    public List<object> Properties { get; init; } = new();
}