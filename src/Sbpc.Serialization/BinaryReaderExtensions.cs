using System.Numerics;
using System.Text;

namespace Sbpc.Serialization;

public static class BinaryReaderExtensions
{
    public static Vector3 ReadIntVector(this BinaryReader binaryReader)
    {
        int x = binaryReader.ReadInt32();
        int y = binaryReader.ReadInt32();
        int z = binaryReader.ReadInt32();

        return new Vector3(x, y, z);
    }

    public static Vector3 ReadVector3(this BinaryReader binaryReader)
    {
        float x = binaryReader.ReadSingle();
        float y = binaryReader.ReadSingle();
        float z = binaryReader.ReadSingle();

        return new Vector3(x, y, z);
    }

    public static Quaternion ReadQuaternion(this BinaryReader binaryReader)
    {
        float x = binaryReader.ReadSingle();
        float y = binaryReader.ReadSingle();
        float z = binaryReader.ReadSingle();
        float w = binaryReader.ReadSingle();

        return new Quaternion(x, y, z, w);
    }

    public static string ReadUnrealString(this BinaryReader binaryReader)
    {
        // Read the length of the string in characters.
        int characterCount = binaryReader.ReadInt32();

        if (characterCount == 0)
        {
            return string.Empty;
        }

        if (characterCount > 0)
        {
            // Null character counts in length.
            char[] characters = new char[characterCount - 1];
            for (int i = 0; i < characterCount - 1; i++)
            {
                characters[i] = binaryReader.ReadUtf8Character();
            }

            // Read the null byte but don't include it in the string.
            binaryReader.ReadUtf8Character();

            return new string(characters);
        }
        else
        {
            throw new NotSupportedException("UTF 16 strings are not supported.");
        }
    }

    public static char ReadUtf8Character(this BinaryReader binaryReader)
    {
        // Read the first byte to determine the length of the UTF-8 character.
        byte firstByte = binaryReader.ReadByte();

        int additionalBytes;
        if ((firstByte & 0b1000_0000) == 0)
        {
            // 1-byte character (ASCII).
            return (char)firstByte;
        }
        else if ((firstByte & 0b1110_0000) == 0b1100_0000)
        {
            // 2-byte character.
            additionalBytes = 1;
        }
        else if ((firstByte & 0b1111_0000) == 0b1110_0000)
        {
            // 3-byte character.
            additionalBytes = 2;
        }
        else if ((firstByte & 0b1111_1000) == 0b1111_0000)
        {
            // 4-byte character.
            additionalBytes = 3;
        }
        else
        {
            throw new InvalidOperationException("Invalid UTF-8 encoding.");
        }

        // Read the additional bytes.
        byte[] bytes = new byte[1 + additionalBytes];
        bytes[0] = firstByte;
        for (int i = 1; i <= additionalBytes; i++)
        {
            bytes[i] = binaryReader.ReadByte();
        }

        // Decode the bytes to a character.
        return Encoding.UTF8.GetChars(bytes)[0];
    }

    public static List<ObjectReference> ReadObjectReferenceList(this BinaryReader binaryReader)
    {
        int length = binaryReader.ReadInt32();
        List<ObjectReference> objectReferences = new(length);

        for (int i = 0; i < length; i++)
        {
            objectReferences.Add(binaryReader.ReadObjectReference());
        }

        return objectReferences;
    }

    public static ObjectReference ReadObjectReference(this BinaryReader binaryReader)
    {
        string levelName = binaryReader.ReadUnrealString();
        string pathName = binaryReader.ReadUnrealString();

        return new ObjectReference(levelName, pathName);
    }

    public static List<BlueprintItemAmount> ReadBlueprintItemAmountList(this BinaryReader binaryReader)
    {
        int length = binaryReader.ReadInt32();
        List<BlueprintItemAmount> items = new(length);

        for (int i = 0; i < length; i++)
        {
            items.Add(binaryReader.ReadBlueprintItemAmount());
        }

        return items;
    }

    public static BlueprintItemAmount ReadBlueprintItemAmount(this BinaryReader binaryReader)
    {
        ObjectReference itemClass = binaryReader.ReadObjectReference();
        int amount = binaryReader.ReadInt32();

        return new BlueprintItemAmount(itemClass, amount);
    }

    public static ActorHeader ReadActorHeader(this BinaryReader binaryReader)
    {
        string className = binaryReader.ReadUnrealString();
        string levelName = binaryReader.ReadUnrealString();
        string instanceName = binaryReader.ReadUnrealString();

        int needTransform = binaryReader.ReadInt32();
        Quaternion rotation = binaryReader.ReadQuaternion();
        Vector3 position = binaryReader.ReadVector3();
        Vector3 scale = binaryReader.ReadVector3();
        int placedInLevel = binaryReader.ReadInt32();

        ActorHeader actorHeader = new()
        {
            ClassName = className,
            LevelName = levelName,
            InstanceName = instanceName,
            NeedTransform = needTransform,
            Rotation = rotation,
            Position = position,
            Scale = scale,
            PlacedInLevel = placedInLevel,
        };

        return actorHeader;
    }

    public static ActorObject ReadActorObject(this BinaryReader binaryReader)
    {
        int sizeInBytes = binaryReader.ReadInt32();

        long startingPositionInStream = binaryReader.BaseStream.Position;

        ObjectReference parent = binaryReader.ReadObjectReference();
        List<ObjectReference> components = binaryReader.ReadObjectReferenceList();

        // TODO: If we are at the end of the entity, we can't read the properties or trailing bytes.

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

        long remainingBytes = sizeInBytes - (binaryReader.BaseStream.Position - startingPositionInStream);
        byte[] reminaingBytes = remainingBytes > 0 ? binaryReader.ReadBytes((int)remainingBytes) : Array.Empty<byte>();

        ActorObject actorObject = new()
        {
            SizeInBytes = sizeInBytes,
            Parent = parent,
            Components = components,
            Properties = properties,
            TrailingBytes = reminaingBytes,
        };

        return actorObject;
    }

    public static ChunkHeader ReadChunkHeader(this BinaryReader binaryReader)
    {
        uint magic1 = binaryReader.ReadUInt32(); // Package signature magic number.
        uint magic2 = binaryReader.ReadUInt32(); // Other more different magic number.

        int maximumChunkSize = binaryReader.ReadInt32();

        binaryReader.ReadByte(); // A zero byte.
        uint magic3 = binaryReader.ReadUInt32(); // Yet another magic number.

        long compressedSize = binaryReader.ReadInt64();
        long uncompressedSize = binaryReader.ReadInt64();

        binaryReader.ReadInt64(); // Compressed size again.
        binaryReader.ReadInt64(); // Uncompressed size again.

        ChunkHeader chunkHeader = new()
        {
            Magic1 = magic1,
            Magic2 = magic2,
            Magic3 = magic3,
            MaximumChunkSize = maximumChunkSize,
            CompressedSize = compressedSize,
            UncompressedSize = uncompressedSize,
        };

        return chunkHeader;
    }
}