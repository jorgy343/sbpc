using System.Numerics;
using Sbpc.Serialization.Properties;

namespace Sbpc.Serialization;

public class Actor
{
    public required string ClassName { get; init; }
    public required string LevelName { get; init; }
    public required string InstanceName { get; init; }

    public Quaternion Rotation { get; set; } = new(0, 0, 0, 1);
    public Vector3 Position { get; set; } = new(0, 0, 0);
    public Vector3 Scale { get; set; } = new(1, 1, 1);

    public ObjectReference Parent { get; init; } = DefaultBlueprintParent;
    public List<ObjectReference> Components { get; init; } = new();
    public PropertyList Properties { get; init; } = new();

    public byte[] TrailingBytes { get; init; } = Array.Empty<byte>();

    public static ObjectReference DefaultBlueprintParent { get; } = new(
        "Persistent_Level",
        "Persistent_Level:PersistentLevel.BuildableSubsystem");

    public PropertyStructPropertyList PropertyCustomizationData
    {
        get
        {
            PropertyStructPropertyList structProperty = Properties.GetOrSetProperty("mCustomizationData", () =>
            {
                return new PropertyStructPropertyList("mCustomizationData", "FactoryCustomizationData");
            });

            return structProperty;
        }
    }

    public Actor SetColorSlot(byte colorSlot)
    {
        PropertyByteByte colorSlotProperty = new("mColorSlot", colorSlot);
        Properties.SetProperty(colorSlotProperty);

        return this;
    }
}