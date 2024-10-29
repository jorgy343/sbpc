namespace Sbpc.Serialization.Properties;

public readonly record struct PropertyBool(string Name, bool Value) : IProperty
{
    public string PropertyType { get; } = "BoolProperty";
}