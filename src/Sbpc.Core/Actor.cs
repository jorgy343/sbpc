﻿using Sbpc.Core.Properties;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json.Serialization;

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

    [JsonIgnore]
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

    public Actor SetSwatch(Swatch swatch)
    {
        ObjectReference swatchObjectReference = new("", swatch.ClassName);
        PropertyObject swatchObjectProperty = new("SwatchDesc", swatchObjectReference);

        PropertyCustomizationData.PropertyList.SetProperty(swatchObjectProperty);
        return this;
    }

    public Actor SetOverrideColor(Vector4 primaryColor, Vector4 secondaryColor, string paintFinish = "/Game/FactoryGame/Buildable/-Shared/Customization/PaintFinishes/PaintFinishDesc_Default.PaintFinishDesc_Default_C")
    {
        PropertyStructLinearColor primaryColorProperty = new("PrimaryColor", primaryColor);
        PropertyStructLinearColor secondaryColorProperty = new("SecondaryColor", secondaryColor);
        PropertyObject paintFinishProperty = new("PaintFinish", new("", paintFinish));

        PropertyStructPropertyList overrideColorProperty = new("OverrideColorData", "FactoryCustomizationColorSlot");

        overrideColorProperty.PropertyList.SetProperty(primaryColorProperty);
        overrideColorProperty.PropertyList.SetProperty(secondaryColorProperty);
        overrideColorProperty.PropertyList.SetProperty(paintFinishProperty);

        PropertyCustomizationData.PropertyList.SetProperty(overrideColorProperty);
        return this;
    }

    public Actor SetLength(float length)
    {
        PropertyFloat lengthProperty = new("mLength", length);
        Properties.SetProperty(lengthProperty);

        return this;
    }

    public Actor SetBuiltWithRecipe(string recipeClassName)
    {
        ObjectReference recipeObjectReference = new("", recipeClassName);
        PropertyObject recipeObjectProperty = new("mBuiltWithRecipe", recipeObjectReference);

        Properties.SetProperty(recipeObjectProperty);
        return this;
    }
}