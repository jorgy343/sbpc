using System.Diagnostics;

namespace Sbpc.Serialization;

public class BinaryWriterSizeWriter
{
    private readonly BinaryWriter _binaryWriter;

    private int _sizePosition = -1;

    private int _startPosition;
    private int _endPosition;

    public BinaryWriterSizeWriter(BinaryWriter binaryWriter)
    {
        _binaryWriter = binaryWriter ?? throw new ArgumentNullException(nameof(binaryWriter));
    }

    public void WriteDummySize()
    {
        _sizePosition = (int)_binaryWriter.BaseStream.Position;

        _binaryWriter.Write(0);
        _binaryWriter.Flush();
    }

    public void BeginTrackingSize()
    {
        _startPosition = (int)_binaryWriter.BaseStream.Position;
    }

    public void EndTrackingSize()
    {
        _endPosition = (int)_binaryWriter.BaseStream.Position;
    }

    public void WriteSize()
    {
        int size = _endPosition - _startPosition;

        Debug.Assert(_sizePosition >= 0, $"Size position must be set before writing size. This can be set by calling {nameof(WriteDummySize)}.");
        Debug.Assert(size >= 0, "The end position is prior to the start position.");

        _binaryWriter.Flush(); // Might not be needed.

        int resetPosition = (int)_binaryWriter.BaseStream.Position;

        _binaryWriter.Seek(_sizePosition, SeekOrigin.Begin);
        _binaryWriter.Write(size);
        _binaryWriter.Seek(resetPosition, SeekOrigin.Begin);
    }
}