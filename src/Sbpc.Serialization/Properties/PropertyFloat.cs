namespace Sbpc.Serialization.Properties;

public readonly record struct PropertyFloat(string Name, float Value) : IProperty
{
    public string PropertyType { get; } = "FloatProperty";
}