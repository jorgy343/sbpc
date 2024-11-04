using Sbpc.Core.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sbpc.Core.JsonConverters;

public class JsonConverterPropertyList : JsonConverter<PropertyList>
{
    public override PropertyList Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        List<IProperty>? properties = JsonSerializer.Deserialize<List<IProperty>>(ref reader, options);
        if (properties is null)
        {
            throw new NotSupportedException();
        }

        PropertyList propertyList = new();
        propertyList.SetProperties(properties);

        return propertyList;
    }

    public override void Write(Utf8JsonWriter writer, PropertyList value, JsonSerializerOptions options)
    {
        List<IProperty> properties = value.GetProperties().ToList();
        JsonSerializer.Serialize(writer, properties, options);
    }
}