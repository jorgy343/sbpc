namespace Sbpc.Serialization.Properties;

public readonly record struct PropertyObject(string Name, ObjectReference Value) : IProperty
{
    public string PropertyType { get; } = "ObjectProperty";
}