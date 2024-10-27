using System.Numerics;
using System.Text;

namespace Sbpc.Serialization;

public static class BinaryWriterExtensions
{
    public static void WriteIntVector(this BinaryWriter binaryWriter, Vector3 value)
    {
        binaryWriter.Write((int)value.X);
        binaryWriter.Write((int)value.Y);
        binaryWriter.Write((int)value.Z);
    }

    public static void WriteVector3(this BinaryWriter binaryWriter, Vector3 value)
    {
        binaryWriter.Write(value.X);
        binaryWriter.Write(value.Y);
        binaryWriter.Write(value.Z);
    }

    public static void WriteQuaternion(this BinaryWriter binaryWriter, Quaternion value)
    {
        binaryWriter.Write(value.X);
        binaryWriter.Write(value.Y);
        binaryWriter.Write(value.Z);
        binaryWriter.Write(value.W);
    }

    public static void WriteUnrealString(this BinaryWriter binaryWriter, string value)
    {
        if (value == string.Empty)
        {
            binaryWriter.Write(0);
            return;
        }

        binaryWriter.Write(value.Length + 1); // Null character counts in length.

        // Assume utf8.
        byte[] utf8Bytes = Encoding.UTF8.GetBytes(value);
        binaryWriter.Write(utf8Bytes);

        binaryWriter.Write((byte)0);
    }

    public static void WriteObjectReferenceList(this BinaryWriter binaryWriter, List<ObjectReference> value)
    {
        binaryWriter.Write(value.Count);

        foreach (ObjectReference objectReference in value)
        {
            binaryWriter.WriteObjectReference(objectReference);
        }
    }

    public static void WriteObjectReference(this BinaryWriter binaryWriter, ObjectReference value)
    {
        binaryWriter.WriteUnrealString(value.LevelName);
        binaryWriter.WriteUnrealString(value.PathName);
    }

    public static void WriteBlueprintItemAmountList(this BinaryWriter binaryWriter, List<BlueprintItemAmount> value)
    {
        binaryWriter.Write(value.Count);

        foreach (BlueprintItemAmount blueprintItemAmount in value)
        {
            binaryWriter.WriteBlueprintItemAmount(blueprintItemAmount);
        }
    }

    public static void WriteBlueprintItemAmount(this BinaryWriter binaryWriter, BlueprintItemAmount value)
    {
        binaryWriter.WriteObjectReference(value.ItemClass);
        binaryWriter.Write(value.Amount);
    }

    public static void WriteActorHeader(this BinaryWriter binaryWriter, Actor actor)
    {
        binaryWriter.WriteUnrealString(actor.ClassName);
        binaryWriter.WriteUnrealString(actor.LevelName);
        binaryWriter.WriteUnrealString(actor.InstanceName);

        binaryWriter.Write(1); // Need transform, always set to 1 for now.

        binaryWriter.WriteQuaternion(actor.Rotation);
        binaryWriter.WriteVector3(actor.Position);
        binaryWriter.WriteVector3(actor.Scale);

        binaryWriter.Write(0); // Placed in level, always set to 0 for now.
    }

    public static void WriteActorEntity(this BinaryWriter binaryWriter, Actor actor)
    {
        int startOfEntity = (int)binaryWriter.BaseStream.Position;

        binaryWriter.Write(0); // Placeholder for the entity size in bytes.

        binaryWriter.WriteObjectReference(actor.Parent);
        binaryWriter.WriteObjectReferenceList(actor.Components);
        binaryWriter.WritePropertyList(actor.Properties);
        binaryWriter.Write(actor.TrailingBytes);

        // Go back and write the size of the entity.
        int entitySizeInBytes = (int)binaryWriter.BaseStream.Position - startOfEntity;

        binaryWriter.Seek(startOfEntity, SeekOrigin.Begin);
        binaryWriter.Write(entitySizeInBytes);
        binaryWriter.Seek(0, SeekOrigin.End);
    }

    public static void WriteChunkHeader(this BinaryWriter binaryWriter, ChunkHeader chunkHeader)
    {
        binaryWriter.Write(chunkHeader.Magic1);
        binaryWriter.Write(chunkHeader.Magic2);

        binaryWriter.Write((byte)0);
        binaryWriter.Write(Constants.MaximumChunkSize);
        binaryWriter.Write(chunkHeader.Magic3);

        binaryWriter.Write(chunkHeader.CompressedSize);
        binaryWriter.Write(chunkHeader.UncompressedSize);

        binaryWriter.Write(chunkHeader.CompressedSize);
        binaryWriter.Write(chunkHeader.UncompressedSize);
    }
}