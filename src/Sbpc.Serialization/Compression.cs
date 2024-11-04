using System.IO;
using System.IO.Compression;
using System.Text;

namespace Sbpc.Serialization;

public static class Compression
{
    /// <summary>
    /// Compresses the given byte array using ZLib compression. The length of the uncompressed data
    /// is prepended to the uncompressed data as a 4-byte signed integer. Note that the length does
    /// not include the size of the prepended integer itself.
    /// </summary>
    /// <param name="uncompressedData">The byte array to compress.</param>
    /// <returns>A byte array containing the compressed data.</returns>
    public static byte[] Compress(byte[] uncompressedData)
    {
        return Ionic.Zlib.ZlibStream.CompressBuffer(uncompressedData);
    }

    /// <summary>
    /// Decompresses the given byte array using ZLib compression. The method expects the compressed
    /// data to have the uncompressed data length prepended as a 4-byte signed integer. This length
    /// integer does not include the size of the prepended integer itself.
    /// </summary>
    /// <param name="compressedData">The byte array containing the compressed data.</param>
    /// <returns>A byte array containing the decompressed data.</returns>
    public static byte[] Decompress(byte[] compressedData, ChunkHeader chunkHeader)
    {
        using MemoryStream compressedStream = new(compressedData);
        using ZLibStream zlibStream = new(compressedStream, CompressionMode.Decompress, true);
        using BinaryReader binaryReader = new(zlibStream, Encoding.Default, true);

        byte[] uncompressedBytes = binaryReader.ReadBytes((int)chunkHeader.UncompressedSize);
        return uncompressedBytes;
    }
}