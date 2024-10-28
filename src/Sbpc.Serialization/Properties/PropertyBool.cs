namespace Sbpc.Serialization.Properties;

public readonly record struct PropertyBool(string Name, bool Value) : IProperty
{
    public static string PropertyType { get; } = "BoolProperty";
}
