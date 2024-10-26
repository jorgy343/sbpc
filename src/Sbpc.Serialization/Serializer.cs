using System.Diagnostics;
using System.Text;
using System.Numerics;

namespace Sbpc.Serialization;

public static class Serializer
{
    public static Blueprint ReadBlueprintFile(string path)
    {
        using FileStream stream = new(path, FileMode.Open, FileAccess.Read);
        using BinaryReader reader = new(stream, Encoding.Default, true);

        uint saveHeaderVersion = reader.ReadUInt32();
        uint saveVersion = reader.ReadUInt32();
        uint saveBuildVersion = reader.ReadUInt32();

        Vector3 dimensions = reader.ReadIntVector();

        List<BlueprintItemAmount> itemCost = reader.ReadBlueprintItemAmountList();
        List<ObjectReference> recipeReferences = reader.ReadObjectReferenceList();

        using MemoryStream uncompressedData = new(); // TODO: Should iterate through the chunk headers and determine total size beforehand. Could then multi thread this.
        while (stream.Position < stream.Length)
        {
            reader.ReadAndUncompressedChunkBytes(uncompressedData);
        }

        uncompressedData.Seek(0, SeekOrigin.Begin);
        List<Actor> actors = ParseBlueprintData(uncompressedData);

        Blueprint blueprint = new()
        {
            HeaderVersion = saveHeaderVersion,
            Version = saveVersion,
            BuildVersion = saveBuildVersion,
            Dimensions = dimensions,
            ItemCost = itemCost,
            RecipeReferences = recipeReferences,
            Actors = actors,
        };

        return blueprint;
    }

    private static List<Actor> ParseBlueprintData(MemoryStream blueprintData)
    {
        using BinaryReader reader = new(blueprintData, Encoding.Default, true);

        reader.ReadInt32(); // Total data blob size?
        reader.ReadInt32(); // Some sort of integrity check?

        int headerCount = reader.ReadInt32();
        List<ActorHeader> actorHeaders = new();

        for (int i = 0; i < headerCount; i++)
        {
            uint headerObjectType = reader.ReadUInt32();

            if (headerObjectType == 1) // Actor type.
            {
                ActorHeader actorHeader = reader.ReadActorHeader();
                actorHeaders.Add(actorHeader);
            }
            else
            {
                throw new NotSupportedException("Only actor objects are supported.");
            }
        }

        reader.ReadInt32(); // Not sure what this is.
        int entityCount = reader.ReadInt32();

        List<ActorObject> actorObjects = new();
        for (int i = 0; i < entityCount; i++)
        {
            ActorObject actorObject = reader.ReadActorObject();
            actorObjects.Add(actorObject);
        }

        Debug.Assert(actorHeaders.Count == actorObjects.Count, "Header count and entity count mismatch.");

        List<Actor> actors = new();
        for (int i = 0; i < actorHeaders.Count; i++)
        {
            ActorHeader actorHeader = actorHeaders[i];
            ActorObject actorObject = actorObjects[i];

            Actor actor = new()
            {
                ClassName = actorHeader.ClassName,
                LevelName = actorHeader.LevelName,
                InstanceName = actorHeader.InstanceName,
                Rotation = actorHeader.Rotation,
                Position = actorHeader.Position,
                Scale = actorHeader.Scale,
                Properties = actorObject.Properties,
            };

            actors.Add(actor);
        }

        return actors;
    }
}