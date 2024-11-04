using System.Numerics;

namespace Sbpc.Core;

public static class Beam
{
    public static Actor CreateConcreteBeam(Vector3 position, Quaternion rotation, float length)
    {
        int id = IdGenerator.GetNextId();

        string className = "/SS_Mod/Builders/Beams/Build_ConcreteBeam_4x4.Build_ConcreteBeam_4x4_C";
        string instanceName = $"Persistent_Level:PersistentLevel.Build_ConcreteBeam_4x4_C_{id}";
        string recipeName = "/SS_Mod/Recipes/Beams/Recipe_SS_ConcreteBeam_4x4.Recipe_SS_ConcreteBeam_4x4_C";

        Actor concreteBeam = new()
        {
            ClassName = className,
            LevelName = "Persistent_Level",
            InstanceName = instanceName,

            Position = position,
            Rotation = rotation,

            TrailingBytes = new byte[4],
        };

        concreteBeam
            .SetLength(length)
            .SetSwatch(Swatch.Concrete)
            .SetBuiltWithRecipe(recipeName);

        return concreteBeam;
    }

    public static Actor CreateEmmisiveBeam(Vector3 position, Quaternion rotation, float length, Vector4 emissiveColor)
    {
        int id = IdGenerator.GetNextId();

        string className = "/SS_Mod/Builders/Beams/Build_EmissiveBeam_2xHalf.Build_EmissiveBeam_2xHalf_C";
        string instanceName = $"Persistent_Level:PersistentLevel.Build_EmissiveBeam_2xHalf_C_{id}";
        string recipeName = "/SS_Mod/Recipes/Beams/Recipe_SS_EmissveBeam_2xHalf.Recipe_SS_EmissveBeam_2xHalf_C";

        Actor concreteBeam = new()
        {
            ClassName = className,
            LevelName = "Persistent_Level",
            InstanceName = instanceName,

            Position = position,
            Rotation = rotation,

            TrailingBytes = new byte[4],
        };

        concreteBeam
            .SetLength(length)
            .SetColorSlot(255)
            .SetSwatch(Swatch.Custom)
            .SetOverrideColor(emissiveColor, emissiveColor)
            .SetBuiltWithRecipe(recipeName);

        return concreteBeam;
    }
}