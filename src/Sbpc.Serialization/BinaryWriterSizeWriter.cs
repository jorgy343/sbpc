using System.Diagnostics;

namespace Sbpc.Serialization;

public sealed class BinaryWriterSizeWriter
{
    private readonly BinaryWriter _binaryWriter;

    private int _sizePosition = -1;

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

    public IDisposable TrackSize()
    {
        return new SizeCalculator(this);
    }

    private class SizeCalculator : IDisposable
    {
        private readonly BinaryWriterSizeWriter _sizeWriter;

        private int _startPosition;

        public SizeCalculator(BinaryWriterSizeWriter sizeWriter)
        {
            _sizeWriter = sizeWriter ?? throw new ArgumentNullException(nameof(sizeWriter));

            _startPosition = (int)_sizeWriter._binaryWriter.BaseStream.Position;
        }

        public void Dispose()
        {
            int endPosition = (int)_sizeWriter._binaryWriter.BaseStream.Position;

            int size = endPosition - _startPosition;

            Debug.Assert(_sizeWriter._sizePosition >= 0, $"Size position must be set before writing size. This can be set by calling {nameof(WriteDummySize)}.");
            Debug.Assert(size >= 0, "The end position is prior to the start position.");

            _sizeWriter._binaryWriter.Flush(); // Might not be needed.

            int resetPosition = (int)_sizeWriter._binaryWriter.BaseStream.Position;

            _sizeWriter._binaryWriter.Seek(_sizeWriter._sizePosition, SeekOrigin.Begin);
            _sizeWriter._binaryWriter.Write(size);
            _sizeWriter._binaryWriter.Seek(resetPosition, SeekOrigin.Begin);
        }
    }
}