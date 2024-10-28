namespace Sbpc.Serialization.Properties;

public readonly record struct PropertyByteByte(string Name, byte Value) : IProperty
{
    public static string PropertyType { get; } = "ByteProperty";
}
