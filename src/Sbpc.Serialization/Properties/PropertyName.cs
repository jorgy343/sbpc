namespace Sbpc.Serialization.Properties;

public readonly record struct PropertyName(string Name, string Value) : IProperty
{
    public static string PropertyType { get; } = "NameProperty";
}
