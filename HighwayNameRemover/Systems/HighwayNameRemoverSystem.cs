using Game;
using Unity.Entities;
using UnityEngine.Scripting;
using HighwayNameRemover.Jobs;
using Unity.Jobs;
using Colossal.Serialization.Entities;

namespace HighwayNameRemover.Systems
{
    public partial class HighwayNameRemoverSystem : GameSystemBase
    {
        private UpdateSignatureBuildingsTypeHandle m_UpdateSignatureBuildingsTypeHandle;
        private EntityQuery m_UpdateSignatureBuildingsJobQuery;

        [Preserve]
        protected override void OnCreate()
        {
            base.OnCreate();

            // Job Queries
            UpdateSignatureBuildingsQuery updateSignatureBuildingsQuery = new();
            m_UpdateSignatureBuildingsJobQuery = GetEntityQuery(updateSignatureBuildingsQuery.Query);

            UnityEngine.Debug.Log("[HighwayNameRemover]: System created.");
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);
            if (!mode.IsGameOrEditor())
            {
                return;
            }

            if (!m_UpdateSignatureBuildingsJobQuery.IsEmptyIgnoreFilter)
            {
                UnityEngine.Debug.Log("[HighwayNameRemoverSystem]: Update KonsiModSystem.");
                m_UpdateSignatureBuildingsTypeHandle.AssignHandles(ref CheckedStateRef);
                UpdateSignatureBuildingsJob updateSignatureBuildingsJob = new()
                {
                    EntityHandle = m_UpdateSignatureBuildingsTypeHandle.EntityTypeHandle,
                    PlaceableObjectDataLookup = m_UpdateSignatureBuildingsTypeHandle.PlaceableObjectDataLookup,
                };
                Dependency = updateSignatureBuildingsJob.Schedule(m_UpdateSignatureBuildingsJobQuery, Dependency);
            }
        }

        [Preserve]
        protected override void OnUpdate()
        {
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
