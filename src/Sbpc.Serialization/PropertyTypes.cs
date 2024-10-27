namespace Sbpc.Serialization;

public readonly record struct BoolProperty(string Name, bool Value);
public readonly record struct IntProperty(string Name, int Value);
public readonly record struct Int8Property(string Name, byte Value);
public readonly record struct Int64Property(string Name, long Value);
public readonly record struct UInt32Property(string Name, uint Value);
public readonly record struct FloatProperty(string Name, float Value);
public readonly record struct DoubleProperty(string Name, double Value);
public readonly record struct NameProperty(string Name, string Value);
public readonly record struct StrProperty(string Name, string Value);
public readonly record struct ObjectProperty(string Name, ObjectReference Value);
public readonly record struct PropertyListStructProperty(string Name, string StructType, List<object> Properties);