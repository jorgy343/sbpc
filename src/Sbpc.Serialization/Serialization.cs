﻿using Sbpc.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sbpc.Serialization;

public static class Serialization
{
    public static Blueprint ReadBlueprintFile(string path)
    {
        using FileStream stream = new(path, FileMode.Open, FileAccess.Read);
        using BinaryReader reader = new(stream, Encoding.Default, true);

        // Parse the file header data.
        uint saveHeaderVersion = reader.ReadUInt32();
        uint saveVersion = reader.ReadUInt32();
        uint saveBuildVersion = reader.ReadUInt32();

        var dimensions = reader.ReadIntVectorAsTuple();

        List<BlueprintItemAmount> itemCost = reader.ReadBlueprintItemAmountList();
        List<ObjectReference> recipeReferences = reader.ReadObjectReferenceList();

        // Parse the blueprint object data.
        using MemoryStream uncompressedData = DecompressAndPieceTogetherChunks(reader);

        File.WriteAllBytes("../../../../../samples/read.bin", uncompressedData.ToArray()); // TESTING

        List<Actor> actors = ParseBlueprintObjectData(uncompressedData);

        // Put it all together into a high level Blueprint object.
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

    private static MemoryStream DecompressAndPieceTogetherChunks(BinaryReader reader)
    {
        // Read each compressed chunk into a list so we can decompress them in parallel.
        List<(ChunkHeader Header, byte[] CompressedData)> compressedChunks = new();
        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            ChunkHeader chunkHeader = reader.ReadChunkHeader();

            Debug.Assert(chunkHeader.CompressedSize > 0 && chunkHeader.CompressedSize <= int.MaxValue);
            Debug.Assert(chunkHeader.UncompressedSize > 0 && chunkHeader.UncompressedSize <= int.MaxValue);

            byte[] compressedChunk = reader.ReadBytes((int)chunkHeader.CompressedSize);
            compressedChunks.Add((chunkHeader, compressedChunk));
        }

        // Decompress each chunk in parallel.
        byte[][] uncompressedChunks = new byte[compressedChunks.Count][];
        Parallel.ForEach(compressedChunks, (compressedChunk, state, index) =>
        {
            byte[] uncompressedBytes = Compression.Decompress(compressedChunk.CompressedData, compressedChunk.Header);
            uncompressedChunks[index] = uncompressedBytes;
        });

        // Combine all the uncompressed chunks into a single stream.
        int totalUncompressedSize = uncompressedChunks.Sum(chunk => chunk.Length);
        MemoryStream uncompressedData = new(totalUncompressedSize);

        foreach (byte[] uncompressedChunk in uncompressedChunks)
        {
            uncompressedData.Write(uncompressedChunk);
        }

        uncompressedData.Flush();
        uncompressedData.Seek(0, SeekOrigin.Begin);

        return uncompressedData;
    }

    private static List<Actor> ParseBlueprintObjectData(MemoryStream blueprintData)
    {
        using BinaryReader reader = new(blueprintData, Encoding.Default, true);

        reader.ReadInt32(); // Total size of all uncompressed chunks not counting this field.
        reader.ReadInt32(); // Total size in bytes of the header objects. This includes the header object count integer.

        int headerObjectCount = reader.ReadInt32();
        List<ActorHeader> actorHeaders = new();

        for (int i = 0; i < headerObjectCount; i++)
        {
            int headerObjectType = reader.ReadInt32();

            if (headerObjectType == 1) // Actor type.
            {
                ActorHeader actorHeader = reader.ReadActorHeader();
                actorHeaders.Add(actorHeader);
            }
            else
            {
                throw new NotSupportedException("Only actor objects are supported right now.");
            }
        }

        reader.ReadInt32(); // Total size in bytes of the entity objects. This includes the entity count integer.
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
                Parent = actorObject.Parent,
                Components = actorObject.Components,
                Properties = actorObject.Properties,
                TrailingBytes = actorObject.TrailingBytes,
            };

            actors.Add(actor);
        }

        return actors;
    }

    public static void WriteBlueprintFile(string path, Blueprint blueprint)
    {
        using FileStream stream = new(path, FileMode.Create, FileAccess.Write);
        using BinaryWriter writer = new(stream, Encoding.Default, true);

        writer.Write(blueprint.HeaderVersion);
        writer.Write(blueprint.Version);
        writer.Write(blueprint.BuildVersion);

        writer.WriteIntVector(blueprint.Dimensions);

        writer.WriteBlueprintItemAmountList(blueprint.ItemCost);
        writer.WriteObjectReferenceList(blueprint.RecipeReferences);

        byte[] uncompressedData = CreateUncompressedBlueprintData(blueprint.Actors);

        File.WriteAllBytes("../../../../../samples/write.bin", uncompressedData.ToArray()); // TESTING

        List<byte[]> uncompressedChunks = uncompressedData.Chunk(Constants.MaximumChunkSize).ToList();

        // Compress each chunk in parallel.
        (ChunkHeader Header, byte[] CompressedData)[] compressedChunks = new (ChunkHeader Header, byte[] CompressedData)[uncompressedChunks.Count];
        Parallel.ForEach(uncompressedChunks, (uncompressedChunk, state, index) =>
        {
            byte[] compressedChunk = Compression.Compress(uncompressedChunk);

            ChunkHeader chunkHeader = new()
            {
                UncompressedSize = (uint)uncompressedChunk.Length,
                CompressedSize = (uint)compressedChunk.Length,
            };

            compressedChunks[index] = (chunkHeader, compressedChunk);
        });

        foreach (var compressedChunk in compressedChunks)
        {
            writer.WriteChunkHeader(compressedChunk.Header);
            writer.Write(compressedChunk.CompressedData);
        }

        writer.Flush();
    }

    private static byte[] CreateUncompressedBlueprintData(List<Actor> actors)
    {
        using MemoryStream uncompressedData = new();
        using BinaryWriter writer = new(uncompressedData, Encoding.Default, true);

        BinaryWriterSizeWriter totalSizeWriter = new(writer);
        totalSizeWriter.WriteDummySize();

        using (totalSizeWriter.TrackSize())
        {
            // Writer the object headers.
            BinaryWriterSizeWriter headerSizeWriter = new(writer);
            headerSizeWriter.WriteDummySize();

            using (headerSizeWriter.TrackSize())
            {
                writer.Write(actors.Count); // Header count.

                foreach (Actor actor in actors)
                {
                    writer.Write(1); // Integer bool specifying this is an actor header.
                    writer.WriteActorHeader(actor);
                }
            }

            // Write the entity objects.
            BinaryWriterSizeWriter entitySizeWriter = new(writer);
            entitySizeWriter.WriteDummySize();

            using (entitySizeWriter.TrackSize())
            {
                writer.Write(actors.Count); // Entity count.
                foreach (Actor actor in actors)
                {
                    writer.WriteActorEntity(actor);
                }

                writer.Flush();
            }
        }

        // Return the bytes.
        byte[] uncompressedBytes = uncompressedData.ToArray();
        return uncompressedBytes;
    }
}