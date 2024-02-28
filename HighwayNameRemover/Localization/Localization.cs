using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Colossal.IO.AssetDatabase;
using Colossal.Json;

namespace HighwayNameRemover
{
	public class Localization
	{
		internal delegate Dictionary<string, Dictionary<string, string>> OnLoadLocalization();
		internal static OnLoadLocalization onLoadLocalization = LoadLocalization;
		internal static Dictionary<string, Dictionary<string, string>> localization;

		internal static void AddCustomLocal(LocaleAsset localeAsset) { //Dictionary<string, string>

			if(onLoadLocalization != null) foreach(Delegate @delegate in onLoadLocalization.GetInvocationList()) {

				object result = @delegate.DynamicInvoke();

				if(result is not Dictionary<string, Dictionary<string, string>> localization) return;

				string loc = localeAsset.localeId;

				if(!localization.ContainsKey(loc)) loc = "en-US";

				foreach(string key in localization[loc].Keys) {
					if(localeAsset.data.entries.ContainsKey(key)) localeAsset.data.entries[key] = localization[loc][key];
					else localeAsset.data.entries.Add(key, localization[loc][key]);

					if(localeAsset.data.indexCounts.ContainsKey(key)) localeAsset.data.indexCounts[key] = localeAsset.data.indexCounts.Count;
					else localeAsset.data.indexCounts.Add(key, localeAsset.data.indexCounts.Count);
				}
			}
		}

		private static Dictionary<string, Dictionary<string, string>> LoadLocalization() {
			var localizationFile = Assembly.GetExecutingAssembly().GetManifestResourceStream("HighwayNameRemover.embedded.Localization.Localization.jsonc");
			localization = Decoder.Decode(new StreamReader(localizationFile).ReadToEnd()).Make<LocalizationJS>().Localization;
			return localization;
		}

	}

	[Serializable]
	public class LocalizationJS
	{	
		public Dictionary<string, Dictionary<string, string>> Localization = [];

	}
}