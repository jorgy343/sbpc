using Sbpc.Serialization.Properties;

namespace Sbpc.Serialization;

public class ActorObject
{
    public required int SizeInBytes { get; set; }
    public required ObjectReference Parent { get; set; }
    public required List<ObjectReference> Components { get; set; }
    public required PropertyList Properties { get; set; }
    public required byte[] TrailingBytes { get; set; }
}