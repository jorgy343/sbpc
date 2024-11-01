using Sbpc.Core.Properties;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Sbpc.Core;

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

    public Actor SetSwatch(string swatchClassName)
    {
        ObjectReference swatchObjectReference = new("", swatchClassName);
        PropertyObject swatchObjectProperty = new("SwatchDesc", swatchObjectReference);

        PropertyCustomizationData.PropertyList.SetProperty(swatchObjectProperty);
        return this;
    }

    public Actor SetBuiltWithRecipe(string recipeClassName)
    {
        ObjectReference recipeObjectReference = new("", recipeClassName);
        PropertyObject recipeObjectProperty = new("mBuiltWithRecipe", recipeObjectReference);

        Properties.SetProperty(recipeObjectProperty);
        return this;
    }

    public static Actor CreateFoundation(Vector3 position, Quaternion rotation)
    {
        int id = IdGenerator.GetNextId();
        Actor foundation = new()
        {
            ClassName = "/Game/FactoryGame/Buildable/Building/Foundation/AsphaltSet/Build_Foundation_Asphalt_8x2.Build_Foundation_Asphalt_8x2_C",
            LevelName = "Persistent_Level",
            InstanceName = "Persistent_Level:PersistentLevel.Build_Foundation_Asphalt_8x2_C_" + id,

            Position = position,
            Rotation = rotation,

            TrailingBytes = new byte[4],
        };

        foundation
            .SetSwatch("/Game/FactoryGame/Buildable/-Shared/Customization/Swatches/SwatchDesc_Concrete.SwatchDesc_Concrete_C")
            .SetBuiltWithRecipe("/Game/FactoryGame/Buildable/Building/Foundation/AsphaltSet/Recipe_Foundation_Asphalt_8x2.Recipe_Foundation_Asphalt_8x2_C");

        return foundation;
    }
}