using System;
using System.Numerics;

namespace Sbpc.PointGeneration;

public static class Rotation
{
    public static Quaternion CreateQuaternionThatPointsToward(Vector3 pointsToward)
    {
        Vector3 u = Vector3.UnitX;
        Vector3 v = pointsToward;

        Vector3 w = Vector3.Cross(u, v);
        float wLength = w.Length();
        float theta = (float)Math.Acos(Vector3.Dot(u, v) / (u.Length() * v.Length()));

        return Quaternion.CreateFromAxisAngle(Vector3.Normalize(w), theta);
    }
}