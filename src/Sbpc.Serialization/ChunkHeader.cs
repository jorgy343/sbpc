namespace Sbpc.Serialization;

public class ChunkHeader
{
    public uint Magic1 { get; init; } = 0x9E_2A_83_C1;
    public uint Magic2 { get; init; } = 0x22_22_22_22;

    public long MaximumChunkSize { get; init; } = Constants.MaximumChunkSize;
    public byte CompressionType { get; init; } = 0x03; // 0x3 is for zlib.

    public required long CompressedSize { get; init; }
    public required long UncompressedSize { get; init; }
}