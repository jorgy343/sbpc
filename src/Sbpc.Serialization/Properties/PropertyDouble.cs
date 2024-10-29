namespace Sbpc.Serialization.Properties;

public readonly record struct PropertyDouble(string Name, double Value) : IProperty
{
    public string PropertyType { get; } = "DoubleProperty";
}