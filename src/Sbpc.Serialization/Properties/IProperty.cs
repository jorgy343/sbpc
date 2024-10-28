namespace Sbpc.Serialization.Properties;

public interface IProperty
{
    static abstract string PropertyType { get; }

    string Name { get; }
}