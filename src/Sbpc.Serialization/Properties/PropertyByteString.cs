namespace Sbpc.Serialization.Properties;

public readonly record struct PropertyByteString(string Name, string ByteType, string Value) : IProperty
{
    public static string PropertyType { get; } = "ByteProperty";
}
