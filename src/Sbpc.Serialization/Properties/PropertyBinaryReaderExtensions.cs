using System.Diagnostics;

namespace Sbpc.Serialization.Properties;

public static class PropertyBinaryReaderExtensions
{
    public static PropertyList ReadPropertyList(this BinaryReader binaryReader)
    {
        PropertyList propertyList = new();

        while (true)
        {
            IProperty? property = binaryReader.ReadProperty();
            if (property is null)
            {
                break;
            }

            propertyList.SetProperty(property);
        }

        return propertyList;
    }

    public static IProperty? ReadProperty(this BinaryReader binaryReader)
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

            return new PropertyBool(name, value);
        }
        else if (type == "ByteProperty")
        {
            string byteType = binaryReader.ReadUnrealString();
            binaryReader.ReadByte(); // GUID indicator.

            if (byteType == "None")
            {
                byte byteValue = binaryReader.ReadByte();
                return new PropertyByteByte(name, byteValue);
            }

            string stringValue = binaryReader.ReadUnrealString();
            return new PropertyByteString(name, byteType, stringValue);
        }
        else if (type == "IntProperty")
        {
            binaryReader.ReadByte(); // GUID indicator.
            int value = binaryReader.ReadInt32();

            return new PropertyInt(name, value);
        }
        else if (type == "Int8Property")
        {
            binaryReader.ReadByte(); // GUID indicator.
            byte value = binaryReader.ReadByte();

            return new PropertyInt(name, value);
        }
        else if (type == "Int64Property")
        {
            binaryReader.ReadByte(); // GUID indicator.
            long value = binaryReader.ReadInt64();

            return new PropertyInt64(name, value);
        }
        else if (type == "UInt32Property")
        {
            binaryReader.ReadByte(); // GUID indicator.
            uint value = binaryReader.ReadUInt32();

            return new PropertyUInt32(name, value);
        }
        else if (type == "FloatProperty")
        {
            binaryReader.ReadByte(); // GUID indicator.
            float value = binaryReader.ReadSingle();

            return new PropertyFloat(name, value);
        }
        else if (type == "DoubleProperty")
        {
            binaryReader.ReadByte(); // GUID indicator.
            double value = binaryReader.ReadDouble();

            return new PropertyDouble(name, value);
        }
        else if (type == "NameProperty")
        {
            binaryReader.ReadByte(); // GUID indicator.
            string value = binaryReader.ReadUnrealString();

            return new PropertyName(name, value);
        }
        else if (type == "StrProperty")
        {
            binaryReader.ReadByte(); // GUID indicator.
            string value = binaryReader.ReadUnrealString();

            return new PropertyStr(name, value);
        }
        else if (type == "ObjectProperty")
        {
            binaryReader.ReadByte(); // GUID indicator.
            ObjectReference value = binaryReader.ReadObjectReference();

            return new PropertyObject(name, value);
        }
        else if (type == "StructProperty")
        {
            string structType = binaryReader.ReadUnrealString();

            binaryReader.ReadInt64(); // Padding. Should always be 0.
            binaryReader.ReadInt64(); // Padding. Should always be 0.

            binaryReader.ReadByte(); // GUID indicator? Maybe?

            if (structType == "LinearColor")
            {
                float r = binaryReader.ReadSingle();
                float g = binaryReader.ReadSingle();
                float b = binaryReader.ReadSingle();
                float a = binaryReader.ReadSingle();

                return new PropertyStructLinearColor(name, r, g, b, a);
            }
            else
            {
                PropertyList properties = binaryReader.ReadPropertyList();
                return new PropertyStructPropertyList(name, structType, properties);
            }
        }

        throw new NotSupportedException($"Property of type {type} is not supported.");
    }
}