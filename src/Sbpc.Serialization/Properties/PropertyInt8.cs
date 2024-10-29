namespace Sbpc.Serialization.Properties;

public readonly record struct PropertyInt8(string Name, byte Value) : IProperty
{
    public string PropertyType { get; } = "Int8Property";
}