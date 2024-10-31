﻿using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sbpc.Core.JsonConverters;

public class JsonConverterQuaternion : JsonConverter<Quaternion>
{
    public override Quaternion Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        reader.Read();

        float x = reader.GetSingle();
        reader.Read();

        float y = reader.GetSingle();
        reader.Read();

        float z = reader.GetSingle();
        reader.Read();

        float w = reader.GetSingle();
        reader.Read();

        reader.Read();

        return new Quaternion(x, y, z, w);
    }

    public override void Write(Utf8JsonWriter writer, Quaternion value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        writer.WriteNumberValue(value.X);
        writer.WriteNumberValue(value.Y);
        writer.WriteNumberValue(value.Z);
        writer.WriteNumberValue(value.W);

        writer.WriteEndArray();
    }
}