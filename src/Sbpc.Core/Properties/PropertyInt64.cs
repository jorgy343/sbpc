namespace Sbpc.Core.Properties;

public readonly record struct PropertyInt64(string Name, long Value) : IProperty
{
    public string PropertyType { get; } = "Int64Property";
}