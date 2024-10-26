using System.Numerics;

namespace Sbpc.Serialization;

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
}