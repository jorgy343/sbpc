namespace Sbpc.Core.Properties;

public readonly record struct PropertyStructLinearColor(string Name, float R, float G, float B, float A) : IProperty
{
    public string PropertyType { get; } = "StructProperty";
}