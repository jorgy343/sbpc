namespace Sbpc.Serialization.Properties;

public readonly record struct PropertyStructPropertyList(string Name, string StructType, List<object> Properties) : IProperty
{
    public static string PropertyType { get; } = "StructProperty";
}
