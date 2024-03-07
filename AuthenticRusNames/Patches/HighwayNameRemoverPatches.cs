using HarmonyLib;
using Colossal.Localization;
using Colossal.IO.AssetDatabase;

namespace AuthenticRusNames.Patches
{
    [HarmonyPatch(typeof(LocalizationManager), "AddLocale", typeof(LocaleAsset))]
    internal class LocalizationManager_AddLocale
    {
        static void Prefix(LocaleAsset asset)
        {
            Localization.AddCustomLocal(asset);
        }
    }
}
