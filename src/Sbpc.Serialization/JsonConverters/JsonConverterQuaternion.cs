﻿using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sbpc.Serialization.JsonConverters;

public class JsonConverterVector3 : JsonConverter<Vector3>
{
    public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        reader.Read();

        float x = reader.GetSingle();
        reader.Read();

        float y = reader.GetSingle();
        reader.Read();

        float z = reader.GetSingle();
        reader.Read();

        reader.Read();

        return new Vector3(x, y, z);
    }

    public override void Write(Utf8JsonWriter writer, Vector3 value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        writer.WriteNumberValue(value.X);
        writer.WriteNumberValue(value.Y);
        writer.WriteNumberValue(value.Z);

        writer.WriteEndArray();
    }
}