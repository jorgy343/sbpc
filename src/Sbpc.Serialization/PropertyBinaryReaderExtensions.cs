using System.Diagnostics;

namespace Sbpc.Serialization;

public static class PropertyBinaryReaderExtensions
{
    public static object? ReadProperty(this BinaryReader binaryReader)
    {
        // Read the property header which is the same for every property type.
        string name = binaryReader.ReadUnrealString();

        if (name == "None")
        {
            return null;
        }

        string type = binaryReader.ReadUnrealString();

        int sizeInBytes = binaryReader.ReadInt32();
        Debug.Assert(sizeInBytes >= 0, "Size of property is negative.");

        int index = binaryReader.ReadInt32();

        // Read the actual property data based on the property type.
        if (type == "BoolProperty")
        {
            bool value = binaryReader.ReadBoolean();
            binaryReader.ReadByte(); // GUID indicator.

            return new BoolProperty(name, value);
        }
        else if (type == "IntProperty")
        {
            binaryReader.ReadByte(); // GUID indicator.
            int value = binaryReader.ReadInt32();

            return new IntProperty(name, value);
        }
        else if (type == "Int8Property")
        {
            binaryReader.ReadByte(); // GUID indicator.
            byte value = binaryReader.ReadByte();

            return new IntProperty(name, value);
        }
        else if (type == "Int64Property")
        {
            binaryReader.ReadByte(); // GUID indicator.
            long value = binaryReader.ReadInt64();

            return new Int64Property(name, value);
        }
        else if (type == "UInt32Property")
        {
            binaryReader.ReadByte(); // GUID indicator.
            uint value = binaryReader.ReadUInt32();

            return new UInt32Property(name, value);
        }
        else if (type == "FloatProperty")
        {
            binaryReader.ReadByte(); // GUID indicator.
            float value = binaryReader.ReadSingle();

            return new FloatProperty(name, value);
        }
        else if (type == "DoubleProperty")
        {
            binaryReader.ReadByte(); // GUID indicator.
            double value = binaryReader.ReadDouble();

            return new DoubleProperty(name, value);
        }
        else if (type == "NameProperty")
        {
            binaryReader.ReadByte(); // GUID indicator.
            string value = binaryReader.ReadUnrealString();

            return new NameProperty(name, value);
        }
        else if (type == "StrProperty")
        {
            binaryReader.ReadByte(); // GUID indicator.
            string value = binaryReader.ReadUnrealString();

            return new StrProperty(name, value);
        }
        else if (type == "ObjectProperty")
        {
            binaryReader.ReadByte(); // GUID indicator.
            ObjectReference value = binaryReader.ReadObjectReference();

            return new ObjectProperty(name, value);
        }
        else if (type == "StructProperty")
        {
            string structType = binaryReader.ReadUnrealString();

            binaryReader.ReadInt64();
            binaryReader.ReadInt64();

            binaryReader.ReadByte(); // GUID indicator? Maybe?

            // TODO: Handle specialized types. Currently this only handles property lists.
            List<object> properties = new();
            while (true)
            {
                object? property = binaryReader.ReadProperty();

                if (property is null)
                {
                    break;
                }

                properties.Add(property);
            }

            return new PropertyListStructProperty(name, properties);
        }

        throw new NotSupportedException($"Property of type {type} is not supported.");
    }
}