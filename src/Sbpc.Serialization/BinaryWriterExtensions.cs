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
}