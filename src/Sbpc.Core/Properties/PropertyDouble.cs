namespace Sbpc.Core.Properties;

public readonly record struct PropertyDouble(string Name, double Value) : IProperty
{
    public string PropertyType { get; } = "DoubleProperty";
}