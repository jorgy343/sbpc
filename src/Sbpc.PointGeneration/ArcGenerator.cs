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

            Vector3 position = new((float)x, (float)y, center.Z);
            Quaternion rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)angle.ToRadians());

            Point point = new(position, rotation);
            yield return point;
        }
    }

    public IEnumerable<LineSegment> GenerateLineSegmentsAlongXYArc(Vector3 center, double radius, Angle startAngle, Angle endAngle, int lineSegmentCount)
    {
        double lastPointX = Math.Round(center.X + radius * Math.Cos(startAngle.ToRadians()), 1, MidpointRounding.AwayFromZero);
        double lastPointY = Math.Round(center.Y + radius * Math.Sin(startAngle.ToRadians()), 1, MidpointRounding.AwayFromZero);

        Angle angleStep = (endAngle - startAngle) / lineSegmentCount;

        for (int i = 0; i < lineSegmentCount; i++)
        {
            Angle angle = startAngle + angleStep * (i + 1);
            Angle halfAngle = angle / 2.0;

            double x = Math.Round(center.X + radius * Math.Cos(angle.ToRadians()), 1, MidpointRounding.AwayFromZero);
            double y = Math.Round(center.Y + radius * Math.Sin(angle.ToRadians()), 1, MidpointRounding.AwayFromZero);

            Vector3 lastPosition = new((float)lastPointX, (float)lastPointY, center.Z);
            Vector3 position = new((float)x, (float)y, center.Z);

            Quaternion rotation = Rotation.CreateQuaternionThatPointsToward(Vector3.Normalize(position - lastPosition));

            float length = (float)Math.Abs(Math.Round((position - lastPosition).Length(), 1, MidpointRounding.AwayFromZero));

            LineSegment lineSegment = new(lastPosition, rotation, length);
            yield return lineSegment;

            lastPointX = x;
            lastPointY = y;
        }
    }
}