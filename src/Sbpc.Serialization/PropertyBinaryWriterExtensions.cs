namespace Sbpc.Serialization;

public static class PropertyBinaryWriterExtensions
{
    public static void WritePropertyList(this BinaryWriter binaryWriter, List<object> properties)
    {
        foreach (object property in properties)
        {
            binaryWriter.WriteProperty(property);
        }

        binaryWriter.WriteNoneProperty();
    }

    public static void WriteNoneProperty(this BinaryWriter binaryWriter)
    {
        binaryWriter.WriteUnrealString("None");
    }

    public static void WriteProperty(this BinaryWriter binaryWriter, object property)
    {
        if (property is BoolProperty boolProperty)
        {
            binaryWriter.WriteUnrealString(boolProperty.Name);
            binaryWriter.WriteUnrealString("BoolProperty");

            binaryWriter.Write(0); // This would be the size but it is always 0 for bools.
            binaryWriter.Write(0); // Index.

            binaryWriter.Write(boolProperty.Value ? (byte)1 : (byte)0);

            binaryWriter.Write((byte)0); // GUID indicator.
        }
        else if (property is IntProperty intProperty)
        {
            binaryWriter.WriteUnrealString(intProperty.Name);
            binaryWriter.WriteUnrealString("IntProperty");

            binaryWriter.Write(4); // Size.
            binaryWriter.Write(0); // Index.
            binaryWriter.Write((byte)0); // GUID indicator.

            binaryWriter.Write(intProperty.Value);
        }
        else if (property is Int8Property int8Property)
        {
            binaryWriter.WriteUnrealString(int8Property.Name);
            binaryWriter.WriteUnrealString("Int8Property");

            binaryWriter.Write(1); // Size.
            binaryWriter.Write(0); // Index.
            binaryWriter.Write((byte)0); // GUID indicator.

            binaryWriter.Write(int8Property.Value);
        }
        else if (property is Int64Property int64Property)
        {
            binaryWriter.WriteUnrealString(int64Property.Name);
            binaryWriter.WriteUnrealString("Int64Property");

            binaryWriter.Write(8); // Size.
            binaryWriter.Write(0); // Index.
            binaryWriter.Write((byte)0); // GUID indicator.

            binaryWriter.Write(int64Property.Value);
        }
        else if (property is UInt32Property uint32Property)
        {
            binaryWriter.WriteUnrealString(uint32Property.Name);
            binaryWriter.WriteUnrealString("UInt32Property");

            binaryWriter.Write(4); // Size.
            binaryWriter.Write(0); // Index.
            binaryWriter.Write((byte)0); // GUID indicator.

            binaryWriter.Write(uint32Property.Value);
        }
        else if (property is FloatProperty floatProperty)
        {
            binaryWriter.WriteUnrealString(floatProperty.Name);
            binaryWriter.WriteUnrealString("FloatProperty");

            binaryWriter.Write(4); // Size.
            binaryWriter.Write(0); // Index.
            binaryWriter.Write((byte)0); // GUID indicator.

            binaryWriter.Write(floatProperty.Value);
        }
        else if (property is DoubleProperty doubleProperty)
        {
            binaryWriter.WriteUnrealString(doubleProperty.Name);
            binaryWriter.WriteUnrealString("DoubleProperty");

            binaryWriter.Write(8); // Size.
            binaryWriter.Write(0); // Index.
            binaryWriter.Write((byte)0); // GUID indicator.

            binaryWriter.Write(doubleProperty.Value);
        }
        else if (property is NameProperty nameProperty)
        {
            binaryWriter.WriteUnrealString(nameProperty.Name);
            binaryWriter.WriteUnrealString("NameProperty");

            int startOfSize = (int)binaryWriter.BaseStream.Position;

            binaryWriter.Write(0); // Placeholder for size.
            binaryWriter.Write(0); // Index.
            binaryWriter.Write((byte)0); // GUID indicator.

            int startOfValue = (int)binaryWriter.BaseStream.Position;
            binaryWriter.WriteUnrealString(nameProperty.Value);
            int sizeOfValue = (int)binaryWriter.BaseStream.Position - startOfValue;

            binaryWriter.Seek(startOfSize, SeekOrigin.Begin);
            binaryWriter.Write(sizeOfValue);
            binaryWriter.Seek(0, SeekOrigin.End);
        }
        else if (property is StrProperty strProperty)
        {
            binaryWriter.WriteUnrealString(strProperty.Name);
            binaryWriter.WriteUnrealString("StrProperty");

            int startOfSize = (int)binaryWriter.BaseStream.Position;

            binaryWriter.Write(0); // Placeholder for size.
            binaryWriter.Write(0); // Index.
            binaryWriter.Write((byte)0); // GUID indicator.

            int startOfValue = (int)binaryWriter.BaseStream.Position;
            binaryWriter.WriteUnrealString(strProperty.Value);
            int sizeOfValue = (int)binaryWriter.BaseStream.Position - startOfValue;

            binaryWriter.Seek(startOfSize, SeekOrigin.Begin);
            binaryWriter.Write(sizeOfValue);
            binaryWriter.Seek(0, SeekOrigin.End);
        }
        else if (property is ObjectProperty objectProperty)
        {
            binaryWriter.WriteUnrealString(objectProperty.Name);
            binaryWriter.WriteUnrealString("ObjectProperty");

            int startOfSize = (int)binaryWriter.BaseStream.Position;

            binaryWriter.Write(0); // Placeholder for size.
            binaryWriter.Write(0); // Index.
            binaryWriter.Write((byte)0); // GUID indicator.

            int startOfValue = (int)binaryWriter.BaseStream.Position;
            binaryWriter.WriteObjectReference(objectProperty.Value);
            int sizeOfValue = (int)binaryWriter.BaseStream.Position - startOfValue;

            binaryWriter.Seek(startOfSize, SeekOrigin.Begin);
            binaryWriter.Write(sizeOfValue);
            binaryWriter.Seek(0, SeekOrigin.End);
        }
        else if (property is PropertyListStructProperty structProperty)
        {
            binaryWriter.WriteUnrealString(structProperty.Name);
            binaryWriter.WriteUnrealString("StructProperty");

            int startOfSize = (int)binaryWriter.BaseStream.Position;

            binaryWriter.Write(0); // Placeholder for size.
            binaryWriter.Write(0); // Index.

            binaryWriter.WriteUnrealString(structProperty.StructType);

            binaryWriter.Write((long)0); // Padding.
            binaryWriter.Write((long)0); // Padding.
            binaryWriter.Write((byte)0); // GUID indicator.

            int startOfValue = (int)binaryWriter.BaseStream.Position;
            binaryWriter.WritePropertyList(structProperty.Properties);
            int sizeOfValue = (int)binaryWriter.BaseStream.Position - startOfValue;

            binaryWriter.Seek(startOfSize, SeekOrigin.Begin);
            binaryWriter.Write(sizeOfValue);
            binaryWriter.Seek(0, SeekOrigin.End);
        }
    }
}