using System.Numerics;

namespace Sbpc.Core.Properties;

public readonly record struct PropertyStructLinearColor(string Name, float R, float G, float B, float A) : IProperty
{
    public PropertyStructLinearColor(string name, Vector4 colorRgba)
        : this(name, colorRgba.X, colorRgba.Y, colorRgba.Z, colorRgba.W)
    {
        
    }

    public string PropertyType { get; } = "StructProperty";
}