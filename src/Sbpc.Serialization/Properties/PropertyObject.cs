namespace Sbpc.Serialization.Properties;

public readonly record struct PropertyObject(string Name, ObjectReference Value) : IProperty
{
    public static string PropertyType { get; } = "ObjectProperty";
}
