using System.Numerics;

namespace Sbpc.PointGeneration;

public readonly record struct Point(Vector3 Position, Quaternion Rotation);