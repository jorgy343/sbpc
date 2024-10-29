namespace Sbpc.Serialization.Properties;

public readonly record struct PropertyStructPropertyList(string Name, string StructType, PropertyList PropertyList) : IProperty
{
    public string PropertyType { get; } = "StructProperty";
}