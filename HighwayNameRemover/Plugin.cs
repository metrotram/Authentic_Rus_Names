using BepInEx;
using HarmonyLib;
using System.Reflection;
using System.Linq;
using BepInEx.Logging;

#if BEPINEX_V6
    using BepInEx.Unity.Mono;
#endif

namespace HighwayNameRemover
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;
        private void Awake()
        {
            Logger = base.Logger;

            Logger.LogInfo("[HighwayNameRemover]: Loading Harmony patches.");

            var harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID + "_Cities2Harmony");
            var patchedMethods = harmony.GetPatchedMethods().ToArray();

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} made patches! Patched methods: " + patchedMethods.Length);
            foreach (var patchedMethod in patchedMethods)
            {
                Logger.LogInfo($"[HighwayNameRemover]: Patched method: {patchedMethod.Module.Name}:{patchedMethod.Name}");
            }
        }
    }
}