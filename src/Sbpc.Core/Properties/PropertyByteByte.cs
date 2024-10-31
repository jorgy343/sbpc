namespace Sbpc.Core.Properties;

public readonly record struct PropertyByteByte(string Name, byte Value) : IProperty
{
    public string PropertyType { get; } = "ByteProperty";
}