namespace Sbpc.Serialization.Properties;

public readonly record struct PropertyStr(string Name, string Value) : IProperty
{
    public string PropertyType { get; } = "StrProperty";
}