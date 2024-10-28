namespace Sbpc.Serialization.Properties;

public readonly record struct PropertyStructLinearColor(string Name, float R, float G, float B, float A) : IProperty
{
    public static string PropertyType { get; } = "StructProperty";
}