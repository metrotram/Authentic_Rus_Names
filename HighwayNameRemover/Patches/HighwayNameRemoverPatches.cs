using System.Collections.Generic;
using HarmonyLib;
using Colossal.Localization;
using Colossal.IO.AssetDatabase;

namespace HighwayNameRemover.Patches
{
    [HarmonyPatch(typeof(LocalizationManager), "AddLocale", typeof(LocaleAsset))]
    internal class LocalizationManager_AddLocale
    {
        static readonly LocalizationJS localizationJS = new();
        static void Prefix(LocaleAsset asset)
        {
            Localization.AddCustomLocal(asset);

            foreach(string key in asset.data.entries.Keys) {
                if(!localizationJS.Localization.ContainsKey(asset.localeId)) {
                    localizationJS.Localization.Add(asset.localeId, []);
                }
                if(!localizationJS.Localization[asset.localeId].ContainsKey(key)) localizationJS.Localization[asset.localeId].Add(key, asset.data.entries[key]);
            }

            Dictionary<string, Dictionary<string, int>> localeIndex = [];

            foreach(string key in asset.data.indexCounts.Keys) {
                if(!localeIndex.ContainsKey(asset.localeId)) {
                    localeIndex.Add(asset.localeId, []);
                }
                if(!localeIndex[asset.localeId].ContainsKey(key)) localeIndex[asset.localeId].Add(key, asset.data.indexCounts[key]);
            }
        }
    }
}
