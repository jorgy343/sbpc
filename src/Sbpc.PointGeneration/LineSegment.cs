using System.Numerics;

namespace Sbpc.PointGeneration;

public readonly record struct LineSegment(Vector3 StartPosition, Quaternion Rotation, float Length);