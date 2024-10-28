namespace Sbpc.Serialization.Properties;

public readonly record struct PropertyStr(string Name, string Value) : IProperty
{
    public static string PropertyType { get; } = "StrProperty";
}
