using System.Collections.Generic;
using System.Numerics;

namespace Sbpc.Core;

public class Blueprint
{
    public uint HeaderVersion { get; init; }
    public uint Version { get; init; }
    public uint BuildVersion { get; init; }

    /// <summary>
    /// Gets the dimensions of the blueprint. This is represented as blocks of 8 meters cubed. Thus
    /// to get the actual size of the blueprint in meters, you would multiply the dimensions by 8.
    /// </summary>
    public Vector3 Dimensions { get; init; }

    public List<BlueprintItemAmount> ItemCost { get; init; } = new();
    public List<ObjectReference> RecipeReferences { get; init; } = new();

    public List<Actor> Actors { get; init; } = new();

    public Actor AddFoundation(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        // TODO: Update ItemCost.
        // TODO: Update RecipeReferences.

        Actor actor = new()
        {
            ClassName = ClassNames.Build.Foundation.Asphalt8x2,
            LevelName = "Persistent_Level",
            InstanceName = "Persistent_Level:Persistent_Level.Build_Foundation_Asphalt_8x2_C_2147467932",

            Position = position,
            Rotation = rotation,
            Scale = scale,

            TrailingBytes = new byte[4],
        };

        Actors.Add(actor);
        return actor;
    }
}