namespace Sbpc.Core.Properties;

public readonly record struct PropertyUInt32(string Name, uint Value) : IProperty
{
    public string PropertyType { get; } = "UInt32Property";
}