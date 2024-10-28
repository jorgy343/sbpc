namespace Sbpc.Serialization.Properties;

public readonly record struct PropertyInt(string Name, int Value) : IProperty
{
    public static string PropertyType { get; } = "IntProperty";
}
