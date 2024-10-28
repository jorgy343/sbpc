namespace Sbpc.Serialization.Properties;

public readonly record struct PropertyInt64(string Name, long Value) : IProperty
{
    public static string PropertyType { get; } = "Int64Property";
}
