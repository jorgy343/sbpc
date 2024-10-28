namespace Sbpc.Serialization.Properties;

public readonly record struct PropertyUInt32(string Name, uint Value) : IProperty
{
    public static string PropertyType { get; } = "UInt32Property";
}
