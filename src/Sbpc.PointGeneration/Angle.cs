using System;
using System.Numerics;

namespace Sbpc.PointGeneration;

public readonly struct Angle :
    IAdditionOperators<Angle, Angle, Angle>,
    IComparable,
    IComparable<Angle>,
    IComparisonOperators<Angle, Angle, bool>,
    IDivisionOperators<Angle, double, Angle>,
    IEquatable<Angle>,
    IEqualityOperators<Angle, Angle, bool>,
    IMinMaxValue<Angle>,
    IModulusOperators<Angle, Angle, Angle>,
    IMultiplyOperators<Angle, double, Angle>,
    ISubtractionOperators<Angle, Angle, Angle>,
    IUnaryNegationOperators<Angle, Angle>,
    IUnaryPlusOperators<Angle, Angle>
{
    private readonly double _radians;

    public static Angle MinValue
    {
        get
        {
            return new Angle(double.MinValue);
        }
    }

    public static Angle MaxValue
    {
        get
        {
            return new Angle(double.MaxValue);
        }
    }

    private Angle(double radians)
    {
        _radians = radians;
    }

    public static Angle FromRadians(double radians)
    {
        return new(radians);
    }

    public static Angle FromDegrees(double degrees)
    {
        return new(degrees * Math.PI / 180);
    }

    public double ToRadians()
    {
        return _radians;
    }

    public double ToDegrees()
    {
        return _radians * 180 / Math.PI;
    }

    public static Angle operator +(Angle angle)
    {
        return angle;
    }

    public static Angle operator -(Angle angle)
    {
        return new Angle(-angle._radians);
    }

    public static Angle operator +(Angle a1, Angle a2)
    {
        return new Angle(a1._radians + a2._radians);
    }

    public static Angle operator -(Angle a1, Angle a2)
    {
        return new Angle(a1._radians - a2._radians);
    }

    public static Angle operator *(Angle left, double right)
    {
        return new Angle(left._radians * right);
    }

    public static Angle operator /(Angle left, double right)
    {
        return new Angle(left._radians / right);
    }

    public static Angle operator %(Angle a1, Angle a2)
    {
        return new Angle(a1._radians % a2._radians);
    }

    public static bool operator ==(Angle a1, Angle a2)
    {
        return a1._radians == a2._radians;
    }

    public static bool operator !=(Angle a1, Angle a2)
    {
        return !(a1 == a2);
    }

    public static bool operator <(Angle a1, Angle a2)
    {
        return a1._radians < a2._radians;
    }

    public static bool operator >(Angle a1, Angle a2)
    {
        return a1._radians > a2._radians;
    }

    public static bool operator <=(Angle a1, Angle a2)
    {
        return a1._radians <= a2._radians;
    }

    public static bool operator >=(Angle a1, Angle a2)
    {
        return a1._radians >= a2._radians;
    }

    public int CompareTo(object? obj)
    {
        if (obj is Angle other)
        {
            return CompareTo(other);
        }

        throw new ArgumentException("Object is not an Angle");
    }

    public int CompareTo(Angle other)
    {
        return _radians.CompareTo(other._radians);
    }

    public override bool Equals(object? obj)
    {
        return obj is Angle angle && Equals(angle);
    }

    public bool Equals(Angle other)
    {
        return _radians == other._radians;
    }

    public override int GetHashCode()
    {
        return _radians.GetHashCode();
    }
}
