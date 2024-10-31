namespace Sbpc.Core.Properties;

public readonly record struct PropertyStructPropertyList(string Name, string StructType, PropertyList PropertyList) : IProperty
{
    public PropertyStructPropertyList(string Name, string StructType)
        : this(Name, StructType, new PropertyList())
    {

    }

    public string PropertyType { get; } = "StructProperty";
}