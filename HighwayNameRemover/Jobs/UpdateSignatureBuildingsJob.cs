using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

namespace HighwayNameRemover.Jobs
{
    struct UpdateSignatureBuildingsQuery
    {
        public EntityQueryDesc[] Query;

        public UpdateSignatureBuildingsQuery()
        {
            Query = [
                new()
                {
                    All =
                    [
                        ComponentType.ReadOnly<BuildingData>(),
                        ComponentType.ReadWrite<PlaceableObjectData>(),
                    ],
                    Any =
                    [
                        ComponentType.ReadOnly<SignatureBuildingData>(),
                        ComponentType.ReadOnly<UniqueObjectData>(),
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

    public struct UpdateSignatureBuildingsTypeHandle
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

    public struct UpdateSignatureBuildingsJob : IJobChunk
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
                placeableObjectData.m_Flags &= ~Game.Objects.PlacementFlags.Unique;
                PlaceableObjectDataLookup[entity] = placeableObjectData;
            }
        }
    }
}