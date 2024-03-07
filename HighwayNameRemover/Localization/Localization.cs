using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Colossal.IO.AssetDatabase;
using Colossal.Json;

namespace AuthenticRusNames
{
	public class Localization
	{
		internal static Dictionary<string, Dictionary<string, string>> localization;

		internal static void AddCustomLocal(LocaleAsset localeAsset) { //Dictionary<string, string>

			if(localization is null) LoadLocalization();

			string loc = localeAsset.localeId;

			if(!localization.ContainsKey(loc)) loc = "en-US";

            foreach(string key in localization[loc].Keys) {
                if(localeAsset.data.entries.ContainsKey(key))
	                localeAsset.data.entries[key] = localization[loc][key];
                else
	                localeAsset.data.entries.Add(key, localization[loc][key]);

                if(!key.Contains(":")) continue;

                string[] parts = key.Split(":");
                if (int.TryParse(parts[1], out int n))
                {
	                n++;
	                if (localeAsset.data.indexCounts.ContainsKey(parts[0]))
	                {
		                if (localeAsset.data.indexCounts[parts[0]] != n) // Was <
		                {
			                localeAsset.data.indexCounts[parts[0]] = n;
		                }
	                }
	                else localeAsset.data.indexCounts.Add(parts[0], n);
                }
            }
		}

		private static void LoadLocalization()
		{
			//var localizationFile = Assembly.GetExecutingAssembly().GetManifestResourceStream("HighwayNameRemover.embedded.Localization.Localization.jsonc");
			var localizationFile = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{Assembly.GetExecutingAssembly().GetName().Name}.embedded.Localization.Localization.jsonc");
			localization = Decoder.Decode(new StreamReader(localizationFile).ReadToEnd()).Make<LocalizationJS>().Localization;
		}

	}

	[Serializable]
	public class LocalizationJS
	{	
		public Dictionary<string, Dictionary<string, string>> Localization = [];

	}
}