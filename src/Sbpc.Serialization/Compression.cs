using System.IO.Compression;
using System.Text;

namespace Sbpc.Serialization;

public static class Compression
{
    public static byte[] Compress(byte[] uncompressedData)
    {
        using MemoryStream compressedStream = new();
        using ZLibStream zlibStream = new(compressedStream, CompressionMode.Compress, true);
        using BinaryWriter binaryWriter = new(zlibStream, Encoding.Default, true);

        binaryWriter.Write(uncompressedData.Length);
        binaryWriter.Write(uncompressedData);

        binaryWriter.Flush();

        byte[] compressedChunk = compressedStream.ToArray();
        return compressedChunk;
    }

    public static byte[] Decompress(byte[] compressedData)
    {
        using MemoryStream compressedStream = new(compressedData);
        using ZLibStream zlibStream = new(compressedStream, CompressionMode.Decompress, true);
        using BinaryReader binaryReader = new(zlibStream, Encoding.Default, true);

        int uncompressedSizeInBytes = binaryReader.ReadInt32();
        byte[] uncompressedBytes = binaryReader.ReadBytes(uncompressedSizeInBytes);

        return uncompressedBytes;
    }
}