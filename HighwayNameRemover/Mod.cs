using Colossal.Logging;
using Game;
using Game.Modding;
using HighwayNameRemover.Systems;

namespace HighwayNameRemover
{
    public sealed class Mod : IMod
    {
        public const string Name = MyPluginInfo.PLUGIN_NAME;
        public static Mod Instance { get; set; }
        internal ILog Log { get; private set; }
        public void OnLoad()
        {
            Instance = this;
            Log = LogManager.GetLogger(Name);
#if VERBOSE
            Log.effectivenessLevel = Level.Verbose;
#elif DEBUG
            Log.effectivenessLevel = Level.Debug;
#endif

            Log.Info("Loading.");
        }

        public void OnCreateWorld(UpdateSystem updateSystem)
        {
            UnityEngine.Debug.Log("[HighwayNameRemover]: Add system to world.");
            updateSystem.UpdateAt<HighwayNameRemoverSystem>(SystemUpdatePhase.ModificationEnd);
        }

        public void OnDispose()
        {
            UnityEngine.Debug.Log("[HighwayNameRemover]: Mod disposed.");
            Instance = null;
        }
    }
}
