using Sbpc.Core;
using System.Collections.Generic;
using System.IO;
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

    public static void WriteIntVector(this BinaryWriter binaryWriter, (int X, int Y, int Z) value)
    {
        binaryWriter.Write(value.X);
        binaryWriter.Write(value.Y);
        binaryWriter.Write(value.Z);
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
            // No null byte is included for an empty string. Just the size of the emptry string (0) is needed.
            binaryWriter.Write(0);
            return;
        }

        binaryWriter.Write(value.Length + 1); // Null character counts in length.

        // Always write the string in UTF8 format since I think UTF8 covers everything UTF16 does.
        byte[] utf8Bytes = Encoding.UTF8.GetBytes(value);
        binaryWriter.Write(utf8Bytes);

        binaryWriter.Write((byte)0); // Terminating null byte required for Unreal strings.
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
        BinaryWriterSizeWriter sizeWriter = new(binaryWriter);

        sizeWriter.WriteDummySize();
        using (sizeWriter.TrackSize())
        {
            binaryWriter.WriteObjectReference(actor.Parent);
            binaryWriter.WriteObjectReferenceList(actor.Components);
            binaryWriter.WritePropertyList(actor.Properties);
            binaryWriter.Write(actor.TrailingBytes);
        }
    }

    public static void WriteChunkHeader(this BinaryWriter binaryWriter, ChunkHeader chunkHeader)
    {
        binaryWriter.Write(chunkHeader.Magic1);
        binaryWriter.Write(chunkHeader.Magic2);

        binaryWriter.Write(chunkHeader.MaximumChunkSize);
        binaryWriter.Write(chunkHeader.CompressionType);

        binaryWriter.Write(chunkHeader.CompressedSize);
        binaryWriter.Write(chunkHeader.UncompressedSize);

        binaryWriter.Write(chunkHeader.CompressedSize);
        binaryWriter.Write(chunkHeader.UncompressedSize);
    }
}