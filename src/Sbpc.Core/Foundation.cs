using System;
using System.Numerics;

namespace Sbpc.Core;

public static class Foundation
{
    public static Actor CreateFoundationFloor(FoundationMaterial material, FoundationThickness thickness, Vector3 position, Quaternion rotation)
    {
        string className = GetClassName(material, thickness);
        string instanceName = GetInstanceName(material, thickness);
        string recipeName = GetRecipeName(material, thickness);

        Actor foundationFloor = new()
        {
            ClassName = className,
            LevelName = "Persistent_Level",
            InstanceName = instanceName,

            Position = position,
            Rotation = rotation,

            TrailingBytes = new byte[4],
        };

        foundationFloor
            .SetSwatch(Swatch.Concrete) // TODO: Set this appropriately.
            .SetBuiltWithRecipe(recipeName);

        return foundationFloor;
    }

    public static string GetClassName(FoundationMaterial material, FoundationThickness thickness)
    {
        string size = GetSizeString(thickness);

        string className = material switch
        {
            FoundationMaterial.FicsIt => $"/Game/FactoryGame/Buildable/Building/Foundation/Build_Foundation_{size}_01.Build_Foundation_{size}_01_C",
            FoundationMaterial.Concrete => $"/Game/FactoryGame/Buildable/Building/Foundation/ConcreteSet/Build_Foundation_Concrete_{size}.Build_Foundation_Asphalt_{size}_C",
            FoundationMaterial.Metal => $"/Game/FactoryGame/Buildable/Building/Foundation/GripMetal/Build_Foundation_Metal_{size}.Build_Foundation_Metal_{size}_C",
            FoundationMaterial.ConcretePolished => $"/Game/FactoryGame/Buildable/Building/Foundation/PolishedConcreteSet/Build_Foundation_ConcretePolished_{size}_2.Build_Foundation_ConcretePolished_{size}_2_C",
            FoundationMaterial.Asphalt => $"/Game/FactoryGame/Buildable/Building/Foundation/AsphaltSet/Build_Foundation_Asphalt_{size}.Build_Foundation_Asphalt_{size}_C",
            _ => throw new ArgumentOutOfRangeException(nameof(material)),
        };

        return className;
    }

    public static string GetInstanceName(FoundationMaterial material, FoundationThickness thickness)
    {
        int id = IdGenerator.GetNextId();
        string size = GetSizeString(thickness);

        string instanceName = material switch
        {
            FoundationMaterial.FicsIt => $"Persistent_Level:PersistentLevel.Build_Foundation_{size}_01_C_{id}",
            FoundationMaterial.Metal => $"Persistent_Level:PersistentLevel.Build_Foundation_Metal_{size}_C_{id}",
            FoundationMaterial.Concrete => $"Persistent_Level:PersistentLevel.Build_Foundation_Concrete_{size}_C_{id}",
            FoundationMaterial.ConcretePolished => $"Persistent_Level:PersistentLevel.Build_Foundation_ConcretePolished_{size}_2_C_{id}",
            FoundationMaterial.Asphalt => $"Persistent_Level:PersistentLevel.Build_Foundation_Asphalt_{size}_C_{id}",
            _ => throw new ArgumentOutOfRangeException(nameof(material)),
        };

        return instanceName;
    }

    public static string GetRecipeName(FoundationMaterial material, FoundationThickness thickness)
    {
        string size = GetSizeString(thickness);

        string recipeName = material switch
        {
            FoundationMaterial.FicsIt => $"/Game/FactoryGame/Recipes/Buildings/Foundations/Recipe_Foundation_{size}_01.Recipe_Foundation_{size}_01_C",
            FoundationMaterial.Metal => $"/Game/FactoryGame/Buildable/Building/Foundation/GripMetal/Recipe_Foundation_Metal_{size}.Recipe_Foundation_Metal_{size}_C",
            FoundationMaterial.Concrete => $"/Game/FactoryGame/Buildable/Building/Foundation/ConcreteSet/Recipe_Foundation_Concrete_{size}.Recipe_Foundation_Concrete_{size}_C",
            FoundationMaterial.ConcretePolished => $"/Game/FactoryGame/Buildable/Building/Foundation/PolishedConcreteSet/Recipe_Foundation_ConcretePolished_{size}.Recipe_Foundation_ConcretePolished_{size}_C",
            FoundationMaterial.Asphalt => $"/Game/FactoryGame/Buildable/Building/Foundation/AsphaltSet/Recipe_Foundation_Asphalt_{size}.Recipe_Foundation_Asphalt_{size}_C",
            _ => throw new ArgumentOutOfRangeException(nameof(material)),
        };

        return recipeName;
    }

    public static string GetSizeString(FoundationThickness thickness)
    {
        string size = thickness switch
        {
            FoundationThickness.OneMeter => "8x1",
            FoundationThickness.TwoMeter => "8x2",
            FoundationThickness.FourMeter => "8x4",
            _ => throw new ArgumentOutOfRangeException(nameof(thickness))
        };

        return size;
    }
}