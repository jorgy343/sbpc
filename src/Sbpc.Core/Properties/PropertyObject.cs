namespace Sbpc.Core.Properties;

public readonly record struct PropertyObject(string Name, ObjectReference Value) : IProperty
{
    public string PropertyType { get; } = "ObjectProperty";
}