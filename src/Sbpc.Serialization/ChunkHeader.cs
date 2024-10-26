namespace Sbpc.Serialization;

public class ChunkHeader
{
    public uint Magic1 { get; init; } = 0x9E_2A_83_C1;
    public uint Magic2 { get; init; } = 0x22_22_22_22;
    public uint Magic3 { get; init; } = 0x03_00_00_00;

    public int MaximumChunkSize { get; init; } = 0x00_00_02_00;

    public required long CompressedSize { get; init; }
    public required long UncompressedSize { get; init; }
}