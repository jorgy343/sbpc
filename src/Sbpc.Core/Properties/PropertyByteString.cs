﻿namespace Sbpc.Core.Properties;

public readonly record struct PropertyByteString(string Name, string ByteType, string Value) : IProperty
{
    public string PropertyType { get; } = "ByteProperty";
}