namespace Sbpc.Core.Properties;

public readonly record struct PropertyBool(string Name, bool Value) : IProperty
{
    public string PropertyType { get; } = "BoolProperty";
}