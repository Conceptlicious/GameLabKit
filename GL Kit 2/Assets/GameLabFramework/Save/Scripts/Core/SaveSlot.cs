using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace GameLab
{
	// TODO: Custom save folder paths
	[Serializable]
	public class SaveSlot
	{
		private static readonly JsonSerializerSettings jsonSerializationSettings = new JsonSerializerSettings()
		{
			TypeNameHandling = TypeNameHandling.All,
			Formatting = Formatting.Indented,
			NullValueHandling = NullValueHandling.Include,
			Converters = { new ScriptableObjectConverter() }
		};

		public string SlotName { get; private set; }

		private readonly FileInfo saveFileInfo = null;

		public SaveSlot(string slotName)
		{
			SlotName = slotName;
			saveFileInfo = new FileInfo(Path.Combine(Application.persistentDataPath, SaveManager.SaveFolderName, SlotName));
			
		}

		/// <summary>
		/// Save <paramref name="saveData"/> as JSON to the path this save slot points.
		/// </summary>
		/// <param name="saveData"></param>
		public void Save(Dictionary<string, Dictionary<string, object>> saveData)
		{
			//TODO: Encrypt
			string data = JsonConvert.SerializeObject(saveData, jsonSerializationSettings);
			saveFileInfo.WriteAllText(data);
		}

		/// <summary>
		/// Load JSON data from the path this save slot points and deserialize it.
		/// </summary>
		/// <returns>Deserialized save data</returns>
		public Dictionary<string, Dictionary<string, object>> Load()
		{
			//TODO: Decrypt
			string data = saveFileInfo.ReadAllText();

			if(data == string.Empty)
			{
				return null;
			}

			return JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(data, jsonSerializationSettings);
		}
	}
}
