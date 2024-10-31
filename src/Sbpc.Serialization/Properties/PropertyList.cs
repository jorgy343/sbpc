using System;
using System.Collections.Generic;

namespace Sbpc.Serialization.Properties;

/// <summary>
/// <para>
/// A PropertyList is not a property in and of itself, but rather a collection of properties. This is
/// becuase a PropertyList does not have a name, size, etc. that all standard properties have. However
/// an actor's properties are represented as a PropertyList as well as a non-specialized struct property.
/// </para>
/// <para>
/// Two assumptions are made here. The first is that a property list will never contain two properties
/// by the same name (case sensitive) even if they are of different types. The second is that the order
/// of properties does not matter.
/// </para>
/// </summary>
public class PropertyList
{
    private readonly Dictionary<string, IProperty> _propertiesByName;

    public PropertyList()
    {
        _propertiesByName = new();
    }

    public IProperty SetProperty(IProperty property)
    {
        _propertiesByName[property.Name] = property;
        return property;
    }

    public T SetProperty<T>(T property)
        where T : IProperty
    {
        _propertiesByName[property.Name] = property;
        return property;
    }

    public T GetOrSetProperty<T>(string name, Func<T> factory)
        where T : IProperty
    {
        if (_propertiesByName.TryGetValue(name, out IProperty? property))
        {
            if (property is not T typedProperty)
            {
                throw new InvalidOperationException($"Property {name} is not of type {typeof(T).Name}.");
            }

            return typedProperty;
        }

        T newProperty = factory();
        _propertiesByName[name] = newProperty;

        return newProperty;
    }

    public IEnumerable<IProperty> GetProperties()
    {
        return _propertiesByName.Values;
    }
}