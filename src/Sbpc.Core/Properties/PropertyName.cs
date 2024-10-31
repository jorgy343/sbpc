namespace Sbpc.Core.Properties;

public readonly record struct PropertyName(string Name, string Value) : IProperty
{
    public string PropertyType { get; } = "NameProperty";
}