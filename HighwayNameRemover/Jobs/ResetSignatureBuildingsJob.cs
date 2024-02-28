using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

namespace HighwayNameRemover.Jobs
{
    struct ResetSignatureBuildingsQuery
    {
        public EntityQueryDesc[] Query;

        public ResetSignatureBuildingsQuery()
        {
            Query = [
                new()
                {
                    All =
                    [
                        ComponentType.ReadOnly<BuildingData>(),
                        ComponentType.ReadWrite<PlaceableObjectData>(),
                        ComponentType.ReadOnly<SignatureBuildingData>(),
                    ],
                    None =
                    [
                        ComponentType.Exclude<Updated>(),
                        ComponentType.Exclude<Deleted>(),
                        ComponentType.Exclude<Temp>(),
                    ],
                }
            ];
        }
    }

    public struct ResetSignatureBuildingsTypeHandle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AssignHandles(ref SystemState state)
        {
            EntityTypeHandle = state.GetEntityTypeHandle();
            PlaceableObjectDataLookup = state.GetComponentLookup<PlaceableObjectData>(false);
        }

        [ReadOnly]
        public EntityTypeHandle EntityTypeHandle;

        public ComponentLookup<PlaceableObjectData> PlaceableObjectDataLookup;
    }

    public struct ResetSignatureBuildingsJob : IJobChunk
    {
        public EntityTypeHandle EntityHandle;
        public ComponentLookup<PlaceableObjectData> PlaceableObjectDataLookup;

        public void Execute(in ArchetypeChunk chunk,
            int unfilteredChunkIndex,
            bool useEnabledMask,
            in v128 chunkEnabledMask)
        {
            NativeArray<Entity> entities = chunk.GetNativeArray(EntityHandle);
            ChunkEntityEnumerator enumerator = new(useEnabledMask, chunkEnabledMask, chunk.Count);
            while (enumerator.NextEntityIndex(out int i))
            {
                Entity entity = entities[i];
                PlaceableObjectData placeableObjectData = PlaceableObjectDataLookup[entity];
                placeableObjectData.m_Flags |= Game.Objects.PlacementFlags.Unique;
                PlaceableObjectDataLookup[entity] = placeableObjectData;
            }
        }
    }
}