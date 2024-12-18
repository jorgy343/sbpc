﻿namespace Sbpc.Core.Properties;

public readonly record struct PropertyInt(string Name, int Value) : IProperty
{
    public string PropertyType { get; } = "IntProperty";
}