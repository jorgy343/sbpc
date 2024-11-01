using System;
using System.Collections.Generic;
using System.Numerics;

namespace Sbpc.PointGeneration;

public class ArcGenerator
{
    public IEnumerable<Point> GeneratePointsAlongXYArc(Vector3 center, double radius, Angle startAngle, Angle endAngle, int steps)
    {
        Angle angleStep = (endAngle - startAngle) / steps;
        for (int i = 0; i < steps; i++)
        {
            Angle angle = startAngle + angleStep / 2.0 + angleStep * i;

            double x = Math.Round(center.X + radius * Math.Cos(angle.ToRadians()), 1, MidpointRounding.AwayFromZero);
            double y = Math.Round(center.Y + radius * Math.Sin(angle.ToRadians()), 1, MidpointRounding.AwayFromZero);

            double directionX = Math.Round(Math.Cos(angle.ToRadians()), 1, MidpointRounding.AwayFromZero);
            double directionY = Math.Round(Math.Sin(angle.ToRadians()), 1, MidpointRounding.AwayFromZero);

            Vector3 position = new((float)x, (float)y, center.Z);
            Quaternion rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)angle.ToRadians());

            Point point = new(position, rotation);
            yield return point;
        }
    }
}